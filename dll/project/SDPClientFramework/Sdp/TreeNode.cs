using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000210 RID: 528
	public class TreeNode
	{
		// Token: 0x060007D1 RID: 2001 RVA: 0x000156DE File Offset: 0x000138DE
		public TreeNode()
		{
			this.Children = new List<TreeNode>();
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000156F4 File Offset: 0x000138F4
		public TreeNode FindNode(object[] values, int index)
		{
			if (Convert.ToInt32(values[index]) == Convert.ToInt32(this.Values[index]))
			{
				return this;
			}
			foreach (TreeNode treeNode in this.Children)
			{
				TreeNode treeNode2 = treeNode.FindNode(values, index);
				if (treeNode2 != null)
				{
					return treeNode2;
				}
			}
			return null;
		}

		// Token: 0x04000774 RID: 1908
		public object[] Values;

		// Token: 0x04000775 RID: 1909
		public List<TreeNode> Children;
	}
}
