---
type: finding
topic: DrawCallAnalysisService and DrawCallQueryService — current role and redundancy
status: investigated
related_paths:
  - SDPCLI/source/Services/Analysis/DrawCallAnalysisService.cs
  - SDPCLI/source/Services/Analysis/DrawCallQueryService.cs
  - SDPCLI/source/Data/SdpDatabase.DrawCalls.cs
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Modes/DrawCallAnalysisMode.cs
  - SDPCLI/source/Application.cs
related_tags:
  - drawcall
  - refactor
  - dead-code
  - service-layer
summary: DrawCallQueryService is mostly a thin dead wrapper — its 4 methods are all delegated to SdpDatabase. Only GetDrawCallInfo() is actually called. DrawCallAnalysisService is a minimal orchestrator that is also mostly redundant with SdpDatabase.GetDrawCallIds + direct SdpDatabase.GetDrawCallInfo. Both classes survive mainly for historical constructor injection reasons.
last_updated: 2026-04-09
---

## Investigation Results

### DrawCallQueryService

**Architecture**: 单纯的转发壳（thin shell）。类上有注释"all SQL is delegated to SdpDatabase partial classes"。

**4个公开方法**:

| 方法 | 实现 | 是否有实际调用方 |
|------|------|-----------------|
| `GetDrawCallInfo(dbPath, captureId, drawCallNumber)` | `new SdpDatabase(...).GetDrawCallInfo(drawCallNumber)` | ✅ 主流程使用 |
| `GetTexturesForDrawCall(dbPath, captureId, drawCallNumber)` | 调用 `GetDrawCallInfo` 取 TextureIDs | ❌ 无调用方 |
| `GetTexturesForPipelineId(dbPath, captureId, pipelineID)` | `new SdpDatabase(...).GetTexturesForApiId(pipelineID)` | ❌ 无调用方 |
| `GetPipelineInfoById(dbPath, captureId, pipelineID)` | `new SdpDatabase(...).GetPipelineInfoById(pipelineID)` | ❌ 无调用方 |

**实际调用**:
- `AnalysisPipeline.cs`：字段 `drawCallQueryService` 声明并接收注入，但 **整个 Pipeline 里从未调用**（body 中 0 处引用）。
- `DrawCallAnalysisMode.cs` line 192：直接本地 `new DrawCallQueryService()` 调用 `GetDrawCallInfo`（独立模式 `dcanalysis`，非主分析管道）。
- `Application.cs`：构造并注入进 AnalysisPipeline，但被忽略。

**文件尾部的注释块** `/*__REMOVED_OLD_SQL_IMPL_BELOW__...*/`：这是从旧版直接内嵌 SQL 迁移到 SdpDatabase 后遗留的墓碑注释，现在只是死代码存档，含 SQLiteConnection/SQLiteCommand 等已删除的实现。

---

### DrawCallAnalysisService

**架构**：编排者（Orchestrator）。内部持有 `DrawCallQueryService _drawCallQuery` 字段，但实际上 **完全不使用它**。

**`AnalyzeAllDrawCalls(dbPath, captureId, cmdBufferFilter)`**:
1. `new SdpDatabase(dbPath, captureId).GetDrawCallIds(cmdBufferFilter)` — 枚举所有 DrawCall ID
2. 对每个 ID 调用 `_drawCallQuery.GetDrawCallInfo(dbPath, captureId, dc)` — 获取完整资源信息
3. 报 fallback 示例数据（若 drawCallIds 为空，填入假 ID `["1","2","3","1.1","1.1.1","1.1.2"]`）
4. 统计汇总 → 返回 `DrawCallAnalysisReport`

**一处设计疑点**：
- fallback 示例数据（line 48-50）会在数据库返回 0 个 DrawCall 时填入虚假 ID，这可能导致静默错误。
- `_drawCallQuery` 字段实际上是间接调用 `SdpDatabase`，等价于直接用 `db.GetDrawCallInfo()`。

**实际调用方**:
- `AnalysisPipeline.cs` line 100（Step 1）：唯一调用点，`analysisService.AnalyzeAllDrawCalls(...)` 产出 `DrawCallAnalysisReport`。

**`DrawCallAnalysisReport`** 产出内容：
- `DrawCallResults`: `List<DrawCallInfo>` — 含 Pipeline、Textures、Shaders、RenderTargets、VertexBuffers、IndexBuffer、BindingSummary 等完整资源信息
- `Statistics`: `CaptureStatistics` — 总 DC、Pipeline、Texture、Shader 数量和均值
- `TotalDrawCalls` / `AnalyzedDrawCalls`

该报告对象是整个分析管道的核心输入：Step 1.5（Shader/Texture 提取）、Step 2（LLM 标注）、Step A3（raw.json）、Step A4/A5（Dashboard/Status JSON）、Step B1/B2（Attribution）都依赖 `report.DrawCallResults`。

---

## 冗余与问题总结

### DrawCallQueryService

| 问题 | 说明 |
|------|------|
| 3/4 方法无调用方 | `GetTexturesForDrawCall`、`GetTexturesForPipelineId`、`GetPipelineInfoById` 从未被调用 |
| AnalysisPipeline 持有字段但从不用 | `drawCallQueryService` 字段声明+注入，body 内 0 处使用 |
| 仅剩 `GetDrawCallInfo` 被调用 | 且只在 `DrawCallAnalysisService` 内部和 `DrawCallAnalysisMode`（独立模式）中使用 |
| 死代码注释块 | 文件尾 `/*__REMOVED_OLD_SQL_IMPL_BELOW__*/` 块保留了 500+ 行旧 SQL 实现的墓碑注释 |

### DrawCallAnalysisService

| 问题 | 说明 |
|------|------|
| 逻辑薄 | 仅做循环+统计，可以直接被 Pipeline 内联 |
| 依赖注入 `DrawCallQueryService` 但可直接用 `SdpDatabase` | 双重间接：Pipeline → AnalysisService → QueryService → SdpDatabase |
| Fallback 假数据 | 空 DB 时注入假 DrawCall ID，可能掩盖真实问题 |
| 唯一调用方 | 仅 `AnalysisPipeline` Step 1 使用 |

---

## 关键结论

**可以安全简化**的方向（需 Executor 执行）：

1. `DrawCallQueryService` 中的 3 个无调用方法可以删除
2. `AnalysisPipeline` 中的 `drawCallQueryService` 字段可以删除（已注入但从不使用）
3. `DrawCallQueryService.cs` 尾部的 `/*__REMOVED_OLD_SQL_IMPL_BELOW__*/` 注释块（约 500 行死代码）可以完全删除
4. `DrawCallAnalysisService` 如果要合并进 Pipeline，是可选的；但如果维持现状也不影响运行

**不可删除**：
- `DrawCallAnalysisService.AnalyzeAllDrawCalls()` — 是 Step 1 的唯一入口，`DrawCallAnalysisReport` 是整个管道输入
- `DrawCallQueryService.GetDrawCallInfo()` — `DrawCallAnalysisService` 和 `DrawCallAnalysisMode` 都在调用
