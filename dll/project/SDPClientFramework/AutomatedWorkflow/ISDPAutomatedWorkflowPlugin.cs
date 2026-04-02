using System;
using System.Threading.Tasks;
using Sdp.Functional;
using Sdp.Logging;
using SDPClientFramework.AutomatedWorkflow.TestableWidgets;

namespace SDPClientFramework.AutomatedWorkflow
{
	// Token: 0x0200004B RID: 75
	public interface ISDPAutomatedWorkflowPlugin
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600018F RID: 399
		string Name { get; }

		// Token: 0x1700002C RID: 44
		// (set) Token: 0x06000190 RID: 400
		ILogger Logger { set; }

		// Token: 0x06000191 RID: 401
		void AddTestableWidget(string name, ITestableWidget widget);

		// Token: 0x06000192 RID: 402
		void RemoveTestableWidget(string name);

		// Token: 0x06000193 RID: 403
		void OnCoreInitialized();

		// Token: 0x06000194 RID: 404
		Result<string> Init(string[] arguments);

		// Token: 0x06000195 RID: 405
		Task<Result<string>> Execute();
	}
}
