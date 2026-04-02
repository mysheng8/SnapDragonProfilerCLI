using System;

namespace QGLPlugin
{
	// Token: 0x02000003 RID: 3
	internal static class Constants
	{
		// Token: 0x04000008 RID: 8
		internal const string VulkanModelName = "VulkanSnapshot";

		// Token: 0x04000009 RID: 9
		internal const string QGLModelName = "QGLModel";

		// Token: 0x0400000A RID: 10
		internal const string GLESModelName = "GLESModel";

		// Token: 0x0400000B RID: 11
		internal const string SnapshotTableName = "VulkanSnapshotScreenshots";

		// Token: 0x0400000C RID: 12
		internal const string MemoryTableName = "VulkanSnapshotMemoryBuffers";

		// Token: 0x0400000D RID: 13
		internal const string MemoryLinksTableName = "VulkanSnapshotMemoryBufferLinks";

		// Token: 0x0400000E RID: 14
		internal const string TexturesTableName = "VulkanSnapshotTextures";

		// Token: 0x0400000F RID: 15
		internal const string PipelineLayoutsTableName = "VulkanSnapshotPipelineLayouts";

		// Token: 0x04000010 RID: 16
		internal const string GraphicsPipelinesTableName = "VulkanSnapshotGraphicsPipelines";

		// Token: 0x04000011 RID: 17
		internal const string ComputePipelinesTableName = "VulkanSnapshotComputePipelines";

		// Token: 0x04000012 RID: 18
		internal const string RaytracingPipelinesTableName = "VulkanSnapshotRaytracingPipelines";

		// Token: 0x04000013 RID: 19
		internal const string PipelineLibraryTableName = "VulkanSnapshotPipelineLibraries";

		// Token: 0x04000014 RID: 20
		internal const string ShaderModulesTableName = "VulkanSnapshotShaderModules";

		// Token: 0x04000015 RID: 21
		internal const string ShaderStagesTableName = "VulkanSnapshotShaderStages";

		// Token: 0x04000016 RID: 22
		internal const string SnapshotShaderDataTableName = "VulkanSnapshotShaderData";

		// Token: 0x04000017 RID: 23
		internal const string InputBindingDescTableName = "VulkanSnapshotInputBindingDescriptions";

		// Token: 0x04000018 RID: 24
		internal const string AttributeDescTableName = "VulkanSnapshotInputAttributeDescriptions";

		// Token: 0x04000019 RID: 25
		internal const string InputAssemblyTableName = "VulkanSnapshotInputAssemblyStates";

		// Token: 0x0400001A RID: 26
		internal const string TessStateTableName = "VulkanSnapshotTesselationStates";

		// Token: 0x0400001B RID: 27
		internal const string ViewPortsTableName = "VulkanSnapshotViewPorts";

		// Token: 0x0400001C RID: 28
		internal const string ScissorsTableName = "VulkanSnapshotViewPortScissors";

		// Token: 0x0400001D RID: 29
		internal const string RasterizationTableName = "VulkanSnapshotRasterizationStates";

		// Token: 0x0400001E RID: 30
		internal const string MultiSampleTableName = "VulkanSnapshotMultiSampleStates";

		// Token: 0x0400001F RID: 31
		internal const string DepthStencilTableName = "VulkanSnapshotDepthStencilStates";

		// Token: 0x04000020 RID: 32
		internal const string ColorBlendTableName = "VulkanSnapshotColorBlendStates";

		// Token: 0x04000021 RID: 33
		internal const string ColorBlendAttachmentTableName = "VulkanSnapshotColorBlendAttachmentState";

		// Token: 0x04000022 RID: 34
		internal const string ColorBlendConstantsTableName = "VulkanSnapshotColorBlendConstants";

		// Token: 0x04000023 RID: 35
		internal const string DynamicStatesTableName = "VulkanSnapshotDynamicStates";

		// Token: 0x04000024 RID: 36
		internal const string ImageViewsTableName = "VulkanSnapshotImageViews";

		// Token: 0x04000025 RID: 37
		internal const string DescriptorSetsTableName = "VulkanSnapshotDescriptorSets";

