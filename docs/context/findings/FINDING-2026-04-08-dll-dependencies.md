---
type: finding
topic: SDPCLI DLL dependencies inventory
status: investigated
related_paths:
  - SDPCLI/SDPCLI.csproj
  - dll/
  - SDPCLI/plugins/
  - SDPCLI/bin/Debug/net472/
related_tags: [dll, dependencies, build]
summary: Complete inventory of all DLLs the CLI depends on — by source category.
last_updated: 2026-04-08
---

## 1. NuGet 包（csproj `<PackageReference>`）

| 包 | 版本 | 作用 |
|---|---|---|
| `System.Data.SQLite.Core` | 1.0.118 | SQLite 托管驱动（附带 `SQLite.Interop.dll` native 层） |
| `System.IO.Compression` | 4.3.0 | ZIP 压缩流 |
| `System.IO.Compression.ZipFile` | 4.3.0 | ZIP 文件读写 |

NuGet 还会自动输出以下 polyfill DLL（net472 补丁）：
- `System.Runtime.CompilerServices.Unsafe.dll`
- `System.Threading.Tasks.Extensions.dll`
- `System.ValueTuple.dll`

SQLite native 层：
- `x64/SQLite.Interop.dll`（NuGet 自动输出，必须随 exe 一起部署）

---

## 2. 本地托管引用（csproj `<Reference HintPath>`）

| DLL | 来源路径 | 作用 |
|---|---|---|
| `SDPClientFramework.dll` | `dll/SDPClientFramework.dll` | Snapdragon Profiler 客户端框架 |
| `SDPCoreWrapper.dll` | `dll/SDPCoreWrapper.dll` | SDP Core .NET 包装层 |
| `QGLPlugin.dll` | `plugins/QGLPlugin.dll` | OpenGL 快照插件（直接引用） |
| `Newtonsoft.Json.dll` | `plugins/Newtonsoft.Json.dll` | JSON 序列化 |

---

## 3. 构建时复制到 bin（csproj `<None CopyToOutputDirectory>`）

以下 DLL 不作为编译引用，只是"随包部署"：

### 3a. dll/ 根目录 全量复制（排除已作为 Reference 的 3 个）

| DLL | 类型 | 说明 |
|---|---|---|
| `SDPCore.dll` | native | SDP Core 本体（单独列出，显式复制） |
| `AtkSharp.dll` | 托管 | GTK# ATK 绑定 |
| `CairoSharp.dll` | 托管 | Cairo 图形绑定 |
| `CoreUtils.dll` | 托管 | SDP 内部工具库 |
| `d3dcompiler_47.dll` | native | DirectX shader 编译器 |
| `dxcompiler.dll` | native | DXC DXIL 编译器 |
| `dxil.dll` | native | DXIL 验证 |
| `FastExpressionCompiler.dll` | 托管 | 表达式树快速编译 |
| `FluentValidation.dll` | 托管 | 验证框架 |
| `GdkSharp.dll` | 托管 | GTK# GDK 绑定 |
| `GioSharp.dll` | 托管 | GTK# GIO 绑定 |
| `GLibSharp.dll` | 托管 | GTK# GLib 绑定 |
| `GtkSharp.dll` | 托管 | GTK# 主库 |
| `GtkSourceSharp.dll` | 托管 | GTK# 代码视图控件 |
| `libcurl.dll` | native | HTTP/设备通信 |
| `libDCAP.dll` | native | DCAP 数据采集 |
| `MetricsAPI.dll` | 托管/native | 性能指标 API |
| `PangoSharp.dll` | 托管 | GTK# 文字排版 |
| `RulesEngine.dll` | 托管 | 规则引擎 |
| `SDPDocking.dll` | 托管 | SDP 停靠窗口 UI |
| `System.Data.SQLite.dll` | 托管 | SQLite 托管层（冗余副本，NuGet 已包含） |
| `System.Linq.Dynamic.Core.dll` | 托管 | 动态 LINQ |
| `TextureConverter.dll` | native/托管 | 纹理格式转换 |
| `Viewer3D.dll` | 托管 | 3D 视图控件 |

### 3b. 构建时复制 GTK# / native 补丁架构文件

无（未在 csproj 显式列出 x86/x64 中间件；仅 NuGet 的 `x64/SQLite.Interop.dll` 会自动复制）

---

## 4. plugins/ 子目录（构建后目标复制）

`DeployPlugins` MSBuild target 负责将 `SDPCLI/plugins/**` → `bin/.../plugins/`：

