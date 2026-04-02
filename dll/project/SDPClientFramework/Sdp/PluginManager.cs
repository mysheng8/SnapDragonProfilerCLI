using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sdp
{
	// Token: 0x02000209 RID: 521
	public class PluginManager
	{
		// Token: 0x060007AD RID: 1965 RVA: 0x00014E6C File Offset: 0x0001306C
		public PluginManager()
		{
			this.m_metricPlugins = new List<IMetricPlugin>();
			this.m_toolPlugins = new List<IToolPlugin>();
			try
			{
				string[] files = Directory.GetFiles("plugins", "*.dll");
				if (files != null && files.Length != 0)
				{
					List<Assembly> list = new List<Assembly>();
					foreach (string text in files)
					{
						try
						{
							AssemblyName assemblyName = AssemblyName.GetAssemblyName(text);
							Assembly assembly = Assembly.Load(assemblyName);
							list.Add(assembly);
						}
						catch
						{
						}
					}
					foreach (Assembly assembly2 in list)
					{
						if (assembly2 != null)
						{
							try
							{
								Type[] types = assembly2.GetTypes();
								foreach (Type type in types)
								{
									if (!type.IsInterface && !type.IsAbstract)
									{
										if (type.GetInterface(typeof(IMetricPlugin).FullName) != null)
										{
											IMetricPlugin metricPlugin = (IMetricPlugin)Activator.CreateInstance(type);
											this.m_metricPlugins.Add(metricPlugin);
										}
										if (type.GetInterface(typeof(IToolPlugin).FullName) != null)
										{
											IToolPlugin toolPlugin = (IToolPlugin)Activator.CreateInstance(type);
											this.m_toolPlugins.Add(toolPlugin);
										}
									}
								}
							}
							catch
							{
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001504C File Offset: 0x0001324C
		public IMetricPlugin GetMetricPlugin(MetricDescription metricDesc)
		{
			foreach (IMetricPlugin metricPlugin in this.m_metricPlugins)
			{
				if (metricPlugin.HandlesMetric(metricDesc))
				{
					return metricPlugin;
				}
			}
			return null;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x000150A8 File Offset: 0x000132A8
		public void Shutdown()
		{
			foreach (IMetricPlugin metricPlugin in this.m_metricPlugins)
			{
				metricPlugin.Shutdown();
			}
		}

		// Token: 0x0400075B RID: 1883
		private List<IMetricPlugin> m_metricPlugins;

		// Token: 0x0400075C RID: 1884
		private List<IToolPlugin> m_toolPlugins;
	}
}
