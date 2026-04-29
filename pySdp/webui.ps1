param(
    [int]   $Port       = 8000,
    [string]$BindHost   = "127.0.0.1",
    [int]   $SdpcliPort = 5000
)

Set-StrictMode -Off
$ErrorActionPreference = "Stop"
$root = $PSScriptRoot

# ── Check .venv ───────────────────────────────────────────────────────────────
$python = "$root\.venv\Scripts\python.exe"
if (-not (Test-Path $python)) {
    Write-Host " Creating .venv..."
    python -m venv "$root\.venv"
    if ($LASTEXITCODE -ne 0) { Write-Error ".venv creation failed"; exit 1 }
}

# ── Install dependencies ──────────────────────────────────────────────────────
& $python -c "import fastapi, uvicorn, aiofiles, requests, duckdb, pytz" 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host " Installing requirements..."
    & $python -m pip install -r "$root\requirements.txt"
    if ($LASTEXITCODE -ne 0) { Write-Error "pip install failed"; exit 1 }
}

# ── Check sdpcli.bat ─────────────────────────────────────────────────────────
$sdpcliBat = "$root\..\sdpcli.bat"
if (-not (Test-Path $sdpcliBat)) {
    Write-Error "sdpcli.bat not found at $sdpcliBat"; exit 1
}

# ── Start SDPCLI Server ───────────────────────────────────────────────────────
Write-Host "`n Starting SDPCLI Server on port $SdpcliPort..."
$sdpcliProc = Start-Process "cmd" -ArgumentList "/k cd /d `"$root\..\`" && sdpcli.bat server --port $SdpcliPort" -WindowStyle Normal -PassThru

# ── Wait for SDPCLI to be ready ───────────────────────────────────────────────
Write-Host " Waiting for SDPCLI Server..."
$ready = $false
for ($i = 0; $i -lt 30; $i++) {
    try {
        Invoke-WebRequest "http://localhost:$SdpcliPort/api/device" -UseBasicParsing -TimeoutSec 1 | Out-Null
        $ready = $true; break
    } catch { Start-Sleep -Seconds 1 }
}
if ($ready) { Write-Host " SDPCLI Server is up." }
else         { Write-Host " [WARN] SDPCLI did not respond after 15s - starting WebUI anyway." }

# ── Start WebUI ───────────────────────────────────────────────────────────────
Write-Host ""
Write-Host "  pySdp WebUI   >  http://${BindHost}:$Port"
Write-Host "  SDPCLI Server >  http://localhost:$SdpcliPort"
Write-Host "  Press ESC to stop."
Write-Host ""

Start-Sleep -Seconds 1
Start-Process "http://${BindHost}:$Port"

# Launch WebUI Python process in its own CMD window so logs are visible live
$pyArgs = "webui\server.py --host $BindHost --port $Port --sdpcli http://localhost:$SdpcliPort"
$proc = Start-Process "cmd" `
    -ArgumentList "/k cd /d `"$root`" && `"$python`" $pyArgs" `
    -WindowStyle Normal -PassThru

# ── ESC to exit ───────────────────────────────────────────────────────────────
while (-not $proc.HasExited) {
    if ([Console]::KeyAvailable) {
        if ([Console]::ReadKey($true).Key -eq "Escape") { break }
    }
    Start-Sleep -Milliseconds 100
}

if (-not $proc.HasExited) {
    taskkill /F /T /PID $proc.Id | Out-Null
}

# Kill SDPCLI cmd window + all its children (SDPCLI.exe etc.)
if ($sdpcliProc -and -not $sdpcliProc.HasExited) {
    taskkill /F /T /PID $sdpcliProc.Id | Out-Null
}

Write-Host "`n Stopped WebUI and SDPCLI Server."
