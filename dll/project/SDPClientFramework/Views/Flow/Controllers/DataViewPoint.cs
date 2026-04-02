using System;
using Sdp;
using SDPClientFramework.Views.EventHandlers.MouseEventHandler;

namespace SDPClientFramework.Views.Flow.Controllers
{
	// Token: 0x02000034 RID: 52
	public class DataViewPoint : Point
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004EEC File Offset: 0x000030EC
		public IDataViewMouseEventHandler Handler { get; }

		// Token: 0x06000124 RID: 292 RVA: 0x00004EF4 File Offset: 0x000030F4
		public DataViewPoint(Point point, IDataViewMouseEventHandler mouseHandler)
		{
			this.X = point.X;
			this.Y = point.Y;
			this.Handler = mouseHandler;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004F1B File Offset: 0x0000311B
		public long Timestamp
		{
			get
			{
				return this.Handler.ToTimestamp(this.X);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00004F2E File Offset: 0x0000312E
		public int RowIndex
		{
			get
			{
				return this.Y / this.Handler.RowSize;
			}
		}
	}
}
