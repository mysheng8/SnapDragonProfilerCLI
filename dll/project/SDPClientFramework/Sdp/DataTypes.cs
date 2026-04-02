using System;
using System.Linq;

namespace Sdp
{
	// Token: 0x0200008E RID: 142
	public static class DataTypes
	{
		// Token: 0x06000314 RID: 788 RVA: 0x000095C5 File Offset: 0x000077C5
		public static bool ShouldUseReflection(string dataType)
		{
			return dataType.Equals("Auto");
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000095D4 File Offset: 0x000077D4
		public static bool IsScalar(string dataType)
		{
			return dataType.Equals("bool") || dataType.Equals("half") || dataType.Equals("float") || dataType.Equals("double") || dataType.Equals("byte") || dataType.Equals("ubyte") || dataType.Equals("int") || dataType.Equals("int16") || dataType.Equals("int32") || dataType.Equals("int64") || dataType.Equals("uint") || dataType.Equals("uint16") || dataType.Equals("uint32") || dataType.Equals("uint64");
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000096A3 File Offset: 0x000078A3
		public static bool IsVector(string dataType)
		{
			return dataType.Contains("vec");
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000096B0 File Offset: 0x000078B0
		public static bool IsMatrix(string dataType)
		{
			return dataType.Contains("mat");
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000096BD File Offset: 0x000078BD
		public static bool IsBasicType(string dataType)
		{
			return (DataTypes.IsScalar(dataType) || DataTypes.IsVector(dataType) || DataTypes.IsMatrix(dataType)) && !dataType.Contains('[');
		}

		// Token: 0x040001F3 RID: 499
		public const string Auto = "Auto";

		// Token: 0x040001F4 RID: 500
		public const string Bool = "bool";

		// Token: 0x040001F5 RID: 501
		public const string Half = "half";

		// Token: 0x040001F6 RID: 502
		public const string Float = "float";

		// Token: 0x040001F7 RID: 503
		public const string Double = "double";

		// Token: 0x040001F8 RID: 504
		public const string Byte = "byte";

		// Token: 0x040001F9 RID: 505
		public const string UByte = "ubyte";

		// Token: 0x040001FA RID: 506
		public const string Int = "int";

		// Token: 0x040001FB RID: 507
		public const string Int16 = "int16";

		// Token: 0x040001FC RID: 508
		public const string Int32 = "int32";

		// Token: 0x040001FD RID: 509
		public const string Int64 = "int64";

		// Token: 0x040001FE RID: 510
		public const string UInt = "uint";

		// Token: 0x040001FF RID: 511
		public const string UInt16 = "uint16";

		// Token: 0x04000200 RID: 512
		public const string UInt32 = "uint32";

		// Token: 0x04000201 RID: 513
		public const string UInt64 = "uint64";

		// Token: 0x04000202 RID: 514
		private const string Vector = "vec";

		// Token: 0x04000203 RID: 515
		private const string Matrix = "mat";
	}
}
