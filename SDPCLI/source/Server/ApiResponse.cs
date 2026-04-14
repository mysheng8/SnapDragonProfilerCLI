using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SnapdragonProfilerCLI.Server
{
    public class ApiResponse
    {
        public bool    Ok    { get; set; }
        public object? Data  { get; set; }
        public string? Error { get; set; }

        public static ApiResponse Success(object? data = null) =>
            new ApiResponse { Ok = true, Data = data };

        public static ApiResponse Failure(string error) =>
            new ApiResponse { Ok = false, Error = error };

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver     = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling    = NullValueHandling.Ignore,
            Formatting           = Formatting.None
        };

        public void WriteTo(HttpListenerResponse response, int statusCode = 200)
        {
            string json  = JsonConvert.SerializeObject(this, _settings);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            response.StatusCode   = statusCode;
            response.ContentType  = "application/json; charset=utf-8";
            response.ContentLength64 = bytes.Length;
            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.OutputStream.Close();
        }
    }
}
