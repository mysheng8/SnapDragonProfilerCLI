using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SnapdragonProfilerCLI.Modes;

namespace SnapdragonProfilerCLI.Services.Analysis
{
    /// <summary>
    /// .sdp 文件管理服务 - 负责文件扫描、解压、验证
    /// </summary>
    public class SdpFileService
    {
        private readonly Config config;
        private readonly ILogger logger;
        
        public SdpFileService(Config config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }
        
        /// <summary>
        /// 扫描指定目录下的所有 .sdp 文件
        /// </summary>
        /// <param name="directory">扫描目录</param>
        /// <returns>按时间倒序排列的 .sdp 文件列表</returns>
        public List<SdpFileInfo> ScanSdpFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                logger.Warning($"Directory does not exist: {directory}");
                return new List<SdpFileInfo>();
            }
            
            logger.Info($"Scanning directory: {directory}");
            
            var sdpFiles = Directory.GetFiles(directory, "*.sdp", SearchOption.AllDirectories)
                .Where(f => new FileInfo(f).Length > 0)   // skip 0-byte stubs
                .OrderByDescending(f => new FileInfo(f).LastWriteTime)
                .Select(filePath =>
                {
                    var fileInfo = new FileInfo(filePath);
                    string relPath = filePath;
                    if (filePath.StartsWith(directory))
                    {
                        relPath = filePath.Substring(directory.Length)
                            .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    }
                    
                    return new SdpFileInfo
                    {
                        FilePath = filePath,
                        SizeBytes = fileInfo.Length,
                        LastModified = fileInfo.LastWriteTime,
                        RelativePath = relPath
                    };
                })
                .ToList();
            
            logger.Info($"Found {sdpFiles.Count} .sdp file(s)");
            return sdpFiles;
        }
        
        /// <summary>
        /// 从 .sdp 文件或目录中查找数据库文件
        /// </summary>
        /// <param name="sdpPath">.sdp 文件路径或目录</param>
        /// <returns>sdp.db 的完整路径，未找到则返回 null</returns>
        public string? FindDatabasePath(string sdpPath)
        {
            // Case 1: .sdp is actually a directory (already extracted)
            if (Directory.Exists(sdpPath))
            {
                string dbInDir = Path.Combine(sdpPath, "sdp.db");
                if (File.Exists(dbInDir))
                {
                    logger.Info($"Found database in directory: {dbInDir}");
                    return dbInDir;
                }
            }
            
            // Case 1b: .sdp is a file — check if sdp.db lives in the same directory as the file
            if (File.Exists(sdpPath))
            {
                string sameDir = Path.GetDirectoryName(sdpPath) ?? "";
                string dbInSameDir = Path.Combine(sameDir, "sdp.db");
                if (File.Exists(dbInSameDir))
                {
                    logger.Info($"Found database in same directory as .sdp: {dbInSameDir}");
                    return dbInSameDir;
                }
            }

            // Case 2: Directory with same name as .sdp file exists
            string sdpDir = Path.GetFileNameWithoutExtension(sdpPath);
            string parentDir = Path.GetDirectoryName(sdpPath) ?? "";
            string adjacentDir = Path.Combine(parentDir, sdpDir);
            
            if (Directory.Exists(adjacentDir))
            {
                string dbInAdjacentDir = Path.Combine(adjacentDir, "sdp.db");
                if (File.Exists(dbInAdjacentDir))
                {
                    logger.Info($"Found database in adjacent directory: {dbInAdjacentDir}");
                    return dbInAdjacentDir;
                }
            }
            
            // Case 3: .sdp is compressed file, need to extract
            if (File.Exists(sdpPath))
            {
                logger.Info("Attempting to extract .sdp file...");
                string? extractedDb = ExtractSdpFile(sdpPath);
                if (extractedDb != null)
                {
                    return extractedDb;
                }
            }
            
            logger.Warning("Database file sdp.db not found in any location");
            return null;
        }
        
        /// <summary>
        /// 解压 .sdp 文件到临时目录
        /// </summary>
        /// <param name="sdpFilePath">.sdp 文件路径</param>
        /// <returns>解压后的 sdp.db 路径，失败则返回 null</returns>
        public string? ExtractSdpFile(string sdpFilePath)
        {
            try
            {
                string extractDir = Path.Combine(Path.GetTempPath(), $"sdp_analysis_{Guid.NewGuid()}");
                Directory.CreateDirectory(extractDir);
                
                logger.Debug($"Extracting to: {extractDir}");
                ZipFile.ExtractToDirectory(sdpFilePath, extractDir);
                
                string dbInExtract = Path.Combine(extractDir, "sdp.db");
                if (File.Exists(dbInExtract))
                {
                    logger.Success("Extraction successful");
                    return dbInExtract;
                }
                else
                {
                    logger.Warning("Extracted, but sdp.db not found in archive");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Warning($"Extraction failed: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 验证 .sdp 文件完整性
        /// </summary>
        /// <param name="sdpPath">.sdp 文件路径或目录</param>
        /// <returns>true 如果文件有效</returns>
        public bool ValidateSdpFile(string sdpPath)
        {
            string? dbPath = FindDatabasePath(sdpPath);
            
            if (string.IsNullOrEmpty(dbPath) || !File.Exists(dbPath))
            {
                logger.Error("Validation failed: Database file not found");
                return false;
            }
            
            // TODO: 进一步验证数据库完整性
            // - 检查数据库是否可打开
            // - 检查必要的表是否存在
            
            logger.Success("Validation passed");
            return true;
        }
        
        /// <summary>
        /// 解析路径（相对路径 → 绝对路径）
        /// </summary>
        /// <param name="path">输入路径</param>
        /// <param name="basePath">基准路径（用于解析相对路径）</param>
        /// <returns>绝对路径</returns>
        public string ResolvePath(string path, string basePath)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            
            // Relative path: combine with base path
            return Path.GetFullPath(Path.Combine(basePath, path));
        }
    }
    
    /// <summary>
    /// .sdp 文件信息 DTO
    /// </summary>
    public class SdpFileInfo
    {
        public string FilePath { get; set; } = "";
        public long SizeBytes { get; set; }
        public DateTime LastModified { get; set; }
        public string RelativePath { get; set; } = "";
        
        public double SizeMB => SizeBytes / 1024.0 / 1024.0;
    }
}
