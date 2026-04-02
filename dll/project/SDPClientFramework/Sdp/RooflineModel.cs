using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using RulesEngine;
using RulesEngine.Models;
using Sdp.Logging;

namespace Sdp
{
	// Token: 0x020001FE RID: 510
	public class RooflineModel
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x0001489C File Offset: 0x00012A9C
		public RooflineModel()
		{
			RooflineEvents rooflineEvents = SdpApp.EventsManager.RooflineEvents;
			rooflineEvents.SetPeaksEvent = (EventHandler<RooflinePeaksEventArgs>)Delegate.Combine(rooflineEvents.SetPeaksEvent, new EventHandler<RooflinePeaksEventArgs>(this.rooflineEvents_SetPeaksData));
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x000148FB File Offset: 0x00012AFB
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x00014903 File Offset: 0x00012B03
		public float BaseRooflinePerf { get; private set; } = 1f;

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x0001490C File Offset: 0x00012B0C
		// (set) Token: 0x06000783 RID: 1923 RVA: 0x00014914 File Offset: 0x00012B14
		public float PeakRooflinePerf { get; private set; } = 1f;

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x0001491D File Offset: 0x00012B1D
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x00014925 File Offset: 0x00012B25
		public float PeakRooflineMemBW { get; private set; } = 1f;

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x0001492E File Offset: 0x00012B2E
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x00014936 File Offset: 0x00012B36
		public long MaxGPUFrequency { get; private set; }

		// Token: 0x06000788 RID: 1928 RVA: 0x00014940 File Offset: 0x00012B40
		public void GetRuleViolations(object kernel, object thresholds, out List<RooflineModel.RuleViolation> violations)
		{
			violations = new List<RooflineModel.RuleViolation>();
			object[] array = new object[] { kernel, thresholds };
			if (this.m_rulesEngine == null)
			{
				this.LoadRooflineRules();
				if (this.m_rulesEngine == null)
				{
					return;
				}
			}
			List<RuleResultTree> result = this.m_rulesEngine.ExecuteAllRulesAsync("Roofline", array).Result;
			foreach (RuleResultTree ruleResultTree in result)
			{
				if (!ruleResultTree.IsSuccess)
				{
					RooflineModel.RuleViolation ruleViolation = new RooflineModel.RuleViolation();
					ruleViolation.Name = ruleResultTree.Rule.RuleName;
					ruleViolation.ErrorMessage = ruleResultTree.Rule.ErrorMessage;
					if (ruleViolation.ErrorMessage.ToLower().Contains("error"))
					{
						ruleViolation.Severity = RooflineModel.ViolationSeverity.Error;
					}
					else
					{
						ruleViolation.Severity = RooflineModel.ViolationSeverity.Warning;
					}
					violations.Add(ruleViolation);
				}
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00014A38 File Offset: 0x00012C38
		public static string GetNameString(string name, uint kernel)
		{
			return name + " 0x" + kernel.ToString("X");
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00014A54 File Offset: 0x00012C54
		private void LoadRooflineRules()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (executingAssembly != null)
			{
				try
				{
					using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("Sdp.Resources.SDPRooflineRules.json"))
					{
						if (manifestResourceStream != null)
						{
							using (StreamReader streamReader = new StreamReader(manifestResourceStream))
							{
								string text = streamReader.ReadToEnd();
								List<Workflow> list = JsonConvert.DeserializeObject<List<Workflow>>(text);
								this.m_rulesEngine = new global::RulesEngine.RulesEngine(list.ToArray(), null);
								goto IL_0065;
							}
						}
						RooflineModel.Logger.LogError("Error loading GPGPU Roofline rules");
						IL_0065:;
					}
				}
				catch (Exception ex)
				{
					RooflineModel.Logger.LogError("Error loading GPGPU Roofline rules: " + ex.ToString());
				}
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00014B1C File Offset: 0x00012D1C
		private void rooflineEvents_SetPeaksData(object sender, RooflinePeaksEventArgs args)
		{
			this.BaseRooflinePerf = args.PeakRooflinePerf;
			this.PeakRooflinePerf = args.PeakRooflinePerf * (float)args.MaxGPUFrequency / 1E+09f;
			this.PeakRooflineMemBW = args.PeakRooflineMemBW;
			this.MaxGPUFrequency = args.MaxGPUFrequency;
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x00014B5C File Offset: 0x00012D5C
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x00014B63 File Offset: 0x00012D63
		private static ILogger Logger { get; set; } = new Sdp.Logging.Logger("RooflineModel");

		// Token: 0x04000743 RID: 1859
		private global::RulesEngine.RulesEngine m_rulesEngine;

		// Token: 0x0200039D RID: 925
		public enum ViolationSeverity
		{
			// Token: 0x04000CC7 RID: 3271
			Info,
			// Token: 0x04000CC8 RID: 3272
			Warning,
			// Token: 0x04000CC9 RID: 3273
			Error
		}

		// Token: 0x0200039E RID: 926
		public class RuleViolation
		{
			// Token: 0x170002FD RID: 765
			// (get) Token: 0x06001210 RID: 4624 RVA: 0x000389A8 File Offset: 0x00036BA8
			// (set) Token: 0x06001211 RID: 4625 RVA: 0x000389B0 File Offset: 0x00036BB0
			public string Name { get; set; }

			// Token: 0x170002FE RID: 766
			// (get) Token: 0x06001212 RID: 4626 RVA: 0x000389B9 File Offset: 0x00036BB9
			// (set) Token: 0x06001213 RID: 4627 RVA: 0x000389C1 File Offset: 0x00036BC1
			public string ErrorMessage { get; set; }

			// Token: 0x04000CCA RID: 3274
			public RooflineModel.ViolationSeverity Severity;
		}
	}
}
