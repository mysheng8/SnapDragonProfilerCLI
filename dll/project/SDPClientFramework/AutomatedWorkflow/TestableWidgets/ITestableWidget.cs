using System;
using System.Threading.Tasks;

namespace SDPClientFramework.AutomatedWorkflow.TestableWidgets
{
	// Token: 0x02000051 RID: 81
	public interface ITestableWidget
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001AB RID: 427
		Task<bool> Enabled { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001AC RID: 428
		Task<bool> Visible { get; }
	}
}
