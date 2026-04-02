using System;
using System.IO;
using System.Threading.Tasks;
using Sdp;
using Sdp.Functional;
using Sdp.Logging;
using SDPClientFramework.AutomatedWorkflow.TestableWidgets;

namespace SDPClientFramework.AutomatedWorkflow
{
	// Token: 0x0200004A RID: 74
	public class AutomatedWorkflowExecutor
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00006268 File Offset: 0x00004468
		public AutomatedWorkflowExecutor(ISDPAutomatedWorkflowPlugin workflowPlugin, string[] workflowArgs)
		{
			AutomatedWorkflowExecutor <>4__this = this;
			this.m_workflowPlugin = workflowPlugin;
			Func<Result<string>> <>9__1;
			Action <>9__2;
			Action<string> <>9__3;
			this.m_automatedWorkflow = Task.Factory.StartNew(delegate
			{
				try
				{
					Result<string> result = <>4__this.m_workflowPlugin.Init(workflowArgs);
					Func<Result<string>> func;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = () => <>4__this.m_workflowPlugin.Execute().Result);
					}
					Result<string> result2 = result.Bind(func);
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate
						{
							<>4__this.m_logger.LogInformation("Workflow Executed Successfully");
						});
					}
					Action<string> action2;
					if ((action2 = <>9__3) == null)
					{
						action2 = (<>9__3 = delegate(string error)
						{
							<>4__this.OnWorkflowError(error);
						});
					}
					result2.Match(action, action2);
				}
				catch (Exception ex)
				{
					<>4__this.OnWorkflowError(ex.Message);
				}
			}, TaskCreationOptions.LongRunning);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000062C3 File Offset: 0x000044C3
		private void OnWorkflowError(string error)
		{
			this.m_logger.LogError(error);
			File.WriteAllText(AutomatedWorkflowExecutor.WorkflowErrorFileName, error);
			new ExitAppCommand().Execute();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000062E6 File Offset: 0x000044E6
		public void AddTestableWidget(string name, ITestableWidget testableWidget)
		{
			this.m_workflowPlugin.AddTestableWidget(name, testableWidget);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000062F5 File Offset: 0x000044F5
		public void RemoveTestableWidget(string name)
		{
			this.m_workflowPlugin.RemoveTestableWidget(name);
		}

		// Token: 0x04000136 RID: 310
		private ISDPAutomatedWorkflowPlugin m_workflowPlugin;

		// Token: 0x04000137 RID: 311
		private readonly Task m_automatedWorkflow;

		// Token: 0x04000138 RID: 312
		private ILogger m_logger = new global::Sdp.Logging.Logger("AutomatedWorkflowExecutor");

		// Token: 0x04000139 RID: 313
		public static string WorkflowErrorFileName = "AutomatedWorkflowError.txt";
	}
}
