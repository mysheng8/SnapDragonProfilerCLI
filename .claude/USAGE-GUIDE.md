# Claude Code Usage Guide for Frostbite Codebase

This guide shows how to effectively use Claude Code with the custom agents in this repository.

## 🎯 Quick Start

### Using Custom Agents (Currently Not Auto-registered)

Custom agents in `.claude/agents/` are **reference implementations** that demonstrate the workflow. Currently, Claude Code doesn't auto-register these as subagent types.

**To use the agent workflows**, you have two options:

#### Option 1: Manual Workflow Execution (Current Approach)
Follow the agent's workflow manually by invoking the appropriate tools:

```
I need to follow the index-workflow to discover a new module.
Target: shader compilation
Paths: Engine/Shaders
Logs: SHEX_ERROR

Follow the discover→build→update workflow from .claude/agents/index-workflow.md
```

#### Option 2: Request Agent Behavior (Future)
When Claude Code supports custom agents, you'll be able to invoke directly:

```
@index-workflow discover shader compilation | paths=Engine/Shaders | logs=SHEX_ERROR
```

---

## 📚 Repository Structure

This codebase uses a layered knowledge system:

```
INDEX.md                    # Root module router (ALWAYS check first)
docs/
  ├── index/               # Code navigation layer
  │   └── modules/        # Per-module indexes (FB.*.md)
  ├── context/            # Operational knowledge
  │   ├── findings/      # Investigation results
  │   ├── plans/         # Implementation plans
  │   └── implementations/ # What was actually done
  └── explanations/       # Durable documentation

.claude/
  └── agents/            # Custom agent definitions
      ├── index-builder.md     # Low-level indexing
      └── index-workflow.md    # Workflow orchestrator

.github/
  └── agents/            # Original Copilot agents (reference)
```

---

## 🔍 Common Workflows

### 1. Finding Code from Error Logs

**Scenario**: You see error `BLZ-20: Failed to build` in logs.

**Step-by-step**:
```
1. Check INDEX.md first
   → Look in "Topic Router" section
   → Search for "BLZ-" pattern

2. If found in INDEX.md:
   → Follow module link
   → Check "Log → Code Map" table
   → Jump to exact location

3. If not in INDEX.md:
   → Request: "Follow index-workflow locate mode for BLZ-20"
   → Grep for "BLZ-20" in likely paths
   → Identify module and update index
```

**Example prompt**:
```
I see error "BLZ-20: Failed to build" in pipeline logs.

First check INDEX.md to see if this error is already mapped.
If not found, follow the index-workflow locate workflow:
1. Grep for "BLZ-20" in Engine/Pipeline
2. Read the file to understand context
3. Determine the module (likely FB.Pipeline)
4. Update the module index with this error code mapping
```

---

### 2. Exploring Unfamiliar Code

**Scenario**: Need to understand shader parameter system.

**Step-by-step**:
```
1. Check INDEX.md
   → Search for "shader" keywords
   → Find relevant modules (FB.Shaders.Base, FB.Shaders.Pipeline, etc.)

2. Read module index
   → Open docs/index/modules/FB.Shaders.Base.md
   → Check "Entry Points" and "Key Classes"
   → Use "Search Hints" section

3. Jump to code
   → Use file:line references from index
   → Read entry points first, then explore

4. If index doesn't exist:
   → Request: "Follow index-workflow discover for shader parameters"
```

**Example prompt**:
```
I need to understand the shader parameter system.

1. First, read INDEX.md and search for "shader parameter"
2. Open the relevant module index (FB.Shaders.Base)
3. Show me the entry points and key classes
4. Then read the main entry file to understand the architecture
```

---

### 3. Adding New Module to Index

**Scenario**: Working in Engine/Cloth and there's no module index yet.

**Step-by-step**:
```
1. Verify module not indexed
   → Check INDEX.md for cloth-related entries
   → Check docs/index/modules/ for existing index

2. Discover module
   → Grep for key classes in Engine/Cloth
   → Identify entry points and boundaries
   → Generate ModuleKey (e.g., FB.Cloth.Simulation)

3. Build module index
   → Extract entry points, key classes, methods
   → Map any log patterns to code
   → Create docs/index/modules/FB.Cloth.Simulation.md

4. Update root index
   → Add row to INDEX.md Module Router table
   → Add topic router entries if relevant
```

**Example prompt**:
```
I'm working in Engine/Cloth but don't see it in INDEX.md.

Follow the index-workflow discover mode:
1. Check if any cloth-related module exists in INDEX.md
2. If not, grep for "cloth" and key classes in Engine/Cloth
3. Read 3-5 key files to understand the module boundary
4. Generate a ModuleKey like FB.Cloth.Simulation
5. Build a new module index following the template
6. Add a row to INDEX.md
```

---

### 4. Refreshing Stale Module Index

**Scenario**: FB.Pipeline module index is a few months old.

**Step-by-step**:
```
1. Read existing index
   → Open docs/index/modules/FB.Pipeline.md
   → Note the SourceScope and entry points

2. Verify entry points still valid
   → Grep for entry symbols in SourceScope
   → Read files to check if structure changed

3. Scan for new additions
   → Grep for new error codes (BLZ-*)
   → Look for new key classes
   → Check for new log patterns

4. Update index
   → Add new entries to relevant sections
   → Update timestamps
   → Keep existing valid content
```

**Example prompt**:
```
Refresh the FB.Pipeline module index:

1. Read docs/index/modules/FB.Pipeline.md
2. Grep for the entry points to verify they still exist
3. Scan Engine/Pipeline for any new BLZ-* error codes
4. Check for new key classes added since last update
5. Update the index with findings
6. Don't rewrite everything - only add what's new
```

