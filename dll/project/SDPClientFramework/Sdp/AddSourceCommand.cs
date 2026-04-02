using System;

namespace Sdp
{
	// Token: 0x0200005B RID: 91
	public class AddSourceCommand : Command
	{
		// Token: 0x0600020F RID: 527 RVA: 0x0000750F File Offset: 0x0000570F
		public AddSourceCommand(MultiSourceViews view, int captureId, int sourceId, string sourceName, string uniqueID = null)
		{
			this.View = view;
			this.CaptureID = captureId;
			this.SourceID = sourceId;
			this.SourceName = sourceName;
			this.UniqueID = uniqueID;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000753C File Offset: 0x0000573C
		// (set) Token: 0x06000211 RID: 529 RVA: 0x00007544 File Offset: 0x00005744
		public int CaptureID { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000754D File Offset: 0x0000574D
		// (set) Token: 0x06000213 RID: 531 RVA: 0x00007555 File Offset: 0x00005755
		public MultiSourceViews View { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000755E File Offset: 0x0000575E
		// (set) Token: 0x06000215 RID: 533 RVA: 0x00007566 File Offset: 0x00005766
		public int SourceID { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000756F File Offset: 0x0000576F
		// (set) Token: 0x06000217 RID: 535 RVA: 0x00007577 File Offset: 0x00005777
		public string SourceName { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00007580 File Offset: 0x00005780
		// (set) Token: 0x06000219 RID: 537 RVA: 0x00007588 File Offset: 0x00005788
		public string UniqueID { get; set; }

		// Token: 0x0600021A RID: 538 RVA: 0x00007594 File Offset: 0x00005794
		protected override void OnExecute()
		{
			MultiSourceViews view = this.View;
			if (view == MultiSourceViews.DataExplorer)
			{
				ViewSource viewSource = new ViewSource();
				viewSource.CaptureID = this.CaptureID;
				viewSource.SourceID = this.SourceID;
				viewSource.SourceName = this.SourceName;
				viewSource.UniqueID = this.UniqueID;
				SdpApp.ModelManager.DataExplorerViewModel.AddViewSource(viewSource);
				return;
			}
			if (view != MultiSourceViews.Resources)
			{
				return;
			}
			ViewSource viewSource2 = new ViewSource();
			viewSource2.CaptureID = this.CaptureID;
			viewSource2.SourceID = this.SourceID;
			viewSource2.SourceName = this.SourceName;
			viewSource2.UniqueID = this.UniqueID;
			SdpApp.ModelManager.ResourcesViewModel.AddViewSource(viewSource2);
		}
	}
}
