using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// 数据库查询服务 - 封装对 sdp.db 的所有查询操作
    /// </summary>
    public class DatabaseQueryService
    {
        private readonly ILogger logger;
        private string? dbPath;

        public DatabaseQueryService(ILogger logger)
        {
            this.logger = logger;
        }
        
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void OpenDatabase(string databasePath)
        {
            this.dbPath = databasePath;
            
            // 验证数据库可访问性
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    conn.Open();
                    logger.Debug($"Database opened: {databasePath}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to open database: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 获取所有 DrawCall ID 列表。
        /// 优先级: DrawCallParameters (CSV导入) > SCOPEDrawStages (编码格式) > pipeline数量推算
        /// </summary>
        /// <param name="cmdBufferFilter">When non-null and the CmdBufferIdx column exists,
        /// only return DrawCalls from that CommandBuffer (1-based, matches GUI numbering).
        /// Pass null or 0 to return all.</param>
        public List<string> GetDrawCallIds(uint captureId, int? cmdBufferFilter = null)
        {
            if (string.IsNullOrEmpty(dbPath))
                throw new InvalidOperationException("Database not opened. Call OpenDatabase first.");

            var drawCallNumbers = new List<string>();

            try
            {
                using var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conn.Open();

                // Priority 1: DrawCallParameters (imported from CSV -- plain numeric ApiIDs)
                if (TableExists(conn, "DrawCallParameters"))
                {
                    drawCallNumbers = GetDrawCallIdsFromParameters(conn, captureId, cmdBufferFilter);
                    if (drawCallNumbers.Count > 0)
                    {
                        logger.Debug($"Got {drawCallNumbers.Count} DrawCalls from DrawCallParameters" +
                            (cmdBufferFilter > 0 ? $" (CmdBuffer={cmdBufferFilter})" : ""));
                        return drawCallNumbers;
                    }
                }

                // Priority 2: SCOPEDrawStages (encoded "submit.cmdbuf.drawcall" format)
                if (TableExists(conn, "SCOPEDrawStages"))
                {
                    drawCallNumbers = GetDrawCallIdsFromSCOPE(conn, captureId);
                    if (drawCallNumbers.Count > 0)
                    {
                        logger.Debug($"Got {drawCallNumbers.Count} DrawCalls from SCOPEDrawStages");
                        return drawCallNumbers;
                    }
                }

                // Priority 3: generate from pipeline count
                drawCallNumbers = GenerateDrawCallIdsFromPipelines(conn, captureId);
                logger.Debug($"Generated {drawCallNumbers.Count} DrawCalls from pipeline count");
            }
            catch (Exception ex)
            {
                logger.Warning($"Could not query DrawCalls: {ex.Message}");
            }

            return drawCallNumbers;
        }
        
        /// <summary>
        /// 查询表是否存在
        /// </summary>
        public bool TableExists(string tableName)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                throw new InvalidOperationException("Database not opened. Call OpenDatabase first.");
            }
            
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                return TableExists(conn, tableName);
            }
        }
        
        /// <summary>
        /// 获取数据库元数据
        /// </summary>
        public DatabaseMetadata GetMetadata()
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                throw new InvalidOperationException("Database not opened. Call OpenDatabase first.");
            }
            
            var metadata = new DatabaseMetadata();
            
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Get all table names
                    metadata.TableNames = GetAllTables(conn);
                    
                    // TODO: Get capture count, record counts per table
                    metadata.CaptureCount = 1; // Placeholder
                    metadata.RecordCounts = new Dictionary<string, int>();
                }
            }
            catch (Exception ex)
            {
                logger.Warning($"Failed to get metadata: {ex.Message}");
            }
            
            return metadata;
        }
        
        // ========== 私有辅助方法 ==========
        
        private bool TableExists(SQLiteConnection conn, string tableName)
        {
            string query = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@tableName";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@tableName", tableName);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Read DrawCallApiIDs from DrawCallParameters (imported from CSV).
        /// Returns them as plain numeric strings, e.g. ["106974", "106981", ...].
        /// Filters by CaptureID when the column exists (new-format CSVs after the multi-capture fix).
        /// </summary>
        private List<string> GetDrawCallIdsFromParameters(SQLiteConnection conn, uint captureId, int? cmdBufferFilter = null)
        {
            var ids = new List<string>();

            // Detect which columns are present (schema may vary between legacy and new SDPs)
            bool hasCaptureIdCol  = false;
            bool hasCmdBufferCol  = false;
            using (var probe = new SQLiteCommand("PRAGMA table_info(DrawCallParameters)", conn))
            using (var pr = probe.ExecuteReader())
            {
                while (pr.Read())
                {
                    string col = pr["name"].ToString() ?? "";
                    if (col == "CaptureID")   hasCaptureIdCol = true;
                    if (col == "CmdBufferIdx") hasCmdBufferCol = true;
                }
            }

            var conditions = new List<string>();
            if (hasCaptureIdCol)                          conditions.Add($"CaptureID={captureId}");
            if (cmdBufferFilter > 0 && hasCmdBufferCol)   conditions.Add($"CmdBufferIdx={cmdBufferFilter}");

            string where = conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : "";
            string sql   = $"SELECT DrawCallApiID FROM DrawCallParameters{where} ORDER BY rowid";

            using var cmd = new SQLiteCommand(sql, conn);
            using var r   = cmd.ExecuteReader();
            while (r.Read())
                ids.Add(r[0].ToString() ?? "");
            return ids;
        }
        
        private List<string> GetDrawCallIdsFromSCOPE(SQLiteConnection conn, uint captureId)
        {
            var drawCallNumbers = new List<string>();
            
            string query = $@"
                SELECT DISTINCT drawCallID 
                FROM SCOPEDrawStages 
                WHERE captureID = {captureId}
                ORDER BY drawCallID";
            
            using (var cmd = new SQLiteCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ulong drawCallID = Convert.ToUInt64(reader[0]);
                    string drawCallStr = DecodeDrawCallId(drawCallID);
                    if (!string.IsNullOrEmpty(drawCallStr))
                    {
                        drawCallNumbers.Add(drawCallStr);
                    }
                }
            }
            
            return drawCallNumbers;
        }
        
        private List<string> GenerateDrawCallIdsFromPipelines(SQLiteConnection conn, uint captureId)
        {
            var drawCallNumbers = new List<string>();
            
            string countQuery = $@"
                SELECT COUNT(*) 
                FROM VulkanSnapshotGraphicsPipelines 
                WHERE captureID = {captureId}";
            
            using (var cmd = new SQLiteCommand(countQuery, conn))
            {
                int pipelineCount = Convert.ToInt32(cmd.ExecuteScalar());
                
                // Generate simple sequential DrawCall numbers
                for (int i = 1; i <= Math.Min(pipelineCount, 100); i++)
                {
                    drawCallNumbers.Add(i.ToString());
                }
            }
            
            return drawCallNumbers;
        }
        
        private string DecodeDrawCallId(ulong drawCallID)
        {
            uint submitIdx = (uint)(drawCallID >> 48) & 0xFFFF;
            uint cmdBufferIdx = (uint)(drawCallID >> 32) & 0xFFFF;
            uint drawcallIdx = (uint)(drawCallID & 0xFFFFFFFF);
            
            if (drawcallIdx == 0)
            {
                return ""; // Not a DrawCall
            }
            
            if (cmdBufferIdx == 0)
            {
                return $"{submitIdx}.{drawcallIdx}";
            }
            else
            {
                return $"{submitIdx}.{cmdBufferIdx}.{drawcallIdx}";
            }
        }
        
        private List<string> GetAllTables(SQLiteConnection conn)
        {
            var tables = new List<string>();
            
            string query = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name";
            using (var cmd = new SQLiteCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
            }
            
            return tables;
        }
    }
    
    /// <summary>
    /// 数据库元数据 DTO
    /// </summary>
    public class DatabaseMetadata
    {
        public int CaptureCount { get; set; }
        public List<string> TableNames { get; set; } = new List<string>();
        public Dictionary<string, int> RecordCounts { get; set; } = new Dictionary<string, int>();
    }
}