		// Token: 0x04000026 RID: 38
		internal const string DescriptorSetLayoutsTableName = "VulkanSnapshotDescriptorSetLayouts";

		// Token: 0x04000027 RID: 39
		internal const string DescriptorSetLayoutBindingsTableName = "VulkanSnapshotDescriptorSetLayoutBindings";

		// Token: 0x04000028 RID: 40
		internal const string DescriptorSetLayoutLinksTableName = "VulkanSnapshotDescriptorSetLayoutLinks";

		// Token: 0x04000029 RID: 41
		internal const string DescriptorSetBindingsTableName = "VulkanSnapshotDescriptorSetBindings";

		// Token: 0x0400002A RID: 42
		internal const string EndCaptureTableName = "VulkanEndCapture";

		// Token: 0x0400002B RID: 43
		internal const string ObjectLabelsTableName = "VulkanSnapshotObjectLabels";

		// Token: 0x0400002C RID: 44
		internal const string VulkanTraceShaderDataTableName = "VulkanTraceShaderData";

		// Token: 0x0400002D RID: 45
		internal const string VulkanRayQueryTableName = "VulkanSnapshotRayQuery";

		// Token: 0x0400002E RID: 46
		internal const string VulkanASInfoTableName = "VulkanSnapshotASInfo";

		// Token: 0x0400002F RID: 47
		internal const string VulkanASInstanceDescriptorTableName = "VulkanSnapshotASInstanceDescriptor";

		// Token: 0x04000030 RID: 48
		internal const string VulkanTileMemoryTableName = "VulkanTileMemoryLinks";

		// Token: 0x04000031 RID: 49
		internal const string PushConstantRangesTableName = "VulkanSnapshotPushConstantRanges";

		// Token: 0x04000032 RID: 50
		internal const string PushConstantDataTableName = "VulkanSnapshotPushConstantData";

		// Token: 0x04000033 RID: 51
		internal const string VulkanTensorTableName = "VulkanSnapshotTensors";

		// Token: 0x04000034 RID: 52
		internal const string VulkanTensorViewTableName = "VulkanSnapshotTensorsView";

		// Token: 0x04000035 RID: 53
		internal const string CaptureIDColName = "captureID";

		// Token: 0x04000036 RID: 54
		internal const string ResourceIDColName = "resourceID";

		// Token: 0x04000037 RID: 55
		internal const string LayerCountColName = "layerCount";

		// Token: 0x04000038 RID: 56
		internal const string LevelCountColName = "levelCount";

		// Token: 0x04000039 RID: 57
		internal const string SampleCountColName = "sampleCount";

		// Token: 0x0400003A RID: 58
		internal const string WidthColName = "width";

		// Token: 0x0400003B RID: 59
		internal const string HeightColName = "height";

		// Token: 0x0400003C RID: 60
		internal const string DepthColName = "depth";

		// Token: 0x0400003D RID: 61
		internal const string FormatColName = "format";

		// Token: 0x0400003E RID: 62
		internal const string PipelineIDColName = "pipelineID";

		// Token: 0x0400003F RID: 63
		internal const string LibraryIDColName = "libraryID";

		// Token: 0x04000040 RID: 64
		internal const string ShaderModuleIDColName = "shaderModuleID";

		// Token: 0x04000041 RID: 65
		internal const string ShaderStageTypeColName = "stageType";

		// Token: 0x04000042 RID: 66
		internal const string ModuleEntryPointColName = "pName";

		// Token: 0x04000043 RID: 67
		internal const string PipelineCacheIDColName = "pipelineCacheID";

		// Token: 0x04000044 RID: 68
		internal const string LayoutIDColName = "layoutID";

		// Token: 0x04000045 RID: 69
		internal const string RenderPassColName = "renderPass";

		// Token: 0x04000046 RID: 70
		internal const string SubpassColName = "subpass";

		// Token: 0x04000047 RID: 71
		internal const string BasePipelineHandleColName = "basePipelineHandle";

		// Token: 0x04000048 RID: 72
		internal const string BasePipelineIndexColName = "basePipelineIndex";

