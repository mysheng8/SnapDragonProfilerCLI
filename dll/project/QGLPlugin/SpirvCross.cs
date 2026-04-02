using System;
using System.Runtime.InteropServices;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x02000007 RID: 7
	internal class SpirvCross
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002C20 File Offset: 0x00000E20
		public static void Convert(IShaderStage shaderStage, SpirvCross.FileFormat format, uint shaderIndexOffset = 0U)
		{
			SpirvCross.Request request = default(SpirvCross.Request);
			request.Header.PipelineID = shaderStage.PipelineID;
			request.Header.ShaderType = (int)VkHelper.ConvertVkShaderEnum(shaderStage.StageType);
			request.Header.ShaderIndex = shaderStage.ShaderIndex + shaderIndexOffset;
			request.ShaderModuleID = shaderStage.ShaderModuleID;
			request.FileFormat = format;
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf<SpirvCross.Request>(request));
			Marshal.StructureToPtr<SpirvCross.Request>(request, intPtr, false);
			SdpApp.EventsManager.Raise<BufferTransferEventArgs>(SdpApp.EventsManager.ConnectionEvents.ClientBufferTransfer, null, new BufferTransferEventArgs
			{
				CaptureID = shaderStage.CaptureID,
				BufferID = 0U,
				BufferCategory = SDPCore.BUFFER_TYPE_VULKAN_SPIRV_CROSS_SHADER_SOURCE_DATA,
				ProviderID = 0U,
				BufferData = intPtr,
				BufferDataLength = (uint)Marshal.SizeOf<SpirvCross.Request>(request)
			});
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x0200004C RID: 76
		public enum FileFormat
		{
			// Token: 0x04000438 RID: 1080
			GLSL,
			// Token: 0x04000439 RID: 1081
			HLSL,
			// Token: 0x0400043A RID: 1082
			Unknown
		}

		// Token: 0x0200004D RID: 77
		public struct Header
		{
			// Token: 0x0400043B RID: 1083
			public int ShaderType;

			// Token: 0x0400043C RID: 1084
			public uint ShaderIndex;

			// Token: 0x0400043D RID: 1085
			public ulong PipelineID;
		}

		// Token: 0x0200004E RID: 78
		public struct Request
		{
			// Token: 0x0400043E RID: 1086
			public SpirvCross.Header Header;

			// Token: 0x0400043F RID: 1087
			public ulong ShaderModuleID;

			// Token: 0x04000440 RID: 1088
			public SpirvCross.FileFormat FileFormat;
		}

		// Token: 0x0200004F RID: 79
		public struct Response
		{
			// Token: 0x04000441 RID: 1089
			public SpirvCross.Header Header;

			// Token: 0x04000442 RID: 1090
			[MarshalAs(UnmanagedType.I1)]
			public bool Success;

			// Token: 0x04000443 RID: 1091
			public string Output;
		}
	}
}
