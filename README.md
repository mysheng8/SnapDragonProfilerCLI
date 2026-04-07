# snapdragon

Snapdragon Profiler 的命令行辅助工具集，用于无头模式抓帧、离线分析和数据导出。

---

## 文件结构

```
snapdragon/
├── SDPCLI/                  # 主工具（C# CLI）— 详见 SDPCLI/README.md
│   ├── source/              # 源代码
│   ├── config.ini           # 运行配置（PackageName、RenderingAPI 等）
│   ├── test/                # 抓帧输出目录（.sdp + 解析结果）
│   ├── android/             # Profiler 服务端 APK（arm64 / armeabi-v7a）
│   └── plugins/             # QGLPlugin 等处理器插件
│
├── dll/                     # SDPCore / QGLPlugin 原生 DLL 及 C# wrapper 项目
├── docs/                    # 背景分析文档与 AI agent 上下文
├── profiler/                # 原始 CSV 报告
├── meminfo_poll.ps1         # 手机内存实时监控脚本
├── monitor_crash.ps1        # logcat 崩溃监控脚本
└── SDPCLI.bat               # 快速启动入口
```

---

## 快速开始

```powershell
# 编译
dotnet build SDPCLI

# 运行（交互模式）
.\SDPCLI.bat
```

详细用法、配置项、模式说明见 [SDPCLI/README.md](SDPCLI/README.md).

---

# Project AI Context Protocol

This repository uses a **context-driven AI workflow** to control how AI agents investigate, plan, and implement changes.

All agents MUST follow this protocol.

---

## 🧠 Core Principle

DO NOT rely on chat history.

ALWAYS use repository context:

README → Context INDEX → Code INDEX → context docs → code

Language policy violations are considered critical errors.

---

## 🚨 Mandatory Rules

Before doing anything, agents MUST:

1. Read `README.md`
2. Read `docs/context/INDEX.md` (Context Index)
3. Read `docs/index/INDEX.md` (Code Index at repository root)
4. Identify relevant context and modules
5. Only then proceed

---

## 📂 Context Structure

Persistent knowledge is stored under:

```
docs/context/
├── INDEX.md          # Master index — read this first
├── findings/         # Investigation results
├── plans/            # Approved implementation plans
└── decisions/        # Architectural decisions
```


