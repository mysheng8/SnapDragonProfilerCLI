using System;
using System.Threading.Tasks;

namespace SDPClientFramework.AutomatedWorkflow.TestableWidgets
{
	// Token: 0x0200004E RID: 78
	public interface IClickableWidget : ITestableWidget
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001A5 RID: 421
		Task<string> Label { get; }

		// Token: 0x060001A6 RID: 422
		void Click();

		// Token: 0x060001A7 RID: 423
		void Press();

		// Token: 0x060001A8 RID: 424
		void Release();
	}
}
