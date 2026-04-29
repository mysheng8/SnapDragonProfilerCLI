param(
    [int]   $Port       = 8000,
    [string]$BindHost   = "127.0.0.1",
    [int]   $SdpcliPort = 5000
)

Set-StrictMode -Off
$ErrorActionPreference = "Stop"
$root = "$PSScriptRoot\pySdp"

& "$root\webui.ps1" -Port $Port -BindHost $BindHost -SdpcliPort $SdpcliPort
