using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sdp
{
	// Token: 0x020002B1 RID: 689
	public class InspectorController : IViewController
	{
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x0002AC8F File Offset: 0x00028E8F
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0002AC98 File Offset: 0x00028E98
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

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0002ACC8 File Offset: 0x00028EC8
		public InspectorController(IInspector view)
		{
			this.m_view = view;
			InspectorViewEvents inspectorViewEvents = SdpApp.EventsManager.InspectorViewEvents;
			inspectorViewEvents.Display = (EventHandler<InspectorViewDisplayEventArgs>)Delegate.Combine(inspectorViewEvents.Display, new EventHandler<InspectorViewDisplayEventArgs>(this.inspectorViewEvents_display));
			this.m_view.ButtonClicked += this.inspectorViewEvents_buttonClicked;
			InspectorViewEvents inspectorViewEvents2 = SdpApp.EventsManager.InspectorViewEvents;
			inspectorViewEvents2.SetStatus = (EventHandler<SetStatusEventArgs>)Delegate.Combine(inspectorViewEvents2.SetStatus, new EventHandler<SetStatusEventArgs>(this.inspectorViewEvents_SetStatus));
			InspectorViewEvents inspectorViewEvents3 = SdpApp.EventsManager.InspectorViewEvents;
			inspectorViewEvents3.HideStatus = (EventHandler<MultiViewArgs>)Delegate.Combine(inspectorViewEvents3.HideStatus, new EventHandler<MultiViewArgs>(this.inspectorViewEvents_HideStatus));
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0002AD7C File Offset: 0x00028F7C
		private void inspectorViewEvents_display(object sender, InspectorViewDisplayEventArgs args)
		{
			if (args != null && args.Content != null)
			{
				this.m_view.setPropertyGridObject(args.Content, args.CaptureID, args.ShowASButton, args.ShowTensorButton);
				if (args.Description.Length > 0)
				{
					SdpApp.AnalyticsManager.TrackInteraction(sender.ToString(), args.Description, "Inspector");
				}
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0002ADE0 File Offset: 0x00028FE0
		private void inspectorViewEvents_buttonClicked(object sender, ButtonClickedArgs args)
		{
			SdpApp.EventsManager.Raise<ButtonClickedArgs>(SdpApp.EventsManager.InspectorViewEvents.ButtonClicked, this, args);
			SdpApp.AnalyticsManager.TrackInteraction(sender.ToString(), args.Description, "Inspector");
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0002AE18 File Offset: 0x00029018
		private void inspectorViewEvents_SetStatus(object sender, SetStatusEventArgs args)
		{
			this.m_view.SetStatus(args.Status, args.StatusText, args.Duration, false, null);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0002AE39 File Offset: 0x00029039
		private void inspectorViewEvents_HideStatus(object sender, MultiViewArgs args)
		{
			this.m_view.HideStatus();
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0002AE48 File Offset: 0x00029048
		public PropertyGridDescriptionObject CreatePropertyGridObject(string input)
		{
			char[] array = new char[] { '\n' };
			char[] array2 = new char[] { '-' };
			string[] array3 = input.Split(array);
			if (array3.Length == 0)
			{
				return null;
			}
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			string[] array4 = array3[0].Split(array2);
			for (int i = 1; i < array3.Length; i++)
			{
				string[] array5 = array3[i].Split(array2);
				PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<string>(array5[0], typeof(string), array5[1], array4[0], "This contains information related to " + array5[0], true);
				list.Add(propertyDescriptor);
			}
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			return propertyGridDescriptionObject;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0002AEF0 File Offset: 0x000290F0
		public static void UpdateInspectorView(object sender, IEnumerable<PropertyDescriptor> properties)
		{
			InspectorViewDisplayEventArgs inspectorViewDisplayEventArgs = new InspectorViewDisplayEventArgs();
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			propertyGridDescriptionObject.AddPropertyGridDescriptors(properties.ToList<PropertyDescriptor>());
			inspectorViewDisplayEventArgs.Content = propertyGridDescriptionObject;
			SdpApp.EventsManager.Raise<InspectorViewDisplayEventArgs>(SdpApp.EventsManager.InspectorViewEvents.Display, sender, inspectorViewDisplayEventArgs);
		}

		// Token: 0x0400097D RID: 2429
		private IInspector m_view;
	}
}
