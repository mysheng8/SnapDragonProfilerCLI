using System;
using System.Runtime.InteropServices;

namespace TextureConverter
{
	// Token: 0x0200000C RID: 12
	public class TextureConverterHelper
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000020A8 File Offset: 0x000002A8
		public static byte[] ConvertImageToRGBA(byte[] data, TFormats format, uint width, uint height, bool flipBR, uint rowStride = 0U)
		{
			if (data == null || data.Length == 0)
			{
				return null;
			}
			byte[] array = null;
			GCHandle gchandle = default(GCHandle);
			GCHandle gchandle2 = default(GCHandle);
			GCHandle gchandle3 = default(GCHandle);
			GCHandle gchandle4 = default(GCHandle);
			try
			{
				TQonvertImage tqonvertImage = new TQonvertImage();
				if (rowStride > 0U)
				{
					TFormatFlags tformatFlags = new TFormatFlags();
					tformatFlags.Stride = rowStride;
					gchandle3 = GCHandle.Alloc(tformatFlags, GCHandleType.Pinned);
					Marshal.StructureToPtr<TFormatFlags>(tformatFlags, gchandle3.AddrOfPinnedObject(), true);
					tqonvertImage.PtrToFormatFlags = gchandle3.AddrOfPinnedObject();
				}
				tqonvertImage.Width = width;
				tqonvertImage.Height = ((height > 0U) ? height : 1U);
				tqonvertImage.Format = (uint)format;
				gchandle = GCHandle.Alloc(data, GCHandleType.Pinned);
				tqonvertImage.DataSize = (uint)data.Length;
				tqonvertImage.PtrToData = gchandle.AddrOfPinnedObject();
				TFormatFlags tformatFlags2 = new TFormatFlags();
				tformatFlags2.Stride = width * 4U;
				tformatFlags2.MaskRed = (flipBR ? 255U : 16711680U);
				tformatFlags2.MaskGreen = 65280U;
				tformatFlags2.MaskBlue = (flipBR ? 16711680U : 255U);
				tformatFlags2.MaskAlpha = 4278190080U;
				gchandle4 = GCHandle.Alloc(tformatFlags2, GCHandleType.Pinned);
				Marshal.StructureToPtr<TFormatFlags>(tformatFlags2, gchandle4.AddrOfPinnedObject(), true);
				TQonvertImage tqonvertImage2 = new TQonvertImage();
				tqonvertImage2.Width = width;
				tqonvertImage2.Height = height;
				tqonvertImage2.Format = 1U;
				tqonvertImage2.PtrToFormatFlags = gchandle4.AddrOfPinnedObject();
				tqonvertImage2.DataSize = 0U;
				tqonvertImage2.PtrToData = IntPtr.Zero;
				uint num = QonvertWrapper.Qonvert(tqonvertImage, tqonvertImage2);
				if (num == 0U && tqonvertImage2.DataSize == tqonvertImage2.Width * tqonvertImage2.Height * 4U)
				{
					array = new byte[tqonvertImage2.DataSize];
					gchandle2 = GCHandle.Alloc(array, GCHandleType.Pinned);
					tqonvertImage2.PtrToData = gchandle2.AddrOfPinnedObject();
					num = QonvertWrapper.Qonvert(tqonvertImage, tqonvertImage2);
				}
				if (num != 0U || array == null)
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("{0}\n{1}", ex.Message, ex.StackTrace);
				array = null;
				if (!(ex is OutOfMemoryException) && !(ex is NullReferenceException))
				{
					throw;
				}
				GC.Collect();
			}
			finally
			{
				if (gchandle3.IsAllocated)
				{
					gchandle3.Free();
				}
				if (gchandle4.IsAllocated)
				{
					gchandle4.Free();
				}
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (gchandle2.IsAllocated)
				{
					gchandle2.Free();
				}
			}
			return array;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002344 File Offset: 0x00000544
		public static void GetASTCHeader(out TextureConverterHelper.ASTC_Header astcHeader, byte xBlocks, byte yBlocks, uint width, uint height)
		{
			astcHeader.signature = 1554098963U;
			astcHeader.x_block = 1;
			astcHeader.y_block = 1;
			astcHeader.z_block = 1;
			byte[] bytes = BitConverter.GetBytes(width);
			byte[] bytes2 = BitConverter.GetBytes(height);
			byte[] bytes3 = BitConverter.GetBytes(1U);
			astcHeader.x_size1 = bytes[0];
			astcHeader.x_size2 = bytes[1];
			astcHeader.x_size3 = bytes[2];
			astcHeader.y_size1 = bytes2[0];
			astcHeader.y_size2 = bytes2[1];
			astcHeader.y_size3 = bytes2[2];
			astcHeader.z_size1 = bytes3[0];
			astcHeader.z_size2 = bytes3[1];
			astcHeader.z_size3 = bytes3[2];
			astcHeader.x_block = xBlocks;
			astcHeader.y_block = yBlocks;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023E8 File Offset: 0x000005E8
		public static void AddAstcHeader(out byte[] combinedBuffer, byte[] inputBuffer, byte xBlocks, byte yBlocks, uint width, uint height)
		{
			TextureConverterHelper.ASTC_Header astc_Header;
			TextureConverterHelper.GetASTCHeader(out astc_Header, xBlocks, yBlocks, width, height);
			uint num = (width + (uint)astc_Header.x_block - 1U) / (uint)astc_Header.x_block;
			uint num2 = (height + (uint)astc_Header.y_block - 1U) / (uint)astc_Header.y_block;
			int num3 = (int)(num * num2 * 16U);
			num3 = Math.Min(num3, inputBuffer.Length);
			uint num4 = (uint)((inputBuffer == null) ? 0 : (num3 + Marshal.SizeOf<TextureConverterHelper.ASTC_Header>(astc_Header)));
			combinedBuffer = new byte[num4];
			GCHandle gchandle = GCHandle.Alloc(combinedBuffer, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			if (inputBuffer != null && intPtr != IntPtr.Zero)
			{
				Marshal.StructureToPtr<TextureConverterHelper.ASTC_Header>(astc_Header, intPtr, false);
				Marshal.Copy(inputBuffer, 0, intPtr + Marshal.SizeOf<TextureConverterHelper.ASTC_Header>(astc_Header), num3);
			}
			gchandle.Free();
		}

		// Token: 0x0200031D RID: 797
		public struct ASTC_Header
		{
			// Token: 0x04000B03 RID: 2819
			public uint signature;

			// Token: 0x04000B04 RID: 2820
			public byte x_block;

			// Token: 0x04000B05 RID: 2821
			public byte y_block;

			// Token: 0x04000B06 RID: 2822
			public byte z_block;

			// Token: 0x04000B07 RID: 2823
			public byte x_size1;

			// Token: 0x04000B08 RID: 2824
			public byte x_size2;

			// Token: 0x04000B09 RID: 2825
			public byte x_size3;

			// Token: 0x04000B0A RID: 2826
			public byte y_size1;

			// Token: 0x04000B0B RID: 2827
			public byte y_size2;

			// Token: 0x04000B0C RID: 2828
			public byte y_size3;

			// Token: 0x04000B0D RID: 2829
			public byte z_size1;

			// Token: 0x04000B0E RID: 2830
			public byte z_size2;

			// Token: 0x04000B0F RID: 2831
			public byte z_size3;
		}
	}
}
