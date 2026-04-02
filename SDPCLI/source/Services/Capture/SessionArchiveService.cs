using System;
using System.IO;
using System.IO.Compression;

namespace SnapdragonProfilerCLI.Services.Capture
{
    /// <summary>
    /// Creates a .sdp ZIP archive from a session directory.
    /// Extracted from Application.CreateSessionArchive.
    /// </summary>
    public class SessionArchiveService
    {

        /// <summary>
        /// 将 <paramref name="captureDir"/> 中的所有文件打包成同名 .sdp ZIP。
        /// captureDir 由调用方在 capture 前创建并填充，已保证每次独立。
        /// </summary>
        public void CreateSessionArchive(string captureDir)
        {
            Console.WriteLine("\n=== Creating Session Archive ===");

            try
            {
                string normalizedDir = captureDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string sdpPath = normalizedDir + ".sdp";

                if (File.Exists(sdpPath)) File.Delete(sdpPath);

                if (!File.Exists(Path.Combine(captureDir, "sdp.db")))
                    Console.WriteLine("⚠  WARNING: sdp.db not found");
                if (!File.Exists(Path.Combine(captureDir, "version.txt")))
                    Console.WriteLine("⚠  WARNING: version.txt not found");

                string[] files = Directory.GetFiles(captureDir, "*", SearchOption.AllDirectories);
                Console.WriteLine($"Creating archive ({files.Length} files): {sdpPath}");

                using (var zip = ZipFile.Open(sdpPath, ZipArchiveMode.Create))
                {
                    foreach (string filePath in files)
                    {
                        string rel = filePath.Substring(captureDir.Length)
                            .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        try
                        {
                            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            using (var bw = new BinaryWriter(zip.CreateEntry(rel).Open()))
                            {
                                byte[] buf = new byte[81920];
                                int n;
                                while ((n = fs.Read(buf, 0, buf.Length)) > 0)
                                    bw.Write(buf, 0, n);
                            }
                            Console.WriteLine($"  {rel} ({new FileInfo(filePath).Length / 1024.0:F1} KB)");
                        }
                        catch (IOException ex) { Console.WriteLine($"  ⚠  Error adding {rel}: {ex.Message}"); }
                    }
                }

                Console.WriteLine($"\n✓ Archive: {sdpPath}  ({new FileInfo(sdpPath).Length / 1024.0:F2} KB)");
                Console.WriteLine($"  Folder:  {captureDir}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠  ERROR creating archive: {ex.Message}");
                Console.WriteLine($"  Folder: {captureDir}");
            }
        }
    }
}
