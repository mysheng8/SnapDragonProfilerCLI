using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Sdp;
using Sdp.Helpers;

namespace QGLPlugin
{
	// Token: 0x0200002D RID: 45
	public class VkSnapshotModel
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00007861 File Offset: 0x00005A61
		public void SnapshotStarted()
		{
			VkSnapshotModel.loadingSnapshotSignal.Reset();
			this.ResourcesPerDrawcall.Clear();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00007879 File Offset: 0x00005A79
		public void SnapshotFinished()
		{
			VkSnapshotModel.loadingSnapshotSignal.Set();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00007888 File Offset: 0x00005A88
		public void GetBoundInfo(uint captureID, uint drawCallID, out VkBoundInfo info)
		{
			VkSnapshotModel.loadingSnapshotSignal.WaitOne();
			Dictionary<uint, VkBoundInfo> dictionary;
			if (!this.m_drawCallInfos.TryGetValue(captureID, out dictionary))
			{
				dictionary = new Dictionary<uint, VkBoundInfo>();
				this.m_drawCallInfos[captureID] = dictionary;
			}
			dictionary.TryGetValue(drawCallID, out info);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000078CC File Offset: 0x00005ACC
		public VkBoundInfo GetParentBoundInfo(uint captureID, ParentBoundInfo parent)
		{
			Dictionary<uint, VkBoundInfo> dictionary;
			if (!this.m_drawCallInfos.TryGetValue(captureID, out dictionary))
			{
				dictionary = new Dictionary<uint, VkBoundInfo>();
				this.m_drawCallInfos[captureID] = dictionary;
			}
			VkBoundInfo vkBoundInfo;
			if (!dictionary.TryGetValue(parent.ParentApiID, out vkBoundInfo))
			{
				Dictionary<uint, VkBoundInfo> dictionary2 = dictionary;
				uint parentApiID = parent.ParentApiID;
				VkBoundInfo vkBoundInfo2 = new VkBoundInfo();
				vkBoundInfo2.IsDrawcallParent = true;
				vkBoundInfo2.ParentPipelines = new HashSet<ulong>();
				VkBoundInfo vkBoundInfo3 = vkBoundInfo2;
				dictionary2[parentApiID] = vkBoundInfo2;
				vkBoundInfo = vkBoundInfo3;
				bool flag = false;
				VkCapture capture = QGLPlugin.VkSnapshotModel.GetCapture((int)captureID);
				foreach (TreeNode treeNode in capture.Builder.DrawcallNodes)
				{
					uint num = (uint)treeNode.Values[0];
					if (num == parent.DrawcallStartID)
					{
						flag = true;
					}
					if (flag)
					{
						VkBoundInfo vkBoundInfo4;
						if (dictionary.TryGetValue(num, out vkBoundInfo4))
						{
							vkBoundInfo.ParentPipelines.Add(vkBoundInfo4.BoundPipeline);
							foreach (KeyValuePair<ulong, DescSetBindings> keyValuePair in vkBoundInfo4.BoundDescriptorSets)
							{
								vkBoundInfo.BoundDescriptorSets[keyValuePair.Value.DescSetID] = keyValuePair.Value;
								vkBoundInfo.DescSetIDs.Add(keyValuePair.Value.DescSetID);
							}
						}
						if (parent.DrawcallEnd == treeNode.Values[1].ToString())
						{
							break;
						}
					}
				}
			}
			return vkBoundInfo;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00007A68 File Offset: 0x00005C68
		public void AddDrawCallInfo(uint captureID, uint apiID, VkBoundInfo info)
		{
			Dictionary<uint, VkBoundInfo> dictionary;
			if (!this.m_drawCallInfos.TryGetValue(captureID, out dictionary))
			{
				dictionary = new Dictionary<uint, VkBoundInfo>();
				this.m_drawCallInfos[captureID] = dictionary;
			}
			dictionary[apiID] = new VkBoundInfo(info);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00007AA8 File Offset: 0x00005CA8
		public IEnumerable<VkSnapshotModel.DescSetInfo> GetPipelineDescriptorSets(uint captureID, ulong pipelineId)
		{
			VkSnapshotModel.loadingSnapshotSignal.WaitOne();
			return Enumerable.Select<KeyValuePair<ulong, DescSetBindings>, VkSnapshotModel.DescSetInfo>(Enumerable.SelectMany<KeyValuePair<uint, VkBoundInfo>, KeyValuePair<ulong, DescSetBindings>>(Enumerable.Where<KeyValuePair<uint, VkBoundInfo>>(this.m_drawCallInfos[captureID], (KeyValuePair<uint, VkBoundInfo> kvp) => kvp.Value.BoundPipeline == pipelineId), (KeyValuePair<uint, VkBoundInfo> kvp) => kvp.Value.BoundDescriptorSets), (KeyValuePair<ulong, DescSetBindings> descSet) => new VkSnapshotModel.DescSetInfo
			{
				ID = descSet.Value.DescSetID,
				DrawCallID = descSet.Key
			});
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00007B34 File Offset: 0x00005D34
		public VkMetricsCapturedModel GetMetricsForCapture(uint captureId)
		{
			VkMetricsCapturedModel vkMetricsCapturedModel;
			if (this.m_metricsCaptured.TryGetValue(captureId, out vkMetricsCapturedModel))
			{
				return vkMetricsCapturedModel;
			}
			vkMetricsCapturedModel = (this.m_metricsCaptured[captureId] = new VkMetricsCapturedModel(captureId));
			QGLPlugin.ClearMetricsBuffer();
			return vkMetricsCapturedModel;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007B70 File Offset: 0x00005D70
		public bool ShouldGenerateImages(int captureID)
		{
			string text;
			return !this.GetCapture(captureID).HasThumbnails && SdpApp.ModelManager.SnapshotModel.DataFilenames.TryGetValue((uint)captureID, out text);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00007BAC File Offset: 0x00005DAC
		public void PopulateDescSets(uint captureID)
		{
			VkCapture capture = this.GetCapture((int)captureID);
			if (QGLPlugin.SnapshotDsbBuffer == null || QGLPlugin.SnapshotDsbBuffer.size == 0U)
			{
				return;
			}
			int num = Marshal.SizeOf<DescSetBindings.DescBindings>(default(DescSetBindings.DescBindings));
			long num2 = (long)((ulong)QGLPlugin.SnapshotDsbBuffer.size / (ulong)((long)num));
			IntPtr intPtr = QGLPlugin.SnapshotDsbBuffer.data;
			int num3 = 0;
			while ((long)num3 < num2)
			{
				DescSetBindings.DescBindings descBindings = Marshal.PtrToStructure<DescSetBindings.DescBindings>(intPtr);
				intPtr += num;
				DescSetBindings descSetBindings;
				if (!capture.AllDescSetBindings.TryGetValue(descBindings.descriptorSetID, out descSetBindings))
				{
					descSetBindings = (capture.AllDescSetBindings[descBindings.descriptorSetID] = new DescSetBindings(descBindings.descriptorSetID));
				}
				descSetBindings.Bindings[(ulong)descBindings.slotNum] = descBindings;
				List<ulong> list;
				if (!this.DsbByApi.TryGetValue(descBindings.apiID, out list))
				{
					list = (this.DsbByApi[descBindings.apiID] = new List<ulong>());
				}
				list.Add(descBindings.descriptorSetID);
				num3++;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00007CBC File Offset: 0x00005EBC
		public VkCapture GetCapture(int capId)
		{
			VkCapture vkCapture;
			if (!this.m_captures.TryGetValue(capId, out vkCapture))
			{
				vkCapture = (this.m_captures[capId] = new VkCapture(capId));
			}
			return vkCapture;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00007CF0 File Offset: 0x00005EF0
		public HashSet<ulong> GetPipelineLibraryUsed(int captureID, ulong pipelineId, ModelObject pipelineLibraryMdlObj)
		{
			HashSet<ulong> hashSet = new HashSet<ulong>();
			ModelObjectDataList data = pipelineLibraryMdlObj.GetData(new StringList
			{
				"captureID",
				captureID.ToString()
			});
			if (data.Count == 0)
			{
				return hashSet;
			}
			this.GetPipelineLibraryUsed(captureID, pipelineId, pipelineLibraryMdlObj, hashSet);
			return hashSet;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00007D3C File Offset: 0x00005F3C
		private void GetPipelineLibraryUsed(int captureID, ulong pipelineId, ModelObject pipelineLibraryMdlObj, HashSet<ulong> pipelineLibraryUsed)
		{
			Dictionary<ulong, HashSet<ulong>> dictionary;
			if (!VkSnapshotModel.pipelineLibraryCache.TryGetValue(captureID, out dictionary))
			{
				dictionary = new Dictionary<ulong, HashSet<ulong>>();
				VkSnapshotModel.pipelineLibraryCache[captureID] = dictionary;
			}
			HashSet<ulong> hashSet;
			if (dictionary.TryGetValue(pipelineId, out hashSet))
			{
				pipelineLibraryUsed.UnionWith(hashSet);
				return;
			}
			ModelObjectDataList data = pipelineLibraryMdlObj.GetData(new StringList
			{
				"captureID",
				captureID.ToString(),
				"pipelineID",
				pipelineId.ToString()
			});
			HashSet<ulong> hashSet2 = new HashSet<ulong>();
			foreach (ModelObjectData modelObjectData in data)
			{
				ulong num = Uint64Converter.Convert(modelObjectData.GetValue("libraryID"));
				hashSet2.Add(num);
				this.GetPipelineLibraryUsed(captureID, num, pipelineLibraryMdlObj, hashSet2);
			}
			dictionary[pipelineId] = hashSet2;
			pipelineLibraryUsed.UnionWith(hashSet2);
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00007E34 File Offset: 0x00006034
		public Dictionary<uint, List<PrepopulateCategoryArgs>> ResourcesPerDrawcall
		{
			get
			{
				return this.m_resourcesPerDrawcall;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00007E3C File Offset: 0x0000603C
		public Dictionary<int, Dictionary<VkSnapshotModel.ResourceKey, List<uint>>> DrawcallsPerResource
		{
			get
			{
				return this.m_drawcallsPerResource;
			}
		}

		// Token: 0x0400036C RID: 876
		private static ManualResetEvent loadingSnapshotSignal = new ManualResetEvent(false);

		// Token: 0x0400036D RID: 877
		private static Dictionary<int, Dictionary<ulong, HashSet<ulong>>> pipelineLibraryCache = new Dictionary<int, Dictionary<ulong, HashSet<ulong>>>();

		// Token: 0x0400036E RID: 878
		private Dictionary<uint, Dictionary<uint, VkBoundInfo>> m_drawCallInfos = new Dictionary<uint, Dictionary<uint, VkBoundInfo>>();

		// Token: 0x0400036F RID: 879
		private Dictionary<uint, List<PrepopulateCategoryArgs>> m_resourcesPerDrawcall = new Dictionary<uint, List<PrepopulateCategoryArgs>>();

		// Token: 0x04000370 RID: 880
		private Dictionary<int, Dictionary<VkSnapshotModel.ResourceKey, List<uint>>> m_drawcallsPerResource = new Dictionary<int, Dictionary<VkSnapshotModel.ResourceKey, List<uint>>>();

		// Token: 0x04000371 RID: 881
		private readonly Dictionary<uint, VkMetricsCapturedModel> m_metricsCaptured = new Dictionary<uint, VkMetricsCapturedModel>();

		// Token: 0x04000372 RID: 882
		public Dictionary<Tuple<int, string>, MetricsCost> ResourceCostList = new Dictionary<Tuple<int, string>, MetricsCost>();

		// Token: 0x04000373 RID: 883
		public Dictionary<uint, List<ulong>> DsbByApi = new Dictionary<uint, List<ulong>>();

		// Token: 0x04000374 RID: 884
		private Dictionary<int, VkCapture> m_captures = new Dictionary<int, VkCapture>();

		// Token: 0x02000058 RID: 88
		public class DescSetInfo
		{
			// Token: 0x0400046E RID: 1134
			public ulong ID;

			// Token: 0x0400046F RID: 1135
			public ulong DrawCallID;
		}

		// Token: 0x02000059 RID: 89
		public class ResourceKey : Tuple<int, long>
		{
			// Token: 0x060001A6 RID: 422 RVA: 0x00013F15 File Offset: 0x00012115
			public ResourceKey(int i, long l)
				: base(i, l)
			{
			}
		}
	}
}
