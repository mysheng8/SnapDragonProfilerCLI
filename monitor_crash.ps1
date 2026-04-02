# monitor_crash.ps1
# 用法: .\monitor_crash.ps1 -Package com.yourapp.package
param(
    [string]$Package = "com.ea.gp.fcmnova"   # 改成你的包名
)

Write-Host "=== Android Crash Monitor ===" -ForegroundColor Cyan
Write-Host "Package: $Package" -ForegroundColor Cyan
Write-Host "Press Ctrl+C to stop`n" -ForegroundColor Cyan

# 先清掉旧 logcat buffer，避免历史干扰
adb logcat -c

# 同时监控：
#   lmkd        — Low Memory Killer 杀进程
#   ActivityManager — 进程死亡 / ANR
#   DEBUG       — native crash (tombstone)
#   libc        — native abort
#   AndroidRuntime — Java crash

$logFile = "crash_monitor_$(Get-Date -Format 'HH-mm-ss').txt"
Write-Host "Saving to: $logFile`n" -ForegroundColor Yellow

adb logcat `
    -v threadtime `
    ActivityManager:I `
    lmkd:I `
    DEBUG:I `
    libc:I `
    AndroidRuntime:E `
    "*:S" |
ForEach-Object {
    $line = $_

    # 高亮关键字并写屏
    if ($line -match $Package) {
        Write-Host $line -ForegroundColor Red
    } elseif ($line -match "lmkd|lowmem|Low Memory") {
        Write-Host $line -ForegroundColor Magenta
    } elseif ($line -match "died|killed|Killed|FATAL|crash|ANR|Abort") {
        Write-Host $line -ForegroundColor Yellow
    } else {
        Write-Host $line -ForegroundColor Gray
    }

    # 写文件
    $line | Out-File -FilePath $logFile -Append -Encoding UTF8
}