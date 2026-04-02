using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using QGLPlugin;

namespace SnapdragonProfilerCLI.Models
{
    /// <summary>
    /// Vulkan Snapshot 数据模型
    /// 复刻 GUI 的 VkSnapshotModel，用于解析和查询 per-DrawCall 资源绑定
    /// </summary>
    public class VulkanSnapshotModel
    {
        // 核心数据结构
        // Key1: captureID, Key2: drawCallID (apiID), Value: 该 DrawCall 的绑定信息
        private readonly Dictionary<uint, Dictionary<uint, VulkanDrawCallInfo>> _drawCallInfos;
        
        // Key: descriptorSetID, Value: 该 DescriptorSet 的所有 Bindings
        private readonly Dictionary<ulong, VulkanDescriptorSet> _allDescriptorSets;
        
        // Framebuffer → Attachments 映射表
        // Key: framebufferID, Value: 该 Framebuffer 包含的 ImageView resourceIDs
        private readonly Dictionary<ulong, List<ulong>> _framebufferAttachments;
        
        // Pipeline → Vertex Input State 映射表
        // Key: pipelineID, Value: Vertex Input State（bindings & attributes）
        private readonly Dictionary<ulong, VulkanPipelineVertexInputState> _pipelineVertexInputStates;
        
        // 反向索引：Resource → DrawCalls
        // Key1: captureID, Key2: (resourceType, resourceID), Value: 使用该资源的 DrawCall 列表
        private readonly Dictionary<uint, Dictionary<ResourceKey, List<uint>>> _drawCallsPerResource;
        
        public VulkanSnapshotModel()
        {
            _drawCallInfos = new Dictionary<uint, Dictionary<uint, VulkanDrawCallInfo>>();
            _allDescriptorSets = new Dictionary<ulong, VulkanDescriptorSet>();
            _framebufferAttachments = new Dictionary<ulong, List<ulong>>();
            _pipelineVertexInputStates = new Dictionary<ulong, VulkanPipelineVertexInputState>();
            _drawCallsPerResource = new Dictionary<uint, Dictionary<ResourceKey, List<uint>>>();
        }
        
        /// <summary>
        /// 加载 Snapshot 数据（从 ImportCapture 生成的 buffers）
        /// </summary>
        /// <param name="captureId">Capture ID</param>
        /// <param name="apiBuffer">API Trace Buffer (SnapshotApiBuffer)</param>
        /// <param name="bindingBuffer">Descriptor Set Binding Buffer (SnapshotDsbBuffer)</param>
        public void LoadSnapshot(uint captureId, BinaryDataPair apiBuffer, BinaryDataPair bindingBuffer)
        {
            if (bindingBuffer.data == IntPtr.Zero || bindingBuffer.size == 0)
            {
                throw new ArgumentException("Invalid binding buffer");
            }
            
            Console.WriteLine($"\n=== Loading Vulkan Snapshot Data ===");
            Console.WriteLine($"Capture ID: {captureId}");
            Console.WriteLine($"Binding Buffer: {bindingBuffer.size} bytes");
            
            // Phase 1: 解析 DescriptorSet 绑定数据
            LoadDescriptorSetBindings(captureId, bindingBuffer);
            
            // Phase 2: 解析 API Trace 并重建 DrawCall 绑定关系
            if (apiBuffer.data != IntPtr.Zero && apiBuffer.size > 0)
            {
                Console.WriteLine($"API Buffer: {apiBuffer.size} bytes");
                ParseApiTrace(captureId, apiBuffer);
            }
            else
            {
                Console.WriteLine("⚠ API Buffer not available, skipping API trace parsing");
            }
            
            // Phase 3: 构建反向索引（Resource → DrawCalls）
            BuildReverseIndex(captureId);
            
            Console.WriteLine($"✓ Loaded {_drawCallInfos[captureId].Count} DrawCalls");
            Console.WriteLine($"✓ Loaded {_allDescriptorSets.Count} DescriptorSets");
        }
        
        /// <summary>
        /// 加载 Snapshot 数据（从 CSV 文件加载 DescriptorSet 绑定）
        /// </summary>
        /// <param name="captureId">Capture ID</param>
        /// <param name="apiBuffer">API Trace Buffer (SnapshotApiBuffer)</param>
        /// <param name="csvPath">CSV 文件路径（保存了 DescriptorSet 绑定数据）</param>
        public void LoadSnapshotFromCSV(uint captureId, BinaryDataPair apiBuffer, string csvPath)
        {
            Console.WriteLine($"\n=== Loading Vulkan Snapshot Data (from CSV) ===");
            Console.WriteLine($"Capture ID: {captureId}");
            Console.WriteLine($"CSV Path: {csvPath}");
            
            // Phase 1: 从 CSV 加载 DescriptorSet 绑定数据
            LoadDescriptorSetBindingsFromCSV(captureId, csvPath);
            
            // Phase 2: 解析 API Trace 并重建 DrawCall 绑定关系
            if (apiBuffer.data != IntPtr.Zero && apiBuffer.size > 0)
            {
                Console.WriteLine($"API Buffer: {apiBuffer.size} bytes");
                ParseApiTrace(captureId, apiBuffer);
            }
            else
            {
                Console.WriteLine("⚠ API Buffer not available, skipping API trace parsing");
            }
            
            // Phase 3: 构建反向索引（Resource → DrawCalls）
            BuildReverseIndex(captureId);
            
            Console.WriteLine($"✓ Loaded {_drawCallInfos[captureId].Count} DrawCalls");
            Console.WriteLine($"✓ Loaded {_allDescriptorSets.Count} DescriptorSets");
        }
        
        /// <summary>
        /// Phase 1: 从 CSV 加载 DescriptorSet 绑定数据
        /// </summary>
        private void LoadDescriptorSetBindingsFromCSV(uint captureId, string csvPath)
        {
            Console.WriteLine("\nPhase 1: Parsing DescriptorSet Bindings from CSV...");
            
            if (!File.Exists(csvPath))
            {
                throw new FileNotFoundException($"CSV file not found: {csvPath}");
            }
            
            int bindingCount = 0;
            
            using (var reader = new StreamReader(csvPath))
            {
                // Skip header
                reader.ReadLine();
                
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    
                    var parts = line.Split(',');
                    if (parts.Length < 14) continue;
                    
                    // CSV format: captureID,apiID,descriptorSetID,slotNum,samplerID,imageViewID,imageLayout,
                    //             texBufferView,bufferID,offset,range,accelerationStructID,tensorID,tensorViewID
                    
                    uint csvCaptureId = uint.Parse(parts[0]);
                    if (csvCaptureId != captureId) continue;
                    
                    var binding = new DescriptorBinding
                    {
                        captureID = csvCaptureId,
                        apiID = uint.Parse(parts[1]),
                        descriptorSetID = ulong.Parse(parts[2]),
                        slotNum = uint.Parse(parts[3]),
                        samplerID = ulong.Parse(parts[4]),
                        imageViewID = ulong.Parse(parts[5]),
                        imageLayout = uint.Parse(parts[6]),
                        texBufferview = ulong.Parse(parts[7]),
                        bufferID = ulong.Parse(parts[8]),
                        offset = ulong.Parse(parts[9]),
                        range = ulong.Parse(parts[10]),
                        accelStructID = ulong.Parse(parts[11]),
                        tensorID = ulong.Parse(parts[12]),
                        tensorViewID = ulong.Parse(parts[13])
                    };
                    
                    // 获取或创建 DescriptorSet
                    if (!_allDescriptorSets.TryGetValue(binding.descriptorSetID, out var descSet))
                    {
                        descSet = new VulkanDescriptorSet(binding.descriptorSetID);
                        _allDescriptorSets[binding.descriptorSetID] = descSet;
                    }
                    
                    // 添加 Binding（按 slotNum 索引）
                    descSet.Bindings[(ulong)binding.slotNum] = binding;
                    bindingCount++;
                }
            }
            
            Console.WriteLine($"  ✓ Parsed {bindingCount} bindings into {_allDescriptorSets.Count} DescriptorSets");
        }
        
