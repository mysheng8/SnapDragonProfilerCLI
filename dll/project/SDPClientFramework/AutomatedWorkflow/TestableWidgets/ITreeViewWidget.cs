using System;
using System.Threading.Tasks;
using Sdp.Functional;

namespace SDPClientFramework.AutomatedWorkflow.TestableWidgets
{
	// Token: 0x02000052 RID: 82
	public interface ITreeViewWidget : ITestableWidget
	{
		// Token: 0x060001AD RID: 429
		Task<Result<string>> TrySelectRootTextNode(string name, int searchColumn);

		// Token: 0x060001AE RID: 430
		Task<Result<string>> TrySelectTextNodeThatStartsWith(TreeViewPath substringPath, int searchColumn);

		// Token: 0x060001AF RID: 431
		Task<bool> ContainsNodeThatStartsWith(TreeViewPath substringPath, int searchColumn);

		// Token: 0x060001B0 RID: 432
		Task<Result<string>> TryExpandNodeThatStartsWith(TreeViewPath substringPath, int searchColumn);

		// Token: 0x060001B1 RID: 433
		Task<Result<string>> TryDoubleClickNodeThatStartsWithSubstring(TreeViewPath substringPath, int searchColumn, int clickColumn);
	}
}
