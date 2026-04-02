# ProcessorPlugin API使用说明

## 正确的架构

### 不要静态引用QGLPlugin.dll
- ❌ 错误：`<Reference Include="QGLPlugin">`
- ✅ 正确：通过ProcessorPluginMgr动态访问

### 为什么不静态引用
1. **GUI使用动态加载**：PluginManager通过反射扫描plugins/目录
2. **插件隔离**：plugins应该是独立的，通过接口通信
3. **避免类型耦合**：不应该直接依赖plugin内部类型

## 正确的实现方式

### 1. 只依赖SDPCoreWrapper
```xml
<ItemGroup>
  <Reference Include="SDPCoreWrapper">
    <HintPath>..\dll\SDPCoreWrapper.dll</HintPath>
  </Reference>
</ItemGroup>
```

### 2. 通过ProcessorPlugin API获取数据
```csharp
// 获取plugin (由ProcessorPluginMgr动态加载)
var plugin = ProcessorPluginMgr.Get().GetPlugin("SDP::QGLPluginProcessor");

// 获取binary buffer
BinaryDataPair dsbBuffer = plugin.GetLocalBuffer(
    SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA,  // bufferCategory
    3,  // bufferID (SnapshotDsbBuffer)
    captureID
);
```

### 3. 自定义结构体（不引用QGLPlugin类型）
```csharp
// 自己定义，匹配QGLPlugin的binary format
[StructLayout(LayoutKind.Sequential)]
public struct DescBindings
{
    public uint captureID;
    public ulong descriptorSetID;
    public uint apiID;
    public uint slotNum;
    public ulong samplerID;
    public ulong imageViewID;
    public uint imageLayout;
    public ulong texBufferview;
    public ulong bufferID;
    public ulong offset;
    public ulong range;
    public ulong accelStructID;
    public ulong tensorID;
    public ulong tensorViewID;
}
```

### 4. 解析binary data
```csharp
var binding = Marshal.PtrToStructure<DescBindings>(ptr);
```

## GUI vs SDPCLI 对比

| 方面 | GUI | SDPCLI |
|------|-----|--------|
| 依赖 | SDPClientFramework + PluginManager | SDPCoreWrapper only |
| Plugin加载 | PluginManager动态扫描plugins/ | ProcessorPluginMgr已加载 |
| QGLPlugin访问 | PluginManager反射加载 | 通过ProcessorPlugin API |
| 类型定义 | 直接用QGLPlugin.DescSetBindings | 自定义struct（匹配format）|
| Binary获取 | QGLPlugin.SnapshotDsbBuffer | plugin.GetLocalBuffer() |

## 关键API

### ProcessorPluginMgr (SDPCoreWrapper)
```csharp
ProcessorPluginMgr.Get()                    // 获取singleton
  .GetPlugin("SDP::QGLPluginProcessor")     // 获取plugin
  .GetLocalBuffer(bufferCategory, bufferID, captureID)  // 获取buffer
```

### SDPCore常量 (SDPCoreWrapper)
```csharp
SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA  // buffer category
```

### BinaryDataPair (SDPCoreWrapper)
```csharp
public class BinaryDataPair
{
    public uint size { get; }      // buffer大小
    public IntPtr data { get; }    // binary data指针
}
```

## 编译测试

```cmd
compile_qglplugin.bat
```

## 优势

✅ 不依赖GUI框架（SDPClientFramework）  
✅ 不静态引用plugin DLLs  
✅ 通过公共API访问数据  
✅ 架构清晰，职责分离  
✅ 与GUI的动态加载机制一致  
