using System;

namespace Sdp
{
	// Token: 0x020002D2 RID: 722
	public class ProgramViewEvents
	{
		// Token: 0x06000EBD RID: 3773 RVA: 0x0002DA64 File Offset: 0x0002BC64
		public ProgramViewEvents()
		{
			this.InvalidateUniform = (EventHandler<ProgramViewInvalidateUniformEventArgs>)Delegate.Combine(this.InvalidateUniform, new EventHandler<ProgramViewInvalidateUniformEventArgs>(this.SetVisible));
			this.InvalidateVariables = (EventHandler<ProgramViewInvalidateVariablesEventArgs>)Delegate.Combine(this.InvalidateVariables, new EventHandler<ProgramViewInvalidateVariablesEventArgs>(this.SetVisible));
			this.Invalidate = (EventHandler<ProgramViewInvalidateEventArgs>)Delegate.Combine(this.Invalidate, new EventHandler<ProgramViewInvalidateEventArgs>(this.SetVisible));
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0002DADD File Offset: 0x0002BCDD
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("ProgramView", null, false, false);
		}

		// Token: 0x040009E4 RID: 2532
		public EventHandler<ProgramViewInvalidateUniformEventArgs> InvalidateUniform;

		// Token: 0x040009E5 RID: 2533
		public EventHandler<ProgramViewInvalidateVariablesEventArgs> InvalidateVariables;

		// Token: 0x040009E6 RID: 2534
		public EventHandler<ProgramViewInvalidateEventArgs> Invalidate;

		// Token: 0x040009E7 RID: 2535
		public EventHandler<ProgramViewShaderSelectedEventArgs> ShaderSelected;

		// Token: 0x040009E8 RID: 2536
		public EventHandler<ProgramViewFormatChangedArgs> FormatChanged;

		// Token: 0x040009E9 RID: 2537
		public EventHandler<SetStatusEventArgs> SetStatus;

		// Token: 0x040009EA RID: 2538
		public EventHandler<EventArgs> HideStatus;

		// Token: 0x040009EB RID: 2539
		public const string PROGRAM_VIEW_TYPENAME = "ProgramView";
	}
}
