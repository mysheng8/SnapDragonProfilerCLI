using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Sdp;
using Sdp.Helpers;
using Sdp.Logging;

namespace QGLPlugin
{
	// Token: 0x02000025 RID: 37
	internal class VkAPITreeModelBuilder
	{
		// Token: 0x0600003D RID: 61 RVA: 0x000057A4 File Offset: 0x000039A4
		public VkAPITreeModelBuilder(uint captureID, VkMetricsCapturedModel metricValues, uint lrzStateMetricID)
		{
			this.m_captureID = captureID;
			this.m_metricsCaptured = metricValues;
			this.m_LRZStateMetricID = lrzStateMetricID;
			this.m_numColumns = 10 + metricValues.Metrics.Count * 2;
			this.m_processors = new VkAPITreeModelBuilder.VkAPITreeProcessors();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00005858 File Offset: 0x00003A58
		public void ProcessAllCalls()
		{
			if (QGLPlugin.SnapshotApiBuffer == null || QGLPlugin.SnapshotApiBuffer.size == 0U)
			{
				VkAPITreeModelBuilder.Logger.LogWarning("No snapshot APIs are available for capture  " + this.m_captureID.ToString() + ".");
				return;
			}
			SDPProcessorPlugin processorPlugin = SdpApp.ConnectionManager.GetProcessorPlugin("SDP::QGLPluginProcessor");
			if (processorPlugin != null)
			{
				BinaryDataPair localBuffer = processorPlugin.GetLocalBuffer(SDPCore.BUFFER_TYPE_VULKAN_REPLAY_HANDLE_MAPPING, 0U, this.m_captureID);
				if (localBuffer != null)
				{
					int num = 0;
					while ((long)num < (long)((ulong)localBuffer.size))
					{
						QGLPlugin.VulkanHandleMapping vulkanHandleMapping = Marshal.PtrToStructure<QGLPlugin.VulkanHandleMapping>(localBuffer.data + num);
						this.m_handleMapping[vulkanHandleMapping.captureHandleID] = vulkanHandleMapping;
						num += Marshal.SizeOf(typeof(QGLPlugin.VulkanHandleMapping));
					}
				}
			}
			int num2 = Marshal.SizeOf<QGLPlugin.VulkanSnapshotApi>(default(QGLPlugin.VulkanSnapshotApi));
			long num3 = (long)((ulong)QGLPlugin.SnapshotApiBuffer.size / (ulong)((long)num2));
			IntPtr intPtr = QGLPlugin.SnapshotApiBuffer.data;
			int num4 = 0;
			while ((long)num4 < num3)
			{
				QGLPlugin.VulkanSnapshotApi vulkanSnapshotApi = Marshal.PtrToStructure<QGLPlugin.VulkanSnapshotApi>(intPtr);
				intPtr += num2;
				if (!this.m_threads.ContainsKey(vulkanSnapshotApi.threadID))
				{
					this.m_threads[vulkanSnapshotApi.threadID] = "0x" + vulkanSnapshotApi.threadID.ToString("X");
				}
				if (VkHelper.IsDrawCall(vulkanSnapshotApi.name) && !this.m_drawcalls.Contains(vulkanSnapshotApi.name))
				{
					this.m_drawcalls.Add(vulkanSnapshotApi.name);
				}
				this.AddApiNode(vulkanSnapshotApi);
				num4++;
			}
			ModelObjectDataList vulkanEndCaptureImage = QGLModel.GetVulkanEndCaptureImage((int)this.m_captureID);
			if (vulkanEndCaptureImage.Count == 1)
			{
				TreeNode treeNode = new TreeNode();
				treeNode.Values = new object[this.m_numColumns];
				ModelObjectData modelObjectData = vulkanEndCaptureImage[0];
				uint num5 = UintConverter.Convert(modelObjectData.GetValue("presentedImage"));
				this.AddPresentCall(treeNode, num5);
			}
			else
			{
				VkAPITreeModelBuilder.Logger.LogError("Couldn't find end capture image");
			}
			QGLPlugin.ClearApiBuffer();
			this.InvalidateDrawcallCount();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00005A5C File Offset: 0x00003C5C
		public void AddPresentCall(TreeNode presentNode, uint presentedImageID)
		{
			this.m_iQueueSubmit += 1U;
			presentNode.Values[2] = "<b> vkQueuePresentKHR (" + presentedImageID.ToString() + ") </b>";
			presentNode.Values[5] = "vkQueuePresentKHR";
			presentNode.Values[7] = 0;
			presentNode.Values[1] = VkHelper.SnapshotApiCallIDToString(this.m_iQueueSubmit, 0U, 0U);
			presentNode.Values[8] = VkHelper.SnapshotApiCallIDToString(this.m_iQueueSubmit, 0U, 0U);
			this.m_allNodes.Add(presentNode);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00005AE8 File Offset: 0x00003CE8
		private void UpdateDrawcallID(TreeNode node, ref uint submitIdx, ref uint cmdBufferIdx, ref uint drawcallIdx)
		{
			string text = (string)node.Values[5];
			if (text.Contains("vkBeginCommandBuffer"))
			{
				VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer = null;
				ulong commandBufferID = VkAPITreeModelBuilder.VkCmdBuffer.GetCommandBufferID(node);
				if (this.m_commandBuffers.TryGetValue(commandBufferID, out vkCmdBuffer) && vkCmdBuffer.IsPrimaryCommandBuffer)
				{
					drawcallIdx = 0U;
					cmdBufferIdx += 1U;
					node.Values[1] = VkHelper.SnapshotApiCallIDToString(submitIdx, cmdBufferIdx, drawcallIdx);
				}
			}
			else if (VkHelper.IsPushConstant(text))
			{
				ulong num = VkHelper.MakeSnapshotApiCallID(submitIdx, cmdBufferIdx, drawcallIdx);
				uint num2 = (uint)node.Values[0];
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture((int)this.m_captureID);
				if (capture != null)
				{
					capture.MapPushConstant(num, num2);
				}
			}
			else
			{
				if (VkHelper.IsDrawCall(text))
				{
					drawcallIdx += 1U;
					this.m_totalDraws += 1U;
					string text2 = VkHelper.SnapshotApiCallIDToString(submitIdx, cmdBufferIdx, drawcallIdx);
					node.Values[1] = text2;
					node.Values[8] = text2;
					ulong num3 = VkHelper.MakeSnapshotApiCallID(submitIdx, cmdBufferIdx, drawcallIdx);
					this.m_drawcallNodes.Add(node);
					QGLPlugin.VkSnapshotModel.ResourcesPerDrawcall[(uint)node.Values[0]] = new List<PrepopulateCategoryArgs>();
					if (this.m_metricsCaptured == null)
					{
						goto IL_041F;
					}
					ulong commandBufferID2 = VkAPITreeModelBuilder.VkCmdBuffer.GetCommandBufferID(node);
					uint num4 = 1U;
					this.m_commandBufferSubmitCount.TryGetValue(commandBufferID2, out num4);
					ulong num5 = commandBufferID2;
					QGLPlugin.VulkanHandleMapping vulkanHandleMapping;
					if (this.m_handleMapping.TryGetValue(commandBufferID2, out vulkanHandleMapping))
					{
						num4 += (uint)(vulkanHandleMapping.submitIndex - 1UL);
						num5 = vulkanHandleMapping.replayHandleID;
					}
					if (this.m_metricsCaptured.HasDrawID(num5, drawcallIdx, num4))
					{
						using (IEnumerator<uint> enumerator = this.m_metricsCaptured.Metrics.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								uint num6 = enumerator.Current;
								double metricValue = this.m_metricsCaptured.GetMetricValue(num5, drawcallIdx, num4, num6);
								node.Values[this.m_metricsCaptured.GetMetricColumn(num6)] = metricValue;
								if (num6 == this.m_LRZStateMetricID)
								{
									node.Values[this.m_metricsCaptured.GetMetricColumn(num6) + 1] = new ObjectWithToolTip(VkAPITreeModelBuilder.GetLRZStateString((uint)metricValue), "");
								}
								else
								{
									node.Values[this.m_metricsCaptured.GetMetricColumn(num6) + 1] = new ObjectWithToolTip(metricValue.ToString(), "");
								}
								string metricName = this.m_metricsCaptured.GetMetricName(num6);
								Dictionary<uint, double> dictionary;
								if (!QGLPlugin.VkSnapshotModel.GetMetricsForCapture(this.m_captureID).MetricsList.TryGetValue(metricName, out dictionary))
								{
									dictionary = (QGLPlugin.VkSnapshotModel.GetMetricsForCapture(this.m_captureID).MetricsList[metricName] = new Dictionary<uint, double>());
								}
								dictionary[(uint)node.Values[0]] = metricValue;
							}
							goto IL_041F;
						}
					}
					if (VkHelper.IsDispatch(text))
					{
						using (IEnumerator<uint> enumerator2 = this.m_metricsCaptured.Metrics.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								uint num7 = enumerator2.Current;
								node.Values[this.m_metricsCaptured.GetMetricColumn(num7)] = -1;
								node.Values[this.m_metricsCaptured.GetMetricColumn(num7) + 1] = new ObjectWithToolTip("N/A", "Snapdragon Profiler does not currently\nsupport collecting metrics for\ncompute calls.");
							}
							goto IL_041F;
						}
					}
					if (this.m_metricsCaptured.Metrics.Count <= 0)
					{
						goto IL_041F;
					}
					VkAPITreeModelBuilder.Logger.LogWarning(string.Format("Drawcall metrics missing, Command Buffer Id: {0}, replay handle: {1}, drawcall ID: {2}", commandBufferID2, num5, drawcallIdx));
					VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer2 = null;
					if (!this.m_commandBuffers.TryGetValue(commandBufferID2, out vkCmdBuffer2))
					{
						goto IL_041F;
					}
					using (IEnumerator<uint> enumerator3 = this.m_metricsCaptured.Metrics.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							uint num8 = enumerator3.Current;
							node.Values[this.m_metricsCaptured.GetMetricColumn(num8)] = -1;
							node.Values[this.m_metricsCaptured.GetMetricColumn(num8) + 1] = new ObjectWithToolTip("N/A", "Metric values for this drawcall are\nmissing.");
						}
						goto IL_041F;
					}
				}
				node.Values[1] = "";
			}
			IL_041F:
			foreach (TreeNode treeNode in node.Children)
			{
				this.UpdateDrawcallID(treeNode, ref submitIdx, ref cmdBufferIdx, ref drawcallIdx);
			}
			if (text.Contains("vkBeginCommandBuffer") || text.Contains("vkCmdBeginRenderPass") || text.Contains("vkCmdBeginRendering") || text.Contains("vkCmdExecuteCommands"))
			{
				node.Values[8] = VkHelper.SnapshotApiCallIDToString(submitIdx, cmdBufferIdx, drawcallIdx);
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00006000 File Offset: 0x00004200
		private void ProcessApiCall(TreeNode apiNode)
		{
			this.m_apiCalls.Add(apiNode);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00006010 File Offset: 0x00004210
		private DescSetBindings GetDescSet(uint descSetID)
		{
			if (!this.m_descBindingInfo.ContainsKey((ulong)descSetID))
			{
				this.UpdateDescSet(descSetID, uint.MaxValue);
			}
			DescSetBindings descSetBindings;
			if (!this.m_descBindingInfo.TryGetValue((ulong)descSetID, out descSetBindings))
			{
				descSetBindings = new DescSetBindings((ulong)descSetID);
				this.m_descBindingInfo[(ulong)descSetID] = descSetBindings;
			}
			return descSetBindings;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000605C File Offset: 0x0000425C
		private void UpdateDescSet(uint api)
		{
			List<ulong> list;
			if (QGLPlugin.VkSnapshotModel.DsbByApi.TryGetValue(api, out list))
			{
				foreach (ulong num in list)
				{
					foreach (KeyValuePair<ulong, DescSetBindings.DescBindings> keyValuePair in QGLPlugin.VkSnapshotModel.GetCapture((int)this.m_captureID).AllDescSetBindings[num].Bindings)
					{
						this.UpdateDescSet(keyValuePair.Value);
					}
				}
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00006120 File Offset: 0x00004320
		public void UpdateDescSet(uint descSetID, uint api)
		{
			DescSetBindings descSetBindings;
			if (QGLPlugin.VkSnapshotModel.GetCapture((int)this.m_captureID).AllDescSetBindings.TryGetValue((ulong)descSetID, out descSetBindings))
			{
				foreach (KeyValuePair<ulong, DescSetBindings.DescBindings> keyValuePair in descSetBindings.Bindings)
				{
					if (keyValuePair.Value.apiID == api)
					{
						this.UpdateDescSet(keyValuePair.Value);
					}
				}
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000061A8 File Offset: 0x000043A8
		private void UpdateDescSet(DescSetBindings.DescBindings d)
		{
			DescSetBindings descSetBindings;
			if (!this.m_descBindingInfo.TryGetValue(d.descriptorSetID, out descSetBindings))
			{
				descSetBindings = new DescSetBindings(d.descriptorSetID);
				this.m_descBindingInfo[d.descriptorSetID] = descSetBindings;
			}
			DescSetBindings.DescBindings descBindings;
			if (!descSetBindings.Bindings.TryGetValue((ulong)d.slotNum, out descBindings))
			{
				descBindings = new DescSetBindings.DescBindings(d.samplerID, d.imageViewID, d.imageLayout, d.bufferID, d.offset, d.range, d.texBufferview, d.accelStructID, d.tensorID, d.tensorViewID);
				descSetBindings.Bindings[(ulong)d.slotNum] = descBindings;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00006254 File Offset: 0x00004454
		private void AddApiNode(QGLPlugin.VulkanSnapshotApi api)
		{
			bool flag = false;
			if (api.name.Equals("BeginMarker"))
			{
				this.m_endMarker = false;
				return;
			}
			if (api.name.Equals("EndMarker"))
			{
				this.m_endMarker = true;
				this.m_cmdBufferInProgress = false;
				return;
			}
			if (api.name.Equals("vkAllocateCommandBuffers"))
			{
				flag = true;
			}
			else if (api.name.Equals("vkBeginCommandBuffer"))
			{
				this.m_cmdBufferInProgress = true;
			}
			else if (api.name.Equals("vkEndCommandBuffer"))
			{
				this.m_cmdBufferInProgress = false;
			}
			if (!this.m_endMarker && !flag && !this.m_cmdBufferInProgress)
			{
				return;
			}
			TreeNode treeNode = new TreeNode();
			treeNode.Values = new object[this.m_numColumns];
			treeNode.Values[0] = api.apiID;
			treeNode.Values[1] = "";
			treeNode.Values[2] = api.name;
			string text = VkHelper.PrettifyJsonParameters(api.parameters);
			treeNode.Values[3] = new ObjectWithToolTip(text, api.parameters);
			treeNode.Values[4] = this.m_threads[api.threadID];
			treeNode.Values[5] = api.name;
			treeNode.Values[6] = api.parameters;
			treeNode.Values[7] = null;
			treeNode.Values[8] = "";
			treeNode.Values[9] = new ObjectWithToolTip(DataExplorerViewMgr.IconNone, "");
			foreach (uint num in this.m_metricsCaptured.Metrics)
			{
				int metricColumn = this.m_metricsCaptured.GetMetricColumn(num);
				treeNode.Values[metricColumn] = -1;
				treeNode.Values[metricColumn + 1] = new ObjectWithToolTip("", "");
			}
			if (api.name.StartsWith("vkCmd"))
			{
				this.m_processors.ProcessVkCmd(this, treeNode, api.encodedParams);
				return;
			}
			string name = api.name;
			if (name != null)
			{
				switch (name.Length)
				{
				case 13:
				{
					char c = name[2];
					if (c != 'Q')
					{
						if (c != 'U')
						{
							goto IL_0426;
						}
						if (!(name == "vkUnmapMemory"))
						{
							goto IL_0426;
						}
						this.ProcessApiCall(treeNode);
						return;
					}
					else
					{
						if (!(name == "vkQueueSubmit"))
						{
							goto IL_0426;
						}
						this.m_processors.ProcessQueueSubmit(this, treeNode, api.encodedParams);
						return;
					}
					break;
				}
				case 14:
					if (!(name == "vkQueueSubmit2"))
					{
						goto IL_0426;
					}
					break;
				case 15:
				case 16:
				case 19:
				case 21:
				case 23:
					goto IL_0426;
				case 17:
				{
					char c = name[7];
					if (c != 'P')
					{
						if (c != 'S')
						{
							goto IL_0426;
						}
						if (!(name == "vkQueueSubmit2KHR"))
						{
							goto IL_0426;
						}
					}
					else
					{
						if (!(name == "vkQueuePresentKHR"))
						{
							goto IL_0426;
						}
						this.m_processors.ProcessQueuePresent(this, treeNode, api.encodedParams);
						return;
					}
					break;
				}
				case 18:
					if (!(name == "vkEndCommandBuffer"))
					{
						goto IL_0426;
					}
					this.m_processors.ProcessEndCommandBuffer(this, treeNode);
					return;
				case 20:
				{
					char c = name[2];
					if (c != 'B')
					{
						if (c != 'R')
						{
							goto IL_0426;
						}
						if (!(name == "vkResetCommandBuffer"))
						{
							goto IL_0426;
						}
						this.m_processors.ProcessResetCommandBuffer(this, api.encodedParams);
						return;
					}
					else
					{
						if (!(name == "vkBeginCommandBuffer"))
						{
							goto IL_0426;
						}
						this.m_processors.ProcessBeginCommandBuffer(this, treeNode, api.encodedParams);
						return;
					}
					break;
				}
				case 22:
				{
					if (!(name == "vkUpdateDescriptorSets"))
					{
						goto IL_0426;
					}
					uint num2 = (uint)treeNode.Values[0];
					this.UpdateDescSet(num2);
					this.ProcessApiCall(treeNode);
					return;
				}
				case 24:
					if (!(name == "vkAllocateCommandBuffers"))
					{
						goto IL_0426;
					}
					this.m_processors.ProcessAllocateCommandBuffers(this, treeNode, api.encodedParams);
					return;
				default:
					goto IL_0426;
				}
				this.m_processors.ProcessQueueSubmit2KHR(this, treeNode, api.encodedParams);
				return;
			}
			IL_0426:
			this.ProcessApiCall(treeNode);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000066A0 File Offset: 0x000048A0
		private static string GetLRZStateString(uint value)
		{
			switch (value)
			{
			case 0U:
				return "Test Disabled, Write Disabled";
			case 1U:
				return "Test Enabled, Write Disabled";
			case 2U:
				return "Test Disabled, Write Enabled";
			case 3U:
				return "Test Enabled, Write Enabled";
			default:
				return "Unknown State";
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000066D8 File Offset: 0x000048D8
		private void InvalidateDrawcallCount()
		{
			if (this.m_metricsCaptured != null && this.m_metricsCaptured.Metrics.Count > 0 && (long)this.m_metricsCaptured.TotalDrawcallCount != (long)((ulong)this.m_totalDraws))
			{
				VkAPITreeModelBuilder.Logger.LogWarning(string.Concat(new string[]
				{
					"Discrepancy in the total number of drawcalls between driver data (",
					this.m_metricsCaptured.TotalDrawcallCount.ToString(),
					") and GFXR information (",
					this.m_totalDraws.ToString(),
					")."
				}));
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00006766 File Offset: 0x00004966
		public List<TreeNode> DrawcallNodes
		{
			get
			{
				return this.m_drawcallNodes;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000676E File Offset: 0x0000496E
		public List<TreeNode> AllNodes
		{
			get
			{
				return this.m_allNodes;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00006776 File Offset: 0x00004976
		public Dictionary<ulong, string> Threads
		{
			get
			{
				return this.m_threads;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000677E File Offset: 0x0000497E
		public List<string> Drawcalls
		{
			get
			{
				return this.m_drawcalls;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00006786 File Offset: 0x00004986
		public Dictionary<ulong, uint> CmdBuffSubmitCount
		{
			get
			{
				return this.m_commandBufferSubmitCount;
			}
		}

		// Token: 0x04000346 RID: 838
		public static ILogger Logger = new global::Sdp.Logging.Logger("QGL API Tree Builder");

		// Token: 0x04000347 RID: 839
		private Dictionary<ulong, VkAPITreeModelBuilder.VkCmdBuffer> m_commandBuffers = new Dictionary<ulong, VkAPITreeModelBuilder.VkCmdBuffer>();

		// Token: 0x04000348 RID: 840
		private Dictionary<ulong, DescSetBindings> m_descBindingInfo = new Dictionary<ulong, DescSetBindings>();

		// Token: 0x04000349 RID: 841
		private List<TreeNode> m_apiCalls = new List<TreeNode>();

		// Token: 0x0400034A RID: 842
		private List<TreeNode> m_drawcallNodes = new List<TreeNode>();

		// Token: 0x0400034B RID: 843
		private List<TreeNode> m_allNodes = new List<TreeNode>();

		// Token: 0x0400034C RID: 844
		private Dictionary<ulong, string> m_threads = new Dictionary<ulong, string>();

		// Token: 0x0400034D RID: 845
		private List<string> m_drawcalls = new List<string>();

		// Token: 0x0400034E RID: 846
		private VkMetricsCapturedModel m_metricsCaptured;

		// Token: 0x0400034F RID: 847
		private VkAPITreeModelBuilder.VkAPITreeProcessors m_processors;

		// Token: 0x04000350 RID: 848
		private uint m_captureID;

		// Token: 0x04000351 RID: 849
		private uint m_iQueueSubmit;

		// Token: 0x04000352 RID: 850
		private int m_numColumns;

		// Token: 0x04000353 RID: 851
		private bool m_endMarker;

		// Token: 0x04000354 RID: 852
		private bool m_cmdBufferInProgress;

		// Token: 0x04000355 RID: 853
		private uint m_LRZStateMetricID = uint.MaxValue;

		// Token: 0x04000356 RID: 854
		private uint m_totalDraws;

		// Token: 0x04000357 RID: 855
		private Dictionary<ulong, uint> m_commandBufferSubmitCount = new Dictionary<ulong, uint>();

		// Token: 0x04000358 RID: 856
		private Dictionary<ulong, QGLPlugin.VulkanHandleMapping> m_handleMapping = new Dictionary<ulong, QGLPlugin.VulkanHandleMapping>();

		// Token: 0x02000052 RID: 82
		private class VkAPITreeProcessors
		{
			// Token: 0x06000173 RID: 371 RVA: 0x000125A4 File Offset: 0x000107A4
			public void ProcessAllocateCommandBuffers(VkAPITreeModelBuilder parent, TreeNode beginNode, string encodedParams)
			{
				JObject jobject = JObject.Parse(encodedParams);
				JArray jarray = (JArray)jobject.SelectToken("pCommandBuffers");
				if (jarray != null)
				{
					foreach (JToken jtoken in jarray)
					{
						ulong uint64Value = VkHelper.GetUint64Value(jtoken);
						if (uint64Value > 0UL && uint64Value < 18446744073709551615UL)
						{
							VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer = new VkAPITreeModelBuilder.VkCmdBuffer(parent, uint64Value, jobject);
							parent.m_commandBuffers[vkCmdBuffer.ID] = vkCmdBuffer;
						}
						else
						{
							VkAPITreeModelBuilder.Logger.LogError("AllocateCommandBuffer: unable to process command buffer ID " + jtoken.ToString() + ".");
						}
					}
				}
			}

			// Token: 0x06000174 RID: 372 RVA: 0x00012654 File Offset: 0x00010854
			public void ProcessBeginCommandBuffer(VkAPITreeModelBuilder parent, TreeNode beginNode, string encodedParams)
			{
				JObject jobject = JObject.Parse(encodedParams);
				JToken jtoken = jobject.SelectToken("commandBuffer");
				if (jtoken != null)
				{
					ulong uint64Value = VkHelper.GetUint64Value(jtoken);
					if (uint64Value > 0UL && uint64Value < 18446744073709551615UL)
					{
						VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer = null;
						if (parent.m_commandBuffers.TryGetValue(uint64Value, out vkCmdBuffer))
						{
							vkCmdBuffer.ProcessBeginCommandBuffer(parent, beginNode, encodedParams);
							return;
						}
						VkAPITreeModelBuilder.Logger.LogError(string.Format("BeginCommandBuffer: command buffer ID {0} not found.", uint64Value));
						return;
					}
					else
					{
						VkAPITreeModelBuilder.Logger.LogError("BeginCommandBuffer: unable to process command buffer ID " + jtoken.ToString() + ".");
					}
				}
			}

			// Token: 0x06000175 RID: 373 RVA: 0x000126E0 File Offset: 0x000108E0
			public void ProcessEndCommandBuffer(VkAPITreeModelBuilder parent, TreeNode endNode)
			{
				string text = (string)endNode.Values[6];
				JObject jobject = JObject.Parse(text);
				JToken jtoken = jobject.SelectToken("commandBuffer");
				if (jtoken != null)
				{
					ulong uint64Value = VkHelper.GetUint64Value(jtoken);
					VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer;
					if (!parent.m_commandBuffers.TryGetValue(uint64Value, out vkCmdBuffer))
					{
						return;
					}
					vkCmdBuffer.ProcessEndCommandBuffer();
				}
			}

			// Token: 0x06000176 RID: 374 RVA: 0x00012730 File Offset: 0x00010930
			public void ProcessResetCommandBuffer(VkAPITreeModelBuilder parent, string parameters)
			{
				JObject jobject = JObject.Parse(parameters);
				JToken jtoken = jobject.SelectToken("commandBuffer");
				if (jtoken != null)
				{
					ulong uint64Value = VkHelper.GetUint64Value(jtoken);
					VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer;
					if (!parent.m_commandBuffers.TryGetValue(uint64Value, out vkCmdBuffer))
					{
						return;
					}
					vkCmdBuffer.ProcessResetCommandBuffer();
				}
			}

			// Token: 0x06000177 RID: 375 RVA: 0x00012774 File Offset: 0x00010974
			public void ProcessQueueSubmit(VkAPITreeModelBuilder parent, TreeNode submitNode, string encodedParams)
			{
				submitNode.Children.AddRange(parent.m_apiCalls);
				object[] values = submitNode.Values;
				int num = 2;
				string text = "<b>";
				object obj = submitNode.Values[5];
				values[num] = text + ((obj != null) ? obj.ToString() : null) + "</b>";
				parent.m_iQueueSubmit += 1U;
				uint num2 = 0U;
				uint num3 = 0U;
				uint num4 = 0U;
				bool flag = true;
				try
				{
					JObject jobject = JObject.Parse(encodedParams);
					JToken jtoken = jobject.SelectToken("pSubmits");
					if (jtoken == null)
					{
						return;
					}
					JArray jarray;
					if (!(jtoken is JArray))
					{
						jarray = new JArray();
						jarray.Add(jtoken);
					}
					else
					{
						jarray = (JArray)jtoken;
					}
					foreach (JToken jtoken2 in jarray)
					{
						JObject jobject2 = (JObject)jtoken2;
						JToken jtoken3 = jobject2.SelectToken("pCommandBuffers");
						if (jtoken3 != null && jtoken3.Type != JTokenType.Null)
						{
							flag = false;
							foreach (JToken jtoken4 in ((IEnumerable<JToken>)jtoken3))
							{
								ulong uint64Value = VkHelper.GetUint64Value(jtoken4);
								VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer;
								if (!parent.m_commandBuffers.TryGetValue(uint64Value, out vkCmdBuffer))
								{
									VkAPITreeModelBuilder.Logger.LogError(string.Format("ProcessQueueSubmit: Couldn't find command buffer {0}", uint64Value));
								}
								else
								{
									TreeNode treeNode = vkCmdBuffer.SubmitCommandBuffer();
									submitNode.Children.Add(treeNode);
									if (!parent.CmdBuffSubmitCount.ContainsKey(uint64Value))
									{
										parent.CmdBuffSubmitCount[uint64Value] = 1U;
									}
									else
									{
										Dictionary<ulong, uint> cmdBuffSubmitCount = parent.CmdBuffSubmitCount;
										ulong num5 = uint64Value;
										uint num6 = cmdBuffSubmitCount[num5];
										cmdBuffSubmitCount[num5] = num6 + 1U;
									}
									if (treeNode != null && treeNode.Values != null)
									{
										object obj2 = treeNode.Values[7];
										if (obj2 != null)
										{
											num4 = (uint)obj2;
										}
										parent.UpdateDrawcallID(treeNode, ref parent.m_iQueueSubmit, ref num3, ref num2);
									}
									else
									{
										VkAPITreeModelBuilder.Logger.LogError(string.Format("Failed to get command buffer node {0}", uint64Value));
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
				}
				if (flag)
				{
					parent.m_iQueueSubmit -= 1U;
					return;
				}
				submitNode.Values[7] = num4;
				submitNode.Values[1] = VkHelper.SnapshotApiCallIDToString(parent.m_iQueueSubmit, 0U, 0U);
				submitNode.Values[8] = VkHelper.SnapshotApiCallIDToString(parent.m_iQueueSubmit, num3, num2);
				uint num7 = 0U;
				foreach (TreeNode treeNode2 in parent.m_apiCalls)
				{
					treeNode2.Values[1] = num7++;
					treeNode2.Values[7] = num4;
				}
				parent.m_apiCalls.Clear();
				parent.m_allNodes.Add(submitNode);
				if (submitNode.Values[7] != null)
				{
					VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture((int)parent.m_captureID);
					if (capture != null)
					{
						capture.SetLastDrawCall((uint)submitNode.Values[7]);
					}
				}
				if (submitNode.Values[8] != null)
				{
					VkCapture capture2 = QGLPlugin.VkSnapshotModel.GetCapture((int)parent.m_captureID);
					if (capture2 == null)
					{
						return;
					}
					capture2.SetLastDrawCallID((string)submitNode.Values[8]);
				}
			}

			// Token: 0x06000178 RID: 376 RVA: 0x00012B0C File Offset: 0x00010D0C
			public void ProcessQueueSubmit2KHR(VkAPITreeModelBuilder parent, TreeNode submitNode, string encodedParams)
			{
				submitNode.Children.AddRange(parent.m_apiCalls);
				object[] values = submitNode.Values;
				int num = 2;
				string text = "<b>";
				object obj = submitNode.Values[5];
				values[num] = text + ((obj != null) ? obj.ToString() : null) + "</b>";
				parent.m_iQueueSubmit += 1U;
				uint num2 = 0U;
				uint num3 = 0U;
				uint num4 = 0U;
				bool flag = true;
				try
				{
					JObject jobject = JObject.Parse(encodedParams);
					JToken jtoken = jobject.SelectToken("pSubmits");
					if (jtoken == null)
					{
						return;
					}
					JArray jarray;
					if (!(jtoken is JArray))
					{
						jarray = new JArray();
						jarray.Add(jtoken);
					}
					else
					{
						jarray = (JArray)jtoken;
					}
					foreach (JToken jtoken2 in jarray)
					{
						JObject jobject2 = (JObject)jtoken2;
						JArray jarray2 = (JArray)jobject2.SelectToken("pCommandBufferInfos");
						if (jarray2 != null && jarray2.Type != JTokenType.Null)
						{
							foreach (JToken jtoken3 in jarray2)
							{
								JToken jtoken4 = jtoken3.SelectToken("commandBuffer");
								if (jtoken4 != null && jtoken4.Type != JTokenType.Null)
								{
									flag = false;
									uint uintValue = VkHelper.GetUintValue(jtoken4);
									VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer;
									if (parent.m_commandBuffers.TryGetValue((ulong)uintValue, out vkCmdBuffer))
									{
										TreeNode treeNode = vkCmdBuffer.SubmitCommandBuffer();
										submitNode.Children.Add(treeNode);
										if (parent.m_endMarker)
										{
											if (!parent.CmdBuffSubmitCount.ContainsKey((ulong)uintValue))
											{
												parent.CmdBuffSubmitCount[(ulong)uintValue] = 1U;
											}
											else
											{
												Dictionary<ulong, uint> cmdBuffSubmitCount = parent.CmdBuffSubmitCount;
												ulong num5 = (ulong)uintValue;
												uint num6 = cmdBuffSubmitCount[num5];
												cmdBuffSubmitCount[num5] = num6 + 1U;
											}
										}
										object obj2 = treeNode.Values[7];
										if (obj2 != null)
										{
											num4 = (uint)obj2;
										}
										parent.UpdateDrawcallID(treeNode, ref parent.m_iQueueSubmit, ref num3, ref num2);
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
				}
				if (flag)
				{
					parent.m_iQueueSubmit -= 1U;
					return;
				}
				submitNode.Values[7] = num4;
				submitNode.Values[1] = VkHelper.SnapshotApiCallIDToString(parent.m_iQueueSubmit, 0U, 0U);
				submitNode.Values[8] = VkHelper.SnapshotApiCallIDToString(parent.m_iQueueSubmit, num3, num2);
				uint num7 = 0U;
				foreach (TreeNode treeNode2 in parent.m_apiCalls)
				{
					treeNode2.Values[1] = num7++;
					treeNode2.Values[7] = num4;
				}
				parent.m_apiCalls.Clear();
				parent.m_allNodes.Add(submitNode);
				if (submitNode.Values[7] != null)
				{
					VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture((int)parent.m_captureID);
					if (capture != null)
					{
						capture.SetLastDrawCall((uint)submitNode.Values[7]);
					}
				}
				if (submitNode.Values[8] != null)
				{
					VkCapture capture2 = QGLPlugin.VkSnapshotModel.GetCapture((int)parent.m_captureID);
					if (capture2 == null)
					{
						return;
					}
					capture2.SetLastDrawCallID((string)submitNode.Values[8]);
				}
			}

			// Token: 0x06000179 RID: 377 RVA: 0x00012E90 File Offset: 0x00011090
			public void ProcessQueuePresent(VkAPITreeModelBuilder parent, TreeNode presentNode, string encodedParams)
			{
				parent.m_iQueueSubmit += 1U;
				object[] values = presentNode.Values;
				int num = 2;
				string text = "<b>";
				object obj = presentNode.Values[5];
				values[num] = text + ((obj != null) ? obj.ToString() : null) + "</b>";
				presentNode.Values[5] = "vkQueuePresentKHR";
				presentNode.Values[7] = 0;
				presentNode.Values[1] = VkHelper.SnapshotApiCallIDToString(parent.m_iQueueSubmit, 0U, 0U);
				presentNode.Values[8] = VkHelper.SnapshotApiCallIDToString(parent.m_iQueueSubmit, 0U, 0U);
				parent.m_allNodes.Add(presentNode);
			}

			// Token: 0x0600017A RID: 378 RVA: 0x00012F28 File Offset: 0x00011128
			public void ProcessVkCmd(VkAPITreeModelBuilder parent, TreeNode vkCmdBufNode, string encodedParams)
			{
				ulong commandBufferID = VkAPITreeModelBuilder.VkCmdBuffer.GetCommandBufferID(vkCmdBufNode);
				VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer;
				if (!parent.m_commandBuffers.TryGetValue(commandBufferID, out vkCmdBuffer))
				{
					return;
				}
				vkCmdBuffer.AddVkCmdNode(vkCmdBufNode, encodedParams);
			}
		}

		// Token: 0x02000053 RID: 83
		private class VkCmdBuffer
		{
			// Token: 0x0600017C RID: 380 RVA: 0x00012F58 File Offset: 0x00011158
			public VkCmdBuffer(VkAPITreeModelBuilder parent, ulong cmbufferId, JObject jsonParams)
			{
				this.m_parent = parent;
				this.m_id = cmbufferId;
				this.m_isPrimaryCommandBuffer = (string)jsonParams["pAllocateInfo"]["level"] == "VK_COMMAND_BUFFER_LEVEL_PRIMARY";
			}

			// Token: 0x1700003B RID: 59
			// (get) Token: 0x0600017D RID: 381 RVA: 0x00012FCB File Offset: 0x000111CB
			public ulong ID
			{
				get
				{
					return this.m_id;
				}
			}

			// Token: 0x0600017E RID: 382 RVA: 0x00012FD4 File Offset: 0x000111D4
			public TreeNode SubmitCommandBuffer()
			{
				if (!this.m_submitted)
				{
					this.m_submitted = true;
					return this.m_commandBufferNode;
				}
				TreeNode treeNode;
				this.CopyTreeNode(this.m_commandBufferNode, out treeNode);
				return treeNode;
			}

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x0600017F RID: 383 RVA: 0x00013006 File Offset: 0x00011206
			public bool IsPrimaryCommandBuffer
			{
				get
				{
					return this.m_isPrimaryCommandBuffer;
				}
			}

			// Token: 0x06000180 RID: 384 RVA: 0x00013010 File Offset: 0x00011210
			public void ProcessBeginCommandBuffer(VkAPITreeModelBuilder parent, TreeNode beginNode, string encodedParams)
			{
				this.m_parent = parent;
				this.m_commandBufferNode = beginNode;
				this.m_commandBufferNode.Values[2] = string.Concat(new string[]
				{
					"<b> ",
					(string)this.m_commandBufferNode.Values[2],
					" (",
					this.m_id.ToString(),
					") </b>"
				});
				this.m_activeParentNodes.Push(new ValueTuple<TreeNode, bool>(this.m_commandBufferNode, false));
			}

			// Token: 0x06000181 RID: 385 RVA: 0x00013095 File Offset: 0x00011295
			public void ProcessEndCommandBuffer()
			{
				this.ProcessPendingDebugMarkersIfAny(this.m_commandBufferNode);
				this.m_activeParentNodes.Clear();
				this.m_beginDebugMarkers.Clear();
			}

			// Token: 0x06000182 RID: 386 RVA: 0x000130B9 File Offset: 0x000112B9
			public void ProcessResetCommandBuffer()
			{
				this.m_beginDebugMarkers.Clear();
				this.m_activeParentNodes.Clear();
				this.m_pendingEndDebugMarkers = 0U;
			}

			// Token: 0x06000183 RID: 387 RVA: 0x000130D8 File Offset: 0x000112D8
			public void AddVkCmdNode(TreeNode vkCmdNode, string encodedParams)
			{
				string text = (string)vkCmdNode.Values[5];
				if (VkHelper.IsDrawCall(text))
				{
					this.ProcessDrawCall(vkCmdNode);
					return;
				}
				if (text != null)
				{
					switch (text.Length)
					{
					case 17:
					{
						char c = text[5];
						if (c != 'B')
						{
							if (c != 'E')
							{
								goto IL_030A;
							}
							if (!(text == "vkCmdEndRendering"))
							{
								goto IL_030A;
							}
							goto IL_02C3;
						}
						else
						{
							if (!(text == "vkCmdBindPipeline"))
							{
								goto IL_030A;
							}
							this.ProcessBindPipeline(vkCmdNode);
							return;
						}
						break;
					}
					case 18:
						if (!(text == "vkCmdEndRenderPass"))
						{
							goto IL_030A;
						}
						goto IL_02C3;
					case 19:
					{
						char c = text[5];
						if (c != 'B')
						{
							if (c != 'E')
							{
								goto IL_030A;
							}
							if (!(text == "vkCmdEndRenderPass2"))
							{
								goto IL_030A;
							}
							goto IL_02C3;
						}
						else
						{
							if (!(text == "vkCmdBeginRendering"))
							{
								goto IL_030A;
							}
							goto IL_02BB;
						}
						break;
					}
					case 20:
					{
						char c = text[6];
						if (c != 'e')
						{
							if (c != 'n')
							{
								if (c != 'x')
								{
									goto IL_030A;
								}
								if (!(text == "vkCmdExecuteCommands"))
								{
									goto IL_030A;
								}
								this.ProcessExecuteCommands(vkCmdNode, encodedParams);
								return;
							}
							else
							{
								if (!(text == "vkCmdEndRenderingKHR"))
								{
									goto IL_030A;
								}
								goto IL_02C3;
							}
						}
						else if (!(text == "vkCmdBeginRenderPass"))
						{
							goto IL_030A;
						}
						break;
					}
					case 21:
						if (!(text == "vkCmdBeginRenderPass2"))
						{
							goto IL_030A;
						}
						break;
					case 22:
					{
						char c = text[5];
						if (c != 'B')
						{
							if (c != 'D')
							{
								goto IL_030A;
							}
							if (!(text == "vkCmdDebugMarkerEndEXT"))
							{
								goto IL_030A;
							}
							goto IL_02FB;
						}
						else
						{
							if (!(text == "vkCmdBeginRenderingKHR"))
							{
								goto IL_030A;
							}
							goto IL_02BB;
						}
						break;
					}
					case 23:
						if (!(text == "vkCmdBindDescriptorSets"))
						{
							goto IL_030A;
						}
						this.ProcessBindDescriptorSet(vkCmdNode, encodedParams);
						return;
					case 24:
					{
						char c = text[5];
						if (c != 'B')
						{
							if (c != 'D')
							{
								goto IL_030A;
							}
							if (!(text == "vkCmdDebugMarkerBeginEXT"))
							{
								goto IL_030A;
							}
							goto IL_02F3;
						}
						else if (!(text == "vkCmdBeginRenderPass2KHR"))
						{
							goto IL_030A;
						}
						break;
					}
					case 25:
						if (!(text == "vkCmdDebugMarkerInsertEXT"))
						{
							goto IL_030A;
						}
						goto IL_0302;
					case 26:
						if (!(text == "vkCmdEndDebugUtilsLabelEXT"))
						{
							goto IL_030A;
						}
						goto IL_02FB;
					case 27:
						goto IL_030A;
					case 28:
					{
						char c = text[5];
						if (c != 'B')
						{
							if (c != 'E')
							{
								goto IL_030A;
							}
							if (!(text == "vkCmdEndPerTileExecutionQCOM"))
							{
								goto IL_030A;
							}
							this.ProcessEndPerTileExecution();
							return;
						}
						else
						{
							if (!(text == "vkCmdBeginDebugUtilsLabelEXT"))
							{
								goto IL_030A;
							}
							goto IL_02F3;
						}
						break;
					}
					case 29:
						if (!(text == "vkCmdInsertDebugUtilsLabelEXT"))
						{
							goto IL_030A;
						}
						goto IL_0302;
					case 30:
						if (!(text == "vkCmdBeginPerTileExecutionQCOM"))
						{
							goto IL_030A;
						}
						this.ProcessBeginPerTileExecution(vkCmdNode);
						return;
					default:
						goto IL_030A;
					}
					this.ProcessBeginRenderPass(vkCmdNode);
					return;
					IL_02BB:
					this.ProcessBeginRendering(vkCmdNode);
					return;
					IL_02C3:
					this.ProcessEndRenderPass();
					return;
					IL_02F3:
					this.ProcessBeginDebugMarkerRegion(vkCmdNode);
					return;
					IL_02FB:
					this.ProcessEndDebugMarkerRegion();
					return;
					IL_0302:
					this.ProcessDebugMarkerInsert(vkCmdNode);
					return;
				}
				IL_030A:
				this.AddToParentNode(vkCmdNode);
			}

			// Token: 0x06000184 RID: 388 RVA: 0x000133F8 File Offset: 0x000115F8
			public void ProcessPendingDebugMarkersIfAny(TreeNode parentNode)
			{
				while (this.m_beginDebugMarkers.Count > 0)
				{
					this.m_activeParentNodes.Pop();
					TreeNode treeNode = this.m_beginDebugMarkers.Pop();
					parentNode.Children.Add(treeNode);
				}
			}

			// Token: 0x06000185 RID: 389 RVA: 0x0001343C File Offset: 0x0001163C
			private void ProcessBeginRenderPass(TreeNode beginCallNode)
			{
				this.m_activeParentNodes.Push(new ValueTuple<TreeNode, bool>(beginCallNode, false));
				string text = (string)beginCallNode.Values[6];
				JObject jobject = JObject.Parse(text);
				JObject jobject2 = (JObject)jobject.SelectToken("pRenderPassBegin");
				if (jobject2 == null)
				{
					return;
				}
				JToken jtoken = jobject2.SelectToken("renderPass");
				if (jtoken != null)
				{
					uint uintValue = VkHelper.GetUintValue(jtoken);
					beginCallNode.Values[2] = string.Concat(new string[]
					{
						"<b> ",
						(string)beginCallNode.Values[2],
						" (",
						uintValue.ToString(),
						") </b>"
					});
					this.m_currentRenderPass = beginCallNode;
					return;
				}
			}

			// Token: 0x06000186 RID: 390 RVA: 0x000134F0 File Offset: 0x000116F0
			private void ProcessEndRenderPass()
			{
				if (this.m_currentRenderPass == null)
				{
					return;
				}
				uint? num = null;
				object obj = this.m_currentRenderPass.Values[7];
				if (obj != null)
				{
					num = new uint?((uint)obj);
				}
				this.ProcessPendingDebugMarkersIfAny(this.m_currentRenderPass);
				this.m_activeParentNodes.Pop();
				this.AddToParentNode(this.m_currentRenderPass);
				this.m_currentRenderPass = null;
			}

			// Token: 0x06000187 RID: 391 RVA: 0x00013557 File Offset: 0x00011757
			private void ProcessBeginPerTileExecution(TreeNode beginCallNode)
			{
				this.m_activeParentNodes.Push(new ValueTuple<TreeNode, bool>(beginCallNode, false));
				beginCallNode.Values[2] = "<b> " + (string)beginCallNode.Values[2] + "</b>";
				this.m_currentTileExecutionNode = beginCallNode;
			}

			// Token: 0x06000188 RID: 392 RVA: 0x00013596 File Offset: 0x00011796
			private void ProcessBeginRendering(TreeNode beginCallNode)
			{
				this.m_activeParentNodes.Push(new ValueTuple<TreeNode, bool>(beginCallNode, false));
				beginCallNode.Values[2] = "<b> " + (string)beginCallNode.Values[2] + "</b>";
				this.m_currentRenderPass = beginCallNode;
			}

			// Token: 0x06000189 RID: 393 RVA: 0x000135D8 File Offset: 0x000117D8
			private void ProcessEndPerTileExecution()
			{
				if (this.m_currentTileExecutionNode == null)
				{
					return;
				}
				uint? num = null;
				object obj = this.m_currentTileExecutionNode.Values[7];
				if (obj != null)
				{
					num = new uint?((uint)obj);
				}
				this.m_activeParentNodes.Pop();
				this.AddToParentNode(this.m_currentTileExecutionNode);
				this.m_currentTileExecutionNode = null;
			}

			// Token: 0x0600018A RID: 394 RVA: 0x00013634 File Offset: 0x00011834
			private void ProcessExecuteCommands(TreeNode vkExecCmdNode, string encodedParams)
			{
				uint? num = null;
				JObject jobject = JObject.Parse(encodedParams);
				JArray jarray = (JArray)jobject.SelectToken("pCommandBuffers");
				if (jarray != null)
				{
					foreach (JToken jtoken in jarray)
					{
						uint uintValue = VkHelper.GetUintValue(jtoken);
						VkAPITreeModelBuilder.VkCmdBuffer vkCmdBuffer;
						if (this.m_parent.m_commandBuffers.TryGetValue((ulong)uintValue, out vkCmdBuffer))
						{
							TreeNode treeNode = vkCmdBuffer.SubmitCommandBuffer();
							if (this.m_parent.m_endMarker)
							{
								if (!this.m_parent.CmdBuffSubmitCount.ContainsKey((ulong)uintValue))
								{
									this.m_parent.CmdBuffSubmitCount[(ulong)uintValue] = 1U;
								}
								else
								{
									Dictionary<ulong, uint> cmdBuffSubmitCount = this.m_parent.CmdBuffSubmitCount;
									ulong num2 = (ulong)uintValue;
									uint num3 = cmdBuffSubmitCount[num2];
									cmdBuffSubmitCount[num2] = num3 + 1U;
								}
							}
							object obj = treeNode.Values[7];
							if (obj != null)
							{
								num = new uint?((uint)obj);
							}
							vkExecCmdNode.Children.Add(treeNode);
						}
					}
				}
				object[] values = vkExecCmdNode.Values;
				int num4 = 2;
				string text = "<b> ";
				object obj2 = vkExecCmdNode.Values[2];
				values[num4] = text + ((obj2 != null) ? obj2.ToString() : null) + " </b>";
				if (num != null)
				{
					vkExecCmdNode.Values[7] = num.Value;
				}
				if (this.m_currentRenderPass != null && num != null)
				{
					this.m_currentRenderPass.Values[7] = num.Value;
				}
				if (this.m_currentTileExecutionNode != null && num != null)
				{
					this.m_currentTileExecutionNode.Values[7] = num.Value;
				}
				this.AddToParentNode(vkExecCmdNode);
				if (num != null)
				{
					this.m_commandBufferNode.Values[7] = num.Value;
				}
			}

			// Token: 0x0600018B RID: 395 RVA: 0x0001381C File Offset: 0x00011A1C
			private void ProcessDrawCall(TreeNode drawNode)
			{
				object[] values = drawNode.Values;
				int num = 2;
				string text = "<b> ";
				object obj = drawNode.Values[2];
				values[num] = text + ((obj != null) ? obj.ToString() : null) + " </b>";
				uint num2 = (uint)drawNode.Values[0];
				if (this.m_currentRenderPass != null)
				{
					this.m_currentRenderPass.Values[7] = num2;
				}
				if (this.m_currentTileExecutionNode != null)
				{
					this.m_currentTileExecutionNode.Values[7] = num2;
				}
				this.AddToParentNode(drawNode);
				this.m_commandBufferNode.Values[7] = num2;
				drawNode.Values[7] = num2;
				QGLPlugin.VkSnapshotModel.AddDrawCallInfo(this.m_parent.m_captureID, num2, this.m_currentBoundInfo);
			}

			// Token: 0x0600018C RID: 396 RVA: 0x000138DC File Offset: 0x00011ADC
			private void ProcessBindPipeline(TreeNode vkCmdNode)
			{
				string text = (string)vkCmdNode.Values[6];
				JObject jobject = JObject.Parse(text);
				JToken jtoken = jobject.SelectToken("pipeline");
				if (jtoken != null)
				{
					uint uintValue = VkHelper.GetUintValue(jtoken);
					this.m_currentBoundInfo.BoundPipeline = (ulong)uintValue;
					this.AddToParentNode(vkCmdNode);
					return;
				}
			}

			// Token: 0x0600018D RID: 397 RVA: 0x00013930 File Offset: 0x00011B30
			private void ProcessBindDescriptorSet(TreeNode vkCmdNode, string encodedParams)
			{
				string text = (string)vkCmdNode.Values[6];
				JObject jobject = JObject.Parse(text);
				JToken jtoken = jobject.SelectToken("firstSet");
				if (jtoken == null)
				{
					return;
				}
				uint uintValue = VkHelper.GetUintValue(jtoken);
				JArray jarray = (JArray)jobject.SelectToken("pDescriptorSets");
				if (jarray == null)
				{
					return;
				}
				foreach (JToken jtoken2 in jarray)
				{
					uint uintValue2 = VkHelper.GetUintValue(jtoken2);
					this.m_currentBoundInfo.BoundDescriptorSets[(ulong)uintValue++] = this.m_parent.GetDescSet(uintValue2);
					this.m_currentBoundInfo.DescSetIDs.Add((ulong)uintValue2);
				}
				this.AddToParentNode(vkCmdNode);
			}

			// Token: 0x0600018E RID: 398 RVA: 0x00013A08 File Offset: 0x00011C08
			private void ProcessBeginDebugMarkerRegion(TreeNode markerBeginNode)
			{
				this.m_activeParentNodes.Push(new ValueTuple<TreeNode, bool>(markerBeginNode, true));
				this.m_beginDebugMarkers.Push(markerBeginNode);
				string text = (string)markerBeginNode.Values[6];
				string text2 = (string)markerBeginNode.Values[4];
				string text3 = (((string)markerBeginNode.Values[2] == "vkCmdDebugMarkerBeginEXT") ? "pMarkerInfo" : "pLabelInfo");
				string text4 = (((string)markerBeginNode.Values[2] == "vkCmdDebugMarkerBeginEXT") ? "pMarkerName" : "pLabelName");
				JObject jobject = JObject.Parse(text);
				JObject jobject2 = (JObject)jobject.SelectToken(text3);
				if (jobject2 == null)
				{
					return;
				}
				JToken jtoken = jobject2.SelectToken(text4);
				if (jtoken != null)
				{
					string text5 = jtoken.ToString();
					JArray jarray = (JArray)jobject2.SelectToken("color");
					text5 = ((jarray != null) ? VkDebugMarkers.ColorMarkerTextFromJArray(jarray, text5, false) : VkDebugMarkers.ColorMarkerTextFromJArray(jarray, text5, true));
					markerBeginNode.Values[2] = "<b> " + text5 + " </b>";
					object[] values = markerBeginNode.Values;
					int num = 5;
					object obj = markerBeginNode.Values[5];
					values[num] = ((obj != null) ? obj.ToString() : null) + " " + text5;
					if (this.m_pendingEndDebugMarkers > 0U)
					{
						this.m_pendingEndDebugMarkers -= 1U;
						this.m_activeParentNodes.Pop();
						TreeNode treeNode = this.m_beginDebugMarkers.Pop();
						this.AddToParentNode(treeNode);
					}
					return;
				}
			}

			// Token: 0x0600018F RID: 399 RVA: 0x00013B84 File Offset: 0x00011D84
			private void ProcessEndDebugMarkerRegion()
			{
				if (this.m_beginDebugMarkers.Count == 0 || (this.m_activeParentNodes.Count != 0 && !this.m_activeParentNodes.Peek().Item2))
				{
					VkAPITreeModelBuilder.Logger.LogDebug("Unexpected node structure/sequence: Debug Markers are out of order.");
					this.m_pendingEndDebugMarkers += 1U;
					return;
				}
				this.m_activeParentNodes.Pop();
				TreeNode treeNode = this.m_beginDebugMarkers.Pop();
				this.AddToParentNode(treeNode);
			}

			// Token: 0x06000190 RID: 400 RVA: 0x00013BFC File Offset: 0x00011DFC
			private void ProcessDebugMarkerInsert(TreeNode markerNode)
			{
				string text = (string)markerNode.Values[6];
				string text2 = (string)markerNode.Values[4];
				string text3 = (((string)markerNode.Values[2] == "vkCmdDebugMarkerInsertEXT") ? "pMarkerInfo" : "pLabelInfo");
				string text4 = (((string)markerNode.Values[2] == "vkCmdDebugMarkerInsertEXT") ? "pMarkerName" : "pLabelName");
				JObject jobject = JObject.Parse(text);
				JObject jobject2 = (JObject)jobject.SelectToken(text3);
				if (jobject2 == null)
				{
					return;
				}
				JToken jtoken = jobject2.SelectToken(text4);
				if (jtoken != null)
				{
					string text5 = jtoken.ToString();
					JArray jarray = (JArray)jobject2.SelectToken("color");
					text5 = ((jarray != null) ? VkDebugMarkers.ColorMarkerTextFromJArray(jarray, text5, false) : VkDebugMarkers.ColorMarkerTextFromJArray(jarray, text5, true));
					this.AddToParentNode(markerNode);
					markerNode.Values[2] = "<b> " + text5 + " </b>";
					object[] values = markerNode.Values;
					int num = 5;
					object obj = markerNode.Values[5];
					values[num] = ((obj != null) ? obj.ToString() : null) + " " + text5;
					return;
				}
			}

			// Token: 0x06000191 RID: 401 RVA: 0x00013D28 File Offset: 0x00011F28
			private void CopyTreeNode(TreeNode source, out TreeNode dest)
			{
				dest = new TreeNode();
				dest.Values = new object[source.Values.Length];
				Array.Copy(source.Values, dest.Values, source.Values.Length);
				dest.Children = new List<TreeNode>();
				foreach (TreeNode treeNode in source.Children)
				{
					TreeNode treeNode2;
					this.CopyTreeNode(treeNode, out treeNode2);
					dest.Children.Add(treeNode2);
				}
			}

			// Token: 0x06000192 RID: 402 RVA: 0x00013DCC File Offset: 0x00011FCC
			private void AddToParentNode(TreeNode childNode)
			{
				ValueTuple<TreeNode, bool> valueTuple = this.m_activeParentNodes.Peek();
				valueTuple.Item1.Children.Add(childNode);
			}

			// Token: 0x06000193 RID: 403 RVA: 0x00013DF8 File Offset: 0x00011FF8
			public static ulong GetCommandBufferID(TreeNode vkCmdNode)
			{
				string text = (string)vkCmdNode.Values[6];
				ulong num = 0UL;
				JObject jobject = JObject.Parse(text);
				JToken jtoken = jobject.SelectToken("commandBuffer");
				if (jtoken != null)
				{
					num = VkHelper.GetUint64Value(jtoken);
				}
				return num;
			}

			// Token: 0x04000453 RID: 1107
			public const string DISPLAY_NAME = "vkBeginCommandBuffer";

			// Token: 0x04000454 RID: 1108
			private VkAPITreeModelBuilder m_parent;

			// Token: 0x04000455 RID: 1109
			private VkBoundInfo m_currentBoundInfo = new VkBoundInfo();

			// Token: 0x04000456 RID: 1110
			private TreeNode m_currentRenderPass;

			// Token: 0x04000457 RID: 1111
			private TreeNode m_currentTileExecutionNode;

			// Token: 0x04000458 RID: 1112
			private TreeNode m_commandBufferNode;

			// Token: 0x04000459 RID: 1113
			private ulong m_id;

			// Token: 0x0400045A RID: 1114
			private bool m_isPrimaryCommandBuffer = true;

			// Token: 0x0400045B RID: 1115
			private bool m_submitted;

			// Token: 0x0400045C RID: 1116
			[TupleElementNames(new string[] { "node", "isBeginDebugMarkerNode" })]
			private Stack<ValueTuple<TreeNode, bool>> m_activeParentNodes = new Stack<ValueTuple<TreeNode, bool>>();

			// Token: 0x0400045D RID: 1117
			private Stack<TreeNode> m_beginDebugMarkers = new Stack<TreeNode>();

			// Token: 0x0400045E RID: 1118
			private uint m_pendingEndDebugMarkers;
		}
	}
}
