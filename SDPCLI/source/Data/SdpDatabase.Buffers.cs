using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace SnapdragonProfilerCLI.Data
{
    /// <summary>Vertex binding description (stride / input rate per binding slot).</summary>
    public class VertexBindingDesc
    {
        public uint Binding   { get; set; }
        public uint Stride    { get; set; }
        public uint InputRate { get; set; }
    }

    /// <summary>Vertex attribute description (location / format / offset).</summary>
    public class VertexAttrDesc
    {
        public uint Location { get; set; }
        public uint Binding  { get; set; }
        public uint Format   { get; set; }
        public uint Offset   { get; set; }
    }

    public sealed partial class SdpDatabase
    {
        /// <summary>
        /// Reads all chunks of a buffer resource from VulkanSnapshotByteBuffers,
        /// concatenates them, and returns the combined bytes.
        /// Returns null if the table is absent or no rows found.
        /// </summary>
        public byte[]? ReadBufferBytes(uint resourceId)
        {
            try
            {
                using var conn = OpenConnection();
                if (!TableExists(conn, "VulkanSnapshotByteBuffers")) return null;

                var chunks = new List<byte[]>();
                using var cmd = new SQLiteCommand(
                    "SELECT data FROM VulkanSnapshotByteBuffers " +
                    $"WHERE captureID={CaptureId} AND resourceID={resourceId} " +
                    "ORDER BY sequenceID", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    long len = r.GetBytes(0, 0, null, 0, 0);
                    if (len > 0)
                    {
                        var chunk = new byte[len];
                        r.GetBytes(0, 0, chunk, 0, (int)len);
                        chunks.Add(chunk);
                    }
                }

                if (chunks.Count == 0) return null;
                if (chunks.Count == 1) return chunks[0];

                int total  = chunks.Sum(c => c.Length);
                var result = new byte[total];
                int pos    = 0;
                foreach (var chunk in chunks)
                {
                    Buffer.BlockCopy(chunk, 0, result, pos, chunk.Length);
                    pos += chunk.Length;
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ReadBufferBytes({resourceId}) failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Loads the vertex input state (binding + attribute descriptions) for a pipeline
        /// from the CSV-imported PipelineVertexInput* tables.
        /// </summary>
        public (List<VertexBindingDesc> bindings, List<VertexAttrDesc> attributes)
            LoadVertexInputState(uint pipelineId)
        {
            var bindings   = new List<VertexBindingDesc>();
            var attributes = new List<VertexAttrDesc>();

            using var conn = OpenConnection();

            if (TableExists(conn, "PipelineVertexInputBindings"))
            {
                try
                {
                    using var cmd = new SQLiteCommand(
                        "SELECT Binding, Stride, InputRate FROM PipelineVertexInputBindings " +
                        $"WHERE CaptureID={CaptureId} AND PipelineID={pipelineId} ORDER BY Binding", conn);
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                        bindings.Add(new VertexBindingDesc
                        {
                            Binding   = Convert.ToUInt32(r[0]),
                            Stride    = Convert.ToUInt32(r[1]),
                            InputRate = r[2] is long lr ? (uint)lr :
                                        string.Equals(r[2]?.ToString(), "INSTANCE",
                                            StringComparison.OrdinalIgnoreCase) ? 1u : 0u
                        });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  PipelineVertexInputBindings query failed: {ex.Message}");
                }
            }

            if (TableExists(conn, "PipelineVertexInputAttributes"))
            {
                try
                {
                    using var cmd = new SQLiteCommand(
                        "SELECT Location, Binding, Format, Offset FROM PipelineVertexInputAttributes " +
                        $"WHERE CaptureID={CaptureId} AND PipelineID={pipelineId} ORDER BY Location", conn);
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                        attributes.Add(new VertexAttrDesc
                        {
                            Location = Convert.ToUInt32(r[0]),
                            Binding  = Convert.ToUInt32(r[1]),
                            Format   = Convert.ToUInt32(r[2]),
                            Offset   = Convert.ToUInt32(r[3])
                        });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  PipelineVertexInputAttributes query failed: {ex.Message}");
                }
            }

            return (bindings, attributes);
        }

        /// <summary>
        /// Returns DrawCallApiIDs that have at least one entry in DrawCallVertexBuffers.
        /// </summary>
        public List<uint> GetDrawCallsWithVertexBuffers(int maxCount = 500)
        {
            var result = new List<uint>();
            try
            {
                using var conn = OpenConnection();
                if (!TableExists(conn, "DrawCallVertexBuffers")) return result;
                if (!TableExists(conn, "DrawCallParameters"))   return result;

                using var cmd = new SQLiteCommand(
                    $"SELECT DISTINCT vb.DrawCallApiID FROM DrawCallVertexBuffers vb " +
                    $"INNER JOIN DrawCallParameters p ON p.DrawCallApiID = vb.DrawCallApiID " +
                    $"ORDER BY vb.DrawCallApiID LIMIT {maxCount}", conn);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    result.Add(Convert.ToUInt32(r[0]));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  GetDrawCallsWithVertexBuffers failed: {ex.Message}");
            }
            return result;
        }
    }
}
