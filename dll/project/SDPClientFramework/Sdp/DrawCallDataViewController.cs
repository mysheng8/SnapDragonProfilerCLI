using System;
using System.IO;

namespace Sdp
{
	// Token: 0x020002C6 RID: 710
	public class DrawCallDataViewController : IViewController
	{
		// Token: 0x06000E84 RID: 3716 RVA: 0x0002C8D0 File Offset: 0x0002AAD0
		public DrawCallDataViewController(IDrawCallDataView view)
		{
			this.m_view = view;
			this.m_view.ExportVertexDataClicked += this.ExportVertexDataToObj;
			DrawCallDataViewEvents drawCallDataViewEvents = SdpApp.EventsManager.DrawCallDataViewEvents;
			drawCallDataViewEvents.InvalidateViews = (EventHandler<InvalidateViewEventArgs>)Delegate.Combine(drawCallDataViewEvents.InvalidateViews, new EventHandler<InvalidateViewEventArgs>(this.OnInvalidateViews));
			DrawCallDataViewEvents drawCallDataViewEvents2 = SdpApp.EventsManager.DrawCallDataViewEvents;
			drawCallDataViewEvents2.SetStatus = (EventHandler<SetStatusEventArgs>)Delegate.Combine(drawCallDataViewEvents2.SetStatus, new EventHandler<SetStatusEventArgs>(this.OnSetStatus));
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0002C958 File Offset: 0x0002AB58
		private void OnInvalidateViews(object sender, InvalidateViewEventArgs args)
		{
			TreeModel treeModel = null;
			if (args.InidicesModel != null)
			{
				treeModel = this.GenerateElementsModel(args.InidicesModel, args.drawMode);
			}
			this.m_currentVertexModel = args.VertexBufferModel;
			this.m_currentElementModel = treeModel;
			this.m_currentInidiciesModel = args.InidicesModel;
			this.m_view.InvalidateView(args.InidicesModel, treeModel, args.VertexBufferModel);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0002C9B9 File Offset: 0x0002ABB9
		private void OnSetStatus(object sender, SetStatusEventArgs args)
		{
			this.m_view.SetStatus(args.Status, args.StatusText, args.Duration);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0002C9D8 File Offset: 0x0002ABD8
		private TreeModel GenerateElementsModel(TreeModel indicesModel, uint drawMode)
		{
			TreeModel treeModel = new TreeModel(new Type[]
			{
				typeof(string),
				typeof(string)
			});
			treeModel.ColumnNames = new string[2];
			switch (drawMode)
			{
			case 0U:
			{
				treeModel.ColumnNames[1] = "GL__POINTS";
				for (int i = 0; i < indicesModel.Nodes.Count; i++)
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Values = new object[2];
					treeNode.Values[0] = i.ToString();
					treeNode.Values[1] = indicesModel.Nodes[i].Values[1].ToString();
					treeModel.Nodes.Add(treeNode);
				}
				break;
			}
			case 1U:
			{
				treeModel.ColumnNames[1] = "GL__LINES";
				int num = 0;
				for (int j = 1; j < indicesModel.Nodes.Count; j += 2)
				{
					TreeNode treeNode2 = new TreeNode();
					treeNode2.Values = new object[3];
					treeNode2.Values[0] = num.ToString();
					treeNode2.Values[1] = indicesModel.Nodes[j - 1].Values[1].ToString() + ", " + indicesModel.Nodes[j].Values[1].ToString();
					treeModel.Nodes.Add(treeNode2);
					num++;
				}
				break;
			}
			case 2U:
			{
				treeModel.ColumnNames[1] = "GL__LINE__LOOP";
				int num2 = 0;
				for (int k = 1; k < indicesModel.Nodes.Count; k++)
				{
					TreeNode treeNode3 = new TreeNode();
					treeNode3.Values = new object[2];
					treeNode3.Values[0] = num2.ToString();
					treeNode3.Values[1] = indicesModel.Nodes[k - 1].Values[1].ToString() + ", " + indicesModel.Nodes[k].Values[1].ToString();
					treeModel.Nodes.Add(treeNode3);
					num2++;
				}
				TreeNode treeNode4 = new TreeNode();
				treeNode4.Values = new object[3];
				treeNode4.Values[0] = num2.ToString();
				treeNode4.Values[1] = indicesModel.Nodes[indicesModel.Nodes.Count - 1].Values[1].ToString() + ", " + indicesModel.Nodes[0].Values[1].ToString();
				treeModel.Nodes.Add(treeNode4);
				break;
			}
			case 3U:
			{
				treeModel.ColumnNames[1] = "GL__LINE__STRIP";
				int num3 = 0;
				for (int l = 1; l < indicesModel.Nodes.Count; l++)
				{
					TreeNode treeNode5 = new TreeNode();
					treeNode5.Values = new object[2];
					treeNode5.Values[0] = num3.ToString();
					treeNode5.Values[1] = indicesModel.Nodes[l - 1].Values[1].ToString() + ", " + indicesModel.Nodes[l].Values[1].ToString();
					treeModel.Nodes.Add(treeNode5);
					num3++;
				}
				break;
			}
			case 4U:
			{
				treeModel.ColumnNames[1] = "GL__TRIANGLES";
				int num4 = 0;
				for (int m = 0; m < indicesModel.Nodes.Count - 2; m += 3)
				{
					TreeNode treeNode6 = new TreeNode();
					treeNode6.Values = new object[2];
					treeNode6.Values[0] = num4.ToString();
					treeNode6.Values[1] = string.Concat(new string[]
					{
						indicesModel.Nodes[m].Values[1].ToString(),
						", ",
						indicesModel.Nodes[m + 1].Values[1].ToString(),
						", ",
						indicesModel.Nodes[m + 2].Values[1].ToString()
					});
					treeModel.Nodes.Add(treeNode6);
					num4++;
				}
				break;
			}
			case 5U:
			{
				treeModel.ColumnNames[1] = "GL__TRIANGLE__STRIP";
				int num5 = 0;
				for (int n = 0; n < indicesModel.Nodes.Count - 2; n++)
				{
					TreeNode treeNode7 = new TreeNode();
					treeNode7.Values = new object[2];
					treeNode7.Values[0] = num5.ToString();
					treeNode7.Values[1] = string.Concat(new string[]
					{
						indicesModel.Nodes[n].Values[1].ToString(),
						", ",
						indicesModel.Nodes[n + 1].Values[1].ToString(),
						", ",
						indicesModel.Nodes[n + 2].Values[1].ToString()
					});
					treeModel.Nodes.Add(treeNode7);
					num5++;
				}
				break;
			}
			case 6U:
			{
				treeModel.ColumnNames[1] = "GL__TRIANGLE__FAN";
				int num6 = 0;
				for (int num7 = 1; num7 < indicesModel.Nodes.Count - 1; num7++)
				{
					TreeNode treeNode8 = new TreeNode();
					treeNode8.Values = new object[2];
					treeNode8.Values[0] = num6.ToString();
					treeNode8.Values[1] = string.Concat(new string[]
					{
						indicesModel.Nodes[0].Values[1].ToString(),
						", ",
						indicesModel.Nodes[num7].Values[1].ToString(),
						", ",
						indicesModel.Nodes[num7 + 1].Values[1].ToString()
					});
					treeModel.Nodes.Add(treeNode8);
					num6++;
				}
				break;
			}
			}
			return treeModel;
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0002D000 File Offset: 0x0002B200
		private async void ExportVertexDataToObj(object sender, EventArgs e)
		{
			if (this.m_currentVertexModel != null)
			{
				using (IDisposable dialog = SdpApp.UIManager.CreateDialog("ExportDrawCallDataDialog") as IDisposable)
				{
					IExportDrawCallDataDialog exportDialog = dialog as IExportDrawCallDataDialog;
					if (exportDialog != null)
					{
						exportDialog.SetAttributes(this.m_currentVertexModel.ColumnNames);
						ExportDrawCallDataDialogController exportDrawCallDataDialogController = new ExportDrawCallDataDialogController(exportDialog);
						bool flag = await exportDrawCallDataDialogController.ShowDialog();
						bool flag2 = flag;
						if (flag2)
						{
							try
							{
								SdpApp.AnalyticsManager.TrackExport("Vertex Drawcall Data");
								using (StreamWriter streamWriter = new StreamWriter(exportDialog.SaveFileLocation, false))
								{
									if (streamWriter != null)
									{
										bool flag3 = false;
										bool flag4 = false;
										bool flag5 = false;
										int num = 0;
										foreach (TreeNode treeNode in this.m_currentVertexModel.Nodes)
										{
											for (int i = 1; i < treeNode.Values.Length; i++)
											{
												if (treeNode.Values[i] != null)
												{
													string text = this.m_currentVertexModel.ColumnNames[i].ToString();
													if (text == exportDialog.PositionSelection)
													{
														if (!flag3)
														{
															flag3 = true;
															num++;
														}
														string text2 = treeNode.Values[i].ToString().Replace(",", "");
														int num2 = text2.Split(new char[] { ' ' }).Length;
														for (int j = 0; j < 3 - num2; j++)
														{
															text2 += " 0";
														}
														streamWriter.Write("v " + text2 + "\n");
													}
													else if (text == exportDialog.NormalSelection)
													{
														if (!flag4)
														{
															flag4 = true;
															num++;
														}
														string text3 = treeNode.Values[i].ToString().Replace(",", "");
														int num3 = text3.Split(new char[] { ' ' }).Length;
														for (int k = 0; k < 3 - num3; k++)
														{
															text3 += " 0";
														}
														streamWriter.Write("vn " + text3 + "\n");
													}
													else if (text == exportDialog.TexCoordSelection)
													{
														if (!flag5)
														{
															flag5 = true;
															num++;
														}
														streamWriter.Write("vt " + treeNode.Values[i].ToString().Replace(",", "") + "\n");
													}
												}
											}
										}
										streamWriter.Write("# " + this.m_currentVertexModel.Nodes.Count.ToString() + " verticies\n");
										if (this.m_currentElementModel != null)
										{
											foreach (TreeNode treeNode2 in this.m_currentElementModel.Nodes)
											{
												for (int l = 1; l < treeNode2.Values.Length; l++)
												{
													if (treeNode2.Values[l] != null)
													{
														this.m_currentElementModel.ColumnNames[l].ToString();
														string[] array = ((string)treeNode2.Values[l]).Replace(" ", string.Empty).Split(new char[] { ',' });
														streamWriter.Write("f");
														for (int m = 0; m < array.Length; m++)
														{
															int num4 = int.Parse(array[m]) + 1;
															streamWriter.Write(" ");
															for (int n = 0; n < num; n++)
															{
																if (n != 0)
																{
																	streamWriter.Write("/");
																}
																streamWriter.Write(num4.ToString());
															}
														}
														streamWriter.Write("\n");
													}
												}
											}
											streamWriter.Write("# " + this.m_currentElementModel.Nodes.Count.ToString() + " elements\n");
										}
									}
								}
							}
							catch
							{
								ShowMessageDialogCommand.ShowErrorDialog("Error exporting draw call data");
							}
						}
					}
					exportDialog = null;
				}
				IDisposable dialog = null;
			}
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0002D038 File Offset: 0x0002B238
		public ViewDesc SaveSettings()
		{
			ViewDesc viewDesc = null;
			if (this.m_view != null)
			{
				viewDesc = new ViewDesc();
				viewDesc.TypeName = this.m_view.TypeName;
			}
			return viewDesc;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0002D067 File Offset: 0x0002B267
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x040009BD RID: 2493
		private TreeModel m_currentVertexModel;

		// Token: 0x040009BE RID: 2494
		private TreeModel m_currentElementModel;

		// Token: 0x040009BF RID: 2495
		private TreeModel m_currentInidiciesModel;

		// Token: 0x040009C0 RID: 2496
		private IDrawCallDataView m_view;
	}
}
