# Explanation Index

This file provides a structured index of durable explanation documents.

Agents MUST:
- read this file before creating a new explanation doc
- use it to detect existing coverage
- prefer updating matching docs over creating duplicates

---

## How to Use This Index

When searching for explanation docs:

1. Match by `topic`
2. Match by `module_key`
3. Match by `based_on`
4. Match by tags
5. Prefer updating an existing matching explanation doc

---

## Explanations

### EXPLAIN-three-modes.md
- topic: SDPCLI 三种模式概览与 SDK 架构
- module_key: SDPCLI
- status: mixed
- based_on:
  - IMPL-2026-04-14-http-server-mode.md
  - IMPL-2026-04-08-drawanalysis-two-pass-refactor.md
  - PLAN-2026-04-11-http-server-mode.md
  - PLAN-2026-04-08-drawanalysis-two-pass-refactor.md
- tags:
  - overview
  - sdk-architecture
  - dll-dependency
  - api-reference
  - data-structure
- summary: 三模式概览索引 + SDK Plugin-Host 架构 + DLL 依赖链 + 核心 API 参考 + DescSetBindings + sdp.db 表清单

### EXPLAIN-snapshot.md
- topic: Snapshot 模式完整实现
- module_key: SDPCLI.Snapshot
- status: stable
- based_on:
  - IMPL-2026-04-14-http-server-mode.md
  - FINDING-2026-04-07-shader-texture-export-structure.md
- tags:
  - snapshot
  - capture
  - sdk-callback
  - manualresetevent
  - csv-import
  - multi-frame
- summary: 完整调用链（8 阶段）、SDK VulkanSnapshot* 表写入时序、DescriptorSet 二进制处理、多帧共存设计、关键超时约束

### EXPLAIN-analysis.md
- topic: Analysis 模式完整实现
- module_key: SDPCLI.Analysis
- status: stable
- based_on:
  - IMPL-2026-04-08-drawanalysis-two-pass-refactor.md
  - IMPL-2026-04-09-rawjson-service-rename.md
  - IMPL-2026-04-09-metrics-dynamic-attribution.md
  - PLAN-2026-04-08-drawanalysis-two-pass-refactor.md
  - FINDING-2026-04-08-parallelism-thread-safety.md
- tags:
  - analysis
  - pipeline
  - analysis-target
  - drawcall
  - data-model
  - llm
  - attribution
  - report
- summary: AnalysisPipeline Pass A/B 全流程、AnalysisTarget 10 标志位与依赖级联、DrawCallInfo/DrawCallMetrics 数据模型、dc.json/status.json/topdc.json 结构、三层归因引擎

### EXPLAIN-server.md
- topic: Server 模式完整实现
- module_key: SDPCLI.Server
- status: wip
- based_on:
  - IMPL-2026-04-14-http-server-mode.md
  - PLAN-2026-04-11-http-server-mode.md
- tags:
  - server
  - http
  - job-system
  - state-machine
  - async
  - rest-api
- summary: HTTP 路由表（11 端点）、Job 数据结构与生命周期、设备状态机（6 状态）、4 个 JobRunner 各阶段、ManualResetEvent 异步桥接、安全设计

---

## Maintenance Rules

When adding or updating explanation docs:
- add a new entry when a new explanation doc is created
- update the existing entry when an explanation doc is refreshed
- keep topic and module_key accurate
- keep summary concise
- do not duplicate equivalent docs under different filenames

---

## Notes for Agents

- `docs/context/` = internal state and evolution history
- `docs/explanations/` = durable project-facing knowledge
- explanation docs must be self-describing
- explanation docs should remain aligned with:
  - context
  - code index
  - source code
