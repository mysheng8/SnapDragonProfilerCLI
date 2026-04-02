using System;

namespace Sdp.Helpers
{
	// Token: 0x02000305 RID: 773
	public static class CoreWraperExtensions
	{
		// Token: 0x06000FF4 RID: 4084 RVA: 0x00030FFD File Offset: 0x0002F1FD
		public static string AsString(this StringList stringList)
		{
			return string.Join(", ", stringList);
		}
	}
}