		// Token: 0x04000049 RID: 73
		internal const string BindingColName = "binding";

		// Token: 0x0400004A RID: 74
		internal const string StrideColName = "stride";

		// Token: 0x0400004B RID: 75
		internal const string InputRateColName = "inputRate";

		// Token: 0x0400004C RID: 76
		internal const string LocationColName = "location";

		// Token: 0x0400004D RID: 77
		internal const string OffsetColName = "offset";

		// Token: 0x0400004E RID: 78
		internal const string SizeColName = "size";

		// Token: 0x0400004F RID: 79
		internal const string DataColName = "data";

		// Token: 0x04000050 RID: 80
		internal const string TopologyColName = "topology";

		// Token: 0x04000051 RID: 81
		internal const string PrimitiveRestartEnableColName = "primitiveRestartEnable";

		// Token: 0x04000052 RID: 82
		internal const string PatchControlPointColName = "patchControlPoints";

		// Token: 0x04000053 RID: 83
		internal const string XColName = "x";

		// Token: 0x04000054 RID: 84
		internal const string YColName = "y";

		// Token: 0x04000055 RID: 85
		internal const string MinDepthColName = "minDepth";

		// Token: 0x04000056 RID: 86
		internal const string MaxDepthColName = "maxDepth";

		// Token: 0x04000057 RID: 87
		internal const string DepthClampEnableColName = "depthClampEnable";

		// Token: 0x04000058 RID: 88
		internal const string RasterizerDiscardEnableColName = "rasterizerDiscardEnable";

		// Token: 0x04000059 RID: 89
		internal const string PolygonModeColName = "polygonMode";

		// Token: 0x0400005A RID: 90
		internal const string CullModeColName = "cullMode";

		// Token: 0x0400005B RID: 91
		internal const string FrontFaceColName = "frontFace";

		// Token: 0x0400005C RID: 92
		internal const string DepthBiasEnableColName = "depthBiasEnable";

		// Token: 0x0400005D RID: 93
		internal const string DepthBiasConstantFactorColName = "depthBiasConstantFactor";

		// Token: 0x0400005E RID: 94
		internal const string DepthBiasClampColName = "depthBiasClamp";

		// Token: 0x0400005F RID: 95
		internal const string DepthBiasSlopeFactorColName = "depthBiasSlopeFactor";

		// Token: 0x04000060 RID: 96
		internal const string LineWidthColName = "lineWidth";

		// Token: 0x04000061 RID: 97
		internal const string RasterizationSamplesColName = "rasterizationSamples";

		// Token: 0x04000062 RID: 98
		internal const string SampleShadingEnableColName = "sampleShadingEnable";

		// Token: 0x04000063 RID: 99
		internal const string MinSampleShadingColName = "minSampleShading";

		// Token: 0x04000064 RID: 100
		internal const string SampleMaskColName = "pSampleMask";

		// Token: 0x04000065 RID: 101
		internal const string AlphaToCoverageEnableColName = "alphaToCoverageEnable";

		// Token: 0x04000066 RID: 102
		internal const string AlphaToOneEnableColName = "alphaToOneEnable";

		// Token: 0x04000067 RID: 103
		internal const string DepthTestEnableColName = "depthTestEnable";

		// Token: 0x04000068 RID: 104
		internal const string DepthWriteEnableColName = "depthWriteEnable";

		// Token: 0x04000069 RID: 105
		internal const string DepthCompareOpColName = "depthCompareOp";

		// Token: 0x0400006A RID: 106
		internal const string DepthBoundsTestColName = "depthBoundsTestEnable";

		// Token: 0x0400006B RID: 107
		internal const string StencilTestEnableColName = "stencilTestEnable";

		// Token: 0x0400006C RID: 108
		internal const string FrontFailOpColName = "front_failOp";

		// Token: 0x0400006D RID: 109
		internal const string FrontPassOpColName = "front_passOp";

		// Token: 0x0400006E RID: 110
		internal const string FrontDepthFailOpColName = "front_depthFailOp";

		// Token: 0x0400006F RID: 111
		internal const string FrontCompareOpColName = "front_compareOp";

