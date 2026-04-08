using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Tools
{
    /// <summary>
    /// Persistent ring-pool cache for LLM prompt→response pairs.
    ///
    /// Design
    /// ──────
    ///   Key    = SHA-256(prompt) first 128 bits (32 hex chars). Collision probability
    ///            is negligible for &lt; ~10 000 unique shader prompts per project.
    ///   Value  = raw LLM response text.
    ///   Pool   = fixed-size circular buffer of <see cref="Capacity"/> slots.
    ///            When full, the oldest entry (FIFO, write-head position) is evicted.
    ///   Lookup = O(1) via an in-memory Dictionary&lt;key → ring position&gt;.
    ///   Disk   = JSON file written after every <see cref="Put"/> call.
    ///            Survives process restarts: same shader in a different SDP session
    ///            will hit the cache immediately.
    ///
    /// Thread Safety
    /// ─────────────
    ///   ReaderWriterLockSlim — multiple concurrent readers, exclusive writer.
    ///   Safe for the parallel Step 2 labeling in AnalysisPipeline.
    ///
    /// Config Keys (read by LlmApiWrapper)
    ///   LlmCacheEnabled  = true | false   (default: true)
    ///   LlmCacheSize     = N              (default: 512)
    ///   LlmCachePath     = path-to-file   (default: WorkingDirectory/llm_cache.json)
    /// </summary>
    public sealed class LlmResponseCache : IDisposable
    {
        // ── Entry model ───────────────────────────────────────────────────────
        private sealed class CacheEntry
        {
            public string Key      { get; set; } = "";
            public string Response { get; set; } = "";
            public string Ts       { get; set; } = "";
        }

        // ── Ring buffer state ─────────────────────────────────────────────────
        private readonly int            _capacity;
        private readonly string?        _persistPath;
        private readonly CacheEntry?[]  _ring;
        private readonly Dictionary<string, int> _index;  // key → ring slot position
        private int                     _writeHead;       // next write position (FIFO eviction target)
        private int                     _count;

        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private readonly ILogger _logger;

        // ── Public properties ─────────────────────────────────────────────────
        public int Capacity => _capacity;
        public int Count
        {
            get
            {
                _rwLock.EnterReadLock();
                try { return _count; }
                finally { _rwLock.ExitReadLock(); }
            }
        }

        // ── Construction ──────────────────────────────────────────────────────
        public LlmResponseCache(int capacity, string? persistPath, ILogger logger)
        {
            _capacity    = Math.Max(8, capacity);
            _persistPath = persistPath;
            _ring        = new CacheEntry?[_capacity];
            _index       = new Dictionary<string, int>(_capacity, StringComparer.Ordinal);
            _writeHead   = 0;
            _count       = 0;
            _logger      = logger;

            if (persistPath != null)
                Load();
        }

        // ── Public API ────────────────────────────────────────────────────────

        /// <summary>
        /// Try to retrieve a cached response for the given prompt.
        /// Returns <c>true</c> and sets <paramref name="response"/> on cache hit.
        /// </summary>
        public bool TryGet(string prompt, out string response)
        {
            string key = Hash(prompt);
            _rwLock.EnterReadLock();
            try
            {
                if (_index.TryGetValue(key, out int pos) && _ring[pos] != null)
                {
                    response = _ring[pos]!.Response;
                    return true;
                }
            }
            finally { _rwLock.ExitReadLock(); }

            response = "";
            return false;
        }

        /// <summary>
        /// Store a prompt→response pair. If the prompt is already in the pool,
        /// updates its response in-place (no eviction). Otherwise evicts the
        /// oldest entry (at the current write-head) and inserts the new one.
        /// </summary>
        public void Put(string prompt, string response)
        {
            string key = Hash(prompt);
            _rwLock.EnterWriteLock();
            try
            {
                // Update in-place when key already exists
                if (_index.TryGetValue(key, out int existing))
                {
                    _ring[existing]!.Response = response;
                    _ring[existing]!.Ts       = UtcNowIso();
                    SaveNoLock();
                    return;
                }

                // Evict the entry currently at _writeHead (oldest, FIFO)
                var victim = _ring[_writeHead];
                if (victim != null)
                {
                    _index.Remove(victim.Key);
                    _count--;
                }

                // Write new entry
                _ring[_writeHead] = new CacheEntry
                {
                    Key      = key,
                    Response = response,
                    Ts       = UtcNowIso(),
                };
                _index[key] = _writeHead;
                _count++;
                _writeHead   = (_writeHead + 1) % _capacity;

                SaveNoLock();
            }
            finally { _rwLock.ExitWriteLock(); }
        }

        // ── Hashing ──────────────────────────────────────────────────────────

        /// <summary>
        /// Compute SHA-256 of the prompt and return the first 16 bytes as a 32-char hex string.
        /// 128-bit hash space → collision probability negligible for &lt; 10 000 entries.
        /// </summary>
        public static string Hash(string prompt)
        {
            using var sha  = SHA256.Create();
            byte[] bytes   = sha.ComputeHash(Encoding.UTF8.GetBytes(prompt));
            // First 16 bytes = 128 bits, sufficient for our scale
            return BitConverter.ToString(bytes, 0, 16).Replace("-", "").ToLowerInvariant();
        }

        // ── Persistence ──────────────────────────────────────────────────────

        private void Load()
        {
            if (_persistPath == null || !File.Exists(_persistPath)) return;
            try
            {
                string json = File.ReadAllText(_persistPath, Encoding.UTF8);
                var root    = JObject.Parse(json);
                var slots   = root["slots"] as JArray;
                if (slots == null) return;

                int loadCount = Math.Min(slots.Count, _capacity);
                for (int i = 0; i < loadCount; i++)
                {
                    var tok = slots[i];
                    if (tok == null || tok.Type == JTokenType.Null) continue;
                    string? key = tok["key"]?.ToString();
                    string? res = tok["response"]?.ToString();
                    if (string.IsNullOrEmpty(key) || res == null) continue;

                    _ring[i] = new CacheEntry
                    {
                        Key      = key!,
                        Response = res,
                        Ts       = tok["ts"]?.ToString() ?? "",
                    };
                    _index[key!] = i;
                    _count++;
                }

                // Restore write-head so FIFO eviction order is preserved across restarts
                int savedHead = root["write_head"]?.Value<int>() ?? _count % _capacity;
                _writeHead = Math.Max(0, Math.Min(savedHead, _capacity - 1));

                _logger.Info($"  [LLM cache] Loaded {_count}/{_capacity} entries from: {_persistPath}");
            }
            catch (Exception ex)
            {
                _logger.Info($"  [LLM cache] ⚠ Failed to load cache: {ex.Message}");
            }
        }

        /// <summary>Serialize the entire ring to disk. Caller must hold the write lock.</summary>
        private void SaveNoLock()
        {
            if (_persistPath == null) return;
            try
            {
                var slots = new JArray();
                for (int i = 0; i < _capacity; i++)
                {
                    var e = _ring[i];
                    if (e == null)
                        slots.Add(JValue.CreateNull());
                    else
                        slots.Add(new JObject
                        {
                            ["key"]      = e.Key,
                            ["response"] = e.Response,
                            ["ts"]       = e.Ts,
                        });
                }

                var root = new JObject
                {
                    ["capacity"]   = _capacity,
                    ["write_head"] = _writeHead,
                    ["count"]      = _count,
                    ["slots"]      = slots,
                };

                string? dir = Path.GetDirectoryName(_persistPath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                // Write atomically: temp file → rename, avoids partial writes on crash
                string tmp = _persistPath + ".tmp";
                File.WriteAllText(tmp, root.ToString(Formatting.Indented), Encoding.UTF8);
                if (File.Exists(_persistPath)) File.Delete(_persistPath);
                File.Move(tmp, _persistPath);
            }
            catch (Exception ex)
            {
                _logger.Info($"  [LLM cache] ⚠ Save failed: {ex.Message}");
            }
        }

        private static string UtcNowIso() =>
            DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        // ── IDisposable ───────────────────────────────────────────────────────
        public void Dispose() => _rwLock.Dispose();
    }
}
