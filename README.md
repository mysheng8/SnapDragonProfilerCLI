# Project AI Context Protocol

This repository uses a **context-driven AI workflow** to control how AI agents investigate, plan, and implement changes.

All agents MUST follow this protocol.

---

## 🧠 Core Principle

DO NOT rely on chat history.

ALWAYS use repository context:

README → Context INDEX → Code INDEX → context docs → code

Language policy violations are considered critical errors.

---

## 🚨 Mandatory Rules

Before doing anything, agents MUST:

1. Read `README.md`
2. Read `docs/context/INDEX.md` (Context Index)
3. Read `/INDEX.md` (Code Index at repository root)
4. Identify relevant context and modules
5. Only then proceed

---

## 📂 Context Structure

Persistent knowledge is stored under:

