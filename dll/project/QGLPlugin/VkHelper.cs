using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Gdk;
using Newtonsoft.Json.Linq;
using Sdp;
using Sdp.Helpers;
using Sdp.Logging;
using TextureConverter;

namespace QGLPlugin
{
	// Token: 0x02000024 RID: 36
	public class VkHelper
	{
		// Token: 0x0600001D RID: 29 RVA: 0x0000352C File Offset: 0x0000172C
		public static string GetTextureFormatString(string format)
		{
			string text;
			if (VkHelper.m_formatStrings.TryGetValue(format, out text))
			{
				return text;
			}
			return "Unknown format (" + format + ")";
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000355A File Offset: 0x0000175A
		public static string ConcatenateSlashAndStringIfNotEmpty(string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				return "/" + str;
			}
			return "";
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003578 File Offset: 0x00001778
		static VkHelper()
		{
			int length = "VK_FORMAT_".Length;
			Array values = Enum.GetValues(typeof(VkFormats));
			foreach (object obj in values)
			{
				VkFormats vkFormats = (VkFormats)obj;
				Dictionary<string, string> formatStrings = VkHelper.m_formatStrings;
				uint num = (uint)vkFormats;
				formatStrings[num.ToString()] = vkFormats.ToString().Substring(length);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003624 File Offset: 0x00001824
		public static byte[] GenerateThumbnail(byte[] data, VkFormats format, uint imgWidth, uint imgHeight, uint thumbnailWidth, uint thumbnailHeight)
		{
			byte[] array = null;
			Pixbuf pixbuf = null;
			Pixbuf pixbuf2 = null;
			Pixbuf pixbuf3 = null;
			if (data != null && data.Length != 0)
			{
				try
				{
					byte[] array2 = null;
					if (format >= VkFormats.VK_FORMAT_ASTC_4x4_UNORM_BLOCK && format <= VkFormats.VK_FORMAT_ASTC_12x12_SRGB_BLOCK)
					{
						VkHelper.AddASTCHeader(out array2, data, format, imgWidth, imgHeight);
					}
					else
					{
						array2 = data;
					}
					byte[] array3 = TextureConverterHelper.ConvertImageToRGBA(array2, VkHelper.GetFormatInfo(format).format, imgWidth, imgHeight, false, 0U);
					pixbuf = new Pixbuf(array3, true, 8, (int)imgWidth, (int)imgHeight, (int)(imgWidth * 4U));
					double num = ((imgWidth > imgHeight) ? (thumbnailWidth / imgWidth) : (thumbnailHeight / imgHeight));
					InterpType interpType = ((num < 1.0) ? 2 : 0);
					byte[] array4 = new byte[thumbnailWidth * thumbnailHeight * 4U];
					pixbuf3 = new Pixbuf(array4, true, 8, (int)thumbnailWidth, (int)thumbnailHeight, (int)(thumbnailWidth * 4U));
					int num2 = Math.Max((int)(imgWidth * num), 1);
					int num3 = Math.Max((int)(imgHeight * num), 1);
					pixbuf2 = pixbuf.ScaleSimple(num2, num3, interpType);
					int num4 = (int)((thumbnailWidth - (uint)num2) / 2U);
					int num5 = (int)((thumbnailHeight - (uint)num3) / 2U);
					pixbuf2.CopyArea(0, 0, pixbuf2.Width, pixbuf2.Height, pixbuf3, num4, num5);
					int num6 = pixbuf3.Width * pixbuf3.Height * 4;
					array = new byte[pixbuf3.Width * pixbuf3.Height * 4];
					Marshal.Copy(pixbuf3.Pixels, array, 0, num6);
				}
				catch (Exception ex)
				{
					array = null;
					if (ex is OutOfMemoryException || ex is NullReferenceException)
					{
						GC.Collect();
					}
					else
					{
						if (!(ex is NotImplementedException))
						{
							throw;
						}
						VkHelper.Logger.LogError("Texture Format Not Yet Implemented");
					}
				}
				finally
				{
					if (pixbuf != null)
					{
						pixbuf.Dispose();
					}
					if (pixbuf2 != null)
					{
						pixbuf2.Dispose();
					}
					if (pixbuf3 != null)
					{
						pixbuf3.Dispose();
					}
				}
			}
			return array;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000380C File Offset: 0x00001A0C
		public static List<List<ImageViewObject>> GenerateImageObjects(byte[] data, VkFormats format, uint width, uint height, uint layerCount, uint levelCount, uint depth)
		{
			List<List<ImageViewObject>> list = new List<List<ImageViewObject>>();
			try
			{
				if (data != null && data.Length != 0)
				{
					VkHelper.VFormatInfo formatInfo = VkHelper.GetFormatInfo(format);
					uint num = 0U;
					int num2 = 0;
					int num3 = 0;
					uint num4 = 0U;
					int num5 = 0;
					while ((long)num5 < (long)((ulong)depth))
					{
						for (uint num6 = 0U; num6 < layerCount; num6 += 1U)
						{
							List<ImageViewObject> list2 = new List<ImageViewObject>();
							int num7 = 0;
							while ((long)num7 < (long)((ulong)levelCount))
							{
								uint num8 = Math.Max(width / (1U << num7), 1U);
								uint num9 = Math.Max(height / (1U << num7), 1U);
								uint num10 = formatInfo.ImageSize(num8, num9);
								if ((long)data.Length < (long)((ulong)num))
								{
									return list;
								}
								byte[] array = new byte[num10];
								Array.Copy(data, (long)((ulong)num), array, 0L, Math.Min((long)((ulong)num10), (long)data.Length - (long)((ulong)num)));
								byte[] array2 = null;
								if (format >= VkFormats.VK_FORMAT_ASTC_4x4_UNORM_BLOCK && format <= VkFormats.VK_FORMAT_ASTC_12x12_SRGB_BLOCK)
								{
									VkHelper.AddASTCHeader(out array2, array, format, num8, num9);
								}
								else
								{
									array2 = array;
								}
								byte[] array3 = TextureConverterHelper.ConvertImageToRGBA(array2, formatInfo.format, num8, num9, true, 0U);
								List<ImageViewObject> list3 = new List<ImageViewObject>();
								list2.Add(new ImageViewObject((int)num8, (int)num9, formatInfo.HasAlpha, num2, num3, num4, array3));
								num += num10;
								num7++;
							}
							list.Add(list2);
						}
						num5++;
					}
				}
			}
			catch (NotImplementedException)
			{
			}
			return list;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003984 File Offset: 0x00001B84
		private static void AddASTCHeader(out byte[] combined, byte[] input, VkFormats format, uint width, uint height)
		{
			byte b = 0;
			byte b2 = 0;
			switch (format)
			{
			case VkFormats.VK_FORMAT_ASTC_4x4_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_4x4_SRGB_BLOCK:
				b = 4;
				b2 = 4;
				break;
			case VkFormats.VK_FORMAT_ASTC_5x4_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_5x4_SRGB_BLOCK:
				b = 5;
				b2 = 4;
				break;
			case VkFormats.VK_FORMAT_ASTC_5x5_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_5x5_SRGB_BLOCK:
				b = 5;
				b2 = 5;
				break;
			case VkFormats.VK_FORMAT_ASTC_6x5_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_6x5_SRGB_BLOCK:
				b = 6;
				b2 = 5;
				break;
			case VkFormats.VK_FORMAT_ASTC_6x6_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_6x6_SRGB_BLOCK:
				b = 6;
				b2 = 6;
				break;
			case VkFormats.VK_FORMAT_ASTC_8x5_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_8x5_SRGB_BLOCK:
				b = 8;
				b2 = 5;
				break;
			case VkFormats.VK_FORMAT_ASTC_8x6_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_8x6_SRGB_BLOCK:
				b = 8;
				b2 = 6;
				break;
			case VkFormats.VK_FORMAT_ASTC_8x8_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_8x8_SRGB_BLOCK:
				b = 8;
				b2 = 8;
				break;
			case VkFormats.VK_FORMAT_ASTC_10x5_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_10x5_SRGB_BLOCK:
				b = 10;
				b2 = 5;
				break;
			case VkFormats.VK_FORMAT_ASTC_10x6_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_10x6_SRGB_BLOCK:
				b = 10;
				b2 = 6;
				break;
			case VkFormats.VK_FORMAT_ASTC_10x8_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_10x8_SRGB_BLOCK:
				b = 10;
				b2 = 8;
				break;
			case VkFormats.VK_FORMAT_ASTC_10x10_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_10x10_SRGB_BLOCK:
				b = 10;
				b2 = 10;
				break;
			case VkFormats.VK_FORMAT_ASTC_12x10_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_12x10_SRGB_BLOCK:
				b = 12;
				b2 = 10;
				break;
			case VkFormats.VK_FORMAT_ASTC_12x12_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_12x12_SRGB_BLOCK:
				b = 12;
				b2 = 12;
				break;
			}
			TextureConverterHelper.AddAstcHeader(out combined, input, b, b2, width, height);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003A7C File Offset: 0x00001C7C
		public static ImagePixelType GetImagePixelType(VkFormats format)
		{
			switch (format)
			{
			case VkFormats.VK_FORMAT_R8_UNORM:
			case VkFormats.VK_FORMAT_R8G8_UNORM:
			case VkFormats.VK_FORMAT_R8G8B8_UNORM:
			case VkFormats.VK_FORMAT_B8G8R8_UNORM:
			case VkFormats.VK_FORMAT_R8G8B8A8_UNORM:
			case VkFormats.VK_FORMAT_B8G8R8A8_UNORM:
			case VkFormats.VK_FORMAT_R16_UNORM:
			case VkFormats.VK_FORMAT_R16G16_UNORM:
			case VkFormats.VK_FORMAT_R16G16B16_UNORM:
			case VkFormats.VK_FORMAT_R16G16B16A16_UNORM:
			case VkFormats.VK_FORMAT_D16_UNORM:
				break;
			case VkFormats.VK_FORMAT_R8_SNORM:
			case VkFormats.VK_FORMAT_R8G8_SNORM:
			case VkFormats.VK_FORMAT_R8G8B8_SNORM:
			case VkFormats.VK_FORMAT_B8G8R8_SNORM:
			case VkFormats.VK_FORMAT_R8G8B8A8_SNORM:
			case VkFormats.VK_FORMAT_B8G8R8A8_SNORM:
			case VkFormats.VK_FORMAT_R16_SNORM:
			case VkFormats.VK_FORMAT_R16G16_SNORM:
			case VkFormats.VK_FORMAT_R16G16B16_SNORM:
			case VkFormats.VK_FORMAT_R16G16B16A16_SNORM:
				return ImagePixelType.SNORM;
			case VkFormats.VK_FORMAT_R8_USCALED:
			case VkFormats.VK_FORMAT_R8_UINT:
			case VkFormats.VK_FORMAT_R8G8_USCALED:
			case VkFormats.VK_FORMAT_R8G8_UINT:
			case VkFormats.VK_FORMAT_R8G8B8_USCALED:
			case VkFormats.VK_FORMAT_R8G8B8_UINT:
			case VkFormats.VK_FORMAT_B8G8R8_USCALED:
			case VkFormats.VK_FORMAT_B8G8R8_UINT:
			case VkFormats.VK_FORMAT_R8G8B8A8_USCALED:
			case VkFormats.VK_FORMAT_R8G8B8A8_UINT:
			case VkFormats.VK_FORMAT_B8G8R8A8_USCALED:
			case VkFormats.VK_FORMAT_B8G8R8A8_UINT:
			case VkFormats.VK_FORMAT_R16_USCALED:
			case VkFormats.VK_FORMAT_R16_UINT:
			case VkFormats.VK_FORMAT_R16G16_USCALED:
			case VkFormats.VK_FORMAT_R16G16_UINT:
			case VkFormats.VK_FORMAT_R16G16B16_USCALED:
			case VkFormats.VK_FORMAT_R16G16B16_UINT:
			case VkFormats.VK_FORMAT_R16G16B16A16_USCALED:
			case VkFormats.VK_FORMAT_R16G16B16A16_UINT:
			case VkFormats.VK_FORMAT_R32_UINT:
			case VkFormats.VK_FORMAT_R32G32_UINT:
			case VkFormats.VK_FORMAT_R32G32B32_UINT:
			case VkFormats.VK_FORMAT_R32G32B32A32_UINT:
			case VkFormats.VK_FORMAT_R64_UINT:
			case VkFormats.VK_FORMAT_R64G64_UINT:
			case VkFormats.VK_FORMAT_R64G64B64_UINT:
			case VkFormats.VK_FORMAT_R64G64B64A64_UINT:
			case VkFormats.VK_FORMAT_S8_UINT:
				return ImagePixelType.UINT;
			case VkFormats.VK_FORMAT_R8_SSCALED:
			case VkFormats.VK_FORMAT_R8_SINT:
			case VkFormats.VK_FORMAT_R8G8_SSCALED:
			case VkFormats.VK_FORMAT_R8G8_SINT:
			case VkFormats.VK_FORMAT_R8G8B8_SSCALED:
			case VkFormats.VK_FORMAT_R8G8B8_SINT:
			case VkFormats.VK_FORMAT_B8G8R8_SSCALED:
			case VkFormats.VK_FORMAT_B8G8R8_SINT:
			case VkFormats.VK_FORMAT_R8G8B8A8_SSCALED:
			case VkFormats.VK_FORMAT_R8G8B8A8_SINT:
			case VkFormats.VK_FORMAT_B8G8R8A8_SSCALED:
			case VkFormats.VK_FORMAT_B8G8R8A8_SINT:
			case VkFormats.VK_FORMAT_R16_SSCALED:
			case VkFormats.VK_FORMAT_R16_SINT:
			case VkFormats.VK_FORMAT_R16G16_SSCALED:
			case VkFormats.VK_FORMAT_R16G16_SINT:
			case VkFormats.VK_FORMAT_R16G16B16_SSCALED:
			case VkFormats.VK_FORMAT_R16G16B16_SINT:
			case VkFormats.VK_FORMAT_R16G16B16A16_SSCALED:
			case VkFormats.VK_FORMAT_R16G16B16A16_SINT:
			case VkFormats.VK_FORMAT_R32_SINT:
			case VkFormats.VK_FORMAT_R32G32_SINT:
			case VkFormats.VK_FORMAT_R32G32B32_SINT:
			case VkFormats.VK_FORMAT_R32G32B32A32_SINT:
			case VkFormats.VK_FORMAT_R64_SINT:
			case VkFormats.VK_FORMAT_R64G64_SINT:
			case VkFormats.VK_FORMAT_R64G64B64_SINT:
			case VkFormats.VK_FORMAT_R64G64B64A64_SINT:
				return ImagePixelType.INT;
			case VkFormats.VK_FORMAT_R8_SRGB:
			case VkFormats.VK_FORMAT_R8G8_SRGB:
			case VkFormats.VK_FORMAT_R8G8B8_SRGB:
			case VkFormats.VK_FORMAT_B8G8R8_SRGB:
			case VkFormats.VK_FORMAT_R8G8B8A8_SRGB:
			case VkFormats.VK_FORMAT_B8G8R8A8_SRGB:
				return ImagePixelType.sRGB;
			case VkFormats.VK_FORMAT_A8B8G8R8_UNORM_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_SNORM_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_USCALED_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_SSCALED_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_UINT_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_SINT_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_SRGB_PACK32:
			case VkFormats.VK_FORMAT_E5B9G9R9_UFLOAT_PACK32:
			case VkFormats.VK_FORMAT_X8_D24_UNORM_PACK32:
			case VkFormats.VK_FORMAT_D16_UNORM_S8_UINT:
				return ImagePixelType.Unknown;
			case VkFormats.VK_FORMAT_A2R10G10B10_UNORM_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_UNORM_PACK32:
				return ImagePixelType.R10G10B10A2_UNORM;
			case VkFormats.VK_FORMAT_A2R10G10B10_SNORM_PACK32:
			case VkFormats.VK_FORMAT_A2R10G10B10_SSCALED_PACK32:
			case VkFormats.VK_FORMAT_A2R10G10B10_SINT_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_SNORM_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_SSCALED_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_SINT_PACK32:
				return ImagePixelType.R10G10B10A2_INT;
			case VkFormats.VK_FORMAT_A2R10G10B10_USCALED_PACK32:
			case VkFormats.VK_FORMAT_A2R10G10B10_UINT_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_USCALED_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_UINT_PACK32:
				return ImagePixelType.R10G10B10A2_UINT;
			case VkFormats.VK_FORMAT_R16_SFLOAT:
			case VkFormats.VK_FORMAT_R16G16_SFLOAT:
			case VkFormats.VK_FORMAT_R16G16B16_SFLOAT:
			case VkFormats.VK_FORMAT_R16G16B16A16_SFLOAT:
			case VkFormats.VK_FORMAT_R32_SFLOAT:
			case VkFormats.VK_FORMAT_R32G32_SFLOAT:
			case VkFormats.VK_FORMAT_R32G32B32_SFLOAT:
			case VkFormats.VK_FORMAT_R32G32B32A32_SFLOAT:
			case VkFormats.VK_FORMAT_R64_SFLOAT:
			case VkFormats.VK_FORMAT_R64G64_SFLOAT:
			case VkFormats.VK_FORMAT_R64G64B64_SFLOAT:
			case VkFormats.VK_FORMAT_R64G64B64A64_SFLOAT:
			case VkFormats.VK_FORMAT_D32_SFLOAT:
			case VkFormats.VK_FORMAT_D32_SFLOAT_S8_UINT:
				return ImagePixelType.FLOAT;
			case VkFormats.VK_FORMAT_B10G11R11_UFLOAT_PACK32:
				return ImagePixelType.R11G11B10_FLOAT;
			case VkFormats.VK_FORMAT_D24_UNORM_S8_UINT:
				return ImagePixelType.D24S8;
			default:
				if (format != VkFormats.VK_FORMAT_A8_UNORM)
				{
					return ImagePixelType.Unknown;
				}
				break;
			}
			return ImagePixelType.UNORM;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003C9C File Offset: 0x00001E9C
		public static VkHelper.VFormatInfo GetFormatInfo(VkFormats format)
		{
			VkHelper.VFormatInfo vformatInfo = default(VkHelper.VFormatInfo);
			vformatInfo.Compressed = false;
			switch (format)
			{
			case VkFormats.VK_FORMAT_R4G4_UNORM_PACK8:
				throw new NotImplementedException();
			case VkFormats.VK_FORMAT_R4G4B4A4_UNORM_PACK16:
			case VkFormats.VK_FORMAT_B4G4R4A4_UNORM_PACK16:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_4444;
				vformatInfo.BPP = 2U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R5G6B5_UNORM_PACK16:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_BGR_565;
				vformatInfo.BPP = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_B5G6R5_UNORM_PACK16:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_BGR_565;
				vformatInfo.BPP = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R5G5B5A1_UNORM_PACK16:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB5_A1UI;
				vformatInfo.BPP = 2U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_B5G5R5A1_UNORM_PACK16:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB5_A1UI;
				vformatInfo.BPP = 2U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_A1R5G5B5_UNORM_PACK16:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB5_A1UI;
				vformatInfo.BPP = 2U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R8_UNORM:
			case VkFormats.VK_FORMAT_R8_USCALED:
			case VkFormats.VK_FORMAT_R8_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_R_8UI;
				vformatInfo.BPP = 1U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R8_SNORM:
			case VkFormats.VK_FORMAT_R8_SSCALED:
			case VkFormats.VK_FORMAT_R8_SINT:
			case VkFormats.VK_FORMAT_R8_SRGB:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_R_8I;
				vformatInfo.BPP = 1U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R8G8_UNORM:
			case VkFormats.VK_FORMAT_R8G8_USCALED:
			case VkFormats.VK_FORMAT_R8G8_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RG_8UI;
				vformatInfo.BPP = 2U;
				vformatInfo.NumberChannels = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R8G8_SNORM:
			case VkFormats.VK_FORMAT_R8G8_SSCALED:
			case VkFormats.VK_FORMAT_R8G8_SINT:
			case VkFormats.VK_FORMAT_R8G8_SRGB:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RG_8I;
				vformatInfo.BPP = 2U;
				vformatInfo.NumberChannels = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R8G8B8_UNORM:
			case VkFormats.VK_FORMAT_R8G8B8_USCALED:
			case VkFormats.VK_FORMAT_R8G8B8_UINT:
			case VkFormats.VK_FORMAT_B8G8R8_UNORM:
			case VkFormats.VK_FORMAT_B8G8R8_USCALED:
			case VkFormats.VK_FORMAT_B8G8R8_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB_8UI;
				vformatInfo.BPP = 3U;
				vformatInfo.NumberChannels = 3U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R8G8B8_SNORM:
			case VkFormats.VK_FORMAT_R8G8B8_SSCALED:
			case VkFormats.VK_FORMAT_R8G8B8_SINT:
			case VkFormats.VK_FORMAT_R8G8B8_SRGB:
			case VkFormats.VK_FORMAT_B8G8R8_SNORM:
			case VkFormats.VK_FORMAT_B8G8R8_SSCALED:
			case VkFormats.VK_FORMAT_B8G8R8_SINT:
			case VkFormats.VK_FORMAT_B8G8R8_SRGB:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB_8I;
				vformatInfo.BPP = 3U;
				vformatInfo.NumberChannels = 3U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R8G8B8A8_UNORM:
			case VkFormats.VK_FORMAT_R8G8B8A8_USCALED:
			case VkFormats.VK_FORMAT_R8G8B8A8_UINT:
			case VkFormats.VK_FORMAT_B8G8R8A8_UNORM:
			case VkFormats.VK_FORMAT_B8G8R8A8_USCALED:
			case VkFormats.VK_FORMAT_B8G8R8A8_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_8UI;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R8G8B8A8_SNORM:
			case VkFormats.VK_FORMAT_R8G8B8A8_SSCALED:
			case VkFormats.VK_FORMAT_R8G8B8A8_SINT:
			case VkFormats.VK_FORMAT_R8G8B8A8_SRGB:
			case VkFormats.VK_FORMAT_B8G8R8A8_SNORM:
			case VkFormats.VK_FORMAT_B8G8R8A8_SSCALED:
			case VkFormats.VK_FORMAT_B8G8R8A8_SINT:
			case VkFormats.VK_FORMAT_B8G8R8A8_SRGB:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_8I;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_A8B8G8R8_UNORM_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_SNORM_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_USCALED_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_SSCALED_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_UINT_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_SINT_PACK32:
			case VkFormats.VK_FORMAT_A8B8G8R8_SRGB_PACK32:
				throw new NotImplementedException();
			case VkFormats.VK_FORMAT_A2R10G10B10_UNORM_PACK32:
			case VkFormats.VK_FORMAT_A2R10G10B10_USCALED_PACK32:
			case VkFormats.VK_FORMAT_A2R10G10B10_UINT_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_UNORM_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_USCALED_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_UINT_PACK32:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB10_A2UI;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_A2R10G10B10_SNORM_PACK32:
			case VkFormats.VK_FORMAT_A2R10G10B10_SSCALED_PACK32:
			case VkFormats.VK_FORMAT_A2R10G10B10_SINT_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_SNORM_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_SSCALED_PACK32:
			case VkFormats.VK_FORMAT_A2B10G10R10_SINT_PACK32:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB10_A2I;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16_UNORM:
			case VkFormats.VK_FORMAT_R16_USCALED:
			case VkFormats.VK_FORMAT_R16_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_R_16UI;
				vformatInfo.BPP = 2U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16_SNORM:
			case VkFormats.VK_FORMAT_R16_SSCALED:
			case VkFormats.VK_FORMAT_R16_SINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_R_16I;
				vformatInfo.BPP = 2U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16_SFLOAT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_R_16F;
				vformatInfo.BPP = 2U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16G16_UNORM:
			case VkFormats.VK_FORMAT_R16G16_USCALED:
			case VkFormats.VK_FORMAT_R16G16_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RG_16UI;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16G16_SNORM:
			case VkFormats.VK_FORMAT_R16G16_SSCALED:
			case VkFormats.VK_FORMAT_R16G16_SINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RG_16I;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16G16_SFLOAT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RG_HF;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16G16B16_UNORM:
			case VkFormats.VK_FORMAT_R16G16B16_USCALED:
			case VkFormats.VK_FORMAT_R16G16B16_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB_16UI;
				vformatInfo.BPP = 6U;
				vformatInfo.NumberChannels = 3U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16G16B16_SNORM:
			case VkFormats.VK_FORMAT_R16G16B16_SSCALED:
			case VkFormats.VK_FORMAT_R16G16B16_SINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB_16I;
				vformatInfo.BPP = 6U;
				vformatInfo.NumberChannels = 3U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16G16B16_SFLOAT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB_HF;
				vformatInfo.BPP = 6U;
				vformatInfo.NumberChannels = 3U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16G16B16A16_UNORM:
			case VkFormats.VK_FORMAT_R16G16B16A16_USCALED:
			case VkFormats.VK_FORMAT_R16G16B16A16_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_16UI;
				vformatInfo.BPP = 8U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16G16B16A16_SNORM:
			case VkFormats.VK_FORMAT_R16G16B16A16_SSCALED:
			case VkFormats.VK_FORMAT_R16G16B16A16_SINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_16I;
				vformatInfo.BPP = 8U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R16G16B16A16_SFLOAT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_HF;
				vformatInfo.BPP = 8U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_R_32UI;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32_SINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_R_32I;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32_SFLOAT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_R_F;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32G32_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RG_32UI;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32G32_SINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RG_32I;
				vformatInfo.BPP = 8U;
				vformatInfo.NumberChannels = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32G32_SFLOAT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RG_F;
				vformatInfo.BPP = 8U;
				vformatInfo.NumberChannels = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32G32B32_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB_32UI;
				vformatInfo.BPP = 12U;
				vformatInfo.NumberChannels = 3U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32G32B32_SINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB_32I;
				vformatInfo.BPP = 12U;
				vformatInfo.NumberChannels = 3U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32G32B32_SFLOAT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB_F;
				vformatInfo.BPP = 12U;
				vformatInfo.NumberChannels = 3U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32G32B32A32_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_32UI;
				vformatInfo.BPP = 16U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32G32B32A32_SINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_32I;
				vformatInfo.BPP = 16U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R32G32B32A32_SFLOAT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_F;
				vformatInfo.BPP = 16U;
				vformatInfo.NumberChannels = 4U;
				vformatInfo.HasAlpha = true;
				return vformatInfo;
			case VkFormats.VK_FORMAT_R64_UINT:
			case VkFormats.VK_FORMAT_R64_SINT:
			case VkFormats.VK_FORMAT_R64_SFLOAT:
				throw new NotImplementedException();
			case VkFormats.VK_FORMAT_R64G64_UINT:
			case VkFormats.VK_FORMAT_R64G64_SINT:
			case VkFormats.VK_FORMAT_R64G64_SFLOAT:
			case VkFormats.VK_FORMAT_R64G64B64_UINT:
			case VkFormats.VK_FORMAT_R64G64B64_SINT:
			case VkFormats.VK_FORMAT_R64G64B64_SFLOAT:
			case VkFormats.VK_FORMAT_R64G64B64A64_UINT:
			case VkFormats.VK_FORMAT_R64G64B64A64_SINT:
			case VkFormats.VK_FORMAT_R64G64B64A64_SFLOAT:
				throw new NotImplementedException();
			case VkFormats.VK_FORMAT_B10G11R11_UFLOAT_PACK32:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB_11_11_10_F;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 3U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_E5B9G9R9_UFLOAT_PACK32:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGB9_E5;
				vformatInfo.BPP = 4U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_D16_UNORM:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_DEPTH_16;
				vformatInfo.BPP = 2U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_X8_D24_UNORM_PACK32:
				throw new NotImplementedException();
			case VkFormats.VK_FORMAT_D32_SFLOAT:
			case VkFormats.VK_FORMAT_D32_SFLOAT_S8_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_DEPTH_32;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 1U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_S8_UINT:
			case VkFormats.VK_FORMAT_D16_UNORM_S8_UINT:
				throw new NotImplementedException();
			case VkFormats.VK_FORMAT_D24_UNORM_S8_UINT:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_DEPTH_24_STENCIL_8;
				vformatInfo.BPP = 4U;
				vformatInfo.NumberChannels = 2U;
				vformatInfo.HasAlpha = false;
				return vformatInfo;
			case VkFormats.VK_FORMAT_BC1_RGB_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_BC1_RGB_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_S3TC_DXT1_RGB;
				vformatInfo.HasAlpha = false;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 8U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_BC1_RGBA_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_BC1_RGBA_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_S3TC_DXT1_RGBA;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 8U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_BC2_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_BC2_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_S3TC_DXT3_RGBA;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_BC3_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_BC3_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_S3TC_DXT5_RGBA;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 8U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_BC4_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_BC4_SNORM_BLOCK:
			case VkFormats.VK_FORMAT_BC5_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_BC5_SNORM_BLOCK:
			case VkFormats.VK_FORMAT_BC6H_UFLOAT_BLOCK:
			case VkFormats.VK_FORMAT_BC6H_SFLOAT_BLOCK:
			case VkFormats.VK_FORMAT_BC7_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_BC7_SRGB_BLOCK:
				throw new NotImplementedException();
			case VkFormats.VK_FORMAT_ETC2_R8G8B8_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ETC2_R8G8B8_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ETC2_RGB8;
				vformatInfo.HasAlpha = false;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 8U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ETC2_R8G8B8A1_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ETC2_R8G8B8A1_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ETC2_SRGB8_PUNCHTHROUGH_ALPHA1;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 8U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ETC2_R8G8B8A8_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ETC2_R8G8B8A8_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ETC2_RGBA8;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_EAC_R11_UNORM_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_EAC_R_UNSIGNED;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 8U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_EAC_R11_SNORM_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_EAC_R_SIGNED;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 8U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_EAC_R11G11_UNORM_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_EAC_RG_UNSIGNED;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_EAC_R11G11_SNORM_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_EAC_RG_SIGNED;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_4x4_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_4x4_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.BPP = 2U;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 4U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_5x4_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_5x4_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 5U;
				vformatInfo.BlockHeight = 4U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_5x5_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_5x5_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 5U;
				vformatInfo.BlockHeight = 5U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_6x5_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_6x5_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 6U;
				vformatInfo.BlockHeight = 5U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_6x6_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_6x6_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 6U;
				vformatInfo.BlockHeight = 6U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_8x5_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_8x5_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.BPP = 2U;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 8U;
				vformatInfo.BlockHeight = 5U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_8x6_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_8x6_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 8U;
				vformatInfo.BlockHeight = 6U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_8x8_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_8x8_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 8U;
				vformatInfo.BlockHeight = 8U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_10x5_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_10x5_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 10U;
				vformatInfo.BlockHeight = 5U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_10x6_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_10x6_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 10U;
				vformatInfo.BlockHeight = 6U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_10x8_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_10x8_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 10U;
				vformatInfo.BlockHeight = 8U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_10x10_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_10x10_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 10U;
				vformatInfo.BlockHeight = 10U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_12x10_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_12x10_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 12U;
				vformatInfo.BlockHeight = 10U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			case VkFormats.VK_FORMAT_ASTC_12x12_UNORM_BLOCK:
			case VkFormats.VK_FORMAT_ASTC_12x12_SRGB_BLOCK:
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_ASTC_16;
				vformatInfo.BPP = 2U;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				vformatInfo.BlockWidth = 12U;
				vformatInfo.BlockHeight = 12U;
				vformatInfo.BlockSize = 16U;
				return vformatInfo;
			default:
				vformatInfo.BPP = 4U;
				vformatInfo.format = global::TextureConverter.TFormats.Q_FORMAT_RGBA_8UI;
				vformatInfo.HasAlpha = true;
				vformatInfo.Compressed = true;
				return vformatInfo;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00004B0C File Offset: 0x00002D0C
		public static string GetShaderStageText(uint shaderStageBits, bool full)
		{
			string text = "";
			if ((shaderStageBits & 1U) != 0U)
			{
				text += (full ? "Vertex" : "VS");
			}
			if ((shaderStageBits & 2U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "TessControl" : "TC");
			}
			if ((shaderStageBits & 4U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "TessEvaluation" : "TE");
			}
			if ((shaderStageBits & 8U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "Geometry" : "GS");
			}
			if ((shaderStageBits & 16U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "Fragment" : "FS");
			}
			if ((shaderStageBits & 32U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "Compute" : "CS");
			}
			if ((shaderStageBits & 128U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "Mesh" : "MS");
			}
			if ((shaderStageBits & 64U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "Task" : "AS");
			}
			if ((shaderStageBits & 256U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "RayGen" : "RGS");
			}
			if ((shaderStageBits & 512U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "AnyHit" : "AHS");
			}
			if ((shaderStageBits & 1024U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "ClosestHit" : "CHS");
			}
			if ((shaderStageBits & 2048U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "Miss" : "MiS");
			}
			if ((shaderStageBits & 4096U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "Intersection" : "InS");
			}
			if ((shaderStageBits & 8192U) != 0U)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += (full ? "Callable" : "CaS");
			}
			return text;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004DBD File Offset: 0x00002FBD
		public static bool IsDrawCall(string apiName)
		{
			return apiName.StartsWith("vkCmdClear", StringComparison.Ordinal) || apiName.StartsWith("vkCmdDraw", StringComparison.Ordinal) || VkHelper.IsDispatch(apiName);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00004DE6 File Offset: 0x00002FE6
		public static bool IsDispatch(string apiName)
		{
			return apiName.StartsWith("vkCmdDispatch", StringComparison.Ordinal) || apiName.StartsWith("vkCmdTraceRaysKHR", StringComparison.Ordinal);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00004E08 File Offset: 0x00003008
		public static bool IsDrawcallParent(string apiName)
		{
			if (apiName != null)
			{
				switch (apiName.Length)
				{
				case 13:
					if (!(apiName == "vkQueueSubmit"))
					{
						return false;
					}
					break;
				case 14:
				case 15:
				case 16:
				case 17:
				case 18:
					return false;
				case 19:
					if (!(apiName == "vkCmdBeginRendering"))
					{
						return false;
					}
					break;
				case 20:
				{
					char c = apiName[5];
					if (c != 'B')
					{
						if (c != 'E')
						{
							if (c != 'i')
							{
								return false;
							}
							if (!(apiName == "vkBeginCommandBuffer"))
							{
								return false;
							}
						}
						else if (!(apiName == "vkCmdExecuteCommands"))
						{
							return false;
						}
					}
					else if (!(apiName == "vkCmdBeginRenderPass"))
					{
						return false;
					}
					break;
				}
				case 21:
					if (!(apiName == "vkCmdBeginRenderPass2"))
					{
						return false;
					}
					break;
				case 22:
					if (!(apiName == "vkCmdBeginRenderingKHR"))
					{
						return false;
					}
					break;
				case 23:
					if (!(apiName == "vkCmdBeginCommandBuffer"))
					{
						return false;
					}
					break;
				case 24:
					if (!(apiName == "vkCmdBeginRenderPass2KHR"))
					{
						return false;
					}
					break;
				default:
					return false;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004F02 File Offset: 0x00003102
		public static bool IsPushConstant(string apiName)
		{
			return apiName.StartsWith("vkCmdPushConstants", StringComparison.Ordinal);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00004F10 File Offset: 0x00003110
		public static ulong MakeSnapshotApiCallID(uint submitIdx, uint cmdbufferIdx, uint drawcallIdx)
		{
			if (submitIdx > 65534U || cmdbufferIdx > 65534U || drawcallIdx > 4294967294U)
			{
				return ulong.MaxValue;
			}
			return (((ulong)submitIdx << 48) & 18446462598732840960UL) | (((ulong)cmdbufferIdx << 32) & 281470681743360UL) | ((ulong)drawcallIdx & (ulong)(-1));
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00004F50 File Offset: 0x00003150
		public static ulong MakeSnapshotApiCallID(string snapshotApiCallIDStr)
		{
			string[] array = snapshotApiCallIDStr.Split(new char[] { '.' });
			uint num;
			uint num2;
			uint num3;
			if (array.Length != 3 || !uint.TryParse(array[0], out num) || !uint.TryParse(array[1], out num2) || !uint.TryParse(array[2], out num3))
			{
				return ulong.MaxValue;
			}
			return VkHelper.MakeSnapshotApiCallID(num, num2, num3);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00004FA4 File Offset: 0x000031A4
		public static string SnapshotApiCallIDToString(ulong snapshotCallID)
		{
			uint num = (uint)((snapshotCallID & 18446462598732840960UL) >> 48);
			uint num2 = (uint)((snapshotCallID & 281470681743360UL) >> 32);
			uint num3 = (uint)(snapshotCallID & (ulong)(-1));
			return VkHelper.SnapshotApiCallIDToString(num, num2, num3);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00004FDF File Offset: 0x000031DF
		public static string SnapshotApiCallIDToString(uint submitIdx, uint cmdbufferIdx, uint drawcallIdx)
		{
			return string.Format("{0}.{1}.{2}", submitIdx, cmdbufferIdx, drawcallIdx);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00005000 File Offset: 0x00003200
		public static string GetImportedMetricName(uint metricId)
		{
			string text = "";
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("ImportSession");
			ModelObject modelObject = dataModel.GetModelObject(model, "ImportedMetrics");
			ModelObjectDataList data = modelObject.GetData("id", metricId.ToString());
			if (data.Count == 1)
			{
				text = data[0].GetValue("name");
			}
			return text;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000506C File Offset: 0x0000326C
		public static uint GetUintValue(JToken token)
		{
			uint num = 0U;
			if (token != null)
			{
				uint.TryParse(token.ToString(), out num);
				return num;
			}
			return num;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00005090 File Offset: 0x00003290
		public static ulong GetUint64Value(JToken token)
		{
			ulong num = 0UL;
			if (token != null)
			{
				ulong.TryParse(token.ToString(), out num);
				return num;
			}
			return num;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000050B4 File Offset: 0x000032B4
		public static float GetFloatValue(JToken token)
		{
			float num = 0f;
			if (token != null)
			{
				float.TryParse(token.ToString(), out num);
				return num;
			}
			return num;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000050DC File Offset: 0x000032DC
		public static bool FirstDrawcall(uint id, List<TreeNode> list, out uint start)
		{
			foreach (TreeNode treeNode in list)
			{
				if (id == (uint)treeNode.Values[0])
				{
					start = VkHelper.FindDrawcallBelow(treeNode);
					return true;
				}
				if (VkHelper.FirstDrawcall(id, treeNode.Children, out start))
				{
					return true;
				}
			}
			start = uint.MaxValue;
			return false;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00005158 File Offset: 0x00003358
		private static uint FindDrawcallBelow(TreeNode start)
		{
			foreach (TreeNode treeNode in start.Children)
			{
				if (VkHelper.IsDrawCall(treeNode.Values[5].ToString()))
				{
					return (uint)treeNode.Values[0];
				}
				if (treeNode.Children.Count > 0)
				{
					uint num = VkHelper.FindDrawcallBelow(treeNode);
					if (num < 4294967295U)
					{
						return num;
					}
				}
			}
			return uint.MaxValue;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000051EC File Offset: 0x000033EC
		public static string ColorfyParameterString(string color, string value)
		{
			return color + value + "</span>";
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000051FC File Offset: 0x000033FC
		public static string PrettifyJsonParameters(string parameters)
		{
			string text = "( ";
			if (string.IsNullOrEmpty(parameters) || parameters == "null")
			{
				return text + ")";
			}
			JObject jobject = JObject.Parse(parameters);
			foreach (JProperty jproperty in jobject.Properties())
			{
				string text2 = jproperty.Name;
				JToken value = jproperty.Value;
				string text3 = value.ToString();
				JTokenType type = value.Type;
				if (type - JTokenType.Object <= 1)
				{
					text3 = string.Concat(text3.Split(null, StringSplitOptions.RemoveEmptyEntries));
					text3 = text3.Replace("\":", "</span> " + VkHelper.ColorfyParameterString("<span foreground='white'>", "= ")).Replace(",\"", " <span foreground='#BBBBBB'>").Replace("{\"", "{ <span foreground='#BBBBBB'>")
						.Replace("\"", "")
						.Replace("}", " }")
						.Replace(",", " , ");
					text3 = VkHelper.ColorfyParameterString("<span foreground='orange'>", text3);
				}
				else
				{
					text3 = VkHelper.ColorfyParameterString("<span foreground='yellow'>", text3);
				}
				text2 = "<i>" + text2 + "</i>";
				text = string.Concat(new string[]
				{
					text,
					VkHelper.ColorfyParameterString("<span foreground='#BBBBBB'>", text2),
					" = ",
					text3,
					" "
				});
			}
			return text + ")";
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000053A0 File Offset: 0x000035A0
		[return: TupleElementNames(new string[] { "hexString", "brightness" })]
		public static ValueTuple<string, float> RGBAJArrayToHex(JArray colorArray)
		{
			float num = VkHelper.GetFloatValue(colorArray[0]) * 255f;
			float num2 = VkHelper.GetFloatValue(colorArray[1]) * 255f;
			float num3 = VkHelper.GetFloatValue(colorArray[2]) * 255f;
			int num4 = (int)(VkHelper.GetFloatValue(colorArray[3]) * 255f);
			ValueTuple<float, float, float, float> valueTuple = VkHelper.ScaleRGBValueForWhiteText(num, num2, num3);
			num = valueTuple.Item1;
			num2 = valueTuple.Item2;
			num3 = valueTuple.Item3;
			float item = valueTuple.Item4;
			return new ValueTuple<string, float>((num4 == 255) ? string.Format("#{0:X2}{1:X2}{2:X2}", (int)num, (int)num2, (int)num3) : string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
			{
				(int)num,
				(int)num2,
				(int)num3,
				num4
			}), item);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000548C File Offset: 0x0000368C
		[return: TupleElementNames(new string[] { "red", "green", "blue", "brightness" })]
		public static ValueTuple<float, float, float, float> ScaleRGBValueForWhiteText(float red, float green, float blue)
		{
			float num = 0.299f * red + 0.587f * green + 0.114f * blue;
			if (num > 125f)
			{
				red = (float)((int)Math.Max(0f, red * 0.75f));
				blue = (float)((int)Math.Max(0f, blue * 0.75f));
				green = (float)((int)Math.Max(0f, green * 0.75f));
			}
			num = 0.299f * red + 0.587f * green + 0.114f * blue;
			return new ValueTuple<float, float, float, float>(red, green, blue, num);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000551C File Offset: 0x0000371C
		internal static List<ShaderStage> GetShaderStages(uint shaderStageBits)
		{
			List<ShaderStage> list = new List<ShaderStage>();
			if ((shaderStageBits & 1U) != 0U)
			{
				list.Add(ShaderStage.Vertex);
			}
			if ((shaderStageBits & 2U) != 0U)
			{
				list.Add(ShaderStage.TessControl);
			}
			if ((shaderStageBits & 4U) != 0U)
			{
				list.Add(ShaderStage.TessEval);
			}
			if ((shaderStageBits & 8U) != 0U)
			{
				list.Add(ShaderStage.Geometry);
			}
			if ((shaderStageBits & 16U) != 0U)
			{
				list.Add(ShaderStage.Fragment);
			}
			if ((shaderStageBits & 32U) != 0U)
			{
				list.Add(ShaderStage.Compute);
			}
			if ((shaderStageBits & 128U) != 0U)
			{
				list.Add(ShaderStage.Mesh);
			}
			if ((shaderStageBits & 64U) != 0U)
			{
				list.Add(ShaderStage.Task);
			}
			if ((shaderStageBits & 256U) != 0U)
			{
				list.Add(ShaderStage.RayGen);
			}
			if ((shaderStageBits & 512U) != 0U)
			{
				list.Add(ShaderStage.AnyHit);
			}
			if ((shaderStageBits & 1024U) != 0U)
			{
				list.Add(ShaderStage.ClosestHit);
			}
			if ((shaderStageBits & 2048U) != 0U)
			{
				list.Add(ShaderStage.Miss);
			}
			if ((shaderStageBits & 4096U) != 0U)
			{
				list.Add(ShaderStage.Intersection);
			}
			if ((shaderStageBits & 8192U) != 0U)
			{
				list.Add(ShaderStage.Callable);
			}
			return list;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00005600 File Offset: 0x00003800
		internal static ShaderStage ConvertVkShaderEnum(VkShaderStageFlagBits shaderType)
		{
			ShaderStage shaderStage = ShaderStage.Vertex;
			if (shaderType <= VkShaderStageFlagBits.VK_SHADER_STAGE_MESH_BIT_EXT)
			{
				if (shaderType <= VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT)
				{
					switch (shaderType)
					{
					case VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT:
						shaderStage = ShaderStage.Vertex;
						break;
					case VkShaderStageFlagBits.VK_SHADER_STAGE_TESSELLATION_CONTROL_BIT:
						shaderStage = ShaderStage.TessControl;
						break;
					case (VkShaderStageFlagBits)3:
						break;
					case VkShaderStageFlagBits.VK_SHADER_STAGE_TESSELLATION_EVALUATION_BIT:
						shaderStage = ShaderStage.TessEval;
						break;
					default:
						if (shaderType != VkShaderStageFlagBits.VK_SHADER_STAGE_GEOMETRY_BIT)
						{
							if (shaderType == VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT)
							{
								shaderStage = ShaderStage.Fragment;
							}
						}
						else
						{
							shaderStage = ShaderStage.Geometry;
						}
						break;
					}
				}
				else if (shaderType != VkShaderStageFlagBits.VK_SHADER_STAGE_COMPUTE_BIT)
				{
					if (shaderType != VkShaderStageFlagBits.VK_SHADER_STAGE_TASK_BIT_EXT)
					{
						if (shaderType == VkShaderStageFlagBits.VK_SHADER_STAGE_MESH_BIT_EXT)
						{
							shaderStage = ShaderStage.Mesh;
						}
					}
					else
					{
						shaderStage = ShaderStage.Task;
					}
				}
				else
				{
					shaderStage = ShaderStage.Compute;
				}
			}
			else if (shaderType <= VkShaderStageFlagBits.VK_SHADER_STAGE_CLOSEST_HIT_BIT_KHR)
			{
				if (shaderType != VkShaderStageFlagBits.VK_SHADER_STAGE_RAYGEN_BIT_KHR)
				{
					if (shaderType != VkShaderStageFlagBits.VK_SHADER_STAGE_ANY_HIT_BIT_KHR)
					{
						if (shaderType == VkShaderStageFlagBits.VK_SHADER_STAGE_CLOSEST_HIT_BIT_KHR)
						{
							shaderStage = ShaderStage.ClosestHit;
						}
					}
					else
					{
						shaderStage = ShaderStage.AnyHit;
					}
				}
				else
				{
					shaderStage = ShaderStage.RayGen;
				}
			}
			else if (shaderType != VkShaderStageFlagBits.VK_SHADER_STAGE_MISS_BIT_KHR)
			{
				if (shaderType != VkShaderStageFlagBits.VK_SHADER_STAGE_INTERSECTION_BIT_KHR)
				{
					if (shaderType == VkShaderStageFlagBits.VK_SHADER_STAGE_CALLABLE_BIT_KHR)
					{
						shaderStage = ShaderStage.Callable;
					}
				}
				else
				{
					shaderStage = ShaderStage.Intersection;
				}
			}
			else
			{
				shaderStage = ShaderStage.Miss;
			}
			return shaderStage;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000056D0 File Offset: 0x000038D0
		public static string GetFlagsEnumStr<TEnum>(uint flags) where TEnum : Enum
		{
			string text = "";
			if (flags == 0U)
			{
				text += Enum.GetName(typeof(TEnum), flags);
			}
			else
			{
				foreach (object obj in Enum.GetValues(typeof(TEnum)))
				{
					uint num = UintConverter.Convert(obj);
					if (num != 0U && (flags & num) == num)
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += " | ";
						}
						text += obj.ToString();
					}
				}
			}
			return text;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00005784 File Offset: 0x00003984
		public static uint GetBufferType(DataRangeType dataRangeType)
		{
			if (dataRangeType == DataRangeType.PushConstant)
			{
				return SDPCore.BUFFER_TYPE_VULKAN_PUSH_CONSTANT_REFLECTION;
			}
			if (dataRangeType != DataRangeType.Uniform)
			{
				return SDPCore.BUFFER_TYPE_UNKNOWN;
			}
			return SDPCore.BUFFER_TYPE_VULKAN_UNIFORM_BUFFER_REFLECTION;
		}

		// Token: 0x04000337 RID: 823
		public static ILogger Logger = new global::Sdp.Logging.Logger("QGL Helper");

		// Token: 0x04000338 RID: 824
		public const ulong SNAPSHOTAPICALLID_INVALID_ID = 18446744073709551615UL;

		// Token: 0x04000339 RID: 825
		public const int SNAPSHOTAPICALLID_SUBMIT_SHIFT = 48;

		// Token: 0x0400033A RID: 826
		public const int SNAPSHOTAPICALLID_CMDBUFFER_SHIFT = 32;

		// Token: 0x0400033B RID: 827
		public const int SNAPSHOTAPICALLID_DRAWCALL_SHIFT = 0;

		// Token: 0x0400033C RID: 828
		public const uint SNAPSHOTAPICALLID_SUBMIT_VAL_MAX = 65534U;

		// Token: 0x0400033D RID: 829
		public const uint SNAPSHOTAPICALLID_CMDBUFFER_VAL_MAX = 65534U;

		// Token: 0x0400033E RID: 830
		public const uint SNAPSHOTAPICALLID_DRAWCALL_VAL_MAX = 4294967294U;

		// Token: 0x0400033F RID: 831
		public const ulong SNAPSHOTAPICALLID_SUBMIT_MASK = 18446462598732840960UL;

		// Token: 0x04000340 RID: 832
		public const ulong SNAPSHOTAPICALLID_CMDBUFFER_MASK = 281470681743360UL;

		// Token: 0x04000341 RID: 833
		public const ulong SNAPSHOTAPICALLID_DRAWCALL_MASK = 4294967295UL;

		// Token: 0x04000342 RID: 834
		public const int GFXRECONSTRUCT_BUFFER_ID = 1;

		// Token: 0x04000343 RID: 835
		public const int GFXRECONSTRUCT_STRIPPED_BUFFER_ID = 2;

		// Token: 0x04000344 RID: 836
		public const int DESCRIPTOR_SET_BUFFER_ID = 3;

		// Token: 0x04000345 RID: 837
		private static Dictionary<string, string> m_formatStrings = new Dictionary<string, string>();

		// Token: 0x02000051 RID: 81
		public struct VFormatInfo
		{
			// Token: 0x06000172 RID: 370 RVA: 0x00012558 File Offset: 0x00010758
			public uint ImageSize(uint width, uint height)
			{
				if (this.Compressed)
				{
					return (uint)Math.Ceiling(width / this.BlockWidth) * (uint)Math.Ceiling(height / this.BlockHeight) * this.BlockSize;
				}
				return width * height * this.BPP;
			}

			// Token: 0x0400044B RID: 1099
			public global::TextureConverter.TFormats format;

			// Token: 0x0400044C RID: 1100
			public uint BPP;

			// Token: 0x0400044D RID: 1101
			public uint NumberChannels;

			// Token: 0x0400044E RID: 1102
			public bool HasAlpha;

			// Token: 0x0400044F RID: 1103
			public bool Compressed;

			// Token: 0x04000450 RID: 1104
			public uint BlockWidth;

			// Token: 0x04000451 RID: 1105
			public uint BlockHeight;

			// Token: 0x04000452 RID: 1106
			public uint BlockSize;
		}
	}
}
