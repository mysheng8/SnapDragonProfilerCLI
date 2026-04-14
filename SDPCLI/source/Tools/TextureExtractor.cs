using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using SnapdragonProfilerCLI.Data;
using SnapdragonProfilerCLI.Logging;
using TextureConverter;

namespace SnapdragonProfilerCLI.Tools
{
    /// <summary>
    /// Texture 提取工具
    /// 直接从 sdp.db 的 VulkanSnapshotByteBuffers 表读取纹理数据并保存为图片
    /// </summary>
    public class TextureExtractor
    {
        private readonly SdpDatabase _db;

        /// <summary>Primary constructor — inject SdpDatabase instance.</summary>
        public TextureExtractor(SdpDatabase db)
        {
            _db = db;
        }

        /// <summary>Backward-compatible constructor (creates its own SdpDatabase).</summary>
        public TextureExtractor(string databasePath, int captureId = 3)
            : this(new SdpDatabase(databasePath, (uint)captureId)) { }

        /// <summary>
        /// 提取 texture 并保存为 PNG
        /// 使用 SDPClientFramework 的 ByteBufferGateway 和 TextureConverterHelper
        /// </summary>
        public bool ExtractTexture(ulong resourceId, string outputPath)
        {
            try
            {
                AppLogger.Info("Texture", $"=== Extracting Texture {resourceId} ===");

                // 1. 查询 texture 元数据
                var metadata = _db.GetTextureMetadata(resourceId);
                if (metadata == null)
                {
                    AppLogger.Warn("Texture", $"Texture {resourceId} not found in database");
                    return false;
                }

                AppLogger.Debug("Texture", $"Size: {metadata.Width}x{metadata.Height}" +
                    (metadata.Depth > 1 ? $"x{metadata.Depth} (3D texture, extracting first slice)" : ""));
                AppLogger.Debug("Texture", $"Format: {metadata.Format} ({GetFormatName(metadata.Format)})");
                AppLogger.Debug("Texture", $"Layers: {metadata.LayerCount}, Levels: {metadata.LevelCount}");

                // Skip textures with invalid dimensions
                if (metadata.Width <= 0 || metadata.Height <= 0)
                {
                    AppLogger.Warn("Texture", $"Skipping: width or height is 0");
                    return false;
                }

                // 2. 直接从数据库读取 VulkanSnapshotByteBuffers 表获取纹理二进制数据
                byte[]? textureData = _db.ReadTextureBytes(resourceId);
                if (textureData == null || textureData.Length == 0)
                {
                    AppLogger.Warn("Texture", $"No texture data found in VulkanSnapshotByteBuffers");
                    return false;
                }

                AppLogger.Debug("Texture", $"Data size: {textureData.Length} bytes");

                // For 3D textures, only extract the first slice.
                // Slice size = width * height * bytesPerPixel (uncompressed) or tile-aligned.
                int extractWidth  = metadata.Width;
                int extractHeight = metadata.Height;
                if (metadata.Depth > 1)
                {
                    // Compute bytes-per-slice and take only the first slice
                    int bpp = GetBytesPerPixel(metadata.Format);
                    if (bpp > 0)
                    {
                        int sliceBytes = metadata.Width * metadata.Height * bpp;
                        if (sliceBytes > 0 && sliceBytes <= textureData.Length)
                        {
                            byte[] slice = new byte[sliceBytes];
                            Array.Copy(textureData, 0, slice, 0, sliceBytes);
                            textureData = slice;
                            AppLogger.Debug("Texture", $"3D slice: using first {sliceBytes} bytes of {metadata.Depth} slices");
                        }
                    }
                }

                // 3. 将 VkFormat 映射到 TFormats
                TextureConverter.TFormats tFormat = VkFormatToTFormat(metadata.Format);
                AppLogger.Debug("Texture", $"TFormat: {tFormat}");

                // 4. 构建输入数据（ASTC 格式需添加文件头）
                byte[] inputData = textureData;
                if (IsAstcFormat(metadata.Format))
                {
                    (byte xBlocks, byte yBlocks) = GetAstcBlockSize(metadata.Format);
                    AppLogger.Debug("Texture", $"ASTC block size: {xBlocks}x{yBlocks}");
                    TextureConverterHelper.AddAstcHeader(
                        out inputData, textureData, xBlocks, yBlocks,
                        (uint)extractWidth, (uint)extractHeight);
                    AppLogger.Debug("Texture", $"Added ASTC header: {inputData.Length} bytes total");
                }

                // 5. 使用 TextureConverterHelper 转换为 RGBA
                byte[] rgbaData = TextureConverterHelper.ConvertImageToRGBA(
                    inputData,
                    tFormat,
                    (uint)extractWidth,
                    (uint)extractHeight,
                    true,  // flipBR: 转换为 BGRA (适合 Bitmap)
                    0U     // rowStride: 自动计算
                );

                if (rgbaData == null)
                {
                    AppLogger.Warn("Texture", $"Failed to convert texture format {metadata.Format} to RGBA");
                    AppLogger.Debug("Texture", $"TFormat mapping: {tFormat}");
                    return false;
                }

                AppLogger.Info("Texture", $"Converted to RGBA: {rgbaData.Length} bytes");

                // 5. 创建 Bitmap 并保存为 PNG
                Bitmap bitmap = new Bitmap(extractWidth, extractHeight, PixelFormat.Format32bppArgb);
                BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, extractWidth, extractHeight),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);

