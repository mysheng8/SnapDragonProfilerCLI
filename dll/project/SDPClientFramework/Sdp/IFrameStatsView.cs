using System;

namespace Sdp
{
	// Token: 0x020001CB RID: 459
	public interface IFrameStatsView : IView
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060005EA RID: 1514
		// (remove) Token: 0x060005EB RID: 1515
		event EventHandler<ContextSelectionChangedArgs> ContextSelectionChanged;

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060005EC RID: 1516
		// (remove) Token: 0x060005ED RID: 1517
		event EventHandler OnCalculateStatsButtonClicked;

		// Token: 0x1700011F RID: 287
		// (set) Token: 0x060005EE RID: 1518
		TreeModel Model { set; }

		// Token: 0x060005EF RID: 1519
		void Reset();

		// Token: 0x060005F0 RID: 1520
		void AddTextColumn(string name, int modelIndex);

		// Token: 0x060005F1 RID: 1521
		void DisableCaculateButton(bool disable);

		// Token: 0x060005F2 RID: 1522
		void DisableContextComboBox(bool disable);

		// Token: 0x060005F3 RID: 1523
		void SetComboBoxStrings(string[] strings);
	}
}
