# Claude Code Agents

This directory contains custom agents for Claude Code.

## Available Agents

### index-workflow (⭐ Recommended)

**Purpose**: Full workflow orchestrator - automatically chains discover→build→update operations.

**Usage**: This is the **main entry point** for indexing work. It handles the complete workflow.

```
@index-workflow locate BLZ-20 error | logs=BLZ-20 | paths=Engine/Pipeline
@index-workflow discover cloth simulation | paths=Engine/Cloth
@index-workflow refresh FB.Pipeline
```

**Modes**:
- `locate` - Find module from runtime evidence (logs/crashes)
- `discover` - Find module from source clues (paths/symbols)
- `refresh` - Update existing module index
- `build-only` - Build index only (skip discovery)
- `root-only` - Update INDEX.md only

**Examples**:
```
# Locate from error log
index-workflow locate vkCreateInstance failed | platform=android | paths=Engine/Render

# Discover new module
index-workflow discover shader compilation | paths=Engine/Shaders | logs=SHEX_ERROR

# Refresh existing module
index-workflow refresh FB.Pipeline

# Build index with known scope
index-workflow build-only FB.Asset.Cook.Texture | scope=Engine/Asset/Cook/Texture
```

**Auto-inference**: If you just provide clues without explicit mode, it infers the right workflow:
- Logs/crashes → `locate`
- Paths/features → `discover`
- Existing ModuleKey → `refresh`

---

### index-builder (Low-level)

**Purpose**: Individual indexing operations (discover, build, update-root, locate).

**Usage**: Used internally by index-workflow, or for fine-grained control.

```
@index-builder discover shader cook | paths=Engine/Shaders | logs=SHEX_ERROR
@index-builder build FB.Pipeline | scope=Engine/Pipeline
@index-builder update-root FB.Pipeline | index=docs/index/modules/FB.Pipeline.md
```

**Note**: Most users should use `index-workflow` instead - it's the orchestrator that calls index-builder operations automatically.

---

### investigator (Investigation & Planning)

**Purpose**: Read-only investigation and planning. Analyzes issues, generates findings, evolves plans. Never modifies implementation code.

**Usage**: Invoke for investigation and planning work.

```
@investigator investigate why BLZ-25 appears during builds
@investigator plan how to optimize shader compilation
@investigator refine PLAN-2026-04-10-network-sync.md with new constraints
```

**Key Features**:
- ✅ Read-only mode (never modifies code)
- ✅ Context-aware (reads docs/context/, INDEX.md)
- ✅ Finding generation (docs/context/findings/)
- ✅ Plan evolution (docs/context/plans/)
- ✅ Implementation awareness (considers past execution outcomes)
- ✅ Code index integration (uses INDEX.md for navigation)

**Workflow**:
1. Read repository context (CLAUDE.md, docs/context/, INDEX.md)
2. Investigate issue or analyze requirements
3. Generate/update findings
4. If planning: Compare options, propose approach
5. Generate/update plans
6. Stop before implementation (hands off to executor)

**Critical Rules**:
- ❌ NEVER modifies source code
- ❌ NEVER edits build/config/test files
- ✅ ONLY writes to docs/context/findings/ and docs/context/plans/
- ✅ Always reads context before investigating
- ✅ Always uses code index for navigation
- ✅ States "Implementation requires the executor agent" when done

---

### executor (Implementation & Validation)

**Purpose**: Implements approved plans, modifies code safely, validates changes with mandatory build loop. Records what was actually done.

**Usage**: Invoke to implement approved plans or apply fixes from findings.

```
@executor implement PLAN-2026-04-10-add-caching.md
@executor fix bug from FINDING-2026-04-14-blz25-warning.md
@executor apply changes from approved plan
```

**Key Features**:
- ✅ Context-aware (reads plans, findings, implementations)
- ✅ Mandatory build validation (max 3 iterations)
- ✅ Test validation (if tests available)
- ✅ Implementation record generation (docs/context/implementations/)
- ✅ Iteration tracking (stops after 3 failed attempts)
- ✅ Learns from past attempts (reads implementation records)

**Workflow**:
1. Read repository context (CLAUDE.md, docs/context/, INDEX.md)
2. Read approved plan (mandatory unless trivial fix)
3. Read related findings and implementation records
4. Implement changes (Edit/Write tools)
5. Build validation loop:
   - Build + Test
   - If fail: analyze, fix, retry (max 3 iterations)
   - If success: proceed
