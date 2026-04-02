using System;

namespace Sdp
{
	// Token: 0x02000268 RID: 616
	public interface IPixelHistoryView : IView
	{
		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06000A68 RID: 2664
		// (remove) Token: 0x06000A69 RID: 2665
		event EventHandler SelectedChanged;

		// Token: 0x06000A6A RID: 2666
		void SetStatus(StatusType statusType, string status, int duration);

		// Token: 0x06000A6B RID: 2667
		void HideStatus();

		// Token: 0x06000A6C RID: 2668
		void Clear();

		// Token: 0x06000A6D RID: 2669
		void AddItem(int id, PixelHistoryItem item);

		// Token: 0x06000A6E RID: 2670
		void SetThumbnail(int x, int y, ImageViewObject screenshot);

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000A6F RID: 2671
		// (set) Token: 0x06000A70 RID: 2672
		int Selected { get; set; }
	}
}
