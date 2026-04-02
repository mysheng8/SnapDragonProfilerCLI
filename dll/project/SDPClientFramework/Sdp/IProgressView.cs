using System;

namespace Sdp
{
	// Token: 0x0200026D RID: 621
	public interface IProgressView
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A7B RID: 2683
		// (set) Token: 0x06000A7C RID: 2684
		double GlobalValue { get; set; }

		// Token: 0x06000A7D RID: 2685
		void AddProgressItem(int id, string title, string description, double value, bool showBar);

		// Token: 0x06000A7E RID: 2686
		void UpdateProgressItem(int id, double value);

		// Token: 0x06000A7F RID: 2687
		void RemoveProgressItem(int id);
	}
}
