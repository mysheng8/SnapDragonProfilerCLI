@echo off
REM ── Snapdragon Profiler CLI helper ──────────────────────────────────────────
REM Usage examples:
REM   SDPCLI.bat                                         interactive mode
REM   SDPCLI.bat analysis capture.sdp                   analyze all snapshots
REM   SDPCLI.bat analysis capture.sdp -s 3              analyze snapshot 3
REM   SDPCLI.bat analysis capture.sdp -s 3 -t label     re-run labeling only
REM   SDPCLI.bat analysis capture.sdp -s 3 -t dashboard re-run dashboard only
REM   SDPCLI.bat analysis capture.sdp -s 3 --no-extract re-gen JSONs, skip extraction
REM   SDPCLI.bat snapshot com.pkg/.Activity              ENTER=capture / ESC=exit+.sdp
REM ────────────────────────────────────────────────────────────────────────────
cd SDPCLI\bin\Debug\net472
.\SDPCLI.exe %*
