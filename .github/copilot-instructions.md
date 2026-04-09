# Snapdragon Repository Instructions

This repository uses a context-driven workflow for AI-assisted investigation, planning, and implementation.

These instructions apply to all AI work in this repository.

---

## 1. Main Goal

AI should help with:

- investigation
- planning
- implementation
- documentation cleanup
- context maintenance

AI must preserve repository knowledge quality and avoid producing stale or misleading documentation.

---

## 2. Repository Focus

This repository is a command-line toolkit around Snapdragon Profiler, mainly for:

- headless frame capture
- offline analysis
- data export
- Android profiling support
- report processing and tooling automation

Main code and runtime entry points are under:

- `SDPCLI/`
- `dll/`
- `docs/`
- `profiler/`

---

## 3. Source of Truth Order

When reasoning about the repository, always prefer sources in this order:

1. current code
2. current config and scripts
3. decisions
4. plans
5. implementations
6. findings
7. old chat-style notes

Historical AI notes are not the source of truth if they conflict with the current repository state.

---

## 4. Required Reading Order

Before making code changes, design suggestions, or documentation updates, AI should read context in this order when relevant:

1. `README.md`
2. `docs/context/INDEX.md` if it exists
3. repository/module index files such as `/INDEX.md` or `docs/index/INDEX.md` if they exist
4. relevant context docs
5. relevant code, config, scripts, and tests

Do not scan the whole repository blindly when an index exists.

If index files do not exist, use directory structure and local module READMEs to narrow scope first.

---

## 5. Context Roles

Use context documents according to their role:

- **findings**
  - investigation results
  - evidence
  - root cause analysis
  - observations

- **plans**
  - intended solution
  - implementation approach
  - task breakdown
  - expected changes

- **implementations**
  - actual execution record
  - code changes made
  - validation result
  - deviations from plan
  - remaining issues

- **decisions**
  - confirmed architectural rules
  - constraints
  - approved direction
  - stable repository rules

Do not treat findings or plans as ground truth when implementation or current code says otherwise.

---

## 6. Mandatory Workflow

For non-trivial work, follow this workflow:

`finding -> plan -> implementation -> validation -> documentation update`

Expected behavior:

- investigate first when facts are unclear
- produce or update a plan before large changes
- record actual implementation results after execution
- validate against current code or actual outputs
- update durable documentation only after validation

Do not jump directly from vague intent to code changes without checking context.

---

## 7. Documentation Governance

This repository contains AI-generated and AI-assisted documents.
AI must keep documentation useful, current, and aligned with the actual repository state.

### 7.1 Document Types

Typical context document types:

- findings
- plans
- implementations
- decisions

### 7.2 Documentation Rules

AI should:

- keep stable knowledge
- remove or archive stale low-value notes
- avoid duplicating the same knowledge across many files
- prefer concise summaries over long raw dumps
- promote only verified knowledge into durable docs

AI should not:

- copy temporary notes into README
- keep outdated plans as if they were current
- preserve incorrect implementation descriptions
- treat speculative notes as facts

### 7.3 README Policy

`README.md` should remain lightweight.

README should mainly contain:

- project purpose
- repository structure
- quick start
- important entry points
- links to deeper docs

README should not become a dump for investigation history, temporary plans, or verbose implementation logs.

### 7.4 Promotion Policy

Only promote content into durable docs when it is:

- still correct
- reusable
- relevant to current design or current status
- verified against current code, config, scripts, or validated outputs

If knowledge is partially outdated, correct it before promoting it.

---

## 8. Validation Rules

Before claiming something is true, AI should verify it against one or more of:

- current source code
- current config files
- current scripts
- runtime entry points
- actual command usage
- generated outputs
- build or execution results when available

If confidence is low:

- say so explicitly
- mark it as needing review
- avoid writing it as confirmed truth

Never present an old plan as the current implementation state without checking.

---

## 9. Implementation Record Rules

When making meaningful changes, AI should create or update an implementation record when this repository uses implementation logs.

Recommended location:

```text
docs/context/implementations/
```

Recommended content:

- what changed
- why it changed
- what was validated
- where actual behavior differs from the original plan
- remaining issues
- next steps

Recommended naming:

```text
IMPL-YYYY-MM-DD-topic.md
```

If the repository later adopts another naming rule, follow the repository rule.

---

## 10. Code Change Behavior

Before changing code, AI should:

- identify the target module first
- understand the relevant entry points
- avoid unrelated edits
- keep changes small and scoped
- preserve existing conventions unless there is a clear reason to improve them

For implementation work:

- prefer minimal safe changes
- explain assumptions
- avoid fake completeness
- do not claim validation that did not happen

---

## 11. Language and Writing Style

Use clear technical writing.

Preferred style:

- concise
- structured
- explicit about uncertainty
- grounded in repository evidence

Avoid:

- vague summaries
- inflated wording
- pretending assumptions are verified
- mixing temporary notes with durable documentation

When updating docs, favor practical descriptions over abstract process language.

---

## 12. Failure Modes to Avoid

Avoid these common mistakes:

- guessing code state without checking code
- reusing stale findings as current truth
- writing plans that are disconnected from actual implementation
- updating README with temporary or noisy details
- losing implementation history after changes
- changing code before locating the correct module and context

---

## 13. Practical Default Behavior

If unsure what to do:

1. read README
2. locate relevant module or context docs
3. inspect current code/config/scripts
4. summarize current state
5. identify stale or conflicting notes
6. update plan or implementation record
7. only then update durable docs

The repository should evolve toward fewer but more reliable documents.