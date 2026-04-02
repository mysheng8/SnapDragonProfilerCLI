# DrawCall Analysis - 快速参考

## 📌 一行代码搞定

```csharp
uint[] textures = DrawCallAnalysisTool.GetTextureIDs(dbPath, "1.1.2");
```

## 🎯 核心功能

**输入**: DrawCall编号 `"1.1.2"` 或 `"10"`  
**输出**: TextureID数组 `uint[]`

## 📖 三种使用方式

### 1️⃣ 静态方法 - 最简单
```csharp
uint[] ids = DrawCallAnalysisTool.GetTextureIDs(
    @"D:\test\sdp.db", 
    "1.1.2", 
    captureID: 3
);
```

### 2️⃣ 实例方法 - 只要IDs
```csharp
var analyzer = new DrawCallAnalysis(dbPath);
uint[] ids = analyzer.GetTexturesForDrawCall("1.1.2");
```

### 3️⃣ 完整信息 - 要详情
```csharp
var analyzer = new DrawCallAnalysis(dbPath);
var info = analyzer.GetDrawCallInfo("1.1.2");

// 访问属性
Console.WriteLine($"Pipeline: {info.PipelineID}");
Console.WriteLine($"Textures: {info.TextureIDs.Length}");
foreach (var tex in info.Textures) {
    Console.WriteLine($"{tex.TextureID}: {tex.Width}x{tex.Height}");
}

// 或直接打印
info.Print();
```

## 🧪 PowerShell测试

```powershell
# 默认测试
.\test_drawcall_analysis.ps1

# 指定DrawCall
.\test_drawcall_analysis.ps1 -DrawCallNumber "1.1.10"

# 简单数字编号
.\test_drawcall_analysis.ps1 -DrawCallNumber "5"
```

## 📊 测试结果示例

```
DrawCall: 1.1.2
Pipeline ID: 816
Textures: 20 found

TextureID Array: [2314, 2266, 2318, 2342, 2347, ...]

Top Textures:
  2314: 32x64 BC1_RGB_UNORM
  2266: 4x1 BC1_RGB_UNORM
  2318: 32x64 BC1_RGB_UNORM
```

## ⚙️ 编号格式

| 输入格式 | 如何解析 | 结果 |
|---------|---------|------|
| `"1.1.2"` | 取最后数字-1 | Pipeline索引 1 |
| `"1.1.10"` | 取最后数字-1 | Pipeline索引 9 |
| `"5"` | 直接数字-1 | Pipeline索引 4 |

## 📂 文件清单

```
SDPCLI/
├── source/
│   ├── DrawCallAnalysis.cs          # 核心分析类
│   └── DrawCallAnalysisTool.cs      # 命令行接口
├── DrawCallAnalysisExample.cs       # 使用示例
├── test_drawcall_analysis.ps1       # 测试脚本
├── DRAWCALL_ANALYSIS_GUIDE.md       # 详细文档
└── DRAWCALL_ANALYSIS_QUICKREF.md    # 本文档
```

## ⚠️ 重要提示

**Snapshot模式局限性**:
- 返回的是Capture中**所有可能使用的Textures**
- 不是特定DrawCall运行时的**确切绑定**
- Pipeline顺序 ≠ 执行顺序

需要精确绑定信息，请使用**Trace模式**捕获。

## 🔗 数据库表结构

```
Pipeline → PipelineLayout → DescriptorSetBindings → ImageView → Texture
  (resourceID)  (layoutID)     (imageViewID)       (imageID)
```

## 💡 实际应用示例

### 找出使用某个TextureID的DrawCalls
```csharp
var analyzer = new DrawCallAnalysis(dbPath);
uint targetTexture = 2314;

for (int i = 1; i <= 100; i++) {
    uint[] textures = analyzer.GetTexturesForDrawCall($"{i}");
    if (textures.Contains(targetTexture)) {
        Console.WriteLine($"DrawCall #{i} uses Texture {targetTexture}");
    }
}
```

### 按Texture尺寸过滤
```csharp
var info = analyzer.GetDrawCallInfo("1.1.2");
var largeTextures = info.Textures
    .Where(t => t.Width >= 1024 && t.Height >= 1024)
    .ToList();
```

### 批量导出到CSV
```csharp
using (var writer = new StreamWriter("drawcalls.csv")) {
    writer.WriteLine("DrawCall,Pipeline,TextureCount,TextureIDs");
    
    for (int i = 1; i <= 50; i++) {
        var info = analyzer.GetDrawCallInfo($"{i}");
        if (info != null) {
            string ids = string.Join("|", info.TextureIDs);
            writer.WriteLine($"{i},{info.PipelineID},{info.TextureIDs.Length},{ids}");
        }
    }
}
```

## 📞 联系与反馈

如有问题或建议，请查看详细文档 [DRAWCALL_ANALYSIS_GUIDE.md](DRAWCALL_ANALYSIS_GUIDE.md)