		// Token: 0x04000070 RID: 112
		internal const string FrontCompareMaskColName = "front_compareMask";

		// Token: 0x04000071 RID: 113
		internal const string FrontWriteMaskColName = "front_writeMask";

		// Token: 0x04000072 RID: 114
		internal const string FrontReferenceColName = "front_reference";

		// Token: 0x04000073 RID: 115
		internal const string BackFailOpColName = "back_failOp";

		// Token: 0x04000074 RID: 116
		internal const string BackPassOpColName = "back_passOp";

		// Token: 0x04000075 RID: 117
		internal const string BackDepthFailOpColName = "back_depthFailOp";

		// Token: 0x04000076 RID: 118
		internal const string BackCompareOpColName = "back_compareOp";

		// Token: 0x04000077 RID: 119
		internal const string BackCompareMaskColName = "back_compareMask";

		// Token: 0x04000078 RID: 120
		internal const string BackWriteMaskColName = "back_writeMask";

		// Token: 0x04000079 RID: 121
		internal const string BackReferenceColName = "back_reference";

		// Token: 0x0400007A RID: 122
		internal const string MinDepthBoundsColName = "minDepthBounds";

		// Token: 0x0400007B RID: 123
		internal const string MaxDepthBoundsColName = "maxDepthBounds";

		// Token: 0x0400007C RID: 124
		internal const string LogicOpEnableColName = "logicOpEnable";

		// Token: 0x0400007D RID: 125
		internal const string LogicOpColName = "logicOp";

		// Token: 0x0400007E RID: 126
		internal const string BlendEnableColName = "blendEnable";

		// Token: 0x0400007F RID: 127
		internal const string SrcColorBlendFactorColName = "srcColorBlendFactor";

		// Token: 0x04000080 RID: 128
		internal const string DstColorBlendFactorColName = "dstColorBlendFactor";

		// Token: 0x04000081 RID: 129
		internal const string ColorBlendOpColName = "colorBlendOp";

		// Token: 0x04000082 RID: 130
		internal const string SrcAlphaBlendFactorColName = "srcAlphaBlendFactor";

		// Token: 0x04000083 RID: 131
		internal const string DstAlphaBlendFactorColName = "dstAlphaBlendFactor";

		// Token: 0x04000084 RID: 132
		internal const string AlphaBlendOpColName = "alphaBlendOp";

		// Token: 0x04000085 RID: 133
		internal const string ColorWriteMaskColName = "colorWriteMask";

		// Token: 0x04000086 RID: 134
		internal const string BlendValueColName = "value";

		// Token: 0x04000087 RID: 135
		internal const string StateColName = "state";

		// Token: 0x04000088 RID: 136
		internal const string ImageIDColName = "imageID";

		// Token: 0x04000089 RID: 137
		internal const string ViewTypeColName = "viewType";

		// Token: 0x0400008A RID: 138
		internal const string ComponentRColName = "componentR";

		// Token: 0x0400008B RID: 139
		internal const string ComponentGColName = "componentG";

		// Token: 0x0400008C RID: 140
		internal const string ComponentBColName = "componentB";

		// Token: 0x0400008D RID: 141
		internal const string ComponentAColName = "componentA";

		// Token: 0x0400008E RID: 142
		internal const string AspectMaskColName = "aspectMask";

		// Token: 0x0400008F RID: 143
		internal const string BaseMipLevelColName = "baseMipLevel";

		// Token: 0x04000090 RID: 144
		internal const string BaseArrayLayerColName = "baseArrayLayer";

		// Token: 0x04000091 RID: 145
		internal const string DescriptorPoolColName = "descriptorPool";

		// Token: 0x04000092 RID: 146
		internal const string DescriptorTypeColName = "descriptorType";

		// Token: 0x04000093 RID: 147
		internal const string DescriptorCountColName = "descriptorCount";

		// Token: 0x04000094 RID: 148
		internal const string StageFlagsColName = "stageFlags";

		// Token: 0x04000095 RID: 149
		internal const string DescriptorSetIDColName = "descriptorSetID";