---

## 🛠️ Tool Usage Patterns

### When to Use Each Tool

**Grep** - Content search:
```
- Finding specific symbols: Grep pattern="class AssetManager" path=Engine/Pipeline
- Finding log messages: Grep pattern="BLZ-" path=Engine/Pipeline
- Finding error handlers: Grep pattern="throw.*Exception" path=Engine
```

**Glob** - File finding:
```
- Find all DDF files: Glob pattern="**/*.ddf"
- Find specific module files: Glob pattern="Engine/Shaders/**/*.h"
- Explore directory structure: Glob pattern="Engine/*/Module.cpp"
```

**Read** - Reading files:
```
- Read entry points: Read file_path=Engine/Pipeline/Module.cpp
- Read module indexes: Read file_path=docs/index/modules/FB.Pipeline.md
- Read with offset for large files: Read file_path=... offset=100 limit=50
```

**Edit** - Updating existing:
```
- Update module index: Edit old_string="..." new_string="..."
- Add error code mapping: Edit old_string="## Log → Code Map" new_string="..."
```

**Write** - Creating new:
```
- Create new module index: Write file_path=docs/index/modules/FB.NewModule.md
- Only after confirming it doesn't exist!
```

---

## 📋 Best Practices

### Always Check INDEX.md First

Before exploring code:
```
✅ DO: Read INDEX.md → Find module → Read module index → Jump to code
❌ DON'T: Grep randomly across entire repository
```

### Use Module Indexes as Entry Points

Module indexes provide:
- Entry points with exact file:line locations
- Call flow skeletons showing how components interact
- Log → code mappings for debugging
- Search hints for common queries

### Follow Agent Workflows

When doing index work:
```
✅ DO: Follow index-workflow.md or index-builder.md workflows
✅ DO: Use compact command syntax for clarity
✅ DO: Stop scanning once boundary is clear
❌ DON'T: Overscan the entire repository
❌ DON'T: Create duplicate module indexes
```

### Maintain Index Quality

When updating indexes:
```
✅ DO: Add file:line references for every symbol
✅ DO: Keep indexes concise (150-250 lines for modules)
✅ DO: Update timestamps and metadata
❌ DON'T: Add speculation or tutorials
❌ DON'T: Duplicate information already in code
```

---

## 🎨 Language Policy

**Critical**: This codebase uses a strict bilingual policy:

```
English ONLY:
- Code identifiers, file paths, commands
- Function/class names
- SQL, logs, error messages

Chinese ONLY:
- Explanations, reasoning, analysis
- Discussion of concepts

NEVER:
- Mix languages in same sentence
- Translate code into Chinese
- Use Japanese or Korean
```

---

## 🔄 Workflow Comparison: Copilot vs Claude Code

| Task | GitHub Copilot | Claude Code |
|------|----------------|-------------|
| Invoke agent | `@agent-name command` | Manual workflow or future `@agent-name` |
| Index building | Automatic via agent | Follow `.claude/agents/` workflow |
| Context awareness | Via `.github/copilot-instructions.md` | Via `CLAUDE.md` + `INDEX.md` |
| Module discovery | Agent executes | Use Grep/Read following agent template |

---

## 🚀 Quick Reference Commands

### Index Workflow (Manual Execution)

```bash
# Discover new module
"Follow index-workflow discover mode for <feature> in <path>"

# Locate from error
"Follow index-workflow locate mode for error <log> in <paths>"

# Refresh existing
"Follow index-workflow refresh mode for <ModuleKey>"

# Build index only
"Build module index for <ModuleKey> with scope <path>"

# Update root only
"Update INDEX.md for <ModuleKey> using module index at <path>"
```

### Direct Tool Usage

```bash
# Search for symbol
Grep pattern="<symbol>" path="<directory>" output_mode="content"

# Find files
Glob pattern="<pattern>"

# Read file with context
Read file_path="<path>" offset=<line> limit=<count>

# Check existing index
Read file_path="INDEX.md"
Read file_path="docs/index/modules/<ModuleKey>.md"
```

---

## 📖 Related Documentation

- **CLAUDE.md** - Repository overview and architecture
- **INDEX.md** - Root module router (ALWAYS START HERE)
- **docs/index/modules/** - Per-module navigation indexes
- **.claude/agents/** - Agent workflow definitions
- **.github/agents/** - Original Copilot agents (reference)
- **.github/copilot-instructions.md** - Original Copilot guidelines (reference)

---

## 💡 Tips & Tricks

1. **Start with INDEX.md** - It's the authoritative router
2. **Use module indexes** - They save massive amounts of exploration time
3. **Follow agent workflows** - They're battle-tested patterns
4. **Keep indexes current** - Update them as you work
5. **Respect language policy** - Code in English, explanations in Chinese
6. **Don't overscan** - Stop once you have what you need
7. **Verify symbols** - Always include file:line in indexes

---

## 🆘 Common Issues

**Issue**: Can't find where a log message comes from
**Solution**: Check INDEX.md "Topic Router" and module "Log → Code Map" tables

**Issue**: Module index doesn't exist for area I'm working in
**Solution**: Follow index-workflow discover mode to create it

**Issue**: Agent not found when trying to invoke
**Solution**: Custom agents not yet auto-registered; follow workflow manually

**Issue**: Index is outdated
**Solution**: Follow index-workflow refresh mode

**Issue**: Don't know which module handles X
**Solution**: Search INDEX.md for keywords, or follow locate/discover workflow
