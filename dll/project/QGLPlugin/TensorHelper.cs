using System;
using System.Runtime.InteropServices;
using Sdp;
using Sdp.Helpers;
using Sdp.Logging;

namespace QGLPlugin
{
	// Token: 0x02000008 RID: 8
	public static class TensorHelper
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002CF8 File Offset: 0x00000EF8
		public static bool TryLoadAndParseTensor(uint captureID, ulong tensorID, out TensorHelper.ParsedTensor parsed)
		{
			parsed = null;
			bool flag;
			try
			{
				DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
				Model model = dataModel.GetModel("VulkanSnapshot");
				ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotTensors");
				ModelObjectDataList data = modelObject.GetData(new StringList
				{
					"captureID",
					captureID.ToString(),
					"resourceID",
					tensorID.ToString()
				});
				if (data == null || data.Count == 0)
				{
					TensorHelper.Logger.LogWarning(string.Format("Tensor {0} not found in capture {1}", tensorID, captureID));
					flag = false;
				}
				else
				{
					ModelObjectData modelObjectData = data[0];
					VkFormats vkFormats = (VkFormats)UintConverter.Convert(modelObjectData.GetValue("format"));
					VkTensorTilingARM vkTensorTilingARM = (VkTensorTilingARM)UintConverter.Convert(modelObjectData.GetValue("tiling"));
					long[] array = TensorHelper.ParseLongArray(modelObjectData.GetValue("pDimensions"));
					long[] array2 = ((vkTensorTilingARM == VkTensorTilingARM.VK_TENSOR_TILING_LINEAR_ARM) ? TensorHelper.ParseLongArray(modelObjectData.GetValue("pStrides")) : null);
					ByteBufferGateway byteBufferGateway = new ByteBufferGateway("VulkanSnapshot", "VulkanSnapshotByteBuffers");
					IByteBuffer byteBuffer = byteBufferGateway.GetByteBuffer((int)captureID, tensorID);
					if (byteBuffer != null && byteBuffer.BDP.data != IntPtr.Zero && byteBuffer.BDP.size > 0U)
					{
						byte[] array3 = new byte[byteBuffer.BDP.size];
						Marshal.Copy(byteBuffer.BDP.data, array3, 0, (int)byteBuffer.BDP.size);
						flag = TensorHelper.ParseTensorBytes(vkFormats, vkTensorTilingARM, array, array2, array3, out parsed);
					}
					else
					{
						TensorHelper.Logger.LogWarning(string.Format("No byte buffer found for tensor {0}", tensorID));
						flag = false;
					}
				}
			}
			catch (Exception ex)
			{
				TensorHelper.Logger.LogError(string.Format("TryLoadAndParseTensor failed for tensor {0}: {1}", tensorID, ex.Message));
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002EF0 File Offset: 0x000010F0
		private static bool ParseTensorBytes(VkFormats format, VkTensorTilingARM tiling, long[] dims, long[] strides, byte[] bytes, out TensorHelper.ParsedTensor parsed)
		{
			parsed = null;
			bool flag;
			try
			{
				if (bytes == null || bytes.Length == 0)
				{
					TensorHelper.Logger.LogError("Tensor has no data");
					flag = false;
				}
				else if (dims == null || dims.Length < 2)
				{
					TensorHelper.Logger.LogError(string.Format("Invalid dimensions: {0}", (dims != null) ? dims.Length : 0));
					flag = false;
				}
				else
				{
					int formatElementSize = TensorHelper.GetFormatElementSize(format);
					string formatName = TensorHelper.GetFormatName(format);
					int num = dims.Length;
					long num2 = ((num >= 3) ? dims[num - 1] : 1L);
					int num3 = ((num2 > 1L) ? 1 : 0);
					if (num < 2 + num3)
					{
						TensorHelper.Logger.LogError(string.Format("Invalid dimension count {0} for channel offset {1}", num, num3));
						flag = false;
					}
					else
					{
						long num4 = dims[num - 1 - num3];
						long num5 = dims[num - 2 - num3];
						for (int i = 0; i < num - 2 - num3; i++)
						{
							num5 *= dims[i];
						}
						TensorHelper.Logger.LogDebug(string.Concat(new string[]
						{
							"Parsing tensor: ",
							formatName,
							", Dims=[",
							string.Join<long>(",", dims),
							"]"
						}));
						TensorHelper.Logger.LogDebug(string.Format("  Interpreted as: {0}x{1}x{2} (HxWxC)", num5, num4, num2));
						TensorHelper.Logger.LogDebug(string.Format("  Element size: {0} bytes, Tiling: {1}", formatElementSize, tiling));
						if (tiling != VkTensorTilingARM.VK_TENSOR_TILING_LINEAR_ARM)
						{
							TensorHelper.Logger.LogWarning("OPTIMAL tiling not yet supported for parsing");
							flag = false;
						}
						else if (strides == null || strides.Length != num)
						{
							TensorHelper.Logger.LogWarning("Invalid strides for LINEAR tiling");
							flag = false;
						}
						else
						{
							double[][][] array = new double[num2][][];
							int num6 = 0;
							while ((long)num6 < num2)
							{
								array[num6] = new double[num5][];
								int num7 = 0;
								while ((long)num7 < num5)
								{
									array[num6][num7] = new double[num4];
									num7++;
								}
								num6++;
							}
							long[] array2 = new long[num];
							long num8 = ((num3 == 1) ? strides[num - 3] : strides[num - 2]);
							long num9 = ((num3 == 1) ? strides[num - 2] : strides[num - 1]);
							long num10 = ((num3 == 1) ? strides[num - 1] : 0L);
							for (long num11 = 0L; num11 < num5; num11 += 1L)
							{
								long num12 = num11 * num8;
								for (long num13 = 0L; num13 < num4; num13 += 1L)
								{
									long num14 = num12 + num13 * num9;
									for (long num15 = 0L; num15 < num2; num15 += 1L)
									{
										long num16 = num14 + num15 * num10;
										if (num16 >= 0L && num16 + (long)formatElementSize <= (long)bytes.Length)
										{
											checked(array[(int)((IntPtr)num15)][(int)((IntPtr)num11)][(int)((IntPtr)num13)]) = TensorHelper.ReadElement(bytes, (int)num16, format, formatElementSize);
										}
									}
								}
							}
							parsed = new TensorHelper.ParsedTensor
							{
								Format = format,
								Tiling = tiling,
								Dims = dims,
								ElementSize = formatElementSize,
								NumChannels = (int)num2,
								Matrices = array,
								FormatName = formatName
							};
							TensorHelper.Logger.LogInformation(string.Format("Parsed tensor into {0} matrices of {1}x{2}", num2, num5, num4));
							flag = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				TensorHelper.Logger.LogError("ParseTensorBytes failed: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003250 File Offset: 0x00001450
		private static long[] ParseLongArray(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}
			string[] array = str.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			long[] array2 = new long[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				long.TryParse(array[i].Trim(), out array2[i]);
			}
			return array2;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000032AC File Offset: 0x000014AC
		private static int GetFormatElementSize(VkFormats format)
		{
			if (format <= VkFormats.VK_FORMAT_R16_SNORM)
			{
				if (format - VkFormats.VK_FORMAT_R8_UNORM <= 5)
				{
					return 1;
				}
				if (format - VkFormats.VK_FORMAT_R16_UNORM > 1)
				{
					goto IL_0034;
				}
			}
			else if (format - VkFormats.VK_FORMAT_R16_UINT > 2)
			{
				if (format - VkFormats.VK_FORMAT_R32_UINT <= 2)
				{
					return 4;
				}
				if (format - VkFormats.VK_FORMAT_R64_UINT > 2)
				{
					goto IL_0034;
				}
				return 8;
			}
			return 2;
			IL_0034:
			TensorHelper.Logger.LogWarning(string.Format("Unknown format {0}, assuming 4 bytes", format));
			return 4;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003308 File Offset: 0x00001508
		private static string GetFormatName(VkFormats format)
		{
			string text;
			try
			{
				int num = (int)format;
				text = VkHelper.GetTextureFormatString(num.ToString());
			}
			catch
			{
				text = format.ToString();
			}
			return text;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003348 File Offset: 0x00001548
		private static double ReadElement(byte[] bytes, int offset, VkFormats format, int elementSize)
		{
			if (offset + elementSize > bytes.Length)
			{
				return 0.0;
			}
			if (format <= VkFormats.VK_FORMAT_R16_SFLOAT)
			{
				switch (format)
				{
				case VkFormats.VK_FORMAT_R8_UNORM:
				case VkFormats.VK_FORMAT_R8_USCALED:
				case VkFormats.VK_FORMAT_R8_UINT:
					return (double)bytes[offset];
				case VkFormats.VK_FORMAT_R8_SNORM:
				case VkFormats.VK_FORMAT_R8_SSCALED:
				case VkFormats.VK_FORMAT_R8_SINT:
					return (double)((sbyte)bytes[offset]);
				default:
					switch (format)
					{
					case VkFormats.VK_FORMAT_R16_UNORM:
					case VkFormats.VK_FORMAT_R16_UINT:
						return (double)BitConverter.ToUInt16(bytes, offset);
					case VkFormats.VK_FORMAT_R16_SNORM:
					case VkFormats.VK_FORMAT_R16_SINT:
						return (double)BitConverter.ToInt16(bytes, offset);
					case VkFormats.VK_FORMAT_R16_SFLOAT:
					{
						ushort num = BitConverter.ToUInt16(bytes, offset);
						return (double)TensorHelper.HalfToSingle(num);
					}
					}
					break;
				}
			}
			else
			{
				switch (format)
				{
				case VkFormats.VK_FORMAT_R32_UINT:
					return BitConverter.ToUInt32(bytes, offset);
				case VkFormats.VK_FORMAT_R32_SINT:
					return (double)BitConverter.ToInt32(bytes, offset);
				case VkFormats.VK_FORMAT_R32_SFLOAT:
					return (double)BitConverter.ToSingle(bytes, offset);
				default:
					switch (format)
					{
					case VkFormats.VK_FORMAT_R64_UINT:
						return BitConverter.ToUInt64(bytes, offset);
					case VkFormats.VK_FORMAT_R64_SINT:
						return (double)BitConverter.ToInt64(bytes, offset);
					case VkFormats.VK_FORMAT_R64_SFLOAT:
						return BitConverter.ToDouble(bytes, offset);
					}
					break;
				}
			}
			return (double)BitConverter.ToSingle(bytes, offset);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003450 File Offset: 0x00001650
		private static float HalfToSingle(ushort half)
		{
			int num = (half >> 15) & 1;
			int num2 = (half >> 10) & 31;
			int num3 = (int)(half & 1023);
			if (num2 == 0)
			{
				if (num3 == 0)
				{
					return (num == 1) ? -0f : 0f;
				}
				return (float)(((num == 1) ? (-1.0) : 1.0) * ((double)num3 / 1024.0) * Math.Pow(2.0, -14.0));
			}
			else
			{
				if (num2 == 31)
				{
					return float.NaN;
				}
				return (float)(((num == 1) ? (-1.0) : 1.0) * (1.0 + (double)num3 / 1024.0) * Math.Pow(2.0, (double)(num2 - 15)));
			}
		}

		// Token: 0x040000E5 RID: 229
		private static ILogger Logger = new global::Sdp.Logging.Logger("Tensor Helper");

		// Token: 0x02000050 RID: 80
		public class ParsedTensor
		{
			// Token: 0x04000444 RID: 1092
			public VkFormats Format;

			// Token: 0x04000445 RID: 1093
			public VkTensorTilingARM Tiling;

			// Token: 0x04000446 RID: 1094
			public long[] Dims;

			// Token: 0x04000447 RID: 1095
			public int ElementSize;

			// Token: 0x04000448 RID: 1096
			public int NumChannels;

			// Token: 0x04000449 RID: 1097
			public double[][][] Matrices;

			// Token: 0x0400044A RID: 1098
			public string FormatName;
		}
	}
}
