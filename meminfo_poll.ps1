# meminfo_poll.ps1
# Usage: .\meminfo_poll.ps1 -Package com.ea.gp.fcmnova -IntervalMs 2000
param(
    [string]$Package    = "com.ea.gp.fcmnova",
    [int]   $IntervalMs = 2000
)

Write-Host "=== meminfo poller ===" -ForegroundColor Cyan
Write-Host "Package : $Package" -ForegroundColor Cyan
Write-Host "Interval: ${IntervalMs}ms  |  Press Ctrl+C to stop`n" -ForegroundColor Cyan

$logFile = "meminfo_$(Get-Date -Format 'HH-mm-ss').txt"
Write-Host "Saving to: $logFile`n" -ForegroundColor Yellow

# Thresholds (MB)
$WarnPssMB   = 1500
$WarnGraphMB = 800

function Get-PidForPackage {
    $pidStr = adb shell "pidof $Package 2>/dev/null"
    if ($pidStr -eq $null) { return '' }
    return ($pidStr -join ' ').Trim()
}

$script:_cnt = 0

while ($true) {
    $ts       = Get-Date -Format "HH:mm:ss.fff"
    $procPid  = Get-PidForPackage

    if ([string]::IsNullOrWhiteSpace($procPid)) {
        $msg = "$ts  [WARN] Process not found -- package may have died or not started yet"
        Write-Host $msg -ForegroundColor Red
        $msg | Out-File $logFile -Append -Encoding UTF8
        Start-Sleep -Milliseconds $IntervalMs
        continue
    }

    # Fast read from /proc/<pid>/status
    $statusRaw  = adb shell "cat /proc/$procPid/status 2>/dev/null | grep -E 'VmRSS|VmSwap|VmPeak'"
    $oomAdjRaw  = adb shell "cat /proc/$procPid/oom_score_adj 2>/dev/null"
    $statusLine = if ($statusRaw) { ($statusRaw -join '  ') -replace '\s+', ' ' } else { '' }

    $vmRssKB = 0
    if ($statusLine -match 'VmRSS:\s+(\d+)') { $vmRssKB = [int]$Matches[1] }
    $vmRssMB = [math]::Round($vmRssKB / 1024, 1)

    $oomAdjStr = if ($oomAdjRaw) { ($oomAdjRaw -join '').Trim() } else { '0' }
    $oomAdjVal = 0
    if ($oomAdjStr -match '^-?\d+$') { $oomAdjVal = [int]$oomAdjStr }

    $summary = "$ts  PID=$procPid  VmRSS=${vmRssMB}MB  oom_adj=$oomAdjVal  $statusLine"

    if ($oomAdjVal -ge 500 -or $vmRssMB -ge $WarnPssMB) {
        Write-Host $summary -ForegroundColor Red
    } elseif ($oomAdjVal -ge 200 -or $vmRssMB -ge ($WarnPssMB * 0.8)) {
        Write-Host $summary -ForegroundColor Yellow
    } else {
        Write-Host $summary -ForegroundColor Green
    }
    $summary | Out-File $logFile -Append -Encoding UTF8

    # Full dumpsys meminfo every 10 samples
    $script:_cnt++
    if ($script:_cnt % 10 -eq 0) {
        $divider = "=" * 60
        $header  = "$ts  === dumpsys meminfo $Package (PID $procPid) ==="
        Write-Host $header -ForegroundColor Cyan

        $dump = adb shell "dumpsys meminfo $Package 2>/dev/null"

        $keyLines = $dump | Where-Object {
            $_ -match 'TOTAL|Graphics|GL mtrack|Native Heap|Dalvik Heap|Stack|Code|Private'
        }
        foreach ($l in $keyLines) {
            if ($l -match 'TOTAL\s+(\d+)') {
                $totalPssMB = [math]::Round([int]$Matches[1] / 1024, 1)
                if ($totalPssMB -ge $WarnPssMB) {
                    Write-Host "  $l   [= ${totalPssMB}MB]" -ForegroundColor Red
                } elseif ($totalPssMB -ge ($WarnPssMB * 0.8)) {
                    Write-Host "  $l   [= ${totalPssMB}MB]" -ForegroundColor Yellow
                } else {
                    Write-Host "  $l   [= ${totalPssMB}MB]" -ForegroundColor Green
                }
            } else {
                Write-Host "  $l" -ForegroundColor Gray
            }
        }

        $divider | Out-File $logFile -Append -Encoding UTF8
        $header  | Out-File $logFile -Append -Encoding UTF8
        ($dump -join "`n") | Out-File $logFile -Append -Encoding UTF8
        $divider | Out-File $logFile -Append -Encoding UTF8
    }

    Start-Sleep -Milliseconds $IntervalMs
}