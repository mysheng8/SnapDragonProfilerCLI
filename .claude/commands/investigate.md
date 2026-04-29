Invoke the investigator agent to analyze the topic: $ARGUMENTS

The investigator agent MUST:
1. Read CLAUDE.md and docs/context/INDEX.md first
2. Search existing findings/plans/implementations for related context
3. Investigate the codebase (read-only — NO code changes)
4. Produce or update a FINDING-YYYY-MM-DD-*.md in docs/context/findings/
5. Produce or update a PLAN-YYYY-MM-DD-*.md in docs/context/plans/
6. Update docs/context/INDEX.md
7. STOP — do NOT implement anything

Output the paths to created/updated context docs and a summary of the plan.
State clearly at the end: "Implementation requires the executor agent (/execute)."
