using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using SnapdragonProfilerCLI.Models;
using SnapdragonProfilerCLI.Services.Analysis;

namespace SnapdragonProfilerCLI.Tools
{
    /// <summary>
    /// Mesh extraction tool.
    /// Reads vertex buffer and index buffer binary data from VulkanSnapshotByteBuffers,
    /// combines with pipeline vertex input state (binding/attribute descriptions),
    /// and exports Wavefront OBJ files.
    /// </summary>
    public class MeshExtractor
    {
        private readonly string _databasePath;
        private readonly int    _captureId;

        public MeshExtractor(string databasePath, int captureId = 3)
        {
            _databasePath = databasePath;
            _captureId    = captureId;
        }

        // ──────────────────────────────────────────────────────────────────────
        // Public API
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Extracts the mesh for a single draw call and writes an OBJ file.
        /// drawCallNumber accepts "1.1.5" (encoded) or a plain ApiID integer ("106974").
        /// Returns true on success.
        /// </summary>
        public bool ExtractMesh(string drawCallNumber, string outputPath)
        {
            Console.WriteLine($"\n=== Extracting Mesh: DrawCall {drawCallNumber} ===");

            try
            {
                // 1. Resolve DrawCallInfo (pipeline + VB/IB bindings + draw parameters)
                var svc  = new DrawCallQueryService();
                var info = svc.GetDrawCallInfo(_databasePath, (uint)_captureId, drawCallNumber);
                if (info == null)
                {
                    Console.WriteLine($"  ✗ DrawCall '{drawCallNumber}' not found");
                    return false;
                }

                return ExtractFromInfo(info, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Exception: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Batch extracts meshes for draw calls that have vertex buffer bindings.
        /// Writes one OBJ per draw call into outputDir.
        /// Returns the count of successfully written files.
        /// </summary>
        public int ExtractAllMeshes(string outputDir, int maxDrawCalls = 50)
        {
            Console.WriteLine($"\n=== Batch Mesh Extraction (max {maxDrawCalls} draw calls) ===");
            Directory.CreateDirectory(outputDir);

            // Collect DrawCallApiIDs that have a vertex buffer entry
            var apiIds = GetDrawCallsWithVertexBuffers(maxDrawCalls);
            Console.WriteLine($"  Found {apiIds.Count} draw calls with vertex buffer bindings");

            int succeeded = 0;
            var svc       = new DrawCallQueryService();

            foreach (uint apiId in apiIds)
            {
                var info = svc.GetDrawCallInfo(_databasePath, (uint)_captureId, apiId.ToString());
                if (info == null) continue;

                string safeId  = apiId.ToString();
                string objPath = Path.Combine(outputDir, $"drawcall_{safeId}.obj");

                if (ExtractFromInfo(info, objPath))
                    succeeded++;
            }

            Console.WriteLine($"\n  Batch complete: {succeeded}/{apiIds.Count} OBJ files written to: {outputDir}");
            return succeeded;
        }

        // ──────────────────────────────────────────────────────────────────────
        // Core extraction logic
        // ──────────────────────────────────────────────────────────────────────

        private bool ExtractFromInfo(DrawCallInfo info, string outputPath)
        {
            Console.WriteLine($"  DrawCall #{info.DrawCallNumber}  ApiID={info.ApiID}  {info.ApiName}");
            Console.WriteLine($"  Pipeline={info.PipelineID}  IndexCount={info.IndexCount}  VertexCount={info.VertexCount}");

            if (info.VertexBuffers.Count == 0)
            {
                Console.WriteLine("  ✗ No vertex buffer bindings found for this draw call");
                return false;
            }

            bool isIndexed = info.IndexBuffer != null &&
                             (info.ApiName.IndexOf("Indexed", StringComparison.OrdinalIgnoreCase) >= 0 ||
                              info.IndexCount > 0);

            // 2. Load vertex input state from database tables
            var (bindings, attributes) = LoadVertexInputState(info.PipelineID);

            if (attributes.Count == 0)
            {
                Console.WriteLine("  ✗ No vertex attribute descriptions found for this pipeline");
                Console.WriteLine($"    (PipelineVertexInputAttributes where CaptureID={_captureId} AND PipelineID={info.PipelineID} returned 0 rows)");
                return false;
            }

            Console.WriteLine($"  Vertex bindings: {bindings.Count}   Attributes: {attributes.Count}");
            foreach (var attr in attributes)
                Console.WriteLine($"    attr: location={attr.Location}  binding={attr.Binding}  " +
                                  $"format={attr.Format}({GetFormatName(attr.Format)})  offset={attr.Offset}");

            // 3. Load index buffer binary (may be null for non-indexed draws)
            int[]? indexArray = null;
            if (isIndexed && info.IndexBuffer != null)
            {
                byte[]? ibBytes = ReadBufferBytes(info.IndexBuffer.BufferID);
                if (ibBytes != null && ibBytes.Length > 0)
                {
                    indexArray = ParseIndexBuffer(ibBytes, info.IndexBuffer.IndexType,
                                                  info.IndexBuffer.Offset, info.IndexCount);
                    Console.WriteLine($"  IndexBuffer: {ibBytes.Length} bytes  → {indexArray.Length} indices");
                }
                else
                {
                    Console.WriteLine($"  ⚠ IndexBuffer {info.IndexBuffer.BufferID}: no binary data in VulkanSnapshotByteBuffers");
                }
            }

            // 4. Load vertex buffer binaries for each binding slot
            var vbData = new Dictionary<uint, byte[]>();  // key = binding slot
            foreach (var vb in info.VertexBuffers)
            {
                byte[]? bytes = ReadBufferBytes(vb.BufferID);
                if (bytes != null && bytes.Length > 0)
                {
                    vbData[vb.Binding] = bytes;
                    Console.WriteLine($"  VertexBuffer binding={vb.Binding} bufferID={vb.BufferID}: {bytes.Length} bytes");
                }
                else
                {
                    Console.WriteLine($"  ⚠ VertexBuffer binding={vb.Binding} bufferID={vb.BufferID}: " +
                                      "no binary data in VulkanSnapshotByteBuffers");
                }
            }

            if (vbData.Count == 0)
            {
                Console.WriteLine("  ✗ No vertex buffer binary data available — cannot reconstruct mesh");
                Console.WriteLine("    Possible causes:");
                Console.WriteLine("      1. VulkanSnapshotByteBuffers does not store vertex buffer data");
                Console.WriteLine("      2. BufferID mismatch between DrawCallVertexBuffers and ByteBuffers tables");
                return false;
            }

            // 5. Determine stride per binding slot (from VertexBindingDesc or infer from attributes)
            var strideMap = new Dictionary<uint, uint>();
            foreach (var bd in bindings)
                strideMap[bd.Binding] = bd.Stride;

            foreach (var attr in attributes.GroupBy(a => a.Binding))
            {
                if (!strideMap.ContainsKey(attr.Key))
                {
                    // Infer stride from max(offset + formatBytes) in this binding
                    uint inferred = attr.Max(a => a.Offset + (uint)GetFormatByteSize(a.Format));
                    strideMap[attr.Key] = inferred;
                    Console.WriteLine($"  ⚠ Binding {attr.Key}: stride not in DB, inferred = {inferred}");
                }
            }

            // 6. Parse vertices
            // Identify which attribute plays each semantic role
            VertexAttrDesc? posAttr = PickPositionAttribute(attributes);
            VertexAttrDesc? nrmAttr = PickNormalAttribute(attributes, posAttr);
            VertexAttrDesc? uvAttr  = PickTexCoordAttribute(attributes, posAttr, nrmAttr);

            if (posAttr == null)
            {
                Console.WriteLine("  ✗ Cannot identify position attribute (expected R32G32B32_SFLOAT at location 0)");
                Console.WriteLine("    Available attributes:");
                foreach (var a in attributes)
                    Console.WriteLine($"      location={a.Location} format={GetFormatName(a.Format)}");
                return false;
            }

            Console.WriteLine($"  Position attr: location={posAttr.Location} format={GetFormatName(posAttr.Format)}");
            if (nrmAttr != null) Console.WriteLine($"  Normal   attr: location={nrmAttr.Location} format={GetFormatName(nrmAttr.Format)}");
            if (uvAttr  != null) Console.WriteLine($"  TexCoord attr: location={uvAttr.Location}  format={GetFormatName(uvAttr.Format)}");

            // Determine vertex range from draw call parameters
            uint firstVertex = 0;
            uint vertexCount = info.VertexCount > 0 ? info.VertexCount : 0;

            if (isIndexed && indexArray != null && indexArray.Length > 0)
            {
                // For indexed draws, derive vertex span from index array + vertexOffset
                int minIdx = indexArray.Min();
                int maxIdx = indexArray.Max();
                firstVertex = (uint)Math.Max(0, minIdx);
                vertexCount = (uint)(maxIdx - minIdx + 1);
            }

            if (vertexCount == 0)
            {
                Console.WriteLine("  ⚠ Vertex count is 0 — attempting full buffer parse");
                uint posStride = strideMap.TryGetValue(posAttr.Binding, out uint s) ? s : 12;
                if (vbData.TryGetValue(posAttr.Binding, out byte[]? vbBytes) && posStride > 0)
                    vertexCount = (uint)(vbBytes.Length / posStride);
            }

            Console.WriteLine($"  Parsing {vertexCount} vertices starting at offset {firstVertex}...");

            var positions = new List<float[]>();
            var normals   = new List<float[]>();
            var texcoords = new List<float[]>();

            for (uint vi = firstVertex; vi < firstVertex + vertexCount; vi++)
            {
                float[]? pos = ParseAttribute(vbData, posAttr, strideMap, vi);
                if (pos == null) break;
                positions.Add(pos);

                if (nrmAttr != null)
                {
                    float[]? nrm = ParseAttribute(vbData, nrmAttr, strideMap, vi);
                    if (nrm != null) normals.Add(nrm);
                }

                if (uvAttr != null)
                {
                    float[]? uv = ParseAttribute(vbData, uvAttr, strideMap, vi);
                    if (uv != null) texcoords.Add(uv);
                }
            }

            Console.WriteLine($"  Parsed: {positions.Count} positions  {normals.Count} normals  {texcoords.Count} UVs");

            if (positions.Count == 0)
            {
                Console.WriteLine("  ✗ No vertex positions parsed");
                return false;
            }

            // 7. Build face list
            var faces = new List<int[]>();
            if (indexArray != null && indexArray.Length >= 3)
            {
                // Remap absolute indices → relative (0-based from firstVertex)
                for (int i = 0; i + 2 < indexArray.Length; i += 3)
                {
                    int a = indexArray[i]     - (int)firstVertex;
                    int b = indexArray[i + 1] - (int)firstVertex;
                    int c = indexArray[i + 2] - (int)firstVertex;
                    if (a >= 0 && b >= 0 && c >= 0 &&
                        a < positions.Count && b < positions.Count && c < positions.Count)
                    {
                        faces.Add(new[] { a, b, c });
                    }
                }
            }
            else
            {
                // Non-indexed draw: generate sequential triangles
                for (int i = 0; i + 2 < positions.Count; i += 3)
                    faces.Add(new[] { i, i + 1, i + 2 });
            }

            Console.WriteLine($"  Faces: {faces.Count} triangles");

            // 8. Write OBJ
            WriteObjFile(outputPath, info, positions, normals, texcoords, faces);
            Console.WriteLine($"  ✓ Written: {Path.GetFullPath(outputPath)}");
            return true;
        }

        // ──────────────────────────────────────────────────────────────────────
        // Database queries
        // ──────────────────────────────────────────────────────────────────────

        private (List<VertexBindingDesc> bindings, List<VertexAttrDesc> attributes)
            LoadVertexInputState(uint pipelineId)
        {
            var bindings   = new List<VertexBindingDesc>();
            var attributes = new List<VertexAttrDesc>();

            using var conn = new SQLiteConnection($"Data Source={_databasePath};Version=3;Read Only=True;");
            conn.Open();

            // Binding descriptions (our own table exported by VulkanSnapshotModel)
            if (TableExists(conn, "PipelineVertexInputBindings"))
            {
                try
                {
                    using var cmd = new SQLiteCommand(
                        "SELECT Binding, Stride, InputRate FROM PipelineVertexInputBindings " +
                        $"WHERE CaptureID={_captureId} AND PipelineID={pipelineId} ORDER BY Binding", conn);
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                        bindings.Add(new VertexBindingDesc
                        {
                            Binding   = Convert.ToUInt32(r[0]),
                            Stride    = Convert.ToUInt32(r[1]),
                            // InputRate is stored as TEXT ("VERTEX"/"INSTANCE") — parse defensively
                            InputRate = r[2] is long lr ? (uint)lr :
                                        string.Equals(r[2]?.ToString(), "INSTANCE",
                                            StringComparison.OrdinalIgnoreCase) ? 1u : 0u
                        });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ⚠ PipelineVertexInputBindings query failed: {ex.Message}");
                }
            }

            // Attribute descriptions (our own table exported by VulkanSnapshotModel)
            if (TableExists(conn, "PipelineVertexInputAttributes"))
            {
                try
                {
                    using var cmd = new SQLiteCommand(
                        "SELECT Location, Binding, Format, Offset FROM PipelineVertexInputAttributes " +
                        $"WHERE CaptureID={_captureId} AND PipelineID={pipelineId} ORDER BY Location", conn);
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
                    Console.WriteLine($"  ⚠ PipelineVertexInputAttributes query failed: {ex.Message}");
                }
            }

            return (bindings, attributes);
        }

        private byte[]? ReadBufferBytes(uint resourceId)
        {
            try
            {
                using var conn = new SQLiteConnection($"Data Source={_databasePath};Version=3;Read Only=True;");
                conn.Open();

                if (!TableExists(conn, "VulkanSnapshotByteBuffers"))
                    return null;

                // Concatenate all chunks ordered by sequenceID
                var chunks = new List<byte[]>();
                using var cmd = new SQLiteCommand(
                    "SELECT data FROM VulkanSnapshotByteBuffers " +
                    $"WHERE captureID={_captureId} AND resourceID={resourceId} " +
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

                // Concatenate multiple chunks
                int total = chunks.Sum(c => c.Length);
                var result = new byte[total];
                int pos = 0;
                foreach (var chunk in chunks)
                {
                    Buffer.BlockCopy(chunk, 0, result, pos, chunk.Length);
                    pos += chunk.Length;
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ ReadBufferBytes({resourceId}) failed: {ex.Message}");
                return null;
            }
        }

        private List<uint> GetDrawCallsWithVertexBuffers(int maxCount)
        {
            var result = new List<uint>();
            try
            {
                using var conn = new SQLiteConnection($"Data Source={_databasePath};Version=3;Read Only=True;");
                conn.Open();

                if (!TableExists(conn, "DrawCallVertexBuffers")) return result;
                if (!TableExists(conn, "DrawCallParameters"))   return result;

                // Join to ensure DrawCall exists in parameters table
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
                Console.WriteLine($"  ⚠ GetDrawCallsWithVertexBuffers failed: {ex.Message}");
            }
            return result;
        }

        // ──────────────────────────────────────────────────────────────────────
        // Attribute semantic selection
        // ──────────────────────────────────────────────────────────────────────

        private static VertexAttrDesc? PickPositionAttribute(List<VertexAttrDesc> attrs)
        {
            // 1. Prefer location=0 with 3-float format
            var cand = attrs.FirstOrDefault(a => a.Location == 0 &&
                                                 (a.Format == 106 || a.Format == 109));
            if (cand != null) return cand;

            // 2. Any location=0 attribute
            cand = attrs.FirstOrDefault(a => a.Location == 0);
            if (cand != null) return cand;

            // 3. First R32G32B32_SFLOAT (106) or R32G32B32A32_SFLOAT (109)
            return attrs.FirstOrDefault(a => a.Format == 106 || a.Format == 109);
        }

        private static VertexAttrDesc? PickNormalAttribute(List<VertexAttrDesc> attrs,
                                                            VertexAttrDesc? posAttr)
        {
            var candidates = attrs.Where(a => a != posAttr).ToList();

            // 1. location=1 with float3 format
            var cand = candidates.FirstOrDefault(a => a.Location == 1 &&
                                                      (a.Format == 106 || a.Format == 38));
            if (cand != null) return cand;

            // 2. Any R8G8B8A8_SNORM (packed normal) — format 38
            cand = candidates.FirstOrDefault(a => a.Format == 38);
            if (cand != null) return cand;

            // 3. Second R32G32B32_SFLOAT
            return candidates.FirstOrDefault(a => a.Format == 106 && a != posAttr);
        }

        private static VertexAttrDesc? PickTexCoordAttribute(List<VertexAttrDesc> attrs,
                                                              VertexAttrDesc? posAttr,
                                                              VertexAttrDesc? nrmAttr)
        {
            var candidates = attrs.Where(a => a != posAttr && a != nrmAttr).ToList();

            // 1. R32G32_SFLOAT (103) — common full-precision UV
            var cand = candidates.FirstOrDefault(a => a.Format == 103);
            if (cand != null) return cand;

            // 2. R16G16_SFLOAT (83) — compressed UV
            return candidates.FirstOrDefault(a => a.Format == 83);
        }

        // ──────────────────────────────────────────────────────────────────────
        // Vertex attribute parsing
        // ──────────────────────────────────────────────────────────────────────

        private static float[]? ParseAttribute(Dictionary<uint, byte[]> vbData,
                                               VertexAttrDesc attr,
                                               Dictionary<uint, uint> strideMap,
                                               uint vertexIndex)
        {
            if (!vbData.TryGetValue(attr.Binding, out byte[]? bytes)) return null;
            if (!strideMap.TryGetValue(attr.Binding, out uint stride) || stride == 0) return null;

            int byteOffset = (int)(vertexIndex * stride + attr.Offset);
            int formatSize  = GetFormatByteSize(attr.Format);

            if (byteOffset + formatSize > bytes.Length) return null;

            using var ms = new MemoryStream(bytes, byteOffset, formatSize);
            using var br = new BinaryReader(ms);

            return attr.Format switch
            {
                100 => new[] { br.ReadSingle() },                                       // R32_SFLOAT
                103 => new[] { br.ReadSingle(), br.ReadSingle() },                      // R32G32
                106 => new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },     // R32G32B32
                109 => new[] { br.ReadSingle(), br.ReadSingle(),                         // R32G32B32A32
                               br.ReadSingle(), br.ReadSingle() },
                 37 => UnpackR8G8B8A8Unorm(br),                                         // normalized byte
                 38 => UnpackR8G8B8A8Snorm(br),                                         // signed normalized byte
                 43 => UnpackR8G8B8A8Uint(br),                                          // uint byte
                 83 => new[] { HalfToFloat(br.ReadUInt16()), HalfToFloat(br.ReadUInt16()) },  // R16G16_SFLOAT
                 97 => new[] { HalfToFloat(br.ReadUInt16()), HalfToFloat(br.ReadUInt16()),    // R16G16B16A16
                               HalfToFloat(br.ReadUInt16()), HalfToFloat(br.ReadUInt16()) },
                  _ => null
            };
        }

        private static float[] UnpackR8G8B8A8Unorm(BinaryReader br)
        {
            byte r = br.ReadByte(), g = br.ReadByte(), b = br.ReadByte(), a = br.ReadByte();
            return new[] { r / 255f, g / 255f, b / 255f, a / 255f };
        }

        private static float[] UnpackR8G8B8A8Snorm(BinaryReader br)
        {
            sbyte r = br.ReadSByte(), g = br.ReadSByte(), b = br.ReadSByte(), a = br.ReadSByte();
            return new[] { Math.Max(r / 127f, -1f), Math.Max(g / 127f, -1f),
                           Math.Max(b / 127f, -1f), Math.Max(a / 127f, -1f) };
        }

        private static float[] UnpackR8G8B8A8Uint(BinaryReader br)
        {
            byte r = br.ReadByte(), g = br.ReadByte(), b = br.ReadByte(), a = br.ReadByte();
            return new[] { (float)r, (float)g, (float)b, (float)a };
        }

        private static float HalfToFloat(ushort half)
        {
            int sign     = (half >> 15) & 0x1;
            int exponent = (half >> 10) & 0x1F;
            int mantissa =  half        & 0x3FF;

            if (exponent == 0)
            {
                if (mantissa == 0) return sign != 0 ? -0f : 0f;
                // Denormal
                float val = (float)(mantissa * Math.Pow(2, -24));
                return sign != 0 ? -val : val;
            }
            else if (exponent == 31)
            {
                return mantissa == 0
                    ? (sign != 0 ? float.NegativeInfinity : float.PositiveInfinity)
                    : float.NaN;
            }
            else
            {
                float val = (float)((1 + mantissa / 1024.0) * Math.Pow(2, exponent - 15));
                return sign != 0 ? -val : val;
            }
        }

        // ──────────────────────────────────────────────────────────────────────
        // Index buffer parsing
        // ──────────────────────────────────────────────────────────────────────

        private static int[] ParseIndexBuffer(byte[] bytes, string indexType, uint offset, uint indexCount)
        {
            bool isUint32 = indexType.IndexOf("32", StringComparison.OrdinalIgnoreCase) >= 0;
            int  bytesPerIndex = isUint32 ? 4 : 2;
            int  startByte     = (int)offset;

            // Determine count: use indexCount from DrawCall params, or infer from buffer size
            int available = (bytes.Length - startByte) / bytesPerIndex;
            int count     = indexCount > 0 ? (int)Math.Min(indexCount, available) : available;

            var indices = new int[count];
            using var ms = new MemoryStream(bytes, startByte, count * bytesPerIndex);
            using var br = new BinaryReader(ms);

            for (int i = 0; i < count; i++)
                indices[i] = isUint32 ? (int)br.ReadUInt32() : br.ReadUInt16();

            return indices;
        }

        // ──────────────────────────────────────────────────────────────────────
        // OBJ writer
        // ──────────────────────────────────────────────────────────────────────

        private static void WriteObjFile(string path,
                                         DrawCallInfo info,
                                         List<float[]> positions,
                                         List<float[]> normals,
                                         List<float[]> texcoords,
                                         List<int[]>   faces)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");

            using var writer = new StreamWriter(path, append: false, encoding: Encoding.UTF8);
            writer.WriteLine("# Extracted by SDPCLI MeshExtractor");
            writer.WriteLine($"# DrawCall: {info.DrawCallNumber}  ApiID: {info.ApiID}  {info.ApiName}");
            writer.WriteLine($"# PipelineID: {info.PipelineID}");
            writer.WriteLine($"# Vertices: {positions.Count}  Faces: {faces.Count}");
            writer.WriteLine($"# Normals: {normals.Count}  TexCoords: {texcoords.Count}");
            writer.WriteLine();

            // Positions
            foreach (var v in positions)
            {
                string x = v.Length > 0 ? v[0].ToString("F6") : "0";
                string y = v.Length > 1 ? v[1].ToString("F6") : "0";
                string z = v.Length > 2 ? v[2].ToString("F6") : "0";
                writer.WriteLine($"v {x} {y} {z}");
            }

            // Normals
            if (normals.Count > 0)
            {
                writer.WriteLine();
                foreach (var n in normals)
                {
                    string x = n.Length > 0 ? n[0].ToString("F6") : "0";
                    string y = n.Length > 1 ? n[1].ToString("F6") : "0";
                    string z = n.Length > 2 ? n[2].ToString("F6") : "0";
                    writer.WriteLine($"vn {x} {y} {z}");
                }
            }

            // TexCoords
            if (texcoords.Count > 0)
            {
                writer.WriteLine();
                foreach (var uv in texcoords)
                {
                    string u = uv.Length > 0 ? uv[0].ToString("F6") : "0";
                    string v = uv.Length > 1 ? uv[1].ToString("F6") : "0";
                    writer.WriteLine($"vt {u} {v}");
                }
            }

            // Faces (OBJ is 1-based)
            if (faces.Count > 0)
            {
                writer.WriteLine();
                bool hasN = normals.Count  == positions.Count;
                bool hasT = texcoords.Count == positions.Count;

                foreach (var f in faces)
                {
                    int a = f[0] + 1, b = f[1] + 1, c = f[2] + 1;
                    // Ensure indices are within range
                    if (a > positions.Count || b > positions.Count || c > positions.Count)
                        continue;

                    string va = FaceVertex(a, hasT, hasN);
                    string vb = FaceVertex(b, hasT, hasN);
                    string vc = FaceVertex(c, hasT, hasN);
                    writer.WriteLine($"f {va} {vb} {vc}");
                }
            }
        }

        private static string FaceVertex(int idx, bool hasT, bool hasN)
        {
            if (hasT && hasN)  return $"{idx}/{idx}/{idx}";
            if (!hasT && hasN) return $"{idx}//{idx}";
            if (hasT && !hasN) return $"{idx}/{idx}";
            return idx.ToString();
        }

        // ──────────────────────────────────────────────────────────────────────
        // Format helpers
        // ──────────────────────────────────────────────────────────────────────

        private static int GetFormatByteSize(uint vkFormat) => vkFormat switch
        {
            100 => 4,   // R32_SFLOAT
            103 => 8,   // R32G32_SFLOAT
            106 => 12,  // R32G32B32_SFLOAT
            109 => 16,  // R32G32B32A32_SFLOAT
             37 => 4,   // R8G8B8A8_UNORM
             38 => 4,   // R8G8B8A8_SNORM
             43 => 4,   // R8G8B8A8_UINT
             83 => 4,   // R16G16_SFLOAT
             84 => 6,   // R16G16B16_SFLOAT
             97 => 8,   // R16G16B16A16_SFLOAT
            _   => 0
        };

        private static string GetFormatName(uint vkFormat) => vkFormat switch
        {
             37 => "R8G8B8A8_UNORM",
             38 => "R8G8B8A8_SNORM",
             43 => "R8G8B8A8_UINT",
             83 => "R16G16_SFLOAT",
             84 => "R16G16B16_SFLOAT",
             97 => "R16G16B16A16_SFLOAT",
            100 => "R32_SFLOAT",
            103 => "R32G32_SFLOAT",
            106 => "R32G32B32_SFLOAT",
            109 => "R32G32B32A32_SFLOAT",
            _   => $"VkFormat({vkFormat})"
        };

        // ──────────────────────────────────────────────────────────────────────
        // Utility
        // ──────────────────────────────────────────────────────────────────────

        private static bool TableExists(SQLiteConnection conn, string tableName)
        {
            using var cmd = new SQLiteCommand(
                $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'", conn);
            return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
        }

        // ──────────────────────────────────────────────────────────────────────
        // Internal DTOs
        // ──────────────────────────────────────────────────────────────────────

        private class VertexBindingDesc
        {
            public uint Binding   { get; set; }
            public uint Stride    { get; set; }
            public uint InputRate { get; set; }
        }

        private class VertexAttrDesc
        {
            public uint Location { get; set; }
            public uint Binding  { get; set; }
            public uint Format   { get; set; }
            public uint Offset   { get; set; }
        }
    }
}
