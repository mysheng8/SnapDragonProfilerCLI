using System;
using System.Collections.Generic;
using System.IO;

namespace Sdp
{
	// Token: 0x0200020F RID: 527
	public class TreeModel
	{
		// Token: 0x060007CB RID: 1995 RVA: 0x000154DB File Offset: 0x000136DB
		public TreeModel(Type[] types)
		{
			this.Nodes = new List<TreeNode>();
			this.ColumnTypes = types;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000154F8 File Offset: 0x000136F8
		public static string ConvertToGTKString(string str)
		{
			string text = "";
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '_')
				{
					text += "__";
				}
				else
				{
					text += str[i].ToString();
				}
			}
			return text;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001554C File Offset: 0x0001374C
		public TreeNode FindNode(object[] values, int index)
		{
			foreach (TreeNode treeNode in this.Nodes)
			{
				TreeNode treeNode2 = treeNode.FindNode(values, index);
				if (treeNode2 != null)
				{
					return treeNode2;
				}
			}
			return null;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000155AC File Offset: 0x000137AC
		public void ExportToCSV(StreamWriter sw)
		{
			SdpApp.AnalyticsManager.TrackExport("Statistics");
			if (this.ColumnNames != null)
			{
				sw.WriteLine(string.Join(",", this.ColumnNames));
			}
			if (this.Nodes != null)
			{
				foreach (TreeNode treeNode in this.Nodes)
				{
					this.ExportTreeNodeToCSV(sw, treeNode);
				}
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00015638 File Offset: 0x00013838
		private void ExportTreeNodeToCSV(StreamWriter sw, TreeNode node)
		{
			if (node.Values != null)
			{
				if (node.Values[0] == null)
				{
					node.Values[0] = string.Empty;
				}
				sw.WriteLine(string.Join(",", node.Values));
			}
			if (node.Children != null)
			{
				foreach (TreeNode treeNode in node.Children)
				{
					this.ExportTreeNodeToCSV(sw, treeNode);
				}
			}
		}

		// Token: 0x0400076D RID: 1901
		public static TreeModel Empty = new TreeModel(new Type[0]);

		// Token: 0x0400076E RID: 1902
		public List<TreeNode> Nodes;

		// Token: 0x0400076F RID: 1903
		public Type[] ColumnTypes;

		// Token: 0x04000770 RID: 1904
		public string[] ColumnNames;

		// Token: 0x04000771 RID: 1905
		public bool UpdateFilters;

		// Token: 0x04000772 RID: 1906
		public TreeNode InitialSelectedNode;

		// Token: 0x04000773 RID: 1907
		public TreeModel.ContainerType Container;

		// Token: 0x0200039F RID: 927
		public enum ParserType
		{
			// Token: 0x04000CCE RID: 3278
			JSON,
			// Token: 0x04000CCF RID: 3279
			CSV,
			// Token: 0x04000CD0 RID: 3280
			STRING
		}

		// Token: 0x020003A0 RID: 928
		public enum ContainerType
		{
			// Token: 0x04000CD2 RID: 3282
			Tree,
			// Token: 0x04000CD3 RID: 3283
			List
		}
	}
}
