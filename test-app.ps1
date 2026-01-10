cd "e:\_DevResources\1. C-Family\C-Sharp\C-Sharp--Projects\WindowsProj\WarehouseManagement"
$debugLogPath = "Build\bin\Debug\net472\debug.log"
Write-Host "Launching application..."
Start-Process "Build\bin\Debug\net472\WarehouseManagement.exe" -NoNewWindow -PassThru
Start-Sleep -Seconds 2

# Check if log file exists
if (Test-Path $debugLogPath) {
    Write-Host "`nDebug log found. Content:"
    Get-Content $debugLogPath
} else {
    Write-Host "Debug log file not found at: $debugLogPath"
}
