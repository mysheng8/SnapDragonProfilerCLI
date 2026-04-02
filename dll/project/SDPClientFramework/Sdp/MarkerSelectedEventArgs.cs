using System;
using Sdp.Charts.Gantt;

namespace Sdp
{
	// Token: 0x0200024A RID: 586
	public class MarkerSelectedEventArgs : EventArgs
	{
		// Token: 0x0600098E RID: 2446 RVA: 0x0001BCA0 File Offset: 0x00019EA0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Series: ",
				this.ElementSeries.Name,
				"\n",
				string.Format("Selected Marker: [{0}]", this.Selected),
				"\n",
				string.Format("SelectedObjectCoun: {0}", this.SelectedObjectCount),
				"\n"
			});
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0001BD14 File Offset: 0x00019F14
		public override bool Equals(object obj)
		{
			MarkerSelectedEventArgs markerSelectedEventArgs = obj as MarkerSelectedEventArgs;
			return markerSelectedEventArgs != null && markerSelectedEventArgs.Selected == this.Selected && markerSelectedEventArgs.ElementSeries == this.ElementSeries && markerSelectedEventArgs.SelectedObjectCount == this.SelectedObjectCount;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0001BD57 File Offset: 0x00019F57
		public override int GetHashCode()
		{
			return this.Selected.GetHashCode() ^ this.ElementSeries.GetHashCode() ^ this.SelectedObjectCount.GetHashCode();
		}

		// Token: 0x04000826 RID: 2086
		public Marker Selected;

		// Token: 0x04000827 RID: 2087
		public Series ElementSeries;

		// Token: 0x04000828 RID: 2088
		public int SelectedObjectCount;
	}
}
