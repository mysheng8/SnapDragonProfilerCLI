"""Stop hook: warn if the last assistant response appears to be in Japanese."""
import json
import sys


def _detect_lang(text: str) -> str | None:
    for ch in text:
        cp = ord(ch)
        if 0x3040 <= cp <= 0x309F or 0x30A0 <= cp <= 0x30FF:
            return "日文"
        if 0xAC00 <= cp <= 0xD7A3 or 0x1100 <= cp <= 0x11FF:
            return "韩文"
    return None


def main():
    try:
        data = json.load(sys.stdin)
    except Exception:
        sys.exit(0)

    response = data.get("response", "") or ""
    lang = _detect_lang(response)
    if lang:
        out = {
            "decision": "block",
            "reason": f"检测到{lang}回复。请用中文重新回复用户，不要使用{lang}。"
        }
        print(json.dumps(out, ensure_ascii=False))

    sys.exit(0)


if __name__ == "__main__":
    main()
