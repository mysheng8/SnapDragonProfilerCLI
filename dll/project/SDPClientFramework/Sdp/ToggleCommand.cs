using System;

namespace Sdp
{
	// Token: 0x02000089 RID: 137
	public abstract class ToggleCommand : Command, IToggleCommand, ICommand
	{
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x0600030B RID: 779 RVA: 0x000094E4 File Offset: 0x000076E4
		// (remove) Token: 0x0600030C RID: 780 RVA: 0x0000951C File Offset: 0x0000771C
		public event EventHandler ToggleStateChanged;

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00009551 File Offset: 0x00007751
		// (set) Token: 0x0600030E RID: 782 RVA: 0x00009559 File Offset: 0x00007759
		public bool Toggled
		{
			get
			{
				return this.m_toggled;
			}
			set
			{
				this.SetToggledState(value);
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00007354 File Offset: 0x00005554
		public ToggleCommand()
		{
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00008AEF File Offset: 0x00006CEF
		protected virtual void OnToggleStateChanged()
		{
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00009562 File Offset: 0x00007762
		private void SetToggledState(bool toggled)
		{
			if (this.m_toggled != toggled)
			{
				this.m_toggled = toggled;
				this.OnToggleStateChanged();
				if (this.ToggleStateChanged != null)
				{
					this.ToggleStateChanged(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x040001DD RID: 477
		private bool m_toggled;
	}
}
