using System;
using System.Reflection;
using Sdp.Logging;

namespace Sdp.Helpers
{
	// Token: 0x02000309 RID: 777
	public class TypeConverter
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x0003285E File Offset: 0x00030A5E
		// (set) Token: 0x0600100C RID: 4108 RVA: 0x00032865 File Offset: 0x00030A65
		public static ILogger Logger { protected get; set; } = new Sdp.Logging.Logger("TypeConverter");

		// Token: 0x0600100D RID: 4109 RVA: 0x0003286D File Offset: 0x00030A6D
		protected static string GetErrorString(MemberInfo m, object o)
		{
			return string.Format("{0} - {1}: failed to convert '{2}' to type {3}", new object[]
			{
				m.ReflectedType.Name,
				m.ToString(),
				o.ToString(),
				o.GetType()
			});
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x000328A8 File Offset: 0x00030AA8
		protected static void LogError(MemberInfo m, object o)
		{
			TypeConverter.Logger.LogError(TypeConverter.GetErrorString(m, o));
		}
	}
}
