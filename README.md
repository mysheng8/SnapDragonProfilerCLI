# snapdragon

Snapdragon Profiler 的命令行辅助工具集，用于无头模式抓帧、离线分析和数据导出。

---

## 📂 Repository Structure

```
snapdragon/
├── SDPCLI/                  # 主工具（C# CLI）
│   ├── source/
│   │   ├── Modes/           # InteractiveMode, AnalysisMode, SnapshotCaptureMode, ServerMode
│   │   ├── Services/        # Capture & Analysis 业务逻辑服务层
│   │   ├── Server/          # HTTP API server（ServerMode 使用）
│   │   ├── Tools/           # TextureExtractor, ShaderExtractor
│   │   ├── Logging/         # AppLogger（双通道：文件 + 控制台）
│   │   ├── Data/            # SdpDatabase, CSV/DB 导入
│   │   ├── Models/          # VulkanSnapshotModel
│   │   ├── Main.cs          # CLI 入口，参数解析
│   │   ├── Application.cs   # subcommand 路由
│   │   ├── SDPClient.cs     # SDPCore SDK 封装
│   │   └── Config.cs        # config.ini 读取
│   ├── config.ini           # 运行时配置
│   └── SDPCLI.sln
├── dll/                     # Native DLL + C# wrapper 引用
│   └── plugins/             # QGLPlugin, SDPClientFramework 等预编译 DLL
├── docs/
│   ├── context/             # AI 上下文（findings / plans / implementations）
│   └── index/               # 代码模块索引
├── project/                 # 运行时工作目录（sdp输出、analysis结果）
└── SDPCLI.bat               # 启动入口
```

---

## 🚀 Quick Start

```powershell
dotnet build SDPCLI
.\SDPCLI.bat
```

---

## 🖥️ Server Mode

```powershell
sdpcli server --port 5000
```

启动本地 HTTP REST API 服务（localhost only），支持脚本 / CI 远程控制抓帧和分析。详见 [SDPCLI/README.md](SDPCLI/README.md#server-模式http-api)。

---

# 🧠 AI Workflow & Project Rules (CRITICAL)

This repository uses a **context-driven AI system**.

All AI actions MUST follow these rules.

---

## 🔁 Core Workflow

```
Investigate → Plan → Execute → Validate → Document → Index Sync
```

No step should be skipped.

---

## 📦 Context System (Source of Evolution)

```
docs/context/
├── INDEX.md
├── findings/
├── plans/
├── implementations/
├── decisions/
```

### Priority (CRITICAL)

```
decisions > implementations > plans > findings > code
```

Code is NOT always truth.

---

## 🧭 Code Index System (Routing Layer)

```
/INDEX.md
docs/index/modules/*.md
```

Purpose:

- module routing
- scope control
- avoid blind search

---

## 📝 Documentation System (Project Knowledge)

```
docs/explanations/
```

This is the **ONLY place for durable project explanations**.

### 🚨 CRITICAL RULE

- explanations MUST NOT be written into `docs/context/`
- `docs/context/` = internal state
- `docs/explanations/` = project knowledge

---

## 🤖 Agent Responsibilities

### Investigator

- analyze
- write findings / plans
- NEVER modify code

---

### Executor

- implement approved plan
- MUST validate (build/test)
- MUST write:

```
docs/context/implementations/
```

---

### Index Sync

- maintain module index
- detect drift
- update `/INDEX.md`

Rules:

- NO rename
- NO merge
- NO split

---

### Doc Explanation (CRITICAL ROLE)

This agent is responsible for **project documentation generation**.

#### Allowed:

```
docs/explanations/
```

#### Forbidden:

- code changes
- writing into random folders

#### MUST:

1. read context (implementations first)
2. read index
3. then read code

#### MUST NOT:

- trust code blindly
- ignore WIP / outdated logic

#### MUST output:

- ModuleKey
- SourceScope
- code evidence
- context vs code analysis

---

## 🚨 Mandatory Reading Order

Before ANY work:

1. README.md
2. docs/context/INDEX.md
3. /INDEX.md
4. module docs
5. context docs
6. code

---

## 🚨 Global Rules

### DO

- use context first
- validate with code
- keep scope minimal
- update instead of duplicating

### DO NOT

- modify code without plan
- trust code blindly
- ignore index
- mix explanation into context

---

## 🔍 Validation

Before claiming correctness:

- check code
- check implementations
- check real output if possible

If unsure:

- say it clearly

---

## 🎯 Goal

Build a system where:

- knowledge is persistent
- behavior is controlled
- code understanding improves over time
- documentation stays reliable
