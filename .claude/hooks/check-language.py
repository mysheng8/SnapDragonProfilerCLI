"""
check-language.py — Stop hook: block if the last assistant message contains Korean.

Claude Code calls this on every Stop event, passing JSON on stdin:
  { "transcript_path": "...", "hook_event_name": "Stop", ... }

Exit 0 + {"decision": "block", "reason": "..."} → Claude must rewrite.
Exit 0 + {}                                      → allow stop.
"""

import json
import re
import sys


def has_korean(text: str) -> bool:
    # Hangul Syllables U+AC00-D7A3, Jamo U+1100-11FF, Compatibility Jamo U+3130-318F
    return bool(re.search(r"[가-힣ᄀ-ᇿ㄰-㆏]", text))


def last_assistant_text(transcript_path: str) -> str:
    """Return the text content of the last assistant message in the transcript."""
    last = None
    try:
        with open(transcript_path, encoding="utf-8") as f:
            for line in f:
                line = line.strip()
                if not line:
                    continue
                try:
                    entry = json.loads(line)
                except json.JSONDecodeError:
                    continue
                if entry.get("type") == "assistant":
                    last = entry
    except (OSError, IOError):
        return ""

    if last is None:
        return ""

    # message.content is a list of content blocks
    content = last.get("message", {}).get("content", [])
    if isinstance(content, str):
        return content

    parts = []
    for block in content:
        if isinstance(block, dict) and block.get("type") == "text":
            parts.append(block.get("text", ""))
    return "\n".join(parts)


def main():
    raw = sys.stdin.read()
    try:
        data = json.loads(raw)
    except json.JSONDecodeError:
        sys.exit(0)

    transcript_path = data.get("transcript_path", "")
    if not transcript_path:
        sys.exit(0)

    text = last_assistant_text(transcript_path)
    if has_korean(text):
        print(json.dumps({
            "decision": "block",
            "reason": (
                "Your response contains Korean characters. "
                "You must only respond in English or Chinese. "
                "Please rewrite the response without any Korean text."
            ),
        }))
    # else: output nothing → allow stop


if __name__ == "__main__":
    main()
