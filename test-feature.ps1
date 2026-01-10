#!/usr/bin/env powershell
# Test script to verify transaction detail functionality

Write-Host "========================================"
Write-Host "Transaction Detail Feature Test"
Write-Host "========================================"
Write-Host ""

# Get the build output path
$appPath = "Build\bin\Debug\net472\WarehouseManagement.exe"
$debugLogPath = "Build\bin\Debug\net472\debug.log"

# Check if app was built
if (!(Test-Path $appPath)) {
    Write-Host "ERROR: Application not found at $appPath" -ForegroundColor Red
    exit 1
}

Write-Host "✓ Application found at: $appPath" -ForegroundColor Green
Write-Host ""

# Clean old log
if (Test-Path $debugLogPath) {
    Remove-Item $debugLogPath -Force
    Write-Host "✓ Cleared old debug log" -ForegroundColor Green
}

Write-Host ""
Write-Host "Starting application (will run for 5 seconds)..."
Write-Host ""

# Start app with timeout
$process = Start-Process $appPath -PassThru
Start-Sleep -Seconds 5

# Kill the process
if ($process -and !$process.HasExited) {
    Stop-Process $process -Force -ErrorAction SilentlyContinue
    Write-Host "✓ Application stopped" -ForegroundColor Green
}

# Check debug log
Write-Host ""
Write-Host "========================================"
Write-Host "Debug Log Output:"
Write-Host "========================================"

if (Test-Path $debugLogPath) {
    Write-Host ""
    Get-Content $debugLogPath | ForEach-Object {
        if ($_ -match "ERROR|ERROR|Exception") {
            Write-Host $_ -ForegroundColor Red
        } elseif ($_ -match "SUCCESS|thành công") {
            Write-Host $_ -ForegroundColor Green
        } else {
            Write-Host $_
        }
    }
    Write-Host ""
} else {
    Write-Host "No debug log found" -ForegroundColor Yellow
}

Write-Host "========================================"
Write-Host "Test Complete"
Write-Host "========================================"
