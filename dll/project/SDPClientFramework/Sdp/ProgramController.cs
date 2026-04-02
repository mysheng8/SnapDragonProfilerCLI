using System;

namespace Sdp
{
	// Token: 0x020002CE RID: 718
	public class ProgramController : IViewController
	{
		// Token: 0x06000EA1 RID: 3745 RVA: 0x0002D5A4 File Offset: 0x0002B7A4
		public ViewDesc SaveSettings()
		{
			ViewDesc viewDesc = null;
			if (this.m_view != null)
			{
				viewDesc = new ViewDesc();
				viewDesc.TypeName = this.m_view.TypeName;
			}
			return viewDesc;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x0002D5D3 File Offset: 0x0002B7D3
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0002D5DC File Offset: 0x0002B7DC
		public ProgramController(IProgramView view)
		{
			ProgramViewEvents programViewEvents = SdpApp.EventsManager.ProgramViewEvents;
			programViewEvents.Invalidate = (EventHandler<ProgramViewInvalidateEventArgs>)Delegate.Combine(programViewEvents.Invalidate, new EventHandler<ProgramViewInvalidateEventArgs>(this.programViewEvents_Invalidate));
			ProgramViewEvents programViewEvents2 = SdpApp.EventsManager.ProgramViewEvents;
			programViewEvents2.InvalidateUniform = (EventHandler<ProgramViewInvalidateUniformEventArgs>)Delegate.Combine(programViewEvents2.InvalidateUniform, new EventHandler<ProgramViewInvalidateUniformEventArgs>(this.programViewEvents_InvalidateUniform));
			ProgramViewEvents programViewEvents3 = SdpApp.EventsManager.ProgramViewEvents;
			programViewEvents3.InvalidateVariables = (EventHandler<ProgramViewInvalidateVariablesEventArgs>)Delegate.Combine(programViewEvents3.InvalidateVariables, new EventHandler<ProgramViewInvalidateVariablesEventArgs>(this.programViewEvents_InvalidateVariables));
			ProgramViewEvents programViewEvents4 = SdpApp.EventsManager.ProgramViewEvents;
			programViewEvents4.SetStatus = (EventHandler<SetStatusEventArgs>)Delegate.Combine(programViewEvents4.SetStatus, new EventHandler<SetStatusEventArgs>(this.programViewEvents_SetStatus));
			ProgramViewEvents programViewEvents5 = SdpApp.EventsManager.ProgramViewEvents;
			programViewEvents5.HideStatus = (EventHandler<EventArgs>)Delegate.Combine(programViewEvents5.HideStatus, new EventHandler<EventArgs>(this.programViewEvents_HideStatus));
			this.m_view = view;
			this.m_view.ShaderClicked += this.m_view_ShaderClicked;
			this.m_view.FormatChanged += this.m_view_FormatChanged;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0002D6FC File Offset: 0x0002B8FC
		private void m_view_ShaderClicked(object sender, ShaderClickedEventArgs e)
		{
			ProgramViewShaderSelectedEventArgs programViewShaderSelectedEventArgs = new ProgramViewShaderSelectedEventArgs();
			programViewShaderSelectedEventArgs.SourceId = this.m_currentSourceId;
			programViewShaderSelectedEventArgs.ProgramId = this.m_currentProgramId;
			programViewShaderSelectedEventArgs.ShaderId = e.ShaderId;
			SdpApp.EventsManager.Raise<ProgramViewShaderSelectedEventArgs>(SdpApp.EventsManager.ProgramViewEvents.ShaderSelected, this, programViewShaderSelectedEventArgs);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0002D750 File Offset: 0x0002B950
		private void m_view_FormatChanged(object sender, FormatChangedArgs e)
		{
			ProgramViewFormatChangedArgs programViewFormatChangedArgs = new ProgramViewFormatChangedArgs();
			programViewFormatChangedArgs.DataType = e.DataType;
			SdpApp.EventsManager.Raise<ProgramViewFormatChangedArgs>(SdpApp.EventsManager.ProgramViewEvents.FormatChanged, this, programViewFormatChangedArgs);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0002D78C File Offset: 0x0002B98C
		public void programViewEvents_Invalidate(object sender, ProgramViewInvalidateEventArgs args)
		{
			this.m_currentProgramId = args.ProgramId;
			this.m_currentSourceId = args.SourceId;
			this.m_view.Clear(args.ToolbarVisible);
			foreach (ProgramViewShader programViewShader in args.Shaders)
			{
				this.m_view.AddShader(programViewShader);
			}
			this.m_view.AddDataRanges(DataRangeType.PushConstant, args.PushConstantRanges);
			this.m_view.AddVariables(DataRangeType.PushConstant, args.PushConstants, false);
			this.m_view.AddDataRanges(DataRangeType.Uniform, args.UniformRanges);
			this.m_view.AddVariables(DataRangeType.Uniform, args.VkUniforms, false);
			foreach (ProgramViewAttribute programViewAttribute in args.Attributes)
			{
				this.m_view.AddAttribute(programViewAttribute.Index, programViewAttribute.TypeName, programViewAttribute.Name);
			}
			foreach (ProgramViewUniform programViewUniform in args.Uniforms)
			{
				this.m_view.AddUniform(programViewUniform.Location, programViewUniform.Name);
			}
			foreach (ProgramViewUniformBlock programViewUniformBlock in args.UniformBlocks)
			{
				this.m_view.AddUniformBlock((int)programViewUniformBlock.Index, programViewUniformBlock.UniformBlockName);
				foreach (ProgramViewUniform programViewUniform2 in programViewUniformBlock.Uniforms)
				{
					this.m_view.AddUniformToUniformBlock((int)programViewUniformBlock.Index, programViewUniform2.Location, programViewUniform2.TypeName, programViewUniform2.Name);
				}
			}
			this.m_view.ExpandAll();
			this.m_view.RestoreSelection();
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0002D9DC File Offset: 0x0002BBDC
		public void programViewEvents_InvalidateUniform(object sender, ProgramViewInvalidateUniformEventArgs args)
		{
			this.m_view.InvalidateUniformValues(args.Location, args.Data);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0002D9F5 File Offset: 0x0002BBF5
		public void programViewEvents_InvalidateVariables(object sender, ProgramViewInvalidateVariablesEventArgs args)
		{
			this.m_view.AddVariables(DataRangeType.PushConstant, args.PushConstants, true);
			this.m_view.AddVariables(DataRangeType.Uniform, args.Uniforms, false);
			this.m_view.ExpandAll();
			this.m_view.RestoreSelection();
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0002DA33 File Offset: 0x0002BC33
		private void programViewEvents_SetStatus(object sender, SetStatusEventArgs e)
		{
			this.m_view.SetStatus(e.Status, e.StatusText, e.Duration, false, null);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0002DA54 File Offset: 0x0002BC54
		private void programViewEvents_HideStatus(object sender, EventArgs e)
		{
			this.m_view.HideStatus();
		}

		// Token: 0x040009DF RID: 2527
		private int m_currentSourceId;

		// Token: 0x040009E0 RID: 2528
		private int m_currentProgramId;

		// Token: 0x040009E1 RID: 2529
		private IProgramView m_view;
	}
}
