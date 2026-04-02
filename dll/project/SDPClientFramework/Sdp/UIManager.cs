using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Sdp.Functional;
using Sdp.Logging;
using SDPClientFramework.AutomatedWorkflow;
using SDPClientFramework.AutomatedWorkflow.TestableWidgets;

namespace Sdp
{
	// Token: 0x02000289 RID: 649
	public class UIManager
	{
		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06000B52 RID: 2898 RVA: 0x00021094 File Offset: 0x0001F294
		// (remove) Token: 0x06000B53 RID: 2899 RVA: 0x000210CC File Offset: 0x0001F2CC
		public event EventHandler MainWindowClosing;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06000B54 RID: 2900 RVA: 0x00021104 File Offset: 0x0001F304
		// (remove) Token: 0x06000B55 RID: 2901 RVA: 0x0002113C File Offset: 0x0001F33C
		public event EventHandler MainWindowClosed;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06000B56 RID: 2902 RVA: 0x00021174 File Offset: 0x0001F374
		// (remove) Token: 0x06000B57 RID: 2903 RVA: 0x000211AC File Offset: 0x0001F3AC
		public event EventHandler LayoutsChanged;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06000B58 RID: 2904 RVA: 0x000211E4 File Offset: 0x0001F3E4
		// (remove) Token: 0x06000B59 RID: 2905 RVA: 0x0002121C File Offset: 0x0001F41C
		public event EventHandler SelectedLayoutChanged;

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06000B5A RID: 2906 RVA: 0x00021254 File Offset: 0x0001F454
		// (remove) Token: 0x06000B5B RID: 2907 RVA: 0x0002128C File Offset: 0x0001F48C
		public event EventHandler<LayoutRenamedEventArgs> LayoutRenamed;

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x000212C4 File Offset: 0x0001F4C4
		// (set) Token: 0x06000B5D RID: 2909 RVA: 0x000212EC File Offset: 0x0001F4EC
		public string SelectedLayout
		{
			get
			{
				string text = "";
				if (this.m_dockHost != null)
				{
					text = this.m_dockHost.CurrentLayout;
				}
				return text;
			}
			set
			{
				SdpApp.Platform.Invoke(delegate
				{
					this.SetSelectedLayout(value, false);
				});
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00021323 File Offset: 0x0001F523
		public List<string> Layouts
		{
			get
			{
				return this.m_layouts;
			}
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0002132C File Offset: 0x0001F52C
		public UIManager()
		{
			this.RegisterViewProviders();
			this.RegisterDialogProviders();
			this.RegisterPolymorphicXmlTypes();
			PluginEvents pluginEvents = SdpApp.EventsManager.PluginEvents;
			pluginEvents.RegisterToolPlugin = (EventHandler)Delegate.Combine(pluginEvents.RegisterToolPlugin, new EventHandler(this.pluginEvents_RegisterToolPlugin));
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00021410 File Offset: 0x0001F610
		public void AddAutomatedWorkflowExecutor(AutomatedWorkflowExecutor testRunner)
		{
			this.m_testRunner = new Maybe<AutomatedWorkflowExecutor>.Some(testRunner);
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00021420 File Offset: 0x0001F620
		public void CacheTestableWidget(string name, ITestableWidget widget)
		{
			this.m_testRunner.IfSome(delegate(AutomatedWorkflowExecutor testRunner)
			{
				testRunner.AddTestableWidget(name, widget);
			});
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00021458 File Offset: 0x0001F658
		public void UnCacheTestableWidget(string name)
		{
			this.m_testRunner.IfSome(delegate(AutomatedWorkflowExecutor testRunner)
			{
				testRunner.RemoveTestableWidget(name);
			});
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00021489 File Offset: 0x0001F689
		public void LoadMainWindow()
		{
			if (this.m_mainWindow == null)
			{
				this.CreateMainWindow();
				if (this.m_mainWindow != null)
				{
					this.LoadUI();
					this.ToggleFloatingWindows(false);
					this.SelectedLayout = "Connect";
				}
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x000214BC File Offset: 0x0001F6BC
		public bool ShowMainWindow()
		{
			bool flag = false;
			if (this.m_mainWindow != null)
			{
				this.m_mainWindow.ShowView();
				this.ToggleFloatingWindows(true);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x000214E8 File Offset: 0x0001F6E8
		public void pluginEvents_RegisterToolPlugin(object sender, EventArgs e)
		{
			IToolPlugin toolPlugin = sender as IToolPlugin;
			LaunchToolCommand launchToolCommand = new LaunchToolCommand();
			launchToolCommand.ToolPlugin = toolPlugin;
			MenuItemController menuItemController = new MenuItemController(toolPlugin.Name, launchToolCommand);
			this.m_mainWindow.AddMenuItem(MenuHeader.Tools, MenuType.Standard, menuItemController, "");
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0002152C File Offset: 0x0001F72C
		public IDialog CreateDialog(string dialogType)
		{
			IDialog dialog = null;
			IDialogProvider dialogProvider = null;
			if (this.m_dialogProviders.TryGetValue(dialogType, out dialogProvider))
			{
				dialog = dialogProvider.CreateDialog();
			}
			return dialog;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00021558 File Offset: 0x0001F758
		public IViewController CreateViewController(string viewType, string uniqueID = null, CaptureType captureType = CaptureType.INVALID)
		{
			IViewController viewController = null;
			IViewProvider viewProvider = null;
			if (this.m_viewProviders.TryGetValue(viewType, out viewProvider))
			{
				IMultiViewProvider multiViewProvider = viewProvider as IMultiViewProvider;
				if (multiViewProvider != null)
				{
					multiViewProvider.UniqueID = uniqueID;
				}
				ICaptureViewProvider captureViewProvider = viewProvider as ICaptureViewProvider;
				if (captureViewProvider != null)
				{
					captureViewProvider.CaptureType = captureType;
				}
				viewController = viewProvider.CreateView();
			}
			return viewController;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000215A8 File Offset: 0x0001F7A8
		public void RenameLayout(string currentLayoutName, string newLayoutName)
		{
			if (!string.IsNullOrEmpty(currentLayoutName) && !string.IsNullOrEmpty(newLayoutName) && this.m_dockHost != null && this.m_layouts.Contains(currentLayoutName))
			{
				this.m_dockHost.RenameLayout(currentLayoutName, newLayoutName);
				for (int i = 0; i < this.m_layouts.Count; i++)
				{
					if (this.m_layouts[i] == currentLayoutName)
					{
						this.m_layouts[i] = newLayoutName;
					}
				}
				if (this.LayoutRenamed != null)
				{
					LayoutRenamedEventArgs layoutRenamedEventArgs = new LayoutRenamedEventArgs(currentLayoutName, newLayoutName);
					this.LayoutRenamed(this, layoutRenamedEventArgs);
				}
			}
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002163C File Offset: 0x0001F83C
		public void ConfirmResetLayouts()
		{
			new ShowMessageDialogCommand
			{
				Message = "Are you sure you want to reset all the layouts to the original state?",
				IconType = IconType.Warning,
				ButtonLayout = ButtonLayout.YesNo,
				OnCompleted = delegate(bool res)
				{
					if (res)
					{
						this.ResetLayouts();
					}
				}
			}.Execute();
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00021680 File Offset: 0x0001F880
		public IViewController CreateCaptureWindowTabbedWith(string title, string typeName, string tabWindowName, bool focus, string layout, bool canClose = true)
		{
			IViewController viewController = this.CreateViewController(typeName, null, this.GetCaptureTypeForLayout(typeName, layout));
			if (viewController != null)
			{
				this.m_viewControllers.Add(viewController);
				IDockHost dockingHost = this.m_mainWindow.DockingHost;
				if (dockingHost != null)
				{
					IView view = viewController.View;
					this.SetSelectedLayout(layout, false);
					IDockWindow dockWindow = dockingHost.CreateDockWindowTabbedWith(title, view, tabWindowName, layout, canClose, true);
					if (dockWindow != null)
					{
						this.m_windows[title] = dockWindow;
						WindowMenuItem windowMenuItem = new WindowMenuItem(dockWindow.Name, new FocusWindowCommand(dockWindow));
						this.m_mainWindow.AddMenuItem(MenuHeader.Window, MenuType.Standard, windowMenuItem, "");
						this.m_mainWindow.EnableMenuItems(MenuHeader.View);
						this.m_mainWindow.EnableMenuItems(MenuHeader.Layout);
						CaptureWindowEventArgs captureWindowEventArgs = new CaptureWindowEventArgs
						{
							Window = dockWindow
						};
						SdpApp.EventsManager.Raise<CaptureWindowEventArgs>(SdpApp.EventsManager.ClientEvents.CaptureWindowAdded, viewController, captureWindowEventArgs);
						dockWindow.IsVisibleChanged += delegate(object sender, EventArgs e)
						{
							CaptureWindowEventArgs captureWindowEventArgs2 = new CaptureWindowEventArgs();
							captureWindowEventArgs2.Window = sender as IDockWindow;
							SdpApp.EventsManager.Raise<CaptureWindowEventArgs>(SdpApp.EventsManager.ClientEvents.WindowVisibilityChanged, sender, captureWindowEventArgs2);
						};
						if (focus)
						{
							foreach (IDockWindow dockWindow2 in this.m_windows.Values)
							{
								if (dockWindow2.IsVisible && dockWindow2.Name != dockWindow.Name)
								{
									dockWindow2.IsVisible = false;
								}
							}
							dockWindow.Focus();
						}
					}
				}
			}
			return viewController;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x000217FC File Offset: 0x0001F9FC
		public void SetWindowName(string windowName, string newName)
		{
			this.m_dockHost = this.m_mainWindow.DockingHost;
			if (this.m_dockHost != null)
			{
				this.m_dockHost.AppendSuffixToWindow(windowName, newName);
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00021824 File Offset: 0x0001FA24
		public bool ContainsWindow(string viewName)
		{
			return this.m_windows.ContainsKey(viewName);
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00021834 File Offset: 0x0001FA34
		public void FocusCaptureWindow(string windowName, string updateLayout = "")
		{
			SdpApp.Platform.Invoke(delegate
			{
				IDockWindow dockWindow;
				if (this.m_windows.TryGetValue(windowName, out dockWindow))
				{
					if (!string.IsNullOrEmpty(updateLayout))
					{
						dockWindow.Layout = updateLayout;
					}
					this.SetSelectedLayout(dockWindow.Layout, false);
					foreach (IDockWindow dockWindow2 in this.m_windows.Values)
					{
						if (dockWindow2.Name != windowName && dockWindow2.IsVisible)
						{
							dockWindow2.IsVisible = false;
						}
					}
					dockWindow.Focus();
					this.m_mainWindow.FocusCaptureWindow(dockWindow.Name);
				}
			});
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00021874 File Offset: 0x0001FA74
		public void PresentView(string typeName, string typeParam = null, bool reparentMessageDialog = false, bool visibleInAllLayouts = false)
		{
			SdpApp.Platform.Invoke(delegate
			{
				if (this.m_views != null)
				{
					DockWindowDesc dockWindowDesc = this.m_uiDescription.DockingWindows.Where((DockWindowDesc i) => i.ViewSettings.TypeName == typeName && i.ViewSettings.TypeParam == typeParam).First<DockWindowDesc>();
					if (dockWindowDesc != null)
					{
						foreach (IDockWindow dockWindow in this.m_views)
						{
							if (string.Compare(dockWindow.Name, dockWindowDesc.Name) == 0)
							{
								dockWindow.IsVisible = true;
								dockWindow.Present();
								if (visibleInAllLayouts)
								{
									dockWindow.SetVisibleInAllLayouts(true);
								}
								if (reparentMessageDialog)
								{
									EventHandler<ReparentMessageDialogArgs> reparentMessageDialog2 = SdpApp.EventsManager.ClientEvents.ReparentMessageDialog;
									if (reparentMessageDialog2 != null)
									{
										reparentMessageDialog2(this, new ReparentMessageDialogArgs
										{
											Widget = dockWindow.View
										});
									}
								}
								break;
							}
						}
					}
				}
			});
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x000218C4 File Offset: 0x0001FAC4
		public string GetWindowNameFromCaptureId(int captureId)
		{
			string text = "";
			foreach (IViewController viewController in this.m_viewControllers)
			{
				if (viewController is SnapshotController && (ulong)((SnapshotController)viewController).CaptureId == (ulong)((long)captureId))
				{
					text = ((SnapshotController)viewController).WindowName;
					break;
				}
				if (viewController is GroupLayoutController && (ulong)((GroupLayoutController)viewController).CaptureId == (ulong)((long)captureId))
				{
					text = ((GroupLayoutController)viewController).WindowName;
					break;
				}
				if (viewController is SamplingController && (ulong)((SamplingController)viewController).CaptureId == (ulong)((long)captureId))
				{
					text = ((SamplingController)viewController).WindowName;
					break;
				}
			}
			return text;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002198C File Offset: 0x0001FB8C
		public bool IsCaptureNameInUse(string newName)
		{
			return this.m_windows.Keys.Contains(newName);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x000219A0 File Offset: 0x0001FBA0
		private void RegisterViewProviders()
		{
			this.m_viewProviders = new ViewProviderRegistry();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				Type typeFromHandle = typeof(IViewProvider);
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (type.IsClass && !type.IsAbstract)
					{
						Type[] interfaces = type.GetInterfaces();
						if (interfaces != null && interfaces.Contains(typeFromHandle))
						{
							IViewProvider viewProvider = Activator.CreateInstance(type) as IViewProvider;
							if (viewProvider != null)
							{
								this.m_viewProviders.Add(viewProvider.ViewTypeName, viewProvider);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00021A60 File Offset: 0x0001FC60
		private void RegisterDialogProviders()
		{
			this.m_dialogProviders = new DialogProviderRegistry();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				Type typeFromHandle = typeof(IDialogProvider);
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (type.IsClass && !type.IsAbstract)
					{
						Type[] interfaces = type.GetInterfaces();
						if (interfaces != null && interfaces.Contains(typeFromHandle))
						{
							IDialogProvider dialogProvider = Activator.CreateInstance(type) as IDialogProvider;
							if (dialogProvider != null)
							{
								this.m_dialogProviders.Add(dialogProvider.DialogTypeName, dialogProvider);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00021B20 File Offset: 0x0001FD20
		private void RegisterPolymorphicXmlTypes()
		{
			this.m_uiXmlTypes = new List<Type>();
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (executingAssembly != null)
			{
				Type[] types = executingAssembly.GetTypes();
				foreach (Type type in types)
				{
					Attribute[] customAttributes = Attribute.GetCustomAttributes(type);
					foreach (Attribute attribute in customAttributes)
					{
						if (attribute is PolymorphicXmlAttribute)
						{
							this.m_uiXmlTypes.Add(type);
						}
					}
				}
			}
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00021BA0 File Offset: 0x0001FDA0
		private void CreateMainWindow()
		{
			this.m_mainWindow = this.CreateViewController("MainWindow", null, (CaptureType)4277009102U) as IMainWindowController;
			if (this.m_mainWindow != null)
			{
				this.m_mainWindow.ViewClosing += this.On_MainWindow_ViewClosing;
				this.m_mainWindow.ViewClosed += this.On_MainWindow_ViewClosed;
				this.m_mainWindow.DockingHost.CaptureWindowFocused += this.dockingHost_CaptureWindowFocused;
			}
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00021C1C File Offset: 0x0001FE1C
		private string GetUISettingsFilename()
		{
			string appDataPath = SdpApp.GetAppDataPath();
			return Path.Combine(appDataPath, "sdp_ui.xml");
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00021C3C File Offset: 0x0001FE3C
		private bool SaveUI()
		{
			string text = this.SaveDockingLayouts(true);
			UIDesc uidesc = new UIDesc();
			uidesc.Version = UIDesc.CurrentVersion;
			uidesc.CurrentLayout = this.SelectedLayout;
			uidesc.DefaultLayout = this.m_defaultLayout;
			uidesc.DockingXml = text;
			this.m_uiDescription.DockingWindows.Sort((DockWindowDesc a, DockWindowDesc b) => a.Name.CompareTo(b.Name));
			foreach (DockWindowDesc dockWindowDesc in this.m_uiDescription.DockingWindows)
			{
				DockWindowDesc dockWindowDesc2 = new DockWindowDesc();
				uidesc.DockingWindows.Add(dockWindowDesc2);
				dockWindowDesc2.Name = dockWindowDesc.Name;
				dockWindowDesc2.ViewSettings = dockWindowDesc.ViewSettings;
			}
			string uisettingsFilename = this.GetUISettingsFilename();
			return this.SaveUI(uidesc, uisettingsFilename);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00021D3C File Offset: 0x0001FF3C
		private bool SaveUI(UIDesc uiDescription, string filename)
		{
			bool flag = false;
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(filename, Encoding.UTF8))
			{
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.Indentation = 4;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(UIDesc), this.m_uiXmlTypes.ToArray());
				try
				{
					xmlSerializer.Serialize(xmlTextWriter, uiDescription);
					flag = true;
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("Error serializing UI: " + ex.ToString());
				}
			}
			return flag;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00021DD0 File Offset: 0x0001FFD0
		private string SaveDockingLayouts(bool exiting)
		{
			string text = "";
			if (this.m_dockHost != null)
			{
				text = this.m_dockHost.SaveLayouts(exiting);
			}
			return text;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00021DFC File Offset: 0x0001FFFC
		private bool LoadUI()
		{
			bool flag = false;
			string uisettingsFilename = this.GetUISettingsFilename();
			if (File.Exists(uisettingsFilename))
			{
				try
				{
					using (FileStream fileStream = new FileStream(uisettingsFilename, FileMode.Open, FileAccess.Read))
					{
						flag = this.LoadUI(fileStream);
					}
					return flag;
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("Error reading UI settings: " + ex.ToString());
					return flag;
				}
			}
			flag = this.LoadDefaultUI();
			return flag;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00021E7C File Offset: 0x0002007C
		private void ParseLayoutChildren()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(this.m_uiDescription.DockingXml);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/layouts/layout");
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlDocument xmlDocument2 = new XmlDocument();
				xmlDocument2.LoadXml(xmlNode.OuterXml);
				XmlNodeList elementsByTagName = xmlDocument2.GetElementsByTagName("item");
				List<string> list = new List<string>();
				foreach (object obj2 in elementsByTagName)
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					XmlAttribute xmlAttribute = xmlNode2.Attributes["id"];
					list.Add(xmlAttribute.Value);
				}
				this.m_layoutChildren.Add(xmlNode.Attributes["name"].Value, list);
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00021FA8 File Offset: 0x000201A8
		private bool LoadUI(Stream uiDescriptionStream)
		{
			bool flag = false;
			using (XmlReader xmlReader = XmlReader.Create(uiDescriptionStream))
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(UIDesc), this.m_uiXmlTypes.ToArray());
					UIDesc uidesc = (UIDesc)xmlSerializer.Deserialize(xmlReader);
					this.m_uiDescription = uidesc;
					if (uidesc.Version != UIDesc.CurrentVersion)
					{
						flag = this.LoadDefaultUI();
					}
					else
					{
						this.RemoveWindows();
						this.RemoveViews();
						this.RemoveLayouts();
						this.ParseLayoutChildren();
						this.AddViewMenuItems();
						this.m_dockHost = this.m_mainWindow.DockingHost;
						if (this.m_dockHost != null)
						{
							this.SetSelectedLayout(uidesc.DefaultLayout ?? uidesc.CurrentLayout, false);
							this.m_layouts = this.m_dockHost.Layouts;
							foreach (string text in this.m_layouts)
							{
								this.AddLayoutMenuItem(text);
							}
							if (this.LayoutsChanged != null)
							{
								this.LayoutsChanged(this, EventArgs.Empty);
							}
							flag = true;
						}
					}
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("Error reading layouts: " + ex.ToString());
				}
			}
			return flag;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00022138 File Offset: 0x00020338
		private bool LoadDefaultUI()
		{
			bool flag = false;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (executingAssembly != null)
			{
				try
				{
					using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("Sdp.Resources.default_sdp_ui.xml"))
					{
						if (manifestResourceStream != null)
						{
							flag = this.LoadUI(manifestResourceStream);
						}
						else
						{
							Console.Error.WriteLine("Error loading default UI settings");
						}
					}
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("Error reading UI settings: " + ex.ToString());
				}
			}
			return flag;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x000221C8 File Offset: 0x000203C8
		public void CreateView(string viewName)
		{
			if (this.m_uiDescription != null)
			{
				DockWindowDescList dockingWindows = this.m_uiDescription.DockingWindows;
				DockWindowDesc dockWindowDesc = dockingWindows.Where((DockWindowDesc i) => i.Name.Equals(viewName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault<DockWindowDesc>();
				if (dockWindowDesc != null)
				{
					ViewDesc viewSettings = dockWindowDesc.ViewSettings;
					if (viewSettings != null)
					{
						IViewController viewController = this.CreateViewController(viewSettings.TypeName, viewSettings.TypeParam, (CaptureType)4277009102U);
						if (viewController != null)
						{
							this.m_viewControllers.Add(viewController);
							viewController.LoadSettings(viewSettings);
							IDockHost dockingHost = this.m_mainWindow.DockingHost;
							if (dockingHost != null)
							{
								IView view = viewController.View;
								IDockWindow dockWindow = dockingHost.CreateDockWindow(viewName, view);
								if (dockWindow != null)
								{
									this.m_views.Add(dockWindow);
									this.UpdateViewMenuItem(dockWindow);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00022298 File Offset: 0x00020498
		private void UpdateViewMenuItem(IDockWindow dockWindow)
		{
			ViewMenuItem viewMenuItem = this.m_viewMenuItems[dockWindow.Name];
			if (viewMenuItem != null)
			{
				viewMenuItem.Command = new ToggleWindowVisibilityCommand(dockWindow);
				dockWindow.IsVisibleChanged += delegate(object sender, EventArgs e)
				{
					CaptureWindowEventArgs captureWindowEventArgs = new CaptureWindowEventArgs();
					captureWindowEventArgs.Window = sender as IDockWindow;
					SdpApp.EventsManager.Raise<CaptureWindowEventArgs>(SdpApp.EventsManager.ClientEvents.WindowVisibilityChanged, sender, captureWindowEventArgs);
				};
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x000222EC File Offset: 0x000204EC
		private void RemoveViews()
		{
			foreach (IDockWindow dockWindow in this.m_views)
			{
				this.m_mainWindow.RemoveMenuItem(MenuHeader.View, dockWindow.Name);
				IDockHost dockingHost = this.m_mainWindow.DockingHost;
				if (dockingHost != null)
				{
					dockingHost.RemoveDockWindow(dockWindow);
				}
			}
			this.m_views.Clear();
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002236C File Offset: 0x0002056C
		private void RemoveWindows()
		{
			foreach (IDockWindow dockWindow in this.m_windows.Values)
			{
				this.m_mainWindow.RemoveMenuItem(MenuHeader.View, dockWindow.Name);
				IDockHost dockingHost = this.m_mainWindow.DockingHost;
				if (dockingHost != null)
				{
					dockingHost.RemoveDockWindow(dockWindow);
				}
			}
			this.m_windows.Clear();
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x000223F4 File Offset: 0x000205F4
		private void RemoveLayouts()
		{
			foreach (string text in this.m_layouts)
			{
				this.m_mainWindow.RemoveMenuItem(MenuHeader.View, text);
				IDockHost dockingHost = this.m_mainWindow.DockingHost;
				if (dockingHost != null)
				{
					dockingHost.RemoveLayout(text);
				}
			}
			this.m_layouts.Clear();
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00022470 File Offset: 0x00020670
		private void AddViewMenuItems()
		{
			DockWindowDescList dockingWindows = this.m_uiDescription.DockingWindows;
			foreach (DockWindowDesc dockWindowDesc in dockingWindows)
			{
				ViewMenuItem viewMenuItem = new ViewMenuItem(dockWindowDesc.Name, null);
				this.m_viewMenuItems.Add(dockWindowDesc.Name, viewMenuItem);
				this.m_mainWindow.AddMenuItem(MenuHeader.View, MenuType.Toggle, viewMenuItem, "");
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x000224F8 File Offset: 0x000206F8
		public void RequestRenameWindow(string oldName, string newName)
		{
			IDockWindow dockWindow;
			if (this.m_windows.TryGetValue(oldName, out dockWindow))
			{
				if (!this.IsCaptureNameInUse(newName))
				{
					this.m_windows[newName] = dockWindow;
					this.m_windows.Remove(dockWindow.Name);
					this.m_mainWindow.RenameMenuItem(dockWindow.Name, newName);
					dockWindow.Name = newName;
					return;
				}
				this.m_logger.LogWarning("Unable to rename " + dockWindow.Name + ": name is already in use");
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00022578 File Offset: 0x00020778
		private void AddLayoutMenuItem(string layoutName)
		{
			if (!string.IsNullOrEmpty(layoutName) && !this.m_hiddenLayouts.Contains(layoutName))
			{
				LayoutMenuItem layoutMenuItem = new LayoutMenuItem(layoutName, new ChangeLayoutCommand(layoutName));
				layoutMenuItem.Active = this.SelectedLayout == layoutName;
				this.m_mainWindow.AddMenuItem(MenuHeader.Layout, MenuType.Radio, layoutMenuItem, "layouts");
			}
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000225D0 File Offset: 0x000207D0
		public void HideLayoutMenuItem(string layoutName)
		{
			SdpApp.Platform.Invoke(delegate
			{
				this.m_hiddenLayouts.Add(layoutName);
				this.m_mainWindow.RemoveMenuItem(MenuHeader.Layout, layoutName);
			});
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00022608 File Offset: 0x00020808
		public CaptureType GetCaptureTypeForLayout(string typeName, string layout)
		{
			if (layout == "Realtime")
			{
				return CaptureType.Realtime;
			}
			if (layout == "Capture" || layout == "Insight")
			{
				return CaptureType.Trace;
			}
			if (layout == "Snapshot")
			{
				return CaptureType.Snapshot;
			}
			if (layout == "CPU Sampling")
			{
				return CaptureType.Sampling;
			}
			if (typeName == "Snapshot")
			{
				return CaptureType.Snapshot;
			}
			if (typeName == "GroupLayoutView")
			{
				return CaptureType.Trace;
			}
			return (CaptureType)4277009102U;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00022684 File Offset: 0x00020884
		private void CreateLayoutChildren(string layoutName)
		{
			List<string> list;
			if (this.m_layoutChildren.TryGetValue(layoutName, out list))
			{
				using (List<string>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string layoutChild = enumerator.Current;
						if (this.m_views.Where((IDockWindow w) => w.Name.Equals(layoutChild)).Count<IDockWindow>() == 0)
						{
							CreateViewCommand createViewCommand = new CreateViewCommand(layoutChild);
							createViewCommand.Execute();
						}
					}
					return;
				}
			}
			this.ResetLayouts();
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002271C File Offset: 0x0002091C
		private void SetSelectedLayout(string layoutName, bool reset = false)
		{
			if (this.m_dockHost != null && (this.m_dockHost.CurrentLayout != layoutName || reset))
			{
				this.CreateLayoutChildren(layoutName);
				if (!string.IsNullOrEmpty(this.m_dockHost.CurrentLayout))
				{
					this.m_uiDescription.DockingXml = this.SaveDockingLayouts(false);
				}
				this.m_dockHost.LoadLayouts(this.m_uiDescription.DockingXml);
				this.m_dockHost.CurrentLayout = layoutName;
				if (this.m_defaultLayout == null)
				{
					this.m_defaultLayout = layoutName;
				}
				foreach (IDockWindow dockWindow in this.m_views)
				{
					this.m_mainWindow.ToggleMenuItem(MenuHeader.View, dockWindow.Name, dockWindow.IsVisibleInLayout(layoutName));
				}
				if (this.SelectedLayoutChanged != null)
				{
					this.SelectedLayoutChanged(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002281C File Offset: 0x00020A1C
		private void ResetLayouts()
		{
			try
			{
				string uisettingsFilename = this.GetUISettingsFilename();
				if (File.Exists(uisettingsFilename))
				{
					File.Delete(uisettingsFilename);
				}
			}
			catch (Exception)
			{
				this.m_logger.LogError("Unable to remove the current layout file.");
			}
			try
			{
				string text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				text = Path.Combine(text, "SnapdragonProfiler");
				if (Directory.Exists(text))
				{
					Directory.Delete(text, true);
				}
			}
			catch (Exception)
			{
			}
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("Sdp.Resources.default_sdp_ui.xml"))
			{
				using (XmlReader xmlReader = XmlReader.Create(manifestResourceStream))
				{
					try
					{
						this.m_dockHost.PrepareVisibleItems();
						string selectedLayout = this.SelectedLayout;
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(UIDesc), this.m_uiXmlTypes.ToArray());
						UIDesc uidesc = (UIDesc)xmlSerializer.Deserialize(xmlReader);
						this.m_uiDescription = uidesc;
						this.m_dockHost.LoadLayouts(this.m_uiDescription.DockingXml);
						this.m_dockHost.RestoreVisibleItems(this.m_windows.Values, "Start Page", selectedLayout);
						this.m_uiDescription.DockingXml = this.SaveDockingLayouts(false);
						this.SetSelectedLayout(selectedLayout, true);
					}
					catch (Exception)
					{
						this.m_logger.LogError("Unable to reset layouts to the original state.");
					}
				}
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00022998 File Offset: 0x00020B98
		private void ToggleFloatingWindows(bool show)
		{
			if (show)
			{
				this.m_dockHost.ShowFloatingWindows();
				return;
			}
			this.m_dockHost.HideFloatingWindows();
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x000229B4 File Offset: 0x00020BB4
		private void On_MainWindow_ViewClosing(object sender, EventArgs e)
		{
			this.SaveUI();
			if (this.MainWindowClosing != null)
			{
				this.MainWindowClosing(this, EventArgs.Empty);
			}
			this.ToggleFloatingWindows(false);
			ExitAppCommand exitAppCommand = new ExitAppCommand();
			SdpApp.ExecuteCommand(exitAppCommand);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x000229F4 File Offset: 0x00020BF4
		private void On_MainWindow_ViewClosed(object sender, EventArgs e)
		{
			if (this.MainWindowClosed != null)
			{
				this.MainWindowClosed(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00022A0F File Offset: 0x00020C0F
		private void dockingHost_CaptureWindowFocused(object sender, CaptureWindowFocusedArgs e)
		{
			this.FocusCaptureWindow(e.WindowName, "");
		}

		// Token: 0x040008C8 RID: 2248
		private string m_defaultLayout;

		// Token: 0x040008C9 RID: 2249
		private IMainWindowController m_mainWindow;

		// Token: 0x040008CA RID: 2250
		private IDockHost m_dockHost;

		// Token: 0x040008CB RID: 2251
		private List<string> m_layouts = new List<string>();

		// Token: 0x040008CC RID: 2252
		private List<IViewController> m_viewControllers = new List<IViewController>();

		// Token: 0x040008CD RID: 2253
		private List<IDockWindow> m_views = new List<IDockWindow>();

		// Token: 0x040008CE RID: 2254
		private Dictionary<string, IDockWindow> m_windows = new Dictionary<string, IDockWindow>();

		// Token: 0x040008CF RID: 2255
		private List<Type> m_uiXmlTypes = new List<Type>();

		// Token: 0x040008D0 RID: 2256
		private HashSet<string> m_hiddenLayouts = new HashSet<string>();

		// Token: 0x040008D1 RID: 2257
		private ViewProviderRegistry m_viewProviders = new ViewProviderRegistry();

		// Token: 0x040008D2 RID: 2258
		private DialogProviderRegistry m_dialogProviders = new DialogProviderRegistry();

		// Token: 0x040008D3 RID: 2259
		private UIDesc m_uiDescription;

		// Token: 0x040008D4 RID: 2260
		private Dictionary<string, List<string>> m_layoutChildren = new Dictionary<string, List<string>>();

		// Token: 0x040008D5 RID: 2261
		private Dictionary<string, ViewMenuItem> m_viewMenuItems = new Dictionary<string, ViewMenuItem>();

		// Token: 0x040008D6 RID: 2262
		private Maybe<AutomatedWorkflowExecutor> m_testRunner = new Maybe<AutomatedWorkflowExecutor>.None();

		// Token: 0x040008D7 RID: 2263
		private Dictionary<string, LayoutMenuItem> m_layoutMenuItems = new Dictionary<string, LayoutMenuItem>();

		// Token: 0x040008D8 RID: 2264
		private ILogger m_logger = new Sdp.Logging.Logger("UI Manager");
	}
}
