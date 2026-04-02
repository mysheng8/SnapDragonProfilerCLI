using System;

namespace Sdp
{
	// Token: 0x02000056 RID: 86
	public class AddFlameGraphCommand : Command
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00007233 File Offset: 0x00005433
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000723B File Offset: 0x0000543B
		public TreeModel Model { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00007244 File Offset: 0x00005444
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000724C File Offset: 0x0000544C
		public int MaxDepth { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00007255 File Offset: 0x00005455
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000725D File Offset: 0x0000545D
		public string Title { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00007266 File Offset: 0x00005466
		// (set) Token: 0x060001DC RID: 476 RVA: 0x0000726E File Offset: 0x0000546E
		public float ChildrenPercent { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00007277 File Offset: 0x00005477
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0000727F File Offset: 0x0000547F
		public FlameGraphController Result { get; set; }

		// Token: 0x060001DF RID: 479 RVA: 0x00007288 File Offset: 0x00005488
		public AddFlameGraphCommand(TreeModel model, int maxDepth, string title, float childrenPercent)
		{
			this.Model = model;
			this.MaxDepth = maxDepth;
			this.Title = title;
			this.ChildrenPercent = childrenPercent;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000072B0 File Offset: 0x000054B0
		protected override void OnExecute()
		{
			if (SdpApp.ModelManager.SamplingModel.CurrentSamplingController != null)
			{
				this.Result = SdpApp.ModelManager.SamplingModel.CurrentSamplingController.AddFlameGraph(this.Model, this.MaxDepth, this.Title, this.ChildrenPercent);
			}
		}
	}
}