		// Token: 0x04000096 RID: 150
		internal const string DescriptorSetLayoutIDColName = "descriptorSetLayoutID";

		// Token: 0x04000097 RID: 151
		internal const string BufferIDColName = "bufferID";

		// Token: 0x04000098 RID: 152
		internal const string RangeColName = "range";

		// Token: 0x04000099 RID: 153
		internal const string SamplerIDColName = "samplerID";

		// Token: 0x0400009A RID: 154
		internal const string ImageViewIDColName = "imageViewID";

		// Token: 0x0400009B RID: 155
		internal const string ImageLayoutColName = "imageLayout";

		// Token: 0x0400009C RID: 156
		internal const string TexelBufferColName = "texBufferView";

		// Token: 0x0400009D RID: 157
		internal const string AccelerationStructColName = "accelerationStructID";

		// Token: 0x0400009E RID: 158
		internal const string TensorColName = "tensorID";

		// Token: 0x0400009F RID: 159
		internal const string TensorViewColName = "tensorViewID";

		// Token: 0x040000A0 RID: 160
		internal const string SlotNumColName = "slotNum";

		// Token: 0x040000A1 RID: 161
		internal const string ApiIDColName = "apiID";

		// Token: 0x040000A2 RID: 162
		internal const string PresentedImageColName = "presentedImage";

		// Token: 0x040000A3 RID: 163
		internal const string MemoryIDColName = "memoryID";

		// Token: 0x040000A4 RID: 164
		internal const string ASIDColName = "structureID";

		// Token: 0x040000A5 RID: 165
		internal const string StructureTypeColName = "type";

		// Token: 0x040000A6 RID: 166
		internal const string ASBuildTypeColName = "buildType";

		// Token: 0x040000A7 RID: 167
		internal const string ASTlasIDColName = "tlasID";

		// Token: 0x040000A8 RID: 168
		internal const string ASBlasIDColName = "blasID";

		// Token: 0x040000A9 RID: 169
		internal const string ASChildrenCountColName = "childrenCount";

		// Token: 0x040000AA RID: 170
		internal const string ASWideNodeCountColName = "wideNodesCount";

		// Token: 0x040000AB RID: 171
		internal const string ASInternalWideNodeCountColName = "internalWideNodesCount";

		// Token: 0x040000AC RID: 172
		internal const string ASPrimitivesCountColName = "primitivesCount";

		// Token: 0x040000AD RID: 173
		internal const string ASBuildFlagsColName = "buildFlags";

		// Token: 0x040000AE RID: 174
		internal const string ASMaxInstancesColName = "maxInstances";

		// Token: 0x040000AF RID: 175
		internal const string ASSahValueColName = "sahValue";

		// Token: 0x040000B0 RID: 176
		internal const string ASGeometriesCountColName = "geometriesCount";

		// Token: 0x040000B1 RID: 177
		internal const string ASInstancesCountColName = "instancesCount";

		// Token: 0x040000B2 RID: 178
		internal const string ASInstanceDescriptorIndexColName = "asInstanceDescriptorIndex";

		// Token: 0x040000B3 RID: 179
		internal const string ASInstanceTransformMatrixColName = "asInstanceTransformMatrix";

		// Token: 0x040000B4 RID: 180
		internal const string ASInstanceCustomIndexColName = "asInstanceCustomIndex";

		// Token: 0x040000B5 RID: 181
		internal const string ASInstanceMaskColName = "asInstanceMask";

		// Token: 0x040000B6 RID: 182
		internal const string ASInstanceOffsetColName = "asInstanceShaderBindingOffset";

		// Token: 0x040000B7 RID: 183
		internal const string ASInstanceFlagsColName = "asInstanceFlags";

		// Token: 0x040000B8 RID: 184
		internal const string ASBlasDeviceAddressColName = "blasDeviceAddress";

		// Token: 0x040000B9 RID: 185
		internal const string ShaderStageColName = "shaderStage";

		// Token: 0x040000BA RID: 186
		internal const string TensorTilingColName = "tiling";

