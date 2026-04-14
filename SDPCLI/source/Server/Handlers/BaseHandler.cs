using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace SnapdragonProfilerCLI.Server.Handlers
{
    public interface IHandler
    {
        void Handle(HttpListenerContext ctx);
    }

    public abstract class BaseHandler : IHandler
    {
        private const int MaxBodyBytes = 65536;  // 64 KB body limit

        public abstract void Handle(HttpListenerContext ctx);

        protected T? ReadJsonBody<T>(HttpListenerRequest request) where T : class
        {
            using var reader = new StreamReader(
                new LimitedStream(request.InputStream, MaxBodyBytes),
                Encoding.UTF8);
            string body = reader.ReadToEnd();
            if (string.IsNullOrWhiteSpace(body)) return null;
            return JsonConvert.DeserializeObject<T>(body);
        }

        protected void WriteOk(HttpListenerContext ctx, object? data = null, int statusCode = 200) =>
            ApiResponse.Success(data).WriteTo(ctx.Response, statusCode);

        protected void WriteError(HttpListenerContext ctx, string error, int statusCode = 400) =>
            ApiResponse.Failure(error).WriteTo(ctx.Response, statusCode);

        /// <summary>
        /// Validates that path is non-empty and resolves under allowedRoot (prevents traversal).
        /// Returns the full resolved path on success, null on failure.
        /// </summary>
        protected string? ValidatePath(string? path, string allowedRoot, HttpListenerContext ctx)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                WriteError(ctx, "path is required");
                return null;
            }

            string full;
            try
            {
                full = Path.GetFullPath(path!);
            }
            catch
            {
                WriteError(ctx, "invalid path");
                return null;
            }

            string root = Path.GetFullPath(allowedRoot).TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) +
                Path.DirectorySeparatorChar;

            if (!full.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            {
                WriteError(ctx, "path outside allowed root");
                return null;
            }

            return full;
        }

        // ─── small helper: stream that reads at most maxBytes ────────────────
        private sealed class LimitedStream : Stream
        {
            private readonly Stream _inner;
            private long _remaining;

            public LimitedStream(Stream inner, long maxBytes)
            {
                _inner     = inner;
                _remaining = maxBytes;
            }

            public override bool CanRead  => true;
            public override bool CanSeek  => false;
            public override bool CanWrite => false;
            public override long Length   => throw new NotSupportedException();
            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (_remaining <= 0) return 0;
                int toRead = (int)Math.Min(count, _remaining);
                int n = _inner.Read(buffer, offset, toRead);
                _remaining -= n;
                return n;
            }

            public override void Flush()  => throw new NotSupportedException();
            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
            public override void SetLength(long value) => throw new NotSupportedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        }
    }
}
