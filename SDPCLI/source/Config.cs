using System;
using System.Collections.Generic;
using System.IO;
using SnapdragonProfilerCLI.Logging;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// Simple INI configuration reader
    /// </summary>
    public class Config
    {
        private readonly Dictionary<string, string> settings = new Dictionary<string, string>();

        public Config(string configPath)
        {
            if (File.Exists(configPath))
            {
                LoadFromFile(configPath);
            }
        }

        private void LoadFromFile(string path)
        {
            try
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    var trimmed = line.Trim();
                    
                    // Skip empty lines and comments
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#") || trimmed.StartsWith(";"))
                        continue;

                    // Parse key=value
                    var parts = trimmed.Split(new[] { '=' }, 2);
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        settings[key] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Config", $"Failed to load config from {path}: {ex.Message}");
            }
        }

        public string Get(string key, string defaultValue = "")
        {
            return settings.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            var value = Get(key);
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            
            return value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("1", StringComparison.OrdinalIgnoreCase);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            var value = Get(key);
            return int.TryParse(value, out var result) ? result : defaultValue;
        }

        public uint GetUInt(string key, uint defaultValue = 0)
        {
            var value = Get(key);
            return uint.TryParse(value, out var result) ? result : defaultValue;
        }

        public bool HasKey(string key)
        {
            return settings.ContainsKey(key);
        }

        /// <summary>Override a config key at runtime (CLI arg injection).</summary>
        public void Set(string key, string value)
        {
            settings[key] = value;
        }
    }
}
