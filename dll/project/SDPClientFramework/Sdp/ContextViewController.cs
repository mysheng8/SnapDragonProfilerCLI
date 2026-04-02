using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x02000227 RID: 551
	public class ContextViewController : IViewController
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x00016EB3 File Offset: 0x000150B3
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00016EBC File Offset: 0x000150BC
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

		// Token: 0x06000867 RID: 2151 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00016EEC File Offset: 0x000150EC
		public ContextViewController(IContextView view)
		{
			this.m_view = view;
			this.m_view.PropertyGridValueChanged += this.m_view_PropertyGridValueChanged;
			ContextViewEvents contextViewEvents = SdpApp.EventsManager.ContextViewEvents;
			contextViewEvents.PopulatePropertyGrid = (EventHandler<PropertyGridPopulateArgs>)Delegate.Combine(contextViewEvents.PopulatePropertyGrid, new EventHandler<PropertyGridPopulateArgs>(this.contextViewEvents_PopulatePropertyGrid));
			ContextViewEvents contextViewEvents2 = SdpApp.EventsManager.ContextViewEvents;
			contextViewEvents2.UpdateVisibility = (EventHandler<UpdateVisibiltyArgs>)Delegate.Combine(contextViewEvents2.UpdateVisibility, new EventHandler<UpdateVisibiltyArgs>(this.contextViewEvents_UpdateVisibilty));
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.OptionAdded = (EventHandler<OptionEventArgs>)Delegate.Combine(connectionEvents.OptionAdded, new EventHandler<OptionEventArgs>(this.connectionEvents_OptionAdded));
			OptionsViewEvents optionsViewEvents = SdpApp.EventsManager.OptionsViewEvents;
			optionsViewEvents.DisableView = (EventHandler<EventArgs>)Delegate.Combine(optionsViewEvents.DisableView, new EventHandler<EventArgs>(this.OptionsViewEvent_DisableView));
			OptionsViewEvents optionsViewEvents2 = SdpApp.EventsManager.OptionsViewEvents;
			optionsViewEvents2.EnableView = (EventHandler<EventArgs>)Delegate.Combine(optionsViewEvents2.EnableView, new EventHandler<EventArgs>(this.OptionsViewEvent_EnableView));
			this.m_optionValueUpdatedFunctionPointer = new Void_UInt_UInt_UInt_Fn(this.OnOptionValueChanged);
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			HashSet<uint> contextOptions = SdpApp.ModelManager.ContextModel.ContextOptions;
			object mutex = this.m_mutex;
			lock (mutex)
			{
				foreach (uint num in contextOptions)
				{
					this.m_contextStateOptions.Add(num);
					Option option = SdpApp.ConnectionManager.GetOption(num, uint.MaxValue);
					option.RegisterOptionChangeHandler(this.m_optionValueUpdatedFunctionPointer);
					PropertyDescriptor propertyDescriptor = FormatHelper.CreatePropertyDescriptorFromOption(option);
					if (propertyDescriptor != null)
					{
						list.Add(propertyDescriptor);
					}
				}
			}
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			this.m_view.SetCurrentObject(propertyGridDescriptionObject);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x000170F0 File Offset: 0x000152F0
		private void m_view_PropertyGridValueChanged(object sender, PropertyDescriptionChangedEventArgs e)
		{
			PropertyDescriptor property = e.property;
			if (property != null)
			{
				string name = property.Name;
				Option option = SdpApp.ConnectionManager.GetOption(name, uint.MaxValue);
				if (option != null)
				{
					object value = property.GetValue(option);
					if (value.Equals(OptionValueRetriever.GetOptionValue(option)))
					{
						return;
					}
					OptionValueRetriever.SetOptionValue(option, value, true);
					OptionValueChangedEventArgs optionValueChangedEventArgs = new OptionValueChangedEventArgs();
					optionValueChangedEventArgs.NewValue = value;
					optionValueChangedEventArgs.OptionName = name;
					optionValueChangedEventArgs.IsLocalChange = true;
					optionValueChangedEventArgs.Pid = uint.MaxValue;
					SdpApp.EventsManager.Raise<OptionValueChangedEventArgs>(SdpApp.EventsManager.OptionsViewEvents.OptionValueChanged, this, optionValueChangedEventArgs);
				}
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00017180 File Offset: 0x00015380
		private void contextViewEvents_PopulatePropertyGrid(object sender, PropertyGridPopulateArgs args)
		{
			this.m_view.SetCurrentObject(args.DescriptionObject);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00017193 File Offset: 0x00015393
		private void connectionEvents_OptionAdded(object sender, OptionEventArgs e)
		{
			if (SdpApp.ConnectionManager.GetOption(e.OptionId, e.ProcessId).IsOptionContextState() && e.ProcessId == 4294967295U)
			{
				this.AddOption(e.OptionId);
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000171C7 File Offset: 0x000153C7
		private void contextViewEvents_UpdateVisibilty(object sender, UpdateVisibiltyArgs e)
		{
			this.m_view.UpdateVisibility(e.Visible);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x000171DC File Offset: 0x000153DC
		private void AddOption(uint optionId)
		{
			object mutex = this.m_mutex;
			lock (mutex)
			{
				this.m_contextStateOptions.Add(optionId);
				List<PropertyDescriptor> list = new List<PropertyDescriptor>();
				foreach (uint num in this.m_contextStateOptions)
				{
					Option option = SdpApp.ConnectionManager.GetOption(num, uint.MaxValue);
					if (num == optionId)
					{
						option.RegisterOptionChangeHandler(this.m_optionValueUpdatedFunctionPointer);
					}
					PropertyDescriptor propertyDescriptor = FormatHelper.CreatePropertyDescriptorFromOption(option);
					if (propertyDescriptor != null)
					{
						list.Add(propertyDescriptor);
					}
				}
				PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
				propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
				this.m_view.SetCurrentObject(propertyGridDescriptionObject);
			}
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x000172B8 File Offset: 0x000154B8
		private void OnOptionValueChanged(uint providerID, uint optionID, uint processID)
		{
			Option option = SdpApp.ConnectionManager.GetOption(optionID, processID);
			if (option != null && option.IsOptionContextState())
			{
				List<PropertyDescriptor> list = new List<PropertyDescriptor>();
				foreach (uint num in this.m_contextStateOptions)
				{
					Option option2 = SdpApp.ConnectionManager.GetOption(num, uint.MaxValue);
					PropertyDescriptor propertyDescriptor = FormatHelper.CreatePropertyDescriptorFromOption(option2);
					if (propertyDescriptor != null)
					{
						list.Add(propertyDescriptor);
					}
				}
				PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
				propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
				this.m_view.SetCurrentObject(propertyGridDescriptionObject);
				OptionValueChangedEventArgs optionValueChangedEventArgs = new OptionValueChangedEventArgs();
				optionValueChangedEventArgs.NewValue = OptionValueRetriever.GetOptionValue(option);
				optionValueChangedEventArgs.OptionName = option.GetName();
				optionValueChangedEventArgs.IsLocalChange = false;
				optionValueChangedEventArgs.Pid = processID;
				SdpApp.EventsManager.Raise<OptionValueChangedEventArgs>(SdpApp.EventsManager.OptionsViewEvents.OptionValueChanged, this, optionValueChangedEventArgs);
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x000173B0 File Offset: 0x000155B0
		private void OptionsViewEvent_DisableView(object sender, EventArgs args)
		{
			this.m_view.DisableView(true);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000173BE File Offset: 0x000155BE
		private void OptionsViewEvent_EnableView(object sender, EventArgs args)
		{
			this.m_view.DisableView(false);
		}

		// Token: 0x040007D1 RID: 2001
		private IContextView m_view;

		// Token: 0x040007D2 RID: 2002
		private HashSet<uint> m_contextStateOptions = new HashSet<uint>();

		// Token: 0x040007D3 RID: 2003
		private Void_UInt_UInt_UInt_Fn m_optionValueUpdatedFunctionPointer;

		// Token: 0x040007D4 RID: 2004
		private object m_mutex = new object();
	}
}
