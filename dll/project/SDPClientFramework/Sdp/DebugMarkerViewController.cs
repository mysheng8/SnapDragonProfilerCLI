using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sdp.Logging;

namespace Sdp
{
	// Token: 0x020002C4 RID: 708
	public class DebugMarkerViewController : IViewController
	{
		// Token: 0x06000E79 RID: 3705 RVA: 0x0002C704 File Offset: 0x0002A904
		public DebugMarkerViewController(IDebugMarkerView view)
		{
			this.m_view = view;
			DebugMarkerViewEvents debugMarkerViewEvents = SdpApp.EventsManager.DebugMarkerViewEvents;
			debugMarkerViewEvents.Invalidate = (EventHandler<InvalidateDebugMarkerViewEventArgs>)Delegate.Combine(debugMarkerViewEvents.Invalidate, new EventHandler<InvalidateDebugMarkerViewEventArgs>(this.debugMarkerViewEvents_Invalidate));
			DebugMarkerViewEvents debugMarkerViewEvents2 = SdpApp.EventsManager.DebugMarkerViewEvents;
			debugMarkerViewEvents2.DebugMarkerSelected = (EventHandler<DebugMarkerSelectedEventArgs>)Delegate.Combine(debugMarkerViewEvents2.DebugMarkerSelected, new EventHandler<DebugMarkerSelectedEventArgs>(this.debugMarkerViewEvents_Selected));
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0002C77F File Offset: 0x0002A97F
		private void debugMarkerViewEvents_Invalidate(object sender, InvalidateDebugMarkerViewEventArgs args)
		{
			this.m_view.Clear();
			this.m_view.AddDebugMarkerVMList(args.DebugMarkerModel);
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0002C7A0 File Offset: 0x0002A9A0
		private void debugMarkerViewEvents_Selected(object sender, DebugMarkerSelectedEventArgs args)
		{
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			string text = "Debug Region";
			if (!args.IsDebugRegion)
			{
				text = "Insert Marker";
			}
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<string>("Label", typeof(string), args.LabelName, text, string.Empty, true);
			list.Add(propertyDescriptor);
			propertyDescriptor = new SdpPropertyDescriptor<ulong>("Timestamp Start", typeof(ulong), args.TimestampBegin, text, string.Empty, true);
			list.Add(propertyDescriptor);
			propertyDescriptor = new SdpPropertyDescriptor<ulong>("Timestamp End", typeof(ulong), args.TimestampEnd, text, string.Empty, true);
			list.Add(propertyDescriptor);
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			inspectorViewDisplayEventArgs.Description = "Debug Marker";
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, this, inspectorViewDisplayEventArgs);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0002C888 File Offset: 0x0002AA88
		public ViewDesc SaveSettings()
		{
			ViewDesc viewDesc = null;
			if (this.m_view != null)
			{
				viewDesc = new ViewDesc();
				viewDesc.TypeName = this.m_view.TypeName;
			}
			return viewDesc;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0002C8B7 File Offset: 0x0002AAB7
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x040009BA RID: 2490
		private IDebugMarkerView m_view;

		// Token: 0x040009BB RID: 2491
		private static ILogger Logger = new Sdp.Logging.Logger("DebugMarkerViewController");

		// Token: 0x040009BC RID: 2492
		private HashSet<ulong> m_contextSet = new HashSet<ulong>();
	}
}
