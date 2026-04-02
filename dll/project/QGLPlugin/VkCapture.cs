using System;
using System.Collections.Generic;
using Sdp.Helpers;

namespace QGLPlugin
{
	// Token: 0x0200002E RID: 46
	public class VkCapture
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00007EBC File Offset: 0x000060BC
		public void PopulateASHierarchies(DataModel dataModel, Model model)
		{
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanSnapshotASInfo");
			ModelObjectDataList data = modelObject.GetData("captureID", this.m_captureId.ToString());
			ModelObject modelObject2 = dataModel.GetModelObject(model, "VulkanSnapshotASInstanceDescriptor");
			ModelObjectDataList data2 = modelObject2.GetData("captureID", this.m_captureId.ToString());
			foreach (ModelObjectData modelObjectData in data)
			{
				ulong num = Uint64Converter.Convert(modelObjectData.GetValue("resourceID"));
				uint num2 = UintConverter.Convert(modelObjectData.GetValue("type"));
				uint num3 = UintConverter.Convert(modelObjectData.GetValue("instancesCount"));
				if (num2 == 0U)
				{
					ModelObjectDataList data3 = modelObject2.GetData(new StringList
					{
						"captureID",
						this.m_captureId.ToString(),
						"tlasID",
						num.ToString()
					});
					foreach (ModelObjectData modelObjectData2 in data3)
					{
						uint num4 = UintConverter.Convert(modelObjectData2.GetValue("asInstanceDescriptorIndex"));
						ulong num5 = Uint64Converter.Convert(modelObjectData2.GetValue("blasID"));
						ulong num6 = Uint64Converter.Convert(modelObjectData2.GetValue("blasDeviceAddress"));
						if (num5 == 0UL && num6 != 0UL)
						{
							ModelObjectDataList data4 = modelObject.GetData(new StringList
							{
								"captureID",
								this.m_captureId.ToString(),
								"blasDeviceAddress",
								num6.ToString()
							});
							if (data4.Count > 0)
							{
								num5 = Uint64Converter.Convert(data4[0].GetValue("resourceID"));
							}
						}
						if (num5 != 0UL)
						{
							ulong[] array;
							if (this.ASHierarchies.TryGetValue(num, out array))
							{
								array[(int)num4] = num5;
							}
							else
							{
								array = new ulong[num3];
								array[(int)num4] = num5;
								this.ASHierarchies[num] = array;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000810C File Offset: 0x0000630C
		public void PopulateTileMemory(DataModel dataModel, Model model)
		{
			ModelObject modelObject = dataModel.GetModelObject(model, "VulkanTileMemoryLinks");
			ModelObjectDataList data = modelObject.GetData("captureID", this.m_captureId.ToString());
			foreach (ModelObjectData modelObjectData in data)
			{
				this.TileMemoryResources.Add(Uint64Converter.Convert(modelObjectData.GetValue("resourceID")));
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00008190 File Offset: 0x00006390
		public void MapPushConstant(ulong currentCallID, uint pushConstantSeqID)
		{
			this.DrawCallIDToPushConstantSeqID.Add(new Tuple<ulong, uint>(currentCallID, pushConstantSeqID));
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000081A4 File Offset: 0x000063A4
		public void SetLastDrawCall(uint apiID)
		{
			this.LastDrawCall = apiID;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000081AD File Offset: 0x000063AD
		public void SetLastDrawCallID(string drawCallID)
		{
			this.LastDrawCallID = drawCallID;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000081B8 File Offset: 0x000063B8
		public VkCapture(int captureId)
		{
			this.m_captureId = captureId;
		}

		// Token: 0x04000375 RID: 885
		internal bool HasThumbnails;

		// Token: 0x04000376 RID: 886
		internal bool HasResources;

		// Token: 0x04000377 RID: 887
		internal VkAPITreeModelBuilder Builder;

		// Token: 0x04000378 RID: 888
		internal Dictionary<ulong, ulong[]> ASHierarchies = new Dictionary<ulong, ulong[]>();

		// Token: 0x04000379 RID: 889
		internal readonly Dictionary<ulong, DescSetBindings> AllDescSetBindings = new Dictionary<ulong, DescSetBindings>();

		// Token: 0x0400037A RID: 890
		internal readonly Dictionary<int, List<ulong>> ResourceIDs = new Dictionary<int, List<ulong>>();

		// Token: 0x0400037B RID: 891
		internal readonly HashSet<ulong> TileMemoryResources = new HashSet<ulong>();

		// Token: 0x0400037C RID: 892
		internal uint LastDrawCall;

		// Token: 0x0400037D RID: 893
		internal string LastDrawCallID;

		// Token: 0x0400037E RID: 894
		internal readonly List<Tuple<ulong, uint>> DrawCallIDToPushConstantSeqID = new List<Tuple<ulong, uint>>();

		// Token: 0x0400037F RID: 895
		private int m_captureId;
	}
}