6. Create implementation record (docs/context/implementations/)
7. Report completion or failure

**Critical Rules**:
- ✅ CAN modify source code (unlike investigator)
- ✅ MUST validate builds (mandatory, not optional)
- ✅ MUST create implementation records
- ✅ MUST stop after 3 iterations (no infinite retries)
- ❌ CANNOT implement without approved plan (except trivial fixes)
- ❌ CANNOT skip build validation

**Build Validation Loop**:
```
Implementation → Build → Success? → Record → Done
                   ↓
                 Fail? → Iteration < 3? → Fix → Retry
                                    ↓
                             Iteration >= 3? → Record Failure → Stop
```

---

### doc-explanation (Documentation Generation)

**Purpose**: Generates durable explanation documentation grounded in source code evidence. Document-only agent that never modifies code.

**Usage**: Invoke to create or update explanation documents.

```
@doc-explanation explain how pipeline build flow works
@doc-explanation update EXPLAIN-pipeline-build-flow.md with new implementation
@doc-explanation create explanation from FINDING-2026-04-14-radiosity-mismatch-boardcaster.md
```

**Key Features**:
- ✅ Document-only (never modifies code)
- ✅ Chinese for explanations, English for code
- ✅ Context-aware (reads findings, plans, implementations)
- ✅ Code index integration (uses INDEX.md for routing)
- ✅ Explanation index binding (prevents duplicates)
- ✅ Evidence-based (every claim needs line citation)
- ✅ Self-describing documents (frontmatter + metadata)
- ✅ 5-phase workflow with user approval

**Workflow**:
1. Phase 0: Context & Scope Lock (define what to explain)
2. Phase 1: Context First (read findings, plans, implementations)
3. Phase 2: Routing Extraction (find ModuleKey, anchors)
4. Phase 3: Outline First (provide structure, wait for approval)
5. Phase 4: Final Explanation (write document with evidence)
6. Phase 5: Explanation Index Update (update INDEX.md)

**Critical Rules**:
- ❌ NEVER modifies source code
- ❌ NEVER creates duplicate explanations (checks index first)
- ✅ ONLY writes to docs/explanations/
- ✅ Always reads context before explaining
- ✅ All explanations in Chinese, all code in English
- ✅ Every claim needs line citation [path:LINE — Symbol]
- ✅ Creates self-describing documents with frontmatter

**Language Policy**:
- Explanations (how things work): Chinese (中文)
- Code (identifiers, paths): English
- No mixed-language sentences (except code identifiers)

---

**Language Policy** (all agents):
- Code/paths/identifiers: English only
- Explanations/reasoning: Chinese
- Never mix languages in same sentence

## Agent Architecture

This follows the AI-driven development workflow:

```
    index-workflow        investigator         executor
         ↓                     ↓                   ↓
   Navigate/Index        Understand/Plan      Implement/Validate
         ↓                     ↓                   ↓
    INDEX.md +           findings/ +          source code +
  module indexes         plans/            implementations/
```

**Separation of Concerns**:
- **index-workflow**: Builds navigation indexes (discover modules, map code)
- **investigator**: Analyzes problems, generates findings, creates plans (read-only)
- **executor**: Implements approved plans, modifies code, validates changes (write mode)
- **doc-explanation**: Generates durable documentation (not yet ported)

**Complete Workflow**:
```
index-workflow → investigator → executor → doc-explanation
  (navigate)    (understand/plan) (implement/validate) (document)
      ↓               ↓                 ↓                 ↓
  Find code →    Analyze issue →   Apply solution →  Create docs
  Build index    Create finding    Validate build    Chinese explanations
                 Propose plan      Record result     Evidence-based
```

## Agent Status

1. ✅ **index-builder** (501 lines) - Low-level indexing operations
2. ✅ **index-workflow** (562 lines) - Full workflow orchestrator
3. ✅ **investigator** (597 lines) - Investigation & planning (read-only)
4. ✅ **executor** (793 lines) - Implementation & validation (write mode)
5. ✅ **doc-explanation** (1,028 lines) - Documentation generation (document-only)

## Context System

Agents interact with:
- `docs/context/` - Operational knowledge (findings, plans, implementations)
- `docs/index/` - Code navigation indexes
- `docs/explanations/` - Durable project documentation
- `INDEX.md` - Root routing index

## See Also

- `.github/agents/` - Original GitHub Copilot agents (reference)
- `.github/copilot-instructions.md` - Original Copilot guidelines
