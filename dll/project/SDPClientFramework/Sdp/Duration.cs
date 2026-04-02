using System;

namespace Sdp
{
	// Token: 0x02000183 RID: 387
	public class Duration : IComparable
	{
		// Token: 0x0600045A RID: 1114 RVA: 0x0000AAFF File Offset: 0x00008CFF
		public Duration(long time)
		{
			this.Value = time;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0000AB0E File Offset: 0x00008D0E
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x0000AB16 File Offset: 0x00008D16
		public long Value { get; set; }

		// Token: 0x0600045D RID: 1117 RVA: 0x0000AB1F File Offset: 0x00008D1F
		public static implicit operator Duration(long time)
		{
			return new Duration(time);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000AB27 File Offset: 0x00008D27
		public static implicit operator long(Duration time)
		{
			if (time == null)
			{
				return 0L;
			}
			return time.Value;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000AB35 File Offset: 0x00008D35
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000AB40 File Offset: 0x00008D40
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000AB5C File Offset: 0x00008D5C
		public override string ToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000AB78 File Offset: 0x00008D78
		public int CompareTo(object obj)
		{
			Duration duration = obj as Duration;
			if (duration != null)
			{
				return this.Value.CompareTo(duration.Value);
			}
			return 0;
		}
	}
}
