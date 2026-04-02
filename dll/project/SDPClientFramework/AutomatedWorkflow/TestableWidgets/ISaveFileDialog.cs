using System;
using System.Threading.Tasks;
using Sdp.Functional;

namespace SDPClientFramework.AutomatedWorkflow.TestableWidgets
{
	// Token: 0x02000050 RID: 80
	public interface ISaveFileDialog : ITestableWidget
	{
		// Token: 0x060001AA RID: 426
		Task<Result<string>> SetFilePath(string path);
	}
}