        /// <summary>
        /// Phase 1: 解析 DescriptorSet 绑定数据
        /// 复刻 GUI 的 VkSnapshotModel.LoadBindingDataFromSdpBuffer
        /// </summary>
        private void LoadDescriptorSetBindings(uint captureId, BinaryDataPair buffer)
        {
            Console.WriteLine("\nPhase 1: Parsing DescriptorSet Bindings...");
            
            int structSize = Marshal.SizeOf<DescriptorBinding>();
            long numRecords = (long)buffer.size / structSize;
            IntPtr ptr = buffer.data;
            
            Console.WriteLine($"  Structure size: {structSize} bytes");
            Console.WriteLine($"  Total records: {numRecords}");
            
            for (int i = 0; i < numRecords; i++)
            {
                // 反序列化 DescriptorBinding 结构体
                DescriptorBinding binding = Marshal.PtrToStructure<DescriptorBinding>(ptr);
                ptr += structSize;
                
                // 只处理匹配的 captureID
                if (binding.captureID != captureId)
                {
                    continue;
                }
                
                // 获取或创建 DescriptorSet
                if (!_allDescriptorSets.TryGetValue(binding.descriptorSetID, out var descSet))
                {
                    descSet = new VulkanDescriptorSet(binding.descriptorSetID);
                    _allDescriptorSets[binding.descriptorSetID] = descSet;
                }
                
                // 添加 Binding（按 slotNum 索引）
                descSet.Bindings[(ulong)binding.slotNum] = binding;
            }
            
            Console.WriteLine($"  ✓ Parsed {_allDescriptorSets.Count} DescriptorSets");
        }
        
        /// <summary>
        /// Phase 2: 解析 API Trace 并重建 DrawCall 绑定关系
        /// 复刻 GUI 的 VkAPITreeModelBuilder.ProcessAllCalls 核心逻辑
        /// </summary>
        private void ParseApiTrace(uint captureId, BinaryDataPair buffer)
        {
            Console.WriteLine("\nPhase 2: Parsing API Trace...");
            
            if (!_drawCallInfos.ContainsKey(captureId))
            {
                _drawCallInfos[captureId] = new Dictionary<uint, VulkanDrawCallInfo>();
            }
            
            // 解析 API Buffer
            int structSize = Marshal.SizeOf<VulkanSnapshotApi>();
            long numRecords = (long)buffer.size / structSize;
            IntPtr ptr = buffer.data;
            
            Console.WriteLine($"  Parsing {numRecords} API calls...");
            
            // 维护当前绑定状态（模拟 CommandBuffer 的状态）
            VulkanDrawCallInfo currentBoundInfo = new VulkanDrawCallInfo();
            RenderPassContext? currentRenderPass = null;  // 当前活跃的 RenderPass

            // GUI encoded ID counters (matches VkAPITreeModelBuilder.UpdateDrawcallID logic)
            uint currentSubmitIdx    = 1U;  // vkQueueSubmit increments this
            uint currentCmdBufferIdx = 0U;  // vkBeginCommandBuffer increments this (per submit)
            uint currentDrawcallIdx  = 0U;  // draw call within current CB

            int drawCallCount = 0;
            int bindPipelineCount = 0;
            int bindDescSetCount = 0;
            int renderPassCount = 0;
            int framebufferCount = 0;
            int createPipelineCount = 0;
            
            for (long i = 0; i < numRecords; i++)
            {
                VulkanSnapshotApi api = Marshal.PtrToStructure<VulkanSnapshotApi>(ptr);
                ptr += structSize;
                
                // 只处理当前 capture 的 API
                if (api.captureID != captureId)
                    continue;
                
                try
                {
                    // 处理关键 API
                    if (api.name == "vkQueueSubmit" || api.name == "vkQueueSubmit2" || api.name == "vkQueueSubmit2KHR")
                    {
                        // New submit batch: increment submit index, reset per-submit CB counter
                        currentSubmitIdx    += 1U;
                        currentCmdBufferIdx  = 0U;
                        currentDrawcallIdx   = 0U;
                        currentBoundInfo     = new VulkanDrawCallInfo();
                        currentRenderPass    = null;
                    }
                    else if (api.name == "vkBeginCommandBuffer")
                    {
                        // New primary command buffer: increment CB index, reset draw counter
                        currentCmdBufferIdx += 1U;
                        currentDrawcallIdx   = 0U;
                        currentBoundInfo     = new VulkanDrawCallInfo();
                        currentRenderPass    = null;
                    }
                    else if (api.name == "vkCreateGraphicsPipelines")
                    {
                        ProcessCreateGraphicsPipelines(api);
                        createPipelineCount++;
                    }
                    else if (api.name == "vkCreateFramebuffer")
                    {
                        ProcessCreateFramebuffer(api);
                        framebufferCount++;
                    }
                    else if (api.name == "vkCmdBindPipeline")
                    {
                        ProcessBindPipeline(api, currentBoundInfo);
                        bindPipelineCount++;
                    }
                    else if (api.name == "vkCmdBindDescriptorSets")
                    {
                        ProcessBindDescriptorSets(api, currentBoundInfo);
                        bindDescSetCount++;
                    }
                    else if (api.name == "vkCmdBindVertexBuffers")
                    {
                        ProcessBindVertexBuffers(api, currentBoundInfo);
                    }
                    else if (api.name == "vkCmdBindIndexBuffer")
                    {
                        ProcessBindIndexBuffer(api, currentBoundInfo);
                    }
                    else if (api.name == "vkCmdBeginRenderPass" || api.name == "vkCmdBeginRenderPass2" || api.name == "vkCmdBeginRenderPass2KHR")
                    {
                        currentRenderPass = ProcessBeginRenderPass(api, captureId);
                        if (currentRenderPass != null)
                        {
                            currentBoundInfo.CurrentRenderPass = currentRenderPass;
                            renderPassCount++;
                        }
                    }
                    else if (api.name == "vkCmdEndRenderPass" || api.name == "vkCmdEndRenderPass2" || api.name == "vkCmdEndRenderPass2KHR")
                    {
                        currentRenderPass = null;
                        currentBoundInfo.CurrentRenderPass = null;
                    }
                    else if (IsDrawCall(api.name))
                    {
                        currentDrawcallIdx += 1U;

                        // 保存当前 DrawCall 的绑定状态（深拷贝）
                        VulkanDrawCallInfo drawCallInfo = new VulkanDrawCallInfo
                        {
                            DrawCallId      = api.apiID,
                            BoundPipeline   = currentBoundInfo.BoundPipeline,
                            CurrentRenderPass = currentRenderPass,
                            Parameters      = ParseDrawCallParameters(api),
                            SubmitIdx       = currentSubmitIdx,
                            CmdBufferIdx    = currentCmdBufferIdx,
                            DrawcallIdxInCb = currentDrawcallIdx,
                        };
                        
                        // 深拷贝 BoundDescriptorSets
                        foreach (var kvp in currentBoundInfo.BoundDescriptorSets)
                        {
                            drawCallInfo.BoundDescriptorSets[kvp.Key] = kvp.Value;
                        }
                        
                        // 深拷贝 BoundVertexBuffers
                        foreach (var kvp in currentBoundInfo.BoundVertexBuffers)
                        {
                            drawCallInfo.BoundVertexBuffers[kvp.Key] = kvp.Value;
                        }
                        
                        // 拷贝 IndexBuffer 绑定
                        drawCallInfo.BoundIndexBuffer = currentBoundInfo.BoundIndexBuffer;
                        drawCallInfo.IndexBufferOffset = currentBoundInfo.IndexBufferOffset;
                        drawCallInfo.IndexType = currentBoundInfo.IndexType;
                        
                        _drawCallInfos[captureId][api.apiID] = drawCallInfo;
                        drawCallCount++;
                    }
                }
                catch (Exception ex)
                {
                    // 跳过解析失败的 API（可能是参数格式问题）
                    if (i < 10) // 只输出前几个错误
                    {
                        Console.WriteLine($"  ⚠ Failed to parse API {api.name} (ID={api.apiID}): {ex.Message}");
                    }
                }
            }
            
            Console.WriteLine($"  ✓ Parsed {drawCallCount} DrawCalls");
            Console.WriteLine($"  ✓ Processed {bindPipelineCount} vkCmdBindPipeline calls");
            Console.WriteLine($"  ✓ Processed {bindDescSetCount} vkCmdBindDescriptorSets calls");
            Console.WriteLine($"  ✓ Processed {renderPassCount} RenderPasses");
            Console.WriteLine($"  ✓ Processed {framebufferCount} Framebuffers");
            Console.WriteLine($"  ✓ Processed {createPipelineCount} vkCreateGraphicsPipelines calls");
            Console.WriteLine($"  ✓ Parsed {_pipelineVertexInputStates.Count} Pipeline Vertex Input States");
        }
        
