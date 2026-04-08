using System;
using System.Data.SQLite;

namespace SnapdragonProfilerCLI.Data
{
    /// <summary>
    /// Unified entry point for all sdp.db access.
    /// Holds DbPath + CaptureId; creates SQLiteConnections on demand with consistent settings.
    ///
    /// Thread-safe: immutable after construction; each OpenConnection() call returns a new
    /// independent connection — safe for Parallel.ForEach workers.
    ///
    /// Split into domain-specific partial class files:
    ///   SdpDatabase.Schema.cs    — TableExists, ColumnExists, FindColumn, GetFormatName,
    ///                              GetShaderStageName, GetAllTables, GetMetadata,
    ///                              ValidateForAnalysis
    ///   SdpDatabase.DrawCalls.cs — full DrawCallInfo resolution + GetDrawCallIds list
    ///   SdpDatabase.Shaders.cs   — GetShaderStages, ReadSpirvBytes, ReadShaderDisasm,
    ///                              ResolvePipelineFromDrawCall
    ///   SdpDatabase.Textures.cs  — GetTextureMetadata, ReadTextureBytes
    ///   SdpDatabase.Buffers.cs   — ReadBufferBytes, LoadVertexInputState,
    ///                              GetDrawCallsWithVertexBuffers
    /// </summary>
    public sealed partial class SdpDatabase
    {
        public string DbPath    { get; }
        public uint   CaptureId { get; }

        public SdpDatabase(string dbPath, uint captureId)
        {
            DbPath    = dbPath;
            CaptureId = captureId;
        }

        /// <summary>
        /// Opens and returns a new SQLiteConnection with standard settings.
        /// Caller is responsible for disposing (use `using`).
        /// readOnly=true (default): adds "Read Only=True;" to the connection string,
        /// preventing even an accidental RESERVED lock on Windows SQLite.
        /// </summary>
        public SQLiteConnection OpenConnection(bool readOnly = true)
        {
            string cs = readOnly
                ? $"Data Source={DbPath};Version=3;Read Only=True;"
                : $"Data Source={DbPath};Version=3;";

            var conn = new SQLiteConnection(cs);
            conn.Open();

            if (!readOnly)
            {
                // WAL mode: concurrent readers + one writer without SQLITE_BUSY
                using var wal = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn);
                wal.ExecuteNonQuery();
            }

            // Retry up to 2000 ms on lock contention before throwing
            using var timeout = new SQLiteCommand("PRAGMA busy_timeout=2000;", conn);
            timeout.ExecuteNonQuery();

            return conn;
        }

        /// <summary>Convenience: open → action → dispose.</summary>
        public void WithConnection(Action<SQLiteConnection> action, bool readOnly = true)
        {
            using var conn = OpenConnection(readOnly);
            action(conn);
        }
    }
}