| DLL | 说明 |
|---|---|
| `CPUPlugin.dll` | CPU 性能追踪插件 |
| `CPUWindowsPlugin.dll` | Windows CPU 插件 |
| `DCAPToolsWrapper.dll` | DCAP 工具包装插件 |
| `DSPPlugin.dll` | DSP 追踪插件 |
| `DX11Plugin.dll` | DX11 快照插件 |
| `DX12Plugin.dll` | DX12 快照插件 |
| `GlobalGPUTracePlugin.dll` | GPU 全局 Trace 插件 |
| `KernelAnalyzer.dll` | 内核分析插件 |
| `Logcat.dll` | Android Logcat 插件 |
| `Newtonsoft.Json.dll` | （plugins 目录副本） |
| `OpenCLPlugin.dll` | OpenCL 追踪插件 |
| `OpenGLPlugin.dll` | OpenGL 追踪插件 |
| `QGLPlugin.dll` | OpenGL 快照插件（plugins 目录副本） |
| `SystracePlugin.dll` | Android Systrace 插件 |
| `Top.dll` | `top` 进程监控插件 |
| `TraceImportExport.dll` | Trace 导入/导出插件 |
| `plugins/processor/` | （处理器插件子目录，内容未单独列出） |

---

## 5. 代码内 P/Invoke（`[DllImport]`）

| DLL | 文件 | 函数用途 |
|---|---|---|
| `kernel32.dll` | `source/Main.cs` | 控制台代码页 / UTF-8 输出设置 |
| `kernel32.dll` | `source/Utility.cs` | `LoadLibraryEx` / `FreeLibrary`（动态加载 native 插件） |

`Utility.cs` 还通过 `Process.GetCurrentProcess().Modules` 枚举已加载 DLL，并扫描 `plugins/*.dll` 做插件发现。

---

## 6. 汇总 — 分类依赖树

```
SDPCLI.exe
├── [编译引用 - 必须]
│   ├── SDPClientFramework.dll       ← dll/
│   ├── SDPCoreWrapper.dll           ← dll/
│   ├── QGLPlugin.dll                ← plugins/
│   ├── Newtonsoft.Json.dll          ← plugins/
│   └── System.Data.SQLite.dll       ← NuGet
│       └── x64/SQLite.Interop.dll   ← NuGet native
│
├── [运行时必须 - bin 根目录]
│   ├── SDPCore.dll                  ← native 核心
│   ├── libcurl.dll                  ← 设备通信
│   ├── libDCAP.dll                  ← 数据采集
│   ├── d3dcompiler_47.dll           ← DirectX shader
│   ├── dxcompiler.dll / dxil.dll    ← DXIL
│   ├── TextureConverter.dll         ← 纹理
│   ├── CoreUtils.dll / MetricsAPI.dll
│   ├── SDPDocking.dll / Viewer3D.dll
│   ├── RulesEngine.dll / FluentValidation.dll
│   ├── FastExpressionCompiler.dll / System.Linq.Dynamic.Core.dll
│   └── GTK# 组 (Atk/Cairo/Gdk/Gio/GLib/Gtk/GtkSource/Pango Sharp)
│
└── [运行时插件 - plugins/]
    ├── CPUPlugin / CPUWindowsPlugin / DSPPlugin
    ├── DX11Plugin / DX12Plugin / GlobalGPUTracePlugin
    ├── OpenCLPlugin / OpenGLPlugin / QGLPlugin
    ├── DCAPToolsWrapper / KernelAnalyzer
    ├── Logcat / SystracePlugin / Top
    └── TraceImportExport
```

---

## 7. 潜在问题 / 整理建议

1. **`Newtonsoft.Json.dll` 三份副本**：`dll/`、`plugins/` 各一份，编译引用 `plugins/` 下的。建议统一来源，或改用 NuGet 包管理。
2. **`System.Data.SQLite.dll` 冗余**：`dll/` 里有一份，NuGet 也会输出一份，两者版本可能不一致。
3. **GTK# 一组**（AtkSharp/CairoSharp/GdkSharp/GioSharp/GLibSharp/GtkSharp/GtkSourceSharp/PangoSharp）：CLI 工具不显示 GUI，这 8 个 DLL 仅因 `SDPClientFramework` 间接依赖而被拖入，可评估是否能换为无 GTK 版本的框架。
4. **`SDPDocking.dll` / `Viewer3D.dll`**：同上，属于 GUI 组件，CLI 若不渲染 UI 可能不需要。
5. **plugins/ 二进制未纳入版本管理**：`processor/` 子目录内容未列出，需确认。
