using System;

namespace Sdp
{
	// Token: 0x02000078 RID: 120
	public abstract class Command : ICommand
	{
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000296 RID: 662 RVA: 0x00008914 File Offset: 0x00006B14
		// (remove) Token: 0x06000297 RID: 663 RVA: 0x0000894C File Offset: 0x00006B4C
		public event EventHandler CanExecuteChanged;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000298 RID: 664 RVA: 0x00008984 File Offset: 0x00006B84
		// (remove) Token: 0x06000299 RID: 665 RVA: 0x000089BC File Offset: 0x00006BBC
		public event EventHandler IconIDChanged;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x0600029A RID: 666 RVA: 0x000089F4 File Offset: 0x00006BF4
		// (remove) Token: 0x0600029B RID: 667 RVA: 0x00008A2C File Offset: 0x00006C2C
		public event EventHandler UINameChanged;

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00008A61 File Offset: 0x00006C61
		// (set) Token: 0x0600029D RID: 669 RVA: 0x00008A69 File Offset: 0x00006C69
		public virtual string UIName
		{
			get
			{
				return this.m_ui_name;
			}
			set
			{
				if (this.m_ui_name != value)
				{
					this.m_ui_name = value;
					if (this.UINameChanged != null)
					{
						this.UINameChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00008A99 File Offset: 0x00006C99
		// (set) Token: 0x0600029F RID: 671 RVA: 0x00008AA1 File Offset: 0x00006CA1
		public virtual string IconID
		{
			get
			{
				return this.m_icon_id;
			}
			set
			{
				if (this.m_icon_id != value)
				{
					this.m_icon_id = value;
					if (this.IconIDChanged != null)
					{
						this.IconIDChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public virtual bool CanExecute
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x00008AD4 File Offset: 0x00006CD4
		public virtual bool ClearsUndo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00008AD7 File Offset: 0x00006CD7
		public void Execute()
		{
			this.OnExecute();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00008ADF File Offset: 0x00006CDF
		public void Undo()
		{
			this.OnUndo();
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00008AE7 File Offset: 0x00006CE7
		public void Redo()
		{
			this.OnRedo();
		}

		// Token: 0x060002A5 RID: 677
		protected abstract void OnExecute();

		// Token: 0x060002A6 RID: 678 RVA: 0x00008AEF File Offset: 0x00006CEF
		protected virtual void OnUndo()
		{
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00008AEF File Offset: 0x00006CEF
		protected virtual void OnRedo()
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00008AF1 File Offset: 0x00006CF1
		protected void RaiseCanExecuteChanged()
		{
			if (this.CanExecuteChanged != null)
			{
				this.CanExecuteChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x040001B3 RID: 435
		private string m_ui_name = "";

		// Token: 0x040001B4 RID: 436
		private string m_icon_id = "";
	}
}
