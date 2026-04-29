---
type: implementation
date: 2026-04-27
title: WebUI Live Log Window + Korean Language Guard Hook
status: complete
files_changed:
  - pySdp/webui/logger.py
  - pySdp/webui.ps1
  - .claude/hooks/check-language.py  (new)
  - .claude/settings.local.json
---

# WebUI Live Log Window + Korean Language Guard Hook

## Part 1: WebUI Live Log Window

### Problem

Python WebUI 进程以 `-NoNewWindow` 方式启动，日志只写文件，没有可见的终端窗口，调试困难。

### What Was Built

**`pySdp/webui/logger.py`**
- 在 `RotatingFileHandler` 之后追加 `StreamHandler(sys.stdout)`，格式相同
- 所有 `log.info/warning/error` 同时输出到 stdout

**`pySdp/webui.ps1`**
- Python 进程改为 `Start-Process "cmd" /k ...`，和 SDPCLI 窗口相同方式启动
- 弹出独立 CMD 窗口，实时显示日志流
- 停止时改用 `taskkill /F /T /PID` 确保子进程一并终止

---

## Part 2: Korean Language Guard Stop Hook

### Problem

模型偶发输出韩文，CLAUDE.md 中的语言规则不足以完全阻止。

### What Was Built

**`.claude/hooks/check-language.py`** (新文件)
- Stop hook 脚本，从 stdin 读取 `transcript_path`
- 读取 transcript JSONL，找最后一条 `assistant` 消息的文本内容
- 用正则 `[가-힣ᄀ-ᇿ㄰-㆏]` 检测韩文音节、字母、兼容字母三个 Unicode 区间
- 检测到韩文 → 输出 `{"decision": "block", "reason": "..."}` 强制模型重写
- 无韩文 → 无输出，正常放行

**`.claude/settings.local.json`**
- 注册 `Stop` hook，命令 `python .claude/hooks/check-language.py`，超时 10 秒
