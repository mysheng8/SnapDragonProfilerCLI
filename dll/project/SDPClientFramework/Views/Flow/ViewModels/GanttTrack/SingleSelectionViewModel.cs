using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sdp;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000023 RID: 35
	public class SingleSelectionViewModel : InspectorViewModel
	{
		// Token: 0x0600009E RID: 158 RVA: 0x000034DF File Offset: 0x000016DF
		public InspectorViewDisplayEventArgs ToEventArgs()
		{
			return new InspectorViewDisplayEventArgs
			{
				Content = new PropertyGridDescriptionObject(this.Properties.Select((PropertyContent property) => new SdpPropertyDescriptor<string>(property.Name, typeof(string), property.Value, this.CategoryName, "Description of a " + property.Name, true)).ToList<PropertyDescriptor>()),
				Description = "GanttTrack"
			};
		}

		// Token: 0x040000C5 RID: 197
		public string CategoryName;

		// Token: 0x040000C6 RID: 198
		public List<PropertyContent> Properties;
	}
}
