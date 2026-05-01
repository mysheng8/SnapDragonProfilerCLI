# snapdragon

Snapdragon Profiler 无头工具集 — GPU 帧抓取、离线分析、Web 可视化。

---

## 组件概览

| 组件 | 语言 | 说明 |
|------|------|------|
| [SDPCLI](SDPCLI/README.md) | C# (.NET 4.7.2) | 核心 CLI：设备连接、帧抓取、分析 Pipeline、HTTP Server |
| [pySdp](pySdp/README.md) | Python (FastAPI) | WebUI + 数据层 + 分析服务 + Python 客户端库 |

```
┌─────────────────────────────────────────────┐
│  Browser  http://localhost:8000             │
│  WebUI (pySdp/webui)                        │
│  API Docs: http://localhost:8000/api/docs   │
└──────────────┬──────────────────────────────┘
               │
┌──────────────▼──────────────────────────────┐
│  pySdp WebUI Server  :8000                  │
│  ├─ /api/sdpcli/*   → proxy (前端专用)       │
│  ├─ /api/snapshot/* → 设备/快照工作流        │
│  ├─ /api/jobs/*     → 提取/分析触发           │
│  ├─ /api/data/*     → DuckDB 查询 (MCP)      │
│  ├─ /api/files/*    → 本地文件服务            │
│  └─ /mcp            → MCP endpoint          │
└──────────────┬──────────────────────────────┘
               │
┌──────────────▼──────────────────────────────┐
│  SDPCLI Server  :5000                       │
│  (.\sdpcli.bat server)                      │
│  Connect → Launch → Capture → Extract       │
└──────────────┬──────────────────────────────┘
               │  Snapdragon Profiler SDK (DLL)
┌──────────────▼──────────────────────────────┐
│  Android Device  (ADB / USB)                │
└─────────────────────────────────────────────┘
```

---

## 快速开始

```powershell
# 1. 编译 SDPCLI（首次或代码变更后）
dotnet build SDPCLI

# 2. 一键启动（自动创建 venv、安装依赖、启动 SDPCLI Server + WebUI、打开浏览器）
.\webui.ps1
```

`webui.ps1` 会自动：
- 创建 `pySdp/.venv` 并安装依赖（首次运行）
- 在独立窗口启动 `sdpcli.bat server --port 5000`
- 等待 SDPCLI 就绪后启动 WebUI（`http://127.0.0.1:8000`）
- 按 ESC 同时关闭两个进程

可选参数：
```powershell
.\webui.ps1 -Port 8080 -SdpcliPort 5001
```

**仅使用 SDPCLI（无 WebUI）：**
```powershell
.\sdpcli.bat                              # 交互模式
.\sdpcli.bat server --port 5000          # Server 模式
.\sdpcli.bat analysis capture.sdp        # 离线分析
```

---

## 典型工作流

```
Connect Device  →  Launch App  →  Capture Frame
      ↓
  .sdp 文件（ZIP 格式）
      ↓
  Analysis（C# 提取 DC / Shader / Texture / Metrics）
      ↓
  Python Pipeline（Label → Status → TopDC → Markdown 报告）
      ↓
  WebUI Explorer（DC 列表 · 指标热力图 · 3D Mesh · Shader 下载）
```

---

## 仓库结构

```
snapdragon/
├── SDPCLI/                  # C# CLI 主工具
│   ├── source/              # 源码（Modes / Services / Server / Tools / Data）
│   ├── config.ini           # 运行时配置（APK、LLM 端点、Metrics 白名单）
│   └── README.md            # ← SDPCLI 详细文档
├── pySdp/                   # Python 层
│   ├── webui/               # FastAPI 应用（后端 + 静态资源）
│   ├── analysis/            # 分析服务（label / status / topdc / dashboard）
│   ├── data/                # DuckDB 数据层（ingest / query / models）
│   ├── pysdp/               # 独立 Python 客户端包
│   └── README.md            # ← pySdp 详细文档
├── dll/                     # Native SDK DLL + C# wrapper 引用
│   └── plugins/             # QGLPlugin 等预编译插件
├── docs/
│   ├── context/             # AI 上下文（findings / plans / implementations）
│   │   └── INDEX.md         # 活跃条目索引
│   ├── index/modules/       # 代码模块路由索引
│   └── explanations/        # 持久化项目文档（架构、Pipeline 详解）
├── project/                 # 运行时工作目录（sdp 输出、analysis 结果）
├── .gitignore
├── sdpcli.bat               # SDPCLI 启动入口
└── webui.ps1                # pySdp WebUI 启动脚本
```

---

## 详细文档

| 文档 | 内容 |
|------|------|
| [SDPCLI/README.md](SDPCLI/README.md) | 编译、三模式使用、CLI 参数、HTTP API 端点、关键约束 |
| [pySdp/README.md](pySdp/README.md) | WebUI 功能、数据层 Schema、分析服务、/api 端点速查、pysdp 客户端 |
| [docs/explanations/EXPLAIN-snapshot.md](docs/explanations/EXPLAIN-snapshot.md) | Snapshot 完整调用链与 SDK 时序 |
| [docs/explanations/EXPLAIN-analysis.md](docs/explanations/EXPLAIN-analysis.md) | Analysis Pipeline、数据模型、报告结构 |
| [docs/explanations/EXPLAIN-server.md](docs/explanations/EXPLAIN-server.md) | HTTP 路由、Job 系统、状态机 |

---

## AI 工作流（开发规范）

本仓库使用结构化上下文系统驱动 AI 辅助开发，所有 AI 操作必须遵循：

```
Investigate → Plan → Execute → Validate → Document → Index Sync
```

上下文优先级：

```
decisions > implementations > plans > findings > code
```

| 路径 | 用途 |
|------|------|
| `docs/context/findings/` | 调查结论 |
| `docs/context/plans/` | 待执行方案 |
| `docs/context/implementations/` | 已执行记录 |
| `docs/context/decisions/` | 稳定决策（不可撤销） |
| `docs/explanations/` | 持久化项目文档（唯一写入位置） |
| `docs/index/modules/` | 模块路由索引 |

> 代码不是唯一真相来源；存在 WIP、部分迁移或与计划不同步的情况，以 `decisions > implementations` 为准。
