using System;
using System.Data.SQLite;

namespace SnapdragonProfilerCLI.Data
{
    /// <summary>Texture metadata DTO returned by GetTextureMetadata().</summary>
    public class TextureMetadata
    {
        public int Width      { get; set; }
        public int Height     { get; set; }
        public int Depth      { get; set; }
        public int Format     { get; set; }
        public int LayerCount { get; set; }
        public int LevelCount { get; set; }
    }

    public sealed partial class SdpDatabase
    {
        /// <summary>
        /// Returns metadata for a texture resource.
        /// Tries captureID-specific row first; falls back to any row for legacy SDPs.
        /// Returns null if not found.
        /// </summary>
        public TextureMetadata? GetTextureMetadata(ulong resourceId)
        {
            using var conn = OpenConnection();
            try
            {
                using (var command = new SQLiteCommand(
                    "SELECT width, height, depth, format, layerCount, levelCount " +
                    "FROM VulkanSnapshotTextures " +
                    "WHERE captureID = @captureId AND resourceID = @resourceId LIMIT 1", conn))
                {
                    command.Parameters.AddWithValue("@captureId",  (long)CaptureId);
                    command.Parameters.AddWithValue("@resourceId", (long)resourceId);
                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                        return new TextureMetadata
                        {
                            Width      = reader.GetInt32(0),
                            Height     = reader.GetInt32(1),
                            Depth      = reader.GetInt32(2),
                            Format     = reader.GetInt32(3),
                            LayerCount = reader.GetInt32(4),
                            LevelCount = reader.GetInt32(5)
                        };
                }

                // Fallback: no captureID filter (legacy SDPs without per-capture rows)
                using (var fallback = new SQLiteCommand(
                    "SELECT width, height, depth, format, layerCount, levelCount " +
                    "FROM VulkanSnapshotTextures WHERE resourceID = @resourceId LIMIT 1", conn))
                {
                    fallback.Parameters.AddWithValue("@resourceId", (long)resourceId);
                    using var reader = fallback.ExecuteReader();
                    if (reader.Read())
                        return new TextureMetadata
                        {
                            Width      = reader.GetInt32(0),
                            Height     = reader.GetInt32(1),
                            Depth      = reader.GetInt32(2),
                            Format     = reader.GetInt32(3),
                            LayerCount = reader.GetInt32(4),
                            LevelCount = reader.GetInt32(5)
                        };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  GetTextureMetadata({resourceId}) failed: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Returns raw texture bytes from VulkanSnapshotByteBuffers.
        /// Returns null if not found.
        /// </summary>
        public byte[]? ReadTextureBytes(ulong resourceId)
        {
            using var conn = OpenConnection();
            try
            {
                using var command = new SQLiteCommand(
                    "SELECT data FROM VulkanSnapshotByteBuffers " +
                    "WHERE captureID = @captureId AND resourceID = @resourceId " +
                    "ORDER BY sequenceID LIMIT 1", conn);
                command.Parameters.AddWithValue("@captureId",  (long)CaptureId);
                command.Parameters.AddWithValue("@resourceId", (long)resourceId);

                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                long dataSize = reader.GetBytes(0, 0, null, 0, 0);
                if (dataSize == 0) return null;

                byte[] buffer = new byte[dataSize];
                reader.GetBytes(0, 0, buffer, 0, (int)dataSize);
                return buffer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ReadTextureBytes({resourceId}) failed: {ex.Message}");
                return null;
            }
        }
    }
}
