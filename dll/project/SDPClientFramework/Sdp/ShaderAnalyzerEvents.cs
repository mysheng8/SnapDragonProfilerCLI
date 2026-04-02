using System;

namespace Sdp
{
	// Token: 0x02000129 RID: 297
	public class ShaderAnalyzerEvents
	{
		// Token: 0x060003CC RID: 972 RVA: 0x0000A294 File Offset: 0x00008494
		public ShaderAnalyzerEvents()
		{
			this.Invalidate = (EventHandler<ShaderAnalyzerProgramEventArgs>)Delegate.Combine(this.Invalidate, new EventHandler<ShaderAnalyzerProgramEventArgs>(this.SetVisible));
			this.InvalidateShaderStatProperties = (EventHandler<ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs>)Delegate.Combine(this.InvalidateShaderStatProperties, new EventHandler<ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs>(this.SetVisible));
			this.InvalidateCurrentShaderStats = (EventHandler<ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs>)Delegate.Combine(this.InvalidateCurrentShaderStats, new EventHandler<ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs>(this.SetVisible));
			this.InvalidateCurrentShaderLogStats = (EventHandler<ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs>)Delegate.Combine(this.InvalidateCurrentShaderLogStats, new EventHandler<ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs>(this.SetVisible));
			this.InvalidateCurrentShaderDisasm = (EventHandler<ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs>)Delegate.Combine(this.InvalidateCurrentShaderDisasm, new EventHandler<ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs>(this.SetVisible));
			this.InvalidateCurrentShaderLogStats = (EventHandler<ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs>)Delegate.Combine(this.InvalidateCurrentShaderLogStats, new EventHandler<ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs>(this.SetVisible));
			this.InvalidateCurrentShaderSource = (EventHandler<ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs>)Delegate.Combine(this.InvalidateCurrentShaderSource, new EventHandler<ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs>(this.SetVisible));
			this.AddSnapshotAPIs = (EventHandler<ShaderAnalyzerAddSnapshotAPIsEventArgs>)Delegate.Combine(this.AddSnapshotAPIs, new EventHandler<ShaderAnalyzerAddSnapshotAPIsEventArgs>(this.SetVisible));
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000A3B7 File Offset: 0x000085B7
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("ShaderAnalyzerView", null, false, false);
		}

		// Token: 0x0400042E RID: 1070
		public EventHandler<ShaderAnalyzerProgramEventArgs> Invalidate;

		// Token: 0x0400042F RID: 1071
		public EventHandler<ShaderAnalyzerProgramEventArgs> ShaderEdited;

		// Token: 0x04000430 RID: 1072
		public EventHandler<ShaderAnalyzerProgramEventArgs> OverrideShader;

		// Token: 0x04000431 RID: 1073
		public EventHandler<ShaderAnalyzerProgramEventArgs> RevertShaderGroup;

		// Token: 0x04000432 RID: 1074
		public EventHandler<ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs> InvalidateShaderStatProperties;

		// Token: 0x04000433 RID: 1075
		public EventHandler<ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs> InvalidateCurrentShaderStats;

		// Token: 0x04000434 RID: 1076
		public EventHandler<ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs> InvalidateCurrentShaderLogStats;

		// Token: 0x04000435 RID: 1077
		public EventHandler<ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs> InvalidateCurrentShaderDisasm;

		// Token: 0x04000436 RID: 1078
		public EventHandler<ShaderAnalyzerInvalidateShaderLanguageArgs> InvalidateShaderLanguage;

		// Token: 0x04000437 RID: 1079
		public EventHandler<ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs> InvalidateCurrentShaderSource;

		// Token: 0x04000438 RID: 1080
		public EventHandler<ShaderAnalyzerExportShaderArgs> ExportShader;

		// Token: 0x04000439 RID: 1081
		public EventHandler<EventArgs> ConvertShaderSource;

		// Token: 0x0400043A RID: 1082
		public EventHandler<ShaderAnalyzerSaveFailedEventArgs> SaveFailed;

		// Token: 0x0400043B RID: 1083
		public EventHandler<ShaderAnalyzerAddSnapshotAPIsEventArgs> AddSnapshotAPIs;

		// Token: 0x0400043C RID: 1084
		public EventHandler<EventArgs> ClearShaderAnalyzerView;

		// Token: 0x0400043D RID: 1085
		public const string SHADER_ANALYZER_VIEW_TYPENAME = "ShaderAnalyzerView";
	}
}
