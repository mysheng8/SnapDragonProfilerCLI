using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x02000263 RID: 611
	public class OptionsViewController : IViewController
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0001C4B2 File Offset: 0x0001A6B2
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0001C4BC File Offset: 0x0001A6BC
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

		// Token: 0x06000A3A RID: 2618 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0001C4EC File Offset: 0x0001A6EC
		public OptionsViewController(IOptionsView view)
		{
			this.m_view = view;
			this.m_view.OptionViewValueChanged += this.m_view_OptionValueChanged;
			this.m_view.ShowAllProcessToggled += this.m_view_ShowAllProcessToggled;
			OptionsViewEvents optionsViewEvents = SdpApp.EventsManager.OptionsViewEvents;
			optionsViewEvents.DisableView = (EventHandler<EventArgs>)Delegate.Combine(optionsViewEvents.DisableView, new EventHandler<EventArgs>(this.OptionsViewEvent_DisableView));
			OptionsViewEvents optionsViewEvents2 = SdpApp.EventsManager.OptionsViewEvents;
			optionsViewEvents2.EnableView = (EventHandler<EventArgs>)Delegate.Combine(optionsViewEvents2.EnableView, new EventHandler<EventArgs>(this.OptionsViewEvent_EnableView));
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.OptionAdded = (EventHandler<OptionEventArgs>)Delegate.Combine(connectionEvents.OptionAdded, new EventHandler<OptionEventArgs>(this.ConnectionEvents_OptionAdded));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.OptionCategoryAdded = (EventHandler<OptionCategoryAddedEventArgs>)Delegate.Combine(connectionEvents2.OptionCategoryAdded, new EventHandler<OptionCategoryAddedEventArgs>(this.ConnectionEvents_OptionCategoryAdded));
			ConnectionEvents connectionEvents3 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents3.ProcessAdded = (EventHandler<ProcessEventArgs>)Delegate.Combine(connectionEvents3.ProcessAdded, new EventHandler<ProcessEventArgs>(this.ConnectionEvents_ProcessListChanged));
			ConnectionEvents connectionEvents4 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents4.ProcessRemoved = (EventHandler<ProcessEventArgs>)Delegate.Combine(connectionEvents4.ProcessRemoved, new EventHandler<ProcessEventArgs>(this.ConnectionEvents_ProcessRemoved));
			ConnectionEvents connectionEvents5 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents5.ProcessStateChanged = (EventHandler<ProcessEventArgs>)Delegate.Combine(connectionEvents5.ProcessStateChanged, new EventHandler<ProcessEventArgs>(this.ConnectionEvents_ProcessListChanged));
			DataSourceViewEvents dataSourceViewEvents = SdpApp.EventsManager.DataSourceViewEvents;
			dataSourceViewEvents.InvalidateSelectedProcesses = (EventHandler<SelectedProcessChangedArgs>)Delegate.Combine(dataSourceViewEvents.InvalidateSelectedProcesses, new EventHandler<SelectedProcessChangedArgs>(this.dataSourceViewEvents_InvalidateSelectedProcesses));
			this.m_optionValueUpdatedFunctionPointer = new Void_UInt_UInt_UInt_Fn(this.OnOptionValueChanged);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0001C6C0 File Offset: 0x0001A8C0
		private void m_view_OptionValueChanged(object sender, OptionViewValueChangedEventArgs e)
		{
			PropertyDescriptor property = e.property;
			if (property != null)
			{
				uint num = 0U;
				foreach (KeyValuePair<uint, List<PropertyDescriptor>> keyValuePair in this.m_props)
				{
					foreach (PropertyDescriptor propertyDescriptor in keyValuePair.Value)
					{
						if (propertyDescriptor == property)
						{
							num = keyValuePair.Key;
							break;
						}
					}
				}
				string name = property.Name;
				Option option = SdpApp.ConnectionManager.GetOption(name, num);
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
					optionValueChangedEventArgs.Pid = num;
					SdpApp.EventsManager.Raise<OptionValueChangedEventArgs>(SdpApp.EventsManager.OptionsViewEvents.OptionValueChanged, this, optionValueChangedEventArgs);
					SdpApp.AnalyticsManager.TrackOptions(name, value);
				}
			}
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0001C7F4 File Offset: 0x0001A9F4
		private void ConnectionEvents_OptionCategoryAdded(object sender, OptionCategoryAddedEventArgs args)
		{
			uint providerId = args.ProviderId;
			OptionCategory optionCategory = args.OptionCategory;
			if (optionCategory != null)
			{
				object propsLock = this.m_propsLock;
				lock (propsLock)
				{
					foreach (uint num in this.m_props.Keys)
					{
						List<PropertyDescriptor> list = null;
						if (this.m_props.TryGetValue(num, out list))
						{
							PropertyDescriptor propertyDescriptor = list.Find((PropertyDescriptor x) => x.DisplayName == "Uncategorized");
							if (propertyDescriptor != null)
							{
								List<PropertyDescriptor> deleteList = new List<PropertyDescriptor>();
								deleteList.Add(propertyDescriptor);
								List<Option> list2 = new List<Option>();
								foreach (PropertyDescriptor propertyDescriptor2 in list)
								{
									Option option = SdpApp.ConnectionManager.GetOption(propertyDescriptor2.Name, num);
									if (option != null && optionCategory.GetID() == option.GetCategory().GetID())
									{
										option.RegisterOptionChangeHandler(this.m_optionValueUpdatedFunctionPointer);
										if (!option.IsOptionHidden() && !option.IsOptionContextState() && !option.IsOptionProcInfo())
										{
											list2.Add(option);
										}
										deleteList.Add(propertyDescriptor2);
									}
								}
								list.RemoveAll((PropertyDescriptor x) => deleteList.Contains(x));
								foreach (Option option2 in list2)
								{
									this.AddOptionPropertyDescriptor(option2);
								}
								if (deleteList.Count > 0 || list2.Count > 0)
								{
									this.PopulateOptions();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0001CA40 File Offset: 0x0001AC40
		private void ConnectionEvents_OptionAdded(object sender, OptionEventArgs args)
		{
			if (args != null)
			{
				uint optionId = args.OptionId;
				uint processId = args.ProcessId;
				Option option = SdpApp.ConnectionManager.GetOption(optionId, processId);
				if (option != null)
				{
					HashSet<uint> hashSet = null;
					if (!this.m_optionIDs.TryGetValue(processId, out hashSet))
					{
						hashSet = new HashSet<uint>();
						this.m_optionIDs.Add(processId, hashSet);
					}
					if (!hashSet.Contains(optionId))
					{
						option.RegisterOptionChangeHandler(this.m_optionValueUpdatedFunctionPointer);
						hashSet.Add(optionId);
						if (!option.IsOptionHidden() && !option.IsOptionContextState() && !option.IsOptionProcInfo())
						{
							this.AddOptionPropertyDescriptor(option);
							this.PopulateOptions();
						}
					}
				}
			}
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0001CAD8 File Offset: 0x0001ACD8
		private void ConnectionEvents_ProcessListChanged(object sender, EventArgs e)
		{
			if (SdpApp.ConnectionManager.IsConnected())
			{
				ProcessList processes = SdpApp.ConnectionManager.GetProcesses();
				if (processes != null && processes.Count > 0)
				{
					List<uint> list = new List<uint>();
					object propsLock = this.m_propsLock;
					lock (propsLock)
					{
						foreach (uint num in this.m_props.Keys)
						{
							if (num != 4294967295U)
							{
								bool flag2 = false;
								foreach (Process process in processes)
								{
									ProcessProperties properties = process.GetProperties();
									if (num == properties.pid && properties.state != ProcessState.ProcessDead)
									{
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									list.Add(num);
								}
							}
						}
						foreach (uint num2 in list)
						{
							this.m_props.Remove(num2);
							this.m_optionIDs.Remove(num2);
						}
					}
					if (list.Count > 0)
					{
						this.PopulateOptions();
					}
				}
			}
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0001CC54 File Offset: 0x0001AE54
		private void ConnectionEvents_ProcessRemoved(object o, ProcessEventArgs e)
		{
			object propsLock = this.m_propsLock;
			lock (propsLock)
			{
				this.m_props.Remove(e.PID);
				this.m_optionIDs.Remove(e.PID);
				this.PopulateOptions();
			}
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0001CCB8 File Offset: 0x0001AEB8
		private void dataSourceViewEvents_InvalidateSelectedProcesses(object sender, SelectedProcessChangedArgs args)
		{
			List<string> list = new List<string> { "System" };
			if (args.SelectedProcesses != null && args.SelectedProcesses.Count > 0)
			{
				this.m_dataSourceProcess = args.SelectedProcesses[0].Name;
				list.Add(this.m_dataSourceProcess);
			}
			else
			{
				this.m_dataSourceProcess = "";
			}
			this.m_view.SetCategoryFilters(list);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0001CD28 File Offset: 0x0001AF28
		private void m_view_ShowAllProcessToggled(object sender, EventArgs e)
		{
			List<string> list = new List<string> { "System" };
			if (!string.IsNullOrEmpty(this.m_dataSourceProcess))
			{
				list.Add(this.m_dataSourceProcess);
			}
			this.m_view.SetCategoryFilters(list);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001CD6C File Offset: 0x0001AF6C
		private void PopulateOptions()
		{
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			object propsLock = this.m_propsLock;
			lock (propsLock)
			{
				list = this.m_props.Values.SelectMany((List<PropertyDescriptor> x) => x).ToList<PropertyDescriptor>();
				if (list.Count > 0)
				{
					PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
					propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
					this.m_view.SetCurrentObject(propertyGridDescriptionObject);
				}
				else
				{
					this.m_view.SetCurrentObject(null);
				}
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0001CE14 File Offset: 0x0001B014
		public static string GetCategoryName(Option option)
		{
			string text = null;
			if (option != null)
			{
				text = "Uncategorized";
				OptionCategory category = option.GetCategory();
				if (category != null)
				{
					text = category.GetName();
				}
			}
			return text;
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0001CE40 File Offset: 0x0001B040
		public static string GetParentCategoryName(Option option)
		{
			string text = null;
			if (option != null)
			{
				text = "Uncategorized";
				OptionCategory category = option.GetCategory();
				if (category != null)
				{
					OptionCategory parentCategory = category.GetParentCategory();
					if (parentCategory != null)
					{
						text = string.Format("{0}", parentCategory.GetName());
					}
				}
			}
			return text;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0001CE80 File Offset: 0x0001B080
		private void AddOptionPropertyDescriptor(Option option)
		{
			if (option == null)
			{
				return;
			}
			uint processId = option.GetProcessId();
			string categoryName = OptionsViewController.GetCategoryName(option);
			string parentCategoryName = OptionsViewController.GetParentCategoryName(option);
			List<PropertyDescriptor> list = null;
			object propsLock = this.m_propsLock;
			bool flag = false;
			try
			{
				Monitor.Enter(propsLock, ref flag);
				string procName = null;
				if (processId == 4294967295U)
				{
					procName = "System";
				}
				else
				{
					Process processByID = SdpApp.ConnectionManager.GetProcessByID(processId);
					procName = ((processByID != null) ? processByID.GetProperties().name : "<Unknown>");
				}
				if (!this.m_props.TryGetValue(processId, out list))
				{
					list = new List<PropertyDescriptor>();
					this.m_props.Add(processId, list);
				}
				if (list.Find((PropertyDescriptor x) => x.DisplayName == procName) == null)
				{
					list.Add(new SdpPropertyDescriptorCategory(procName, "", processId));
				}
				if (parentCategoryName.Equals("Uncategorized"))
				{
					parentCategoryName = procName;
				}
				else if (list.Find((PropertyDescriptor x) => x.DisplayName == parentCategoryName) == null)
				{
					list.Add(new SdpPropertyDescriptorCategory(parentCategoryName, procName, processId));
				}
				if (list.Find((PropertyDescriptor x) => x.DisplayName == categoryName) == null)
				{
					list.Add(new SdpPropertyDescriptorCategory(categoryName, parentCategoryName, processId));
				}
				if (list != null)
				{
					PropertyDescriptor propertyDescriptor = FormatHelper.CreatePropertyDescriptorFromOption(option);
					if (propertyDescriptor != null)
					{
						list.Add(propertyDescriptor);
						this.m_view.AddOption(propertyDescriptor);
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(propsLock);
				}
			}
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0001D030 File Offset: 0x0001B230
		private void OnOptionValueChanged(uint providerID, uint optionID, uint processID)
		{
			Option option = SdpApp.ConnectionManager.GetOption(optionID, processID);
			if (option != null)
			{
				string optionName = option.GetName();
				object optionValue = OptionValueRetriever.GetOptionValue(option);
				List<PropertyDescriptor> list = null;
				object propsLock = this.m_propsLock;
				lock (propsLock)
				{
					if (this.m_props.TryGetValue(option.GetProcessId(), out list))
					{
						PropertyDescriptor propertyDescriptor = list.Find((PropertyDescriptor x) => x.Name == optionName);
						if (propertyDescriptor != null && !propertyDescriptor.GetValue(null).Equals(optionValue))
						{
							propertyDescriptor.SetValue(null, optionValue);
							this.m_view.RefreshPropertyGrid();
						}
					}
				}
				OptionValueChangedEventArgs optionValueChangedEventArgs = new OptionValueChangedEventArgs();
				optionValueChangedEventArgs.NewValue = optionValue;
				optionValueChangedEventArgs.OptionName = optionName;
				optionValueChangedEventArgs.IsLocalChange = false;
				optionValueChangedEventArgs.Pid = option.GetProcessId();
				SdpApp.EventsManager.Raise<OptionValueChangedEventArgs>(SdpApp.EventsManager.OptionsViewEvents.OptionValueChanged, this, optionValueChangedEventArgs);
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0001D13C File Offset: 0x0001B33C
		private void OptionsViewEvent_DisableView(object sender, EventArgs args)
		{
			this.m_view.DisableView(true);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0001D14A File Offset: 0x0001B34A
		private void OptionsViewEvent_EnableView(object sender, EventArgs args)
		{
			this.m_view.DisableView(false);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0001D158 File Offset: 0x0001B358
		public PropertyGridDescriptionObject CreateTest()
		{
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<string>("Test String", typeof(string), "Test String Value", "Test Category 1", "Test String Description", false);
			list.Add(propertyDescriptor);
			PropertyDescriptor propertyDescriptor2 = new SdpPropertyDescriptor<string>("Test String Readonly", typeof(string), "Test String Readonly Value", "Test Category 1", "Test String Readonly Description", true);
			list.Add(propertyDescriptor2);
			PropertyDescriptor propertyDescriptor3 = new SdpPropertyDescriptor<bool>("Test Bool", typeof(bool), false, "Test Category 2", "Test Bool Description", false);
			list.Add(propertyDescriptor3);
			PropertyDescriptor propertyDescriptor4 = new SdpPropertyDescriptor<OptionsViewController.TestEnum>("Test Enum", typeof(OptionsViewController.TestEnum), OptionsViewController.TestEnum.TestEnum1, "Test Category 3", "Test Enum Description", false);
			list.Add(propertyDescriptor4);
			PropertyDescriptor propertyDescriptor5 = new SdpPropertyDescriptor<OptionsViewController.MultiHue>("Test Flags", typeof(OptionsViewController.MultiHue), OptionsViewController.MultiHue.Blue, "Test Category 3", "Test Flags Description", false);
			list.Add(propertyDescriptor5);
			PropertyDescriptor propertyDescriptor6 = new SdpPropertyDescriptor<int>("Test Integer 1", typeof(int), 4, "Test Category 3", "Test Integer Description", false);
			list.Add(propertyDescriptor6);
			PropertyDescriptor propertyDescriptor7 = new SdpRangedPropertyDescriptor<int>("Test Integer Ranged", typeof(int), 4, "Test Category 3", "Test Integer Ranged Description", false, 0, 255);
			list.Add(propertyDescriptor7);
			PropertyDescriptor propertyDescriptor8 = new SdpPropertyDescriptor<double>("Test Double 1", typeof(double), 15.3, "Test Category 3", "Test Double Description", false);
			list.Add(propertyDescriptor8);
			propertyGridDescriptionObject.AddPropertyGridDescriptors(list);
			return propertyGridDescriptionObject;
		}

		// Token: 0x0400085D RID: 2141
		private IOptionsView m_view;

		// Token: 0x0400085E RID: 2142
		private Void_UInt_UInt_UInt_Fn m_optionValueUpdatedFunctionPointer;

		// Token: 0x0400085F RID: 2143
		private object m_propsLock = new object();

		// Token: 0x04000860 RID: 2144
		private Dictionary<uint, List<PropertyDescriptor>> m_props = new Dictionary<uint, List<PropertyDescriptor>>();

		// Token: 0x04000861 RID: 2145
		private Dictionary<uint, HashSet<uint>> m_optionIDs = new Dictionary<uint, HashSet<uint>>();

		// Token: 0x04000862 RID: 2146
		private string m_dataSourceProcess;

		// Token: 0x020003AF RID: 943
		public enum TestEnum
		{
			// Token: 0x04000D04 RID: 3332
			TestEnum1,
			// Token: 0x04000D05 RID: 3333
			TestEnum2,
			// Token: 0x04000D06 RID: 3334
			TestEnum3,
			// Token: 0x04000D07 RID: 3335
			TestEnum4
		}

		// Token: 0x020003B0 RID: 944
		[Flags]
		private enum MultiHue
		{
			// Token: 0x04000D09 RID: 3337
			None = 0,
			// Token: 0x04000D0A RID: 3338
			Black = 1,
			// Token: 0x04000D0B RID: 3339
			Red = 2,
			// Token: 0x04000D0C RID: 3340
			Green = 4,
			// Token: 0x04000D0D RID: 3341
			Blue = 8,
			// Token: 0x04000D0E RID: 3342
			Cyan = 16,
			// Token: 0x04000D0F RID: 3343
			Pink = 32
		}
	}
}
