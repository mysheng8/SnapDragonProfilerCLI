Invoke the executor agent to implement the plan: $ARGUMENTS

The executor agent MUST:
1. Read CLAUDE.md and docs/context/INDEX.md first
2. Read the specified PLAN-*.md (or find the most relevant plan if not specified)
3. Read referenced FINDING-*.md and any related IMPL-*.md
4. Implement the plan step by step
5. After ALL changes: run `dotnet build SDPCLI` and verify 0 errors (max 3 iterations)
6. Write IMPL-YYYY-MM-DD-*.md to docs/context/implementations/
7. Update docs/context/INDEX.md with the new IMPL entry

Output: files modified/created, build result, path to implementation record.