		// Token: 0x040000BB RID: 187
		internal const string TensorDimensionsColName = "pDimensions";

		// Token: 0x040000BC RID: 188
		internal const string TensorStridesColName = "pStrides";

		// Token: 0x040000BD RID: 189
		internal const string TensorUsageColName = "usage";

		// Token: 0x040000BE RID: 190
		public const string QGLPluginProcessorName = "SDP::QGLPluginProcessor";

		// Token: 0x040000BF RID: 191
		public const string SNAPSHOT_DISPLAY_NAME = "Vulkan";

		// Token: 0x040000C0 RID: 192
		public const int VK_SNAPSHOT_SOURCE = 353;

		// Token: 0x040000C1 RID: 193
		public const int VK_API_TRACE_SOURCE = 2401;

		// Token: 0x040000C2 RID: 194
		public const int VK_QUEUE_SUBMIT_SOURCE = 2402;

		// Token: 0x040000C3 RID: 195
		public const string VK_SNAPSHOT_METRIC_NAME = "Vulkan Snapshot";

		// Token: 0x040000C4 RID: 196
		public const uint VK_ACCELERATION_STRUCTURE_TYPE_TOP_LEVEL_KHR = 0U;

		// Token: 0x040000C5 RID: 197
		public const uint VK_ACCELERATION_STRUCTURE_TYPE_BOTTOM_LEVEL_KHR = 1U;

		// Token: 0x040000C6 RID: 198
		public const uint VK_ACCELERATION_STRUCTURE_TYPE_GENERIC_KHR = 2U;

		// Token: 0x040000C7 RID: 199
		public const string RESOURCE_ID_PARAM_NAME = "Resource ID";

		// Token: 0x040000C8 RID: 200
		public const string CAPTURE_ID_PARAM_NAME = "Capture ID";

		// Token: 0x040000C9 RID: 201
		public const string GREEN = "<span foreground='green'>";

		// Token: 0x040000CA RID: 202
		public const string ORANGE = "<span foreground='orange'>";

		// Token: 0x040000CB RID: 203
		public const string YELLOW = "<span foreground='yellow'>";

		// Token: 0x040000CC RID: 204
		public const string WHITE = "<span foreground='white'>";

		// Token: 0x040000CD RID: 205
		public const string GREY = "<span foreground='#BBBBBB'>";

		// Token: 0x040000CE RID: 206
		public const string TILE_MEM_LABEL = "\n<span foreground='orange'>pGMEM</span>";

		// Token: 0x040000CF RID: 207
		internal const uint INIT_CONTENTS = 4294967295U;

		// Token: 0x040000D0 RID: 208
		internal const int SNAPSHOT_COLUMN_SEQUENTIAL_ID = 0;

		// Token: 0x040000D1 RID: 209
		internal const int SNAPSHOT_COLUMN_DISPLAY_ID = 1;

		// Token: 0x040000D2 RID: 210
		internal const int SNAPSHOT_COLUMN_MARKUP_NAME = 2;

		// Token: 0x040000D3 RID: 211
		internal const int SNAPSHOT_COLUMN_MARKUP_PARAMETERS = 3;

		// Token: 0x040000D4 RID: 212
		internal const int SNAPSHOT_COLUMN_THREADID = 4;

		// Token: 0x040000D5 RID: 213
		internal const int SNAPSHOT_COLUMN_FILTER_NAME = 5;

		// Token: 0x040000D6 RID: 214
		internal const int SNAPSHOT_COLUMN_FILTER_PARAMETERS = 6;

		// Token: 0x040000D7 RID: 215
		internal const int SNAPSHOT_COLUMN_DRAW_CALL_API = 7;

		// Token: 0x040000D8 RID: 216
		internal const int SNAPSHOT_COLUMN_SNAPSHOT_DRAW_CALL_API_TO_PLAY = 8;

		// Token: 0x040000D9 RID: 217
		internal const int SNAPSHOT_COLUMN_HAVE_REPLAY = 9;

		// Token: 0x040000DA RID: 218
		internal const int SNAPSHOT_COLUMN_SNAPSHOT_FIRST_METRIC = 10;
	}
}