        /// <summary>
        /// 解析 DrawCall 参数
        /// </summary>
        private DrawCallParameters? ParseDrawCallParameters(VulkanSnapshotApi api)
        {
            if (string.IsNullOrEmpty(api.parameters))
                return null;
            
            try
            {
                JObject json = JObject.Parse(api.parameters);
                DrawCallParameters parameters = new DrawCallParameters
                {
                    ApiName = api.name
                };
                
                // vkCmdDraw
                if (api.name == "vkCmdDraw")
                {
                    parameters.VertexCount = json["vertexCount"]?.Value<uint>() ?? 0;
                    parameters.InstanceCount = json["instanceCount"]?.Value<uint>() ?? 0;
                    parameters.FirstVertex = json["firstVertex"]?.Value<uint>() ?? 0;
                    parameters.FirstInstance = json["firstInstance"]?.Value<uint>() ?? 0;
                }
                // vkCmdDrawIndexed
                else if (api.name == "vkCmdDrawIndexed")
                {
                    parameters.IndexCount = json["indexCount"]?.Value<uint>() ?? 0;
                    parameters.InstanceCount = json["instanceCount"]?.Value<uint>() ?? 0;
                    parameters.FirstIndex = json["firstIndex"]?.Value<uint>() ?? 0;
                    parameters.VertexOffset = json["vertexOffset"]?.Value<int>() ?? 0;
                    parameters.FirstInstance = json["firstInstance"]?.Value<uint>() ?? 0;
                }
                // vkCmdDrawIndirect
                else if (api.name == "vkCmdDrawIndirect")
                {
                    parameters.DrawCount = json["drawCount"]?.Value<uint>() ?? 0;
                }
                // vkCmdDrawIndexedIndirect
                else if (api.name == "vkCmdDrawIndexedIndirect")
                {
                    parameters.DrawCount = json["drawCount"]?.Value<uint>() ?? 0;
                }
                // vkCmdDispatch
                else if (api.name == "vkCmdDispatch")
                {
                    parameters.GroupCountX = json["groupCountX"]?.Value<uint>() ?? 0;
                    parameters.GroupCountY = json["groupCountY"]?.Value<uint>() ?? 0;
                    parameters.GroupCountZ = json["groupCountZ"]?.Value<uint>() ?? 0;
                }
                
                return parameters;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        /// <summary>
        /// 处理 vkCmdBindPipeline
        /// </summary>
        private void ProcessBindPipeline(VulkanSnapshotApi api, VulkanDrawCallInfo currentBoundInfo)
        {
            if (string.IsNullOrEmpty(api.parameters))
                return;
            
            JObject json = JObject.Parse(api.parameters);
            JToken? pipelineToken = json.SelectToken("pipeline");
            
            if (pipelineToken != null)
            {
                currentBoundInfo.BoundPipeline = pipelineToken.Value<ulong>();
            }
        }
        
        /// <summary>
        /// 处理 vkCmdBindDescriptorSets
        /// </summary>
        private void ProcessBindDescriptorSets(VulkanSnapshotApi api, VulkanDrawCallInfo currentBoundInfo)
        {
            if (string.IsNullOrEmpty(api.parameters))
                return;
            
            JObject json = JObject.Parse(api.parameters);
            
            // 获取 firstSet
            JToken? firstSetToken = json.SelectToken("firstSet");
            if (firstSetToken == null)
                return;
            
            uint firstSet = firstSetToken.Value<uint>();
            
            // 获取 pDescriptorSets 数组
            JArray? descSetsArray = json.SelectToken("pDescriptorSets") as JArray;
            if (descSetsArray == null)
                return;
            
            // 绑定每个 DescriptorSet，以 pipeline layout slot index 为 key（与 GUI 一致，后绑定覆盖前绑定）
            uint setIndex = firstSet;
            foreach (JToken descSetToken in descSetsArray)
            {
                ulong descSetId = descSetToken.Value<ulong>();
                
                if (_allDescriptorSets.TryGetValue(descSetId, out var descSet))
                {
                    currentBoundInfo.BoundDescriptorSets[setIndex] = descSet;
                }
                
                setIndex++;
            }
        }
        
        /// <summary>
        /// 处理 vkCmdBindVertexBuffers
        /// </summary>
        private void ProcessBindVertexBuffers(VulkanSnapshotApi api, VulkanDrawCallInfo currentBoundInfo)
        {
            if (string.IsNullOrEmpty(api.parameters))
                return;
            
            try
            {
                JObject json = JObject.Parse(api.parameters);
                
                // 获取 firstBinding
                uint firstBinding = json["firstBinding"]?.Value<uint>() ?? 0;
                
                // 获取 pBuffers 数组
                JArray? buffersArray = json.SelectToken("pBuffers") as JArray;
                if (buffersArray == null)
                    return;
                
                // 绑定每个 Vertex Buffer
                uint bindingIndex = firstBinding;
                foreach (JToken bufferToken in buffersArray)
                {
                    ulong bufferId = bufferToken.Value<ulong>();
                    if (bufferId != 0)  // 0 表示解绑
                    {
                        currentBoundInfo.BoundVertexBuffers[bindingIndex] = bufferId;
                    }
                    bindingIndex++;
                }
            }
            catch (Exception)
            {
                // 解析失败，跳过
            }
        }
        
        /// <summary>
        /// 处理 vkCmdBindIndexBuffer
        /// </summary>
        private void ProcessBindIndexBuffer(VulkanSnapshotApi api, VulkanDrawCallInfo currentBoundInfo)
        {
            if (string.IsNullOrEmpty(api.parameters))
                return;
            
            try
            {
                JObject json = JObject.Parse(api.parameters);
                
                // 获取 buffer resourceID
                ulong bufferId = json["buffer"]?.Value<ulong>() ?? 0;
                if (bufferId != 0)
                {
                    currentBoundInfo.BoundIndexBuffer = bufferId;
                    currentBoundInfo.IndexBufferOffset = json["offset"]?.Value<ulong>() ?? 0;
                    
                    // 解析 indexType（可能是字符串或数字）
                    var indexTypeToken = json["indexType"];
                    if (indexTypeToken != null)
                    {
                        if (indexTypeToken.Type == JTokenType.String)
                        {
                            currentBoundInfo.IndexType = ParseIndexType(indexTypeToken.Value<string>());
                        }
                        else
                        {
                            currentBoundInfo.IndexType = indexTypeToken.Value<uint>();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // 解析失败，跳过
            }
        }
        
        /// <summary>
        /// 处理 vkCreateGraphicsPipelines - 解析 Vertex Input State
        /// </summary>
        private void ProcessCreateGraphicsPipelines(VulkanSnapshotApi api)
        {
            if (string.IsNullOrEmpty(api.parameters))
                return;
            
            try
            {
                JObject json = JObject.Parse(api.parameters);
                
                // 获取 pCreateInfos 数组（可能创建多个 pipeline）
                JArray? createInfosArray = json["pCreateInfos"] as JArray;
                JArray? pipelinesArray = json["pPipelines"] as JArray;  // 输出的 pipeline IDs
                
                if (createInfosArray == null || pipelinesArray == null)
                {
                    // Debug: 打印第一个失败的示例
                    if (_pipelineVertexInputStates.Count == 0)
                    {
                        Console.WriteLine($"  [DEBUG] First vkCreateGraphicsPipelines has no pCreateInfos or pPipelines");
                        Console.WriteLine($"  [DEBUG] JSON keys: {string.Join(", ", json.Properties().Select(p => p.Name))}");
                    }
                    return;
                }
                
                // 遍历每个 pipeline
                for (int i = 0; i < createInfosArray.Count && i < pipelinesArray.Count; i++)
                {
                    JObject? createInfo = createInfosArray[i] as JObject;
                    if (createInfo == null)
                        continue;
                    
                    ulong pipelineId = pipelinesArray[i].Value<ulong>();
                    if (pipelineId == 0)
                        continue;
                    
                    // 解析 pVertexInputState
                    JObject? vertexInputState = createInfo["pVertexInputState"] as JObject;
                    if (vertexInputState == null)
                    {
                        // Debug: 打印第一个没有 pVertexInputState 的示例
                        if (_pipelineVertexInputStates.Count == 0)
                        {
                            Console.WriteLine($"  [DEBUG] Pipeline {pipelineId} has no pVertexInputState");
                            Console.WriteLine($"  [DEBUG] CreateInfo keys: {string.Join(", ", createInfo.Properties().Select(p => p.Name))}");
                        }
                        continue;
                    }
                    
                    var pipelineState = new VulkanPipelineVertexInputState
                    {
                        PipelineId = pipelineId
                    };
                    
                    // 解析 pVertexBindingDescriptions
                    JArray? bindingsArray = vertexInputState["pVertexBindingDescriptions"] as JArray;
                    if (bindingsArray != null && bindingsArray.Type != JTokenType.Null)
                    {
                        foreach (JToken bindingToken in bindingsArray)
                        {
                            if (bindingToken is JObject bindingObj)
                            {
                                var binding = new VertexInputBinding
                                {
                                    Binding = bindingObj["binding"]?.Value<uint>() ?? 0,
                                    Stride = bindingObj["stride"]?.Value<uint>() ?? 0,
                                    InputRate = ParseInputRate(bindingObj["inputRate"]?.Value<string>())
                                };
                                pipelineState.Bindings.Add(binding);
                            }
                        }
                    }
                    
                    // 解析 pVertexAttributeDescriptions
                    JArray? attributesArray = vertexInputState["pVertexAttributeDescriptions"] as JArray;
                    if (attributesArray != null && attributesArray.Type != JTokenType.Null)
                    {
                        foreach (JToken attrToken in attributesArray)
                        {
                            if (attrToken is JObject attrObj)
                            {
                                var attribute = new VertexInputAttribute
                                {
                                    Location = attrObj["location"]?.Value<uint>() ?? 0,
                                    Binding = attrObj["binding"]?.Value<uint>() ?? 0,
                                    Format = ParseVkFormat(attrObj["format"]?.Value<string>()),
                                    Offset = attrObj["offset"]?.Value<uint>() ?? 0
                                };
                                pipelineState.Attributes.Add(attribute);
                            }
                        }
                    }
                    
                    // 保存到 Dictionary（如果有 bindings 或 attributes）
                    if (pipelineState.Bindings.Count > 0 || pipelineState.Attributes.Count > 0)
                    {
                        _pipelineVertexInputStates[pipelineId] = pipelineState;
                    }
                    else
                    {
                        // Debug: 打印第一个没有 bindings/attributes 的示例
                        if (_pipelineVertexInputStates.Count == 0)
                        {
                            Console.WriteLine($"  [DEBUG] Pipeline {pipelineId} has pVertexInputState but no bindings/attributes");
                            Console.WriteLine($"  [DEBUG] VertexInputState keys: {string.Join(", ", vertexInputState.Properties().Select(p => p.Name))}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 解析失败，打印详细错误信息（仅第一次）
                if (_pipelineVertexInputStates.Count == 0)
                {
                    Console.WriteLine($"  [DEBUG] vkCreateGraphicsPipelines parse error: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 判断是否是 DrawCall
        /// </summary>
        private bool IsDrawCall(string apiName)
        {
            return apiName.StartsWith("vkCmdDraw") || 
                   apiName.StartsWith("vkCmdDispatch") ||
                   apiName == "vkCmdDrawIndirect" ||
                   apiName == "vkCmdDrawIndexedIndirect" ||
                   apiName == "vkCmdDrawIndirectCount" ||
                   apiName == "vkCmdDrawIndexedIndirectCount";
        }
        
        /// <summary>
        /// 解析 VkVertexInputRate 字符串枚举
        /// </summary>
        private uint ParseInputRate(string? inputRate)
        {
            if (string.IsNullOrEmpty(inputRate))
                return 0;
            
            return inputRate switch
            {
                "VK_VERTEX_INPUT_RATE_VERTEX" => 0,
                "VK_VERTEX_INPUT_RATE_INSTANCE" => 1,
                _ => uint.TryParse(inputRate, out uint val) ? val : 0
            };
        }
        
        /// <summary>
        /// 解析 VkFormat 字符串枚举
        /// </summary>
        private uint ParseVkFormat(string? format)
        {
            if (string.IsNullOrEmpty(format))
                return 0;
            
            // 如果已经是数字，直接返回
            if (uint.TryParse(format, out uint numericValue))
                return numericValue;
            
            // 常见格式映射（只列出常用的，根据需要扩展）
            return format switch
            {
                "VK_FORMAT_R32G32B32_SFLOAT" => 106,      // 3-component float
                "VK_FORMAT_R32G32_SFLOAT" => 103,          // 2-component float
                "VK_FORMAT_R32_SFLOAT" => 100,             // 1-component float
                "VK_FORMAT_R32G32B32A32_SFLOAT" => 109,    // 4-component float
                "VK_FORMAT_R8G8B8A8_UNORM" => 37,          // 4-component byte normalized
                "VK_FORMAT_R16G16_SFLOAT" => 83,           // 2-component half float
                "VK_FORMAT_R16G16B16A16_SFLOAT" => 97,     // 4-component half float
                _ => 0  // 未知格式返回 0
            };
        }
        
        /// <summary>
        /// 解析 VkIndexType 字符串枚举
        /// </summary>
        private uint ParseIndexType(string? indexType)
        {
            if (string.IsNullOrEmpty(indexType))
                return 0;
            
            return indexType switch
            {
                "VK_INDEX_TYPE_UINT16" => 0,
                "VK_INDEX_TYPE_UINT32" => 1,
                "VK_INDEX_TYPE_UINT8_EXT" => 1000265000,
                "VK_INDEX_TYPE_UINT8_KHR" => 1000265000,
                "VK_INDEX_TYPE_NONE_KHR" => 1000165000,
                _ => uint.TryParse(indexType, out uint val) ? val : 0
            };
        }
        
        /// <summary>
        /// 处理 vkCreateFramebuffer - 建立 Framebuffer → Attachments 映射
        /// </summary>
        private void ProcessCreateFramebuffer(VulkanSnapshotApi api)
        {
            if (string.IsNullOrEmpty(api.parameters))
                return;
            
            try
            {
                JObject json = JObject.Parse(api.parameters);
                
                // 解析 pCreateInfo 结构
                JObject? createInfo = json.SelectToken("pCreateInfo") as JObject;
                if (createInfo == null)
                    return;
                
                // 解析 pAttachments 数组（ImageView resourceIDs）
                JArray? attachmentsArray = createInfo.SelectToken("pAttachments") as JArray;
                if (attachmentsArray == null)
                    return;
                
                // 解析 pFramebuffer 返回值（输出参数）
                JToken? framebufferToken = json.SelectToken("pFramebuffer");
                if (framebufferToken == null)
                    return;
                
                ulong framebufferId = framebufferToken.Value<ulong>();
                
                // 构建 Attachment 列表
                List<ulong> attachments = new List<ulong>();
                foreach (JToken attachmentToken in attachmentsArray)
                {
                    ulong imageViewId = attachmentToken.Value<ulong>();
                    attachments.Add(imageViewId);
                }
                
                // 保存映射关系
                _framebufferAttachments[framebufferId] = attachments;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Failed to parse vkCreateFramebuffer (API={api.apiID}): {ex.Message}");
            }
        }
        
        /// <summary>
        /// 处理 vkCmdBeginRenderPass - 提取 Framebuffer 和 Attachment 信息
        /// </summary>
        private RenderPassContext? ProcessBeginRenderPass(VulkanSnapshotApi api, uint captureId)
        {
            if (string.IsNullOrEmpty(api.parameters))
                return null;
            
            try
            {
                JObject json = JObject.Parse(api.parameters);
                
                // 解析 pRenderPassBegin 结构
                JObject? renderPassBegin = json.SelectToken("pRenderPassBegin") as JObject;
                if (renderPassBegin == null)
                    return null;
                
                JToken? renderPassToken = renderPassBegin.SelectToken("renderPass");
                JToken? framebufferToken = renderPassBegin.SelectToken("framebuffer");
                
                if (renderPassToken == null || framebufferToken == null)
                    return null;
                
                ulong renderPassId = renderPassToken.Value<ulong>();
                ulong framebufferId = framebufferToken.Value<ulong>();
                
                // 创建 RenderPass 上下文
                RenderPassContext context = new RenderPassContext
                {
                    RenderPassId = renderPassId,
                    FramebufferId = framebufferId,
                    BeginApiId = api.apiID
                };
                
                // 从 Framebuffer 映射表中获取 Attachments
                if (_framebufferAttachments.TryGetValue(framebufferId, out var attachments))
                {
                    // 填充 ColorAttachments (假设最后一个是 Depth/Stencil)
                    for (int i = 0; i < attachments.Count; i++)
                    {
                        // 注意：这里简化处理，假设最后一个 attachment 是 Depth/Stencil
                        // 实际应该根据 RenderPass 定义来判断
                        if (i < attachments.Count - 1)
                        {
                            context.ColorAttachments[(uint)i] = attachments[i];
                        }
                        else
                        {
                            // 最后一个可能是 Depth/Stencil，需要根据 format 判断
                            // 这里简化处理，假设是 Color Attachment
                            context.ColorAttachments[(uint)i] = attachments[i];
                        }
                    }
                }
                
                return context;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Failed to parse vkCmdBeginRenderPass (API={api.apiID}): {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Phase 3: 构建反向索引（Resource → DrawCalls）
        /// </summary>
        private void BuildReverseIndex(uint captureId)
        {
            Console.WriteLine("\nPhase 3: Building reverse index (Resource → DrawCalls)...");
            
            if (!_drawCallsPerResource.ContainsKey(captureId))
            {
                _drawCallsPerResource[captureId] = new Dictionary<ResourceKey, List<uint>>();
            }
            
            var reverseIndex = _drawCallsPerResource[captureId];
            
            // 遍历所有 DrawCall，构建反向索引
            if (_drawCallInfos.TryGetValue(captureId, out var drawCalls))
            {
                foreach (var kvp in drawCalls)
                {
                    uint drawCallId = kvp.Key;
                    VulkanDrawCallInfo drawCallInfo = kvp.Value;
                    
                    foreach (var descSet in drawCallInfo.BoundDescriptorSets.Values)
                    {
                        foreach (var binding in descSet.Bindings.Values)
                        {
                            // 索引 Image 资源
                            if (binding.imageViewID != 0)
                            {
                                var key = new ResourceKey(ResourceType.Image, binding.imageViewID);
                                if (!reverseIndex.TryGetValue(key, out var drawCallList))
                                {
                                    drawCallList = new List<uint>();
                                    reverseIndex[key] = drawCallList;
                                }
                                if (!drawCallList.Contains(drawCallId))
                                {
                                    drawCallList.Add(drawCallId);
                                }
                            }
                            
                            // 索引 Buffer 资源
                            if (binding.bufferID != 0)
                            {
                                var key = new ResourceKey(ResourceType.Buffer, binding.bufferID);
                                if (!reverseIndex.TryGetValue(key, out var drawCallList))
                                {
                                    drawCallList = new List<uint>();
                                    reverseIndex[key] = drawCallList;
                                }
                                if (!drawCallList.Contains(drawCallId))
                                {
                                    drawCallList.Add(drawCallId);
                                }
                            }
                            
                            // 索引 Sampler 资源
                            if (binding.samplerID != 0)
                            {
                                var key = new ResourceKey(ResourceType.Sampler, binding.samplerID);
                                if (!reverseIndex.TryGetValue(key, out var drawCallList))
                                {
                                    drawCallList = new List<uint>();
                                    reverseIndex[key] = drawCallList;
                                }
                                if (!drawCallList.Contains(drawCallId))
                                {
                                    drawCallList.Add(drawCallId);
                                }
                            }
                        }
                    }
                }
            }
            
            Console.WriteLine($"  ✓ Indexed {reverseIndex.Count} unique resources");
        }
        
        /// <summary>
        /// 获取指定 DrawCall 的绑定信息
        /// </summary>
        public VulkanDrawCallInfo? GetDrawCallInfo(uint captureId, uint drawCallId)
        {
            if (_drawCallInfos.TryGetValue(captureId, out var captureData))
            {
                if (captureData.TryGetValue(drawCallId, out var info))
                {
                    return info;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 获取使用指定资源的所有 DrawCall
        /// </summary>
        public List<uint> GetDrawCallsByResource(uint captureId, ResourceType resourceType, ulong resourceId)
        {
            if (_drawCallsPerResource.TryGetValue(captureId, out var reverseIndex))
            {
                var key = new ResourceKey(resourceType, resourceId);
                if (reverseIndex.TryGetValue(key, out var drawCallList))
                {
                    return new List<uint>(drawCallList);
                }
            }
            return new List<uint>();
        }
        
        /// <summary>
        /// 获取所有 DrawCall ID
        /// </summary>
        public List<uint> GetAllDrawCalls(uint captureId)
        {
            if (_drawCallInfos.TryGetValue(captureId, out var captureData))
            {
                return captureData.Keys.ToList();
            }
            return new List<uint>();
        }
        
        /// <summary>
        /// 获取所有 DescriptorSet（用于调试）
        /// </summary>
        public Dictionary<ulong, VulkanDescriptorSet> GetAllDescriptorSets()
        {
            return _allDescriptorSets;
        }
        
        /// <summary>
        /// 手动添加 DrawCall 信息（用于测试或从数据库重建）
        /// </summary>
        public void AddDrawCallInfo(uint captureId, uint drawCallId, VulkanDrawCallInfo info)
        {
            if (!_drawCallInfos.ContainsKey(captureId))
            {
                _drawCallInfos[captureId] = new Dictionary<uint, VulkanDrawCallInfo>();
            }
            
            _drawCallInfos[captureId][drawCallId] = info;
        }
        
        /// <summary>
        /// 导出 DrawCall 绑定到 CSV 文件
        /// </summary>
        public string? ExportDrawCallBindingsToCSV(uint captureId, string outputPath)
        {
            try
            {
                Console.WriteLine($"\n=== Exporting DrawCall Bindings (Snapshot Model) ===");
                
                if (!_drawCallInfos.ContainsKey(captureId))
                {
                    Console.WriteLine($"  ⚠ No DrawCall data for captureId={captureId}");
                    return null;
                }
                
                var drawCalls = _drawCallInfos[captureId];
                Console.WriteLine($"  Found {drawCalls.Count} DrawCalls");
                
                int totalBindings = 0;
                
                using (var writer = new StreamWriter(outputPath))
                {
                    // CSV Header - 添加 DrawCallApiID 列
                    writer.WriteLine("DrawCallApiID,CaptureID,PipelineID,DescriptorSetID,SlotNum,ImageViewID,BufferID,SamplerID,ImageLayout,TexBufferView,Offset,Range,AccelStructID,TensorID,TensorViewID");
                    
                    // 遍历每个 DrawCall
                    foreach (var dcKvp in drawCalls)
                    {
                        uint drawCallApiId = dcKvp.Key;
                        VulkanDrawCallInfo dcInfo = dcKvp.Value;
                        
                        // 遍历该 DrawCall 绑定的所有 DescriptorSet（key = pipeline layout slot index）
                        foreach (var descSetKvp in dcInfo.BoundDescriptorSets)
                        {
                            VulkanDescriptorSet descSet = descSetKvp.Value;
                            ulong descSetId = descSet.DescriptorSetId;  // 实际的 DescriptorSet 资源 ID
                            
                            // 遍历 DescriptorSet 中的每个绑定
                            foreach (var bindingKvp in descSet.Bindings)
                            {
                                var binding = bindingKvp.Value;
                                
                                writer.WriteLine($"{drawCallApiId},{captureId},{dcInfo.BoundPipeline}," +
                                    $"{descSetId},{binding.slotNum}," +
                                    $"{binding.imageViewID},{binding.bufferID},{binding.samplerID}," +
                                    $"{binding.imageLayout},{binding.texBufferview}," +
                                    $"{binding.offset},{binding.range}," +
                                    $"{binding.accelStructID},{binding.tensorID},{binding.tensorViewID}");
                                totalBindings++;
                            }
                        }
                    }
                }
                
                Console.WriteLine($"  ✓ Exported {totalBindings} bindings from {drawCalls.Count} DrawCalls");
                Console.WriteLine($"  Path: {outputPath}");
                
                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Export failed: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 判断 Vulkan format 是否为 Depth/Stencil 格式
        /// Vulkan Depth/Stencil formats: 124-130
        /// - 124: VK_FORMAT_D16_UNORM
        /// - 125: VK_FORMAT_X8_D24_UNORM_PACK32
        /// - 126: VK_FORMAT_D32_SFLOAT
        /// - 127: VK_FORMAT_S8_UINT
        /// - 128: VK_FORMAT_D16_UNORM_S8_UINT
        /// - 129: VK_FORMAT_D24_UNORM_S8_UINT
        /// - 130: VK_FORMAT_D32_SFLOAT_S8_UINT
        /// </summary>
        private bool IsDepthStencilFormat(uint format)
        {
            return format >= 124 && format <= 130;
        }
        
        /// <summary>
        /// 从数据库加载所有 ImageView 的 format
        /// Key: ImageView resourceID, Value: format
        /// </summary>
        private Dictionary<ulong, uint> LoadImageViewFormats(string databasePath, uint captureId)
        {
            var formats = new Dictionary<ulong, uint>();
            
            try
            {
                if (!File.Exists(databasePath))
                {
                    Console.WriteLine($"  ⚠ Database not found: {databasePath}");
                    return formats;
                }
                
                // 重试机制：数据库可能被 C++ 端锁定
                int maxRetries = 5;
                TimeSpan retryDelay = TimeSpan.FromMilliseconds(1000);
                
                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    try
                    {
                        // 使用只读模式，不使用 WAL（WAL 模式即使只读也需要写权限）
                        using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;Read Only=True;Pooling=False;"))
                        {
                            connection.Open();
                            
                            string query = "SELECT resourceID, format FROM VulkanSnapshotImageViews WHERE captureID = @captureId";
                            using (var command = new SQLiteCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@captureId", captureId);
                                
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        ulong resourceId = Convert.ToUInt64(reader["resourceID"]);
                                        uint format = Convert.ToUInt32(reader["format"]);
                                        formats[resourceId] = format;
                                    }
                                }
                            }
                        }
                        
                        // 成功，跳出重试循环
                        break;
                    }
                    catch (SQLiteException sqlEx) when ((sqlEx.Message.Contains("locked") || sqlEx.Message.Contains("readonly")) && attempt < maxRetries)
                    {
                        Console.WriteLine($"  ⚠ Database access error (attempt {attempt}/{maxRetries}): {sqlEx.Message}");
                        Console.WriteLine($"  Retrying in {retryDelay.TotalMilliseconds}ms...");
                        System.Threading.Thread.Sleep(retryDelay);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠ Failed to load ImageView formats: {ex.Message}");
            }
            
            return formats;
        }
        
        /// <summary>
        /// 导出 DrawCall 的 RenderTarget Attachments 到 CSV 文件
        /// 记录每个 DrawCall 写入的 RenderTarget resourceIDs
        /// </summary>
        public string? ExportRenderTargetsToCSV(uint captureId, string outputPath, string databasePath)
        {
            try
            {
                Console.WriteLine($"\n=== Exporting DrawCall RenderTargets ===");
                
                if (!_drawCallInfos.ContainsKey(captureId))
                {
                    Console.WriteLine($"  ⚠ No DrawCall data for captureId={captureId}");
                    return null;
                }
                
                var drawCalls = _drawCallInfos[captureId];
                Console.WriteLine($"  Found {drawCalls.Count} DrawCalls");
                
                // 查询所有 ImageView 的 format（用于判断 Depth/Stencil）
                var imageViewFormats = LoadImageViewFormats(databasePath, captureId);
                Console.WriteLine($"  Loaded {imageViewFormats.Count} ImageView formats");
                
                int drawCallsWithRT = 0;
                int totalAttachments = 0;
                int colorAttachments = 0;
                int depthStencilAttachments = 0;
                
                using (var writer = new StreamWriter(outputPath))
                {
                    // CSV Header
                    writer.WriteLine("DrawCallApiID,CaptureID,RenderPassID,FramebufferID,AttachmentIndex,AttachmentResourceID,AttachmentType");
                    
                    // 遍历每个 DrawCall
                    foreach (var dcKvp in drawCalls.OrderBy(x => x.Key))
                    {
                        uint drawCallApiId = dcKvp.Key;
                        VulkanDrawCallInfo dcInfo = dcKvp.Value;
                        
                        // 检查是否有 RenderPass（写入 RenderTarget）
                        if (dcInfo.CurrentRenderPass == null)
                            continue;
                        
                        drawCallsWithRT++;
                        var renderPass = dcInfo.CurrentRenderPass;
                        
                        // 导出所有 Attachments，根据 format 判断类型
                        foreach (var attachment in renderPass.ColorAttachments.OrderBy(x => x.Key))
                        {
                            ulong imageViewId = attachment.Value;
                            string attachmentType = "Color";
                            
                            // 查询 ImageView format 来判断类型
                            if (imageViewFormats.TryGetValue(imageViewId, out uint format))
                            {
                                if (IsDepthStencilFormat(format))
                                {
                                    attachmentType = "DepthStencil";
                                    depthStencilAttachments++;
                                }
                                else
                                {
                                    colorAttachments++;
                                }
                            }
                            else
                            {
                                // 如果查询不到 format，默认为 Color
                                colorAttachments++;
                            }
                            
                            writer.WriteLine($"{drawCallApiId},{captureId}," +
                                $"{renderPass.RenderPassId}," +
                                $"{renderPass.FramebufferId}," +
                                $"{attachment.Key}," +
                                $"{imageViewId}," +
                                $"{attachmentType}");
                            totalAttachments++;
                        }
                    }
                }
                
                Console.WriteLine($"  ✓ Exported {totalAttachments} attachments from {drawCallsWithRT} DrawCalls (with RenderTargets)");
                Console.WriteLine($"    - Color attachments: {colorAttachments}");
                Console.WriteLine($"    - DepthStencil attachments: {depthStencilAttachments}");
                Console.WriteLine($"  Path: {outputPath}");
                
                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Export failed: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 导出 DrawCall Parameters 到 CSV 文件
        /// 记录每个 DrawCall 的参数（indexCount, vertexCount 等）
        /// </summary>
        public string? ExportDrawCallParametersToCSV(uint captureId, string outputPath)
        {
            try
            {
                Console.WriteLine($"\n=== Exporting DrawCall Parameters ===");
                
                if (!_drawCallInfos.ContainsKey(captureId))
                {
                    Console.WriteLine($"  ⚠ No DrawCall data for captureId={captureId}");
                    return null;
                }
                
                var drawCalls = _drawCallInfos[captureId];
                Console.WriteLine($"  Found {drawCalls.Count} DrawCalls");
                
                int exportedCount = 0;
                
                using (var writer = new StreamWriter(outputPath))
                {
                    // CSV Header
                    writer.WriteLine("DrawCallApiID,CaptureID,ApiName,SubmitIdx,CmdBufferIdx,DrawcallIdx,VertexCount,IndexCount,InstanceCount,FirstVertex,FirstIndex,VertexOffset,FirstInstance,DrawCount,GroupCountX,GroupCountY,GroupCountZ");
                    
                    // 遍历每个 DrawCall
                    foreach (var dcKvp in drawCalls.OrderBy(x => x.Key))
                    {
                        uint drawCallApiId = dcKvp.Key;
                        VulkanDrawCallInfo dcInfo = dcKvp.Value;
                        
                        if (dcInfo.Parameters == null)
                            continue;
                        
                        var p = dcInfo.Parameters;
                        
                        writer.WriteLine($"{drawCallApiId},{captureId}," +
                            $"{p.ApiName}," +
                            $"{dcInfo.SubmitIdx}," +
                            $"{dcInfo.CmdBufferIdx}," +
                            $"{dcInfo.DrawcallIdxInCb}," +
                            $"{p.VertexCount}," +
                            $"{p.IndexCount}," +
                            $"{p.InstanceCount}," +
                            $"{p.FirstVertex}," +
                            $"{p.FirstIndex}," +
                            $"{p.VertexOffset}," +
                            $"{p.FirstInstance}," +
                            $"{p.DrawCount}," +
                            $"{p.GroupCountX}," +
                            $"{p.GroupCountY}," +
                            $"{p.GroupCountZ}");
                        
                        exportedCount++;
                    }
                }
                
                Console.WriteLine($"  ✓ Exported {exportedCount} DrawCall parameters");
                Console.WriteLine($"  Path: {outputPath}");
                
                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Export failed: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 导出 DrawCall 与 Vertex Buffers 的绑定关系到 CSV 文件
        /// </summary>
        public string? ExportDrawCallVertexBuffersToCSV(uint captureId, string outputPath)
        {
            try
            {
                Console.WriteLine($"\n=== Exporting DrawCall Vertex Buffers ===");
                
                if (!_drawCallInfos.ContainsKey(captureId))
                {
                    Console.WriteLine($"  ⚠ No DrawCall data for captureId={captureId}");
                    return null;
                }
                
                var drawCalls = _drawCallInfos[captureId];
                Console.WriteLine($"  Found {drawCalls.Count} DrawCalls");
                
                int exportedCount = 0;
                
                using (var writer = new StreamWriter(outputPath))
                {
                    // CSV Header
                    writer.WriteLine("DrawCallApiID,CaptureID,Binding,BufferID");
                    
                    // 遍历每个 DrawCall
                    foreach (var dcKvp in drawCalls.OrderBy(x => x.Key))
                    {
                        uint drawCallApiId = dcKvp.Key;
                        VulkanDrawCallInfo dcInfo = dcKvp.Value;
                        
                        // 导出 Vertex Buffer 绑定
                        foreach (var vb in dcInfo.BoundVertexBuffers.OrderBy(x => x.Key))
                        {
                            writer.WriteLine($"{drawCallApiId},{captureId},{vb.Key},{vb.Value}");
                            exportedCount++;
                        }
                    }
                }
                
                Console.WriteLine($"  ✓ Exported {exportedCount} Vertex Buffer bindings");
                Console.WriteLine($"  Path: {outputPath}");
                
                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Export failed: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 导出 DrawCall 与 Index Buffer 的绑定关系到 CSV 文件
        /// </summary>
        public string? ExportDrawCallIndexBuffersToCSV(uint captureId, string outputPath)
        {
            try
            {
                Console.WriteLine($"\n=== Exporting DrawCall Index Buffers ===");
                
                if (!_drawCallInfos.ContainsKey(captureId))
                {
                    Console.WriteLine($"  ⚠ No DrawCall data for captureId={captureId}");
                    return null;
                }
                
                var drawCalls = _drawCallInfos[captureId];
                Console.WriteLine($"  Found {drawCalls.Count} DrawCalls");
                
                int exportedCount = 0;
                
                using (var writer = new StreamWriter(outputPath))
                {
                    // CSV Header
                    writer.WriteLine("DrawCallApiID,CaptureID,BufferID,Offset,IndexType");
                    
                    // 遍历每个 DrawCall
                    foreach (var dcKvp in drawCalls.OrderBy(x => x.Key))
                    {
                        uint drawCallApiId = dcKvp.Key;
                        VulkanDrawCallInfo dcInfo = dcKvp.Value;
                        
                        // 导出 Index Buffer 绑定
                        if (dcInfo.BoundIndexBuffer.HasValue)
                        {
                            string indexTypeStr = dcInfo.IndexType.HasValue 
                                ? dcInfo.IndexType.Value switch
                                {
                                    0 => "UINT16",
                                    1 => "UINT32",
                                    1000265000 => "UINT8",
                                    1000165000 => "NONE",
                                    _ => $"UNKNOWN({dcInfo.IndexType.Value})"
                                }
                                : "NULL";
                            
                            writer.WriteLine($"{drawCallApiId},{captureId}," +
                                $"{dcInfo.BoundIndexBuffer.Value}," +
                                $"{dcInfo.IndexBufferOffset ?? 0}," +
                                $"{indexTypeStr}");
                            exportedCount++;
                        }
                    }
                }
                
                Console.WriteLine($"  ✓ Exported {exportedCount} Index Buffer bindings");
                Console.WriteLine($"  Path: {outputPath}");
                
                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Export failed: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 导出 Pipeline Vertex Input State 到 CSV 文件
        /// 包含 Vertex Bindings 和 Attributes 信息
        /// </summary>
        public string? ExportPipelineVertexInputStateToCSV(uint captureId, string outputPathBindings, string outputPathAttributes)
        {
            try
            {
                Console.WriteLine($"\n=== Exporting Pipeline Vertex Input State (Capture {captureId}) ===");
                
                if (_pipelineVertexInputStates.Count == 0)
                {
                    Console.WriteLine("  ⚠ No Vertex Input State data found");
                    return null;
                }
                
                Console.WriteLine($"  Found {_pipelineVertexInputStates.Count} Pipelines with Vertex Input State");
                
                int bindingsCount = 0;
                int attributesCount = 0;
                
                // 导出 Vertex Bindings
                using (var writer = new StreamWriter(outputPathBindings))
                {
                    // CSV Header
                    writer.WriteLine("PipelineID,CaptureID,Binding,Stride,InputRate");
                    
                    foreach (var pipelineKvp in _pipelineVertexInputStates.OrderBy(x => x.Key))
                    {
                        ulong pipelineId = pipelineKvp.Key;
                        VulkanPipelineVertexInputState state = pipelineKvp.Value;
                        
                        foreach (var binding in state.Bindings.OrderBy(b => b.Binding))
                        {
                            writer.WriteLine($"{pipelineId},{captureId}," +
                                $"{binding.Binding}," +
                                $"{binding.Stride}," +
                                $"{binding.InputRateString}");
                            bindingsCount++;
                        }
                    }
                }
                
                // 导出 Vertex Attributes
                using (var writer = new StreamWriter(outputPathAttributes))
                {
                    // CSV Header
                    writer.WriteLine("PipelineID,CaptureID,Location,Binding,Format,Offset");
                    
                    foreach (var pipelineKvp in _pipelineVertexInputStates.OrderBy(x => x.Key))
                    {
                        ulong pipelineId = pipelineKvp.Key;
                        VulkanPipelineVertexInputState state = pipelineKvp.Value;
                        
                        foreach (var attr in state.Attributes.OrderBy(a => a.Location))
                        {
                            writer.WriteLine($"{pipelineId},{captureId}," +
                                $"{attr.Location}," +
                                $"{attr.Binding}," +
                                $"{attr.Format}," +
                                $"{attr.Offset}");
                            attributesCount++;
                        }
                    }
                }
                
                Console.WriteLine($"  ✓ Exported {bindingsCount} Vertex Bindings");
                Console.WriteLine($"  ✓ Exported {attributesCount} Vertex Attributes");
                Console.WriteLine($"  Bindings Path: {outputPathBindings}");
                Console.WriteLine($"  Attributes Path: {outputPathAttributes}");
                
                return outputPathBindings;  // 返回其中一个路径
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ Export failed: {ex.Message}");
                return null;
            }
        }
    }
    
    /// <summary>
    /// DrawCall 绑定信息
    /// </summary>
    public class VulkanDrawCallInfo
    {
        public uint DrawCallId { get; set; }
        public ulong BoundPipeline { get; set; }

        /// <summary>
        /// GUI encoded ID components: submit.cmdBuffer.drawcall (matches VkAPITreeModelBuilder numbering)
        /// </summary>
        public uint SubmitIdx      { get; set; }
        public uint CmdBufferIdx   { get; set; }
        public uint DrawcallIdxInCb { get; set; }
        
        /// <summary>
        /// 绑定的 DescriptorSets（Key: DescriptorSet ID）
        /// </summary>
        public Dictionary<ulong, VulkanDescriptorSet> BoundDescriptorSets { get; set; }
        
        /// <summary>
        /// 当前 RenderPass 上下文（写入的 RenderTargets）
        /// </summary>
        public RenderPassContext? CurrentRenderPass { get; set; }
        
        /// <summary>
        /// DrawCall 参数（从 API parameters 解析）
        /// </summary>
        public DrawCallParameters? Parameters { get; set; }
        
        /// <summary>
        /// 绑定的 Vertex Buffers（Key: binding index, Value: buffer resourceID）
        /// </summary>
        public Dictionary<uint, ulong> BoundVertexBuffers { get; set; }
        
        /// <summary>
        /// 绑定的 Index Buffer
        /// </summary>
        public ulong? BoundIndexBuffer { get; set; }
        public ulong? IndexBufferOffset { get; set; }
        public uint? IndexType { get; set; }  // 0=UINT16, 1=UINT32, 1000265000=UINT8_EXT
        
        public VulkanDrawCallInfo()
        {
            BoundDescriptorSets = new Dictionary<ulong, VulkanDescriptorSet>();
            BoundVertexBuffers = new Dictionary<uint, ulong>();
        }
        
        /// <summary>
        /// 获取所有绑定的资源
        /// </summary>
        public List<ResourceBinding> GetBoundResources()
        {
            var resources = new List<ResourceBinding>();
            
            foreach (var descSet in BoundDescriptorSets.Values)
            {
                foreach (var binding in descSet.Bindings.Values)
                {
                    if (binding.imageViewID != 0)
                    {
                        resources.Add(new ResourceBinding
                        {
                            Type = ResourceType.Image,
                            ResourceId = binding.imageViewID,
                            BindingPoint = binding.slotNum,
                            DescriptorSetId = binding.descriptorSetID
                        });
                    }
                    
                    if (binding.bufferID != 0)
                    {
                        resources.Add(new ResourceBinding
                        {
                            Type = ResourceType.Buffer,
                            ResourceId = binding.bufferID,
                            BindingPoint = binding.slotNum,
                            DescriptorSetId = binding.descriptorSetID
                        });
                    }
                    
                    if (binding.samplerID != 0)
                    {
                        resources.Add(new ResourceBinding
                        {
                            Type = ResourceType.Sampler,
                            ResourceId = binding.samplerID,
                            BindingPoint = binding.slotNum,
                            DescriptorSetId = binding.descriptorSetID
                        });
                    }
                }
            }
            
            return resources;
        }
        
        /// <summary>
        /// 获取指定类型的资源
        /// </summary>
        public List<ResourceBinding> GetResourcesByType(ResourceType resourceType)
        {
            return GetBoundResources().Where(r => r.Type == resourceType).ToList();
        }
    }
    
    /// <summary>
    /// DescriptorSet 数据
    /// </summary>
    public class VulkanDescriptorSet
    {
        public ulong DescriptorSetId { get; set; }
        
        /// <summary>
        /// Bindings（Key: slotNum）
        /// </summary>
        public Dictionary<ulong, DescriptorBinding> Bindings { get; set; }
        
        public VulkanDescriptorSet(ulong descriptorSetId)
        {
            DescriptorSetId = descriptorSetId;
            Bindings = new Dictionary<ulong, DescriptorBinding>();
        }
    }
    
    /// <summary>
    /// DescriptorSet Binding 结构体
    /// 对应 GUI 的 DescSetBindings.DescBindings
    /// 必须与 C++ 插件的结构体布局完全一致
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DescriptorBinding
    {
        public uint captureID;
        public ulong descriptorSetID;
        public uint apiID;              // ⚠️ Snapshot 模式下为 0xFFFFFFFF
        public uint slotNum;
        public ulong samplerID;
        public ulong imageViewID;       // ⚠️ 关键：Image 资源 ID
        public uint imageLayout;
        public ulong texBufferview;
        public ulong bufferID;          // Buffer 资源 ID
        public ulong offset;
        public ulong range;
        public ulong accelStructID;     // Acceleration Structure (raytracing)
        public ulong tensorID;          // Tensor (AI/ML)
        public ulong tensorViewID;
        
        public override string ToString()
        {
            var parts = new List<string>();
            parts.Add($"DescSet={descriptorSetID}");
            parts.Add($"Slot={slotNum}");
            if (imageViewID != 0) parts.Add($"Image={imageViewID}");
            if (bufferID != 0) parts.Add($"Buffer={bufferID}");
            if (samplerID != 0) parts.Add($"Sampler={samplerID}");
            return string.Join(", ", parts);
        }
    }
    
    /// <summary>
    /// 资源绑定信息（简化版）
    /// </summary>
    public class ResourceBinding
    {
        public ResourceType Type { get; set; }
        public ulong ResourceId { get; set; }
        public uint BindingPoint { get; set; }
        public ulong DescriptorSetId { get; set; }
        
        public override string ToString()
        {
            return $"{Type} ID={ResourceId} @ Binding={BindingPoint}";
        }
    }
    
    /// <summary>
    /// 资源类型枚举
    /// </summary>
    public enum ResourceType
    {
        Unknown = 0,
        Image = 1,          // ImageView / Texture
        Buffer = 2,         // Uniform Buffer, Storage Buffer
        Sampler = 3,
        Pipeline = 4,
        Shader = 5
    }
    
    /// <summary>
    /// 资源索引键（用于反向索引）
    /// </summary>
    public struct ResourceKey : IEquatable<ResourceKey>
    {
        public ResourceType Type { get; }
        public ulong ResourceId { get; }
        
        public ResourceKey(ResourceType type, ulong resourceId)
        {
            Type = type;
            ResourceId = resourceId;
        }
        
        public bool Equals(ResourceKey other)
        {
            return Type == other.Type && ResourceId == other.ResourceId;
        }
        
        public override bool Equals(object? obj)
        {
            return obj is ResourceKey other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Type.GetHashCode();
                hash = hash * 31 + ResourceId.GetHashCode();
                return hash;
            }
        }
        
        public override string ToString()
        {
            return $"{Type}_{ResourceId}";
        }
    }
    
    /// <summary>
    /// Vulkan Snapshot API 调用记录
    /// 对应 GUI 的 QGLPlugin.VulkanSnapshotApi
    /// 必须与 C++ 插件的结构体布局完全一致
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VulkanSnapshotApi
    {
        public uint captureID;
        
        public uint apiID;              // API 调用 ID（DrawCall 的唯一标识）
        
        public ulong threadID;
        
        public ulong timestamp;
        
        [MarshalAs(UnmanagedType.LPStr)]
        public string name;             // API 函数名（如 "vkCmdDraw", "vkCmdBindDescriptorSets"）
        
        [MarshalAs(UnmanagedType.LPStr)]
        public string parameters;       // JSON 格式的参数（如 {"pipeline": 12345}）
        
        [MarshalAs(UnmanagedType.LPStr)]
        public string encodedParams;    // 编码后的参数
    }
    
    /// <summary>
    /// RenderPass 上下文（包含 DrawCall 写入的 RenderTargets）
    /// </summary>
    public class RenderPassContext
    {
        public ulong RenderPassId { get; set; }
        public ulong FramebufferId { get; set; }
        public uint BeginApiId { get; set; }  // vkCmdBeginRenderPass 的 apiID
        
        /// <summary>
        /// Color Attachments（写入的颜色缓冲区）
        /// Key: attachment index, Value: ImageView resourceID
        /// </summary>
        public Dictionary<uint, ulong> ColorAttachments { get; set; } = new Dictionary<uint, ulong>();
        
        /// <summary>
        /// Depth/Stencil Attachment（写入的深度/模板缓冲区）
        /// </summary>
        public ulong? DepthStencilAttachment { get; set; }
        
        public override string ToString()
        {
            int attachmentCount = ColorAttachments.Count + (DepthStencilAttachment.HasValue ? 1 : 0);
            return $"RenderPass={RenderPassId}, Framebuffer={FramebufferId}, Attachments={attachmentCount}";
        }
    }
    
    /// <summary>
    /// DrawCall 参数
    /// 从 vkCmdDraw/vkCmdDrawIndexed 等 API 的 parameters 字段解析
    /// </summary>
    public class DrawCallParameters
    {
        public string ApiName { get; set; } = "";
        
        // vkCmdDraw / vkCmdDrawIndexed 通用
        public uint InstanceCount { get; set; }
        public uint FirstInstance { get; set; }
        
        // vkCmdDraw
        public uint VertexCount { get; set; }
        public uint FirstVertex { get; set; }
        
        // vkCmdDrawIndexed
        public uint IndexCount { get; set; }
        public uint FirstIndex { get; set; }
        public int VertexOffset { get; set; }
        
        // vkCmdDrawIndirect / vkCmdDrawIndexedIndirect
        public uint DrawCount { get; set; }
        
        // vkCmdDispatch
        public uint GroupCountX { get; set; }
        public uint GroupCountY { get; set; }
        public uint GroupCountZ { get; set; }
    }
    
    /// <summary>
    /// Vertex Input Binding Description
    /// 对应 VkVertexInputBindingDescription
    /// </summary>
    public class VertexInputBinding
    {
        public uint Binding { get; set; }
        public uint Stride { get; set; }
        public uint InputRate { get; set; }  // 0=VK_VERTEX_INPUT_RATE_VERTEX, 1=VK_VERTEX_INPUT_RATE_INSTANCE
        
        public string InputRateString => InputRate switch
        {
            0 => "VERTEX",
            1 => "INSTANCE",
            _ => "UNKNOWN"
        };
    }
    
    /// <summary>
    /// Vertex Input Attribute Description
    /// 对应 VkVertexInputAttributeDescription
    /// </summary>
    public class VertexInputAttribute
    {
        public uint Location { get; set; }
        public uint Binding { get; set; }
        public uint Format { get; set; }  // VkFormat 枚举值
        public uint Offset { get; set; }
    }
    
    /// <summary>
    /// Pipeline 的 Vertex Input State
    /// 从 vkCreateGraphicsPipelines 的 pVertexInputState 解析
    /// </summary>
    public class VulkanPipelineVertexInputState
    {
        public ulong PipelineId { get; set; }
        public List<VertexInputBinding> Bindings { get; set; } = new List<VertexInputBinding>();
        public List<VertexInputAttribute> Attributes { get; set; } = new List<VertexInputAttribute>();
    }
}
