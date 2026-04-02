using System;
using Cairo;

namespace Sdp
{
	// Token: 0x0200018D RID: 397
	public interface IGraphDataSeries
	{
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060004A0 RID: 1184
		// (remove) Token: 0x060004A1 RID: 1185
		event EventHandler DataChanged;

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060004A2 RID: 1186
		int SeriesID { get; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060004A3 RID: 1187
		string Name { get; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060004A4 RID: 1188
		bool Enabled { get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060004A5 RID: 1189
		// (set) Token: 0x060004A6 RID: 1190
		Color Color { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060004A7 RID: 1191
		Color LineColor { get; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060004A8 RID: 1192
		Color AreaColor { get; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060004A9 RID: 1193
		DataPointList DataPoints { get; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004AA RID: 1194
		double MinValue { get; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004AB RID: 1195
		double MaxValue { get; }
	}
}