                Marshal.Copy(rgbaData, 0, bmpData.Scan0, rgbaData.Length);
                bitmap.UnlockBits(bmpData);

                string finalPath = outputPath;
                if (!outputPath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    finalPath += ".png";
                }

                bitmap.Save(finalPath, ImageFormat.Png);
                bitmap.Dispose();

                AppLogger.Info("Texture", $"Saved to: {finalPath}");
                return true;
            }
            catch (Exception ex)
            {
                AppLogger.Exception("Texture", ex, "Error extracting texture");
                return false;
            }
        }

        private static bool IsAstcFormat(int vkFormat)
        {
            // VK_FORMAT_ASTC_4x4_UNORM_BLOCK(157) to VK_FORMAT_ASTC_12x12_SRGB_BLOCK(184)
            return vkFormat >= 157 && vkFormat <= 184;
        }

        /// <summary>
        /// Returns the ASTC block dimensions (xBlocks, yBlocks) for a given Vulkan ASTC format.
        /// Vulkan ASTC formats start at 157 (ASTC_4x4_UNORM) and increase by 2 per block size.
        /// </summary>
        private (byte x, byte y) GetAstcBlockSize(int vkFormat)
        {
            // Pairs of (xBlocks, yBlocks) corresponding to VK_FORMAT_ASTC_*x*_UNORM/SRGB starting at 157
            (byte, byte)[] sizes = {
                (4, 4),   // 157 & 158: ASTC 4x4
                (5, 4),   // 159 & 160: ASTC 5x4
                (5, 5),   // 161 & 162: ASTC 5x5
                (6, 5),   // 163 & 164: ASTC 6x5
                (6, 6),   // 165 & 166: ASTC 6x6
                (8, 5),   // 167 & 168: ASTC 8x5
                (8, 6),   // 169 & 170: ASTC 8x6
                (8, 8),   // 171 & 172: ASTC 8x8
                (10, 5),  // 173 & 174: ASTC 10x5
                (10, 6),  // 175 & 176: ASTC 10x6
                (10, 8),  // 177 & 178: ASTC 10x8
                (10, 10), // 179 & 180: ASTC 10x10
                (12, 10), // 181 & 182: ASTC 12x10
                (12, 12), // 183 & 184: ASTC 12x12
            };
            int index = (vkFormat - 157) / 2;
            if (index >= 0 && index < sizes.Length)
                return sizes[index];
            return (4, 4); // fallback
        }

        private static int GetBytesPerPixel(int vkFormat) => vkFormat switch
        {
            37  => 4,   // R8G8B8A8_UNORM
            43  => 4,   // B8G8R8A8_UNORM
            97  => 8,   // R16G16B16A16_SFLOAT
            100 => 4,   // R32_SFLOAT
            103 => 8,   // R32G32_SFLOAT
            106 => 12,  // R32G32B32_SFLOAT
            109 => 16,  // R32G32B32A32_SFLOAT
            _   => 0    // unknown/compressed — skip slice trimming
        };

        private TextureConverter.TFormats VkFormatToTFormat(int vkFormat)
        {
            // ASTC formats: 157 (ASTC_4x4_UNORM) to 184 (ASTC_12x12_SRGB)
            if (vkFormat >= 157 && vkFormat <= 184)
                return TextureConverter.TFormats.Q_FORMAT_ASTC_8;

            return vkFormat switch
            {
                // 8-bit 格式
                37 => TextureConverter.TFormats.Q_FORMAT_RGBA_8UI,     // VK_FORMAT_R8G8B8A8_UNORM
                43 => TextureConverter.TFormats.Q_FORMAT_BGRA_8888,    // VK_FORMAT_B8G8R8A8_UNORM

                // 半浮点格式 (16-bit float)
                97 => TextureConverter.TFormats.Q_FORMAT_RGBA_HF,      // VK_FORMAT_R16G16B16A16_SFLOAT

                // 浮点格式 (32-bit float)
                103 => TextureConverter.TFormats.Q_FORMAT_RG_F,        // VK_FORMAT_R32G32_SFLOAT
                106 => TextureConverter.TFormats.Q_FORMAT_RGB_F,       // VK_FORMAT_R32G32B32_SFLOAT
                109 => TextureConverter.TFormats.Q_FORMAT_RGBA_F,      // VK_FORMAT_R32G32B32A32_SFLOAT

                // 默认使用 RGBA8
                _ => TextureConverter.TFormats.Q_FORMAT_RGBA_8UI
            };
        }

        private string GetFormatName(int format)
        {
            return format switch
            {
                37 => "R8G8B8A8_UNORM",
                43 => "B8G8R8A8_UNORM",
                97 => "R16G16B16A16_SFLOAT",
                109 => "R32G32B32A32_SFLOAT",
                183 => "ASTC_4x4_UNORM_BLOCK",
                _ => $"Format{format}"
            };
        }

    }
}
