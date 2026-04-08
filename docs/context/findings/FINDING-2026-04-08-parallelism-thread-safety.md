---
type: finding
topic: parallelism thread-safety audit for analysis pipeline
status: investigated
related_paths:
  - SDPCLI/source/Analysis/AnalysisPipeline.cs
  - SDPCLI/source/Tools/ShaderExtractor.cs
  - SDPCLI/source/Tools/TextureExtractor.cs
  - SDPCLI/source/Services/Analysis/DrawCallLabelService.cs
  - SDPCLI/source/Tools/LlmApiWrapper.cs
  - SDPCLI/source/Logging/AppLogger.cs
related_tags:
  - parallel
  - async
  - thread-safety
  - shader
  - texture
  - llm
  - concurrency
summary: |
  Thread-safety audit of all components involved in parallel analysis pipeline execution.
  ShaderExtractor is safe for full CPU parallelism; TextureExtractor/Qonvert DLL is
  conservative at degree=4; DrawCallLabelService._llmCache required ConcurrentDictionary
  fix; LlmApiWrapper is stateless and safe; AppLogger uses an internal lock.
last_updated: 2026-04-08
---

# Finding: Parallelism Thread-Safety Audit — Analysis Pipeline

## 1. Motivation

The analysis pipeline's Step 1.5 (shader + texture extraction) and Step 2 (LLM labeling)
were running serially. The question was whether these could be safely parallelized.

---

## 2. Component Audit

### 2a. AppLogger (`Logging/AppLogger.cs`)

**Result: SAFE — no change required**

`AppLogger` uses an internal `_lock` (`object`) before every write operation.
Concurrent calls from multiple threads produce interleaved but individually atomic log lines.
No structural issue at any concurrency level.

---

### 2b. ShaderExtractor (`Tools/ShaderExtractor.cs`)

**Result: SAFE — full CPU parallelism permitted**

- Each `ShaderExtractor` instance is constructed with a `dbPath` string and `captureId`.
- Internally it opens a new `SqliteConnection` on every call to `ExtractShadersForPipeline`.
- No shared mutable state between instances.
- Output is written to uniquely-named files (keyed by `pipelineId` and stage name).
  Two threads writing the same pipeline to the same path would produce identical bytes
  (deterministic shader binary), so even a race on the same file path is benign.

**Recommended degree:** `Environment.ProcessorCount` (no cap needed)

---

### 2c. TextureExtractor + Qonvert P/Invoke DLL (`Tools/TextureExtractor.cs`)

**Result: LOW RISK — conservative degree required**

- `TextureExtractor` wraps a native Qonvert DLL via P/Invoke.
- The native DLL was not confirmed as fully reentrant; internal state during format
  conversion is unknown.
- Each texture writes to a unique output path (`texture_{resourceId}.png`) — no file
  conflict between concurrent calls.
- Risk is in the DLL's internal allocators/state during concurrent conversion.

**Recommended degree:** `TextureExtractionDegree=4` (conservative default)

**Configurable** via `config.ini` key `TextureExtractionDegree`.

---

### 2d. DrawCallLabelService (`Services/Analysis/DrawCallLabelService.cs`)

**Result: REQUIRED FIX — ConcurrentDictionary**

- `_llmCache` was `Dictionary<uint, DrawCallLabel>` — not thread-safe for concurrent writes.
- Multiple labeler threads could corrupt the dictionary during parallel `Label()` calls.

**Fix applied:** `Dictionary<uint, DrawCallLabel>` → `ConcurrentDictionary<uint, DrawCallLabel>`

No other shared mutable state was found in the class. `BuildPrompt()` is a pure function.
`_ruleEngine` (rule-based fallback) only reads from immutable rule tables.

---

### 2e. LlmApiWrapper (`Tools/LlmApiWrapper.cs`)

**Result: SAFE — no change required for HTTP call**

- `LlmApiWrapper.Chat(prompt)` constructs a new `HttpRequestMessage` per call.
- `HttpClient` is shared but `HttpClient` is documented as thread-safe for concurrent calls.
- No per-instance state is mutated during a `Chat()` call.

**After Phase 3** (LlmResponseCache integration): `LlmResponseCache.Put/TryGet` uses
`ReaderWriterLockSlim` internally, so concurrent access from the labeler thread pool
is safe.

---

## 3. Summary Table

| Component | Thread-safe as-is? | Change required | Max concurrency |
|-----------|-------------------|-----------------|-----------------|
| `AppLogger` | Yes | None | Unlimited |
| `ShaderExtractor` | Yes | None | `ProcessorCount` |
| `TextureExtractor` (Qonvert DLL) | Unknown | None (constrain degree) | 4 (configurable) |
| `DrawCallLabelService._llmCache` | **No** | `ConcurrentDictionary` | `LlmMaxConcurrentRequests` |
| `LlmApiWrapper.Chat` | Yes | None (HttpClient is thread-safe) | `LlmMaxConcurrentRequests` |
| `LlmResponseCache` (new) | Yes (built-in lock) | N/A (new class) | `LlmMaxConcurrentRequests` |

---

## 4. Implementation Notes

Changes were implemented in `PLAN-2026-04-07-unified-shader-texture-export-json.md`
Phase 2 and Phase 3 sections.

Key config keys added:

```ini
LlmMaxConcurrentRequests=8
TextureExtractionDegree=4
```

These can be tuned per-device based on native DLL stability and GPU driver concurrency behavior.
