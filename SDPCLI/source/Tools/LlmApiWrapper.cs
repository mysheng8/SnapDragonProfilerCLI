using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Tools
{
    /// <summary>
    /// Generic OpenAI-compatible Chat Completions API wrapper.
    /// Provides a single <see cref="Chat"/> method that sends a prompt and returns the response text.
    /// This class has no knowledge of draw calls, shaders, or categories — callers own the prompt.
    ///
    /// Config keys:
    ///   LlmApiEndpoint    — full chat/completions URL
    ///   LlmApiKey         — Bearer API key
    ///   LlmModel          — model name (e.g. gpt-4o, deepseek-chat)
    ///   LlmTimeoutSeconds — HTTP timeout in seconds (default 60)
    /// </summary>
    public class LlmApiWrapper
    {
        private readonly string  _endpoint;
        private readonly string  _apiKey;
        private readonly string  _model;
        private readonly int     _timeoutSeconds;
        private readonly ILogger _logger;

        /// <summary>Whether endpoint and API key are both configured.</summary>
        public bool   IsEnabled { get; }
        public string Model     => _model;
        public string Endpoint  => _endpoint;

        /// <summary>Error message from the last Chat() call, or null if it succeeded.</summary>
        public string? LastError { get; private set; }

        private readonly int     _maxOutputTokens;

        public LlmApiWrapper(Config config, ILogger logger)
        {
            _logger          = logger;
            _endpoint        = config.Get("LlmApiEndpoint", "").Trim();
            _apiKey          = config.Get("LlmApiKey",      "").Trim();
            _model           = config.Get("LlmModel",       "gpt-4o").Trim();
            _timeoutSeconds  = config.GetInt("LlmTimeoutSeconds",  60);
            _maxOutputTokens = config.GetInt("LlmMaxOutputTokens", 800);
            IsEnabled        = !string.IsNullOrEmpty(_endpoint) && !string.IsNullOrEmpty(_apiKey);
        }

        /// <summary>
        /// Send a single-turn chat prompt and return the model's reply text.
        /// Returns null on any error (HTTP failure, JSON parse error, etc.).
        /// The caller is responsible for building the full prompt and parsing the response.
        /// </summary>
        public string? Chat(string prompt)
        {
            LastError = null;
            if (!IsEnabled) { LastError = "LLM not configured"; return null; }
            try
            {
                return CallApi(prompt);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                _logger.Info($"    [LLM] Request failed: {ex.Message}");
                return null;
            }
        }

        // ── Private HTTP implementation ───────────────────────────────────────

        private string CallApi(string userMessage)
        {
            var requestBody = new JObject
            {
                ["model"]       = _model,
                ["max_tokens"]  = _maxOutputTokens,
                ["temperature"] = 0.0,
                ["messages"] = new JArray
                {
                    new JObject { ["role"] = "user", ["content"] = userMessage }
                }
            };

            byte[] bodyBytes = Encoding.UTF8.GetBytes(requestBody.ToString(Formatting.None));

            var req = (HttpWebRequest)WebRequest.Create(_endpoint);
            req.Method        = "POST";
            req.ContentType   = "application/json";
            req.ContentLength = bodyBytes.Length;
            req.Timeout       = _timeoutSeconds * 1000;
            req.Headers.Add("Authorization", "Bearer " + _apiKey);

            using (var stream = req.GetRequestStream())
                stream.Write(bodyBytes, 0, bodyBytes.Length);

            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex) when (ex.Response != null)
            {
                string errBody;
                using (var sr = new StreamReader(ex.Response.GetResponseStream()))
                    errBody = sr.ReadToEnd();
                throw new Exception("HTTP " + (int)((HttpWebResponse)ex.Response).StatusCode +
                    ": " + errBody.Substring(0, Math.Min(300, errBody.Length)));
            }

            using (var sr = new StreamReader(resp.GetResponseStream()))
            {
                string body = sr.ReadToEnd();
                var json = JObject.Parse(body);
                return json["choices"]?[0]?["message"]?["content"]?.ToString() ?? "";
            }
        }
    }
}
