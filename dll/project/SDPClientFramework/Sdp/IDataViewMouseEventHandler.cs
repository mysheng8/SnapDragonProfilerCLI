using System;
using SDPClientFramework.Views.EventHandlers.MouseEventHandler;

namespace Sdp
{
	// Token: 0x02000247 RID: 583
	public interface IDataViewMouseEventHandler : IMouseEventHandler
	{
		// Token: 0x0600096E RID: 2414
		long ToTimestamp(int X);

		// Token: 0x0600096F RID: 2415
		int ToDataViewXAxisPoint(long timestamp);

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000970 RID: 2416
		int RowSize { get; }
	}
}
