using System;

namespace SDPClientFramework.AutomatedWorkflow.TestableWidgets
{
	// Token: 0x02000053 RID: 83
	public class TreeViewPath
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x0000640D File Offset: 0x0000460D
		public static implicit operator TreeViewPath(string[] path)
		{
			return new TreeViewPath
			{
				Path = path
			};
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000641B File Offset: 0x0000461B
		public override string ToString()
		{
			return string.Join(":", this.Path);
		}

		// Token: 0x04000144 RID: 324
		public string[] Path;
	}
}
