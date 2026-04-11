# snapdragon

Snapdragon Profiler 的命令行辅助工具集，用于无头模式抓帧、离线分析和数据导出。

---

## 📂 Repository Structure

```
snapdragon/
├── SDPCLI/                  # 主工具（C# CLI）— see SDPCLI/README.md
│   ├── source/              # 源代码
│   ├── config.ini           # 运行配置（PackageName、RenderingAPI 等）
│   └── android/             # Profiler 服务端 APK（arm64 / armeabi-v7a）
│
├── project/                 # 运行时数据（WorkingDirectory/project/）
│   ├── sdp/                 # snapshot 会话输出（<timestamp>/）
│   └── analysis/            # analysis 分析输出（<sdp_basename>/snapshot_N/）
│
├── dll/                     # SDPCore / QGLPlugin 原生 DLL 及 C# wrapper
├── docs/                    # 文档与分析记录
├── meminfo_poll.ps1         # 手机内存实时监控脚本
├── monitor_crash.ps1        # logcat 崩溃监控脚本
└── SDPCLI.bat               # 快速启动入口
```

---

## 🚀 Quick Start

```powershell
# build
dotnet build SDPCLI

# run (interactive mode)
.\SDPCLI.bat
```

更多用法、配置项、模式说明见：

👉 `SDPCLI/README.md`

---

## 🧩 Main Components

### SDPCLI
核心 CLI 工具，负责：

- 控制 Snapdragon Profiler
- 抓取 frame 数据（.sdp）
- 解析和导出分析结果
- 调用 plugins 进行数据处理

### dll
包含：

- `SDPCore.dll`
- `QGLPlugin.dll`
- 对应 C# wrapper

用于与 Snapdragon Profiler SDK 交互。


### scripts

- `meminfo_poll.ps1` → 实时监控设备内存
- `monitor_crash.ps1` → logcat 崩溃检测

---

## 📚 Documentation

- `SDPCLI/README.md`  
  → CLI 使用说明与配置

- `docs/`  
  → 分析文档、实验记录、AI 辅助输出

- `docs/context/`  
  → 结构化的分析发现、计划、决策等

- `docs/index/`
  → 代码索引与模块概览

（注：AI workflow 和文档治理规则见 `.copilot-instructions.md`）

---

## 🧭 Notes

- 本仓库同时用于：
  - 工具开发
  - profiling 分析
  - 自动化实验
- 文档可能包含 AI 辅助生成内容，请以代码和实际结果为准，谨慎参考分析结论。