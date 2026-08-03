#Requires -Version 7.0
param(
    [string[]]$AppArgs = @()
)
$ErrorActionPreference = "Stop"

$RepoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$ProfilesDir = Join-Path $RepoRoot ".profiles"
$Dll = Join-Path $RepoRoot "CpProject" "bin" "Debug" "net9.0" "CpProject.dll"
$TracePath = Join-Path $ProfilesDir "trace.nettrace"
$SpeedscopePath = Join-Path $ProfilesDir "trace.speedscope.json"
$CountersPath = Join-Path $ProfilesDir "counters.csv"
$CountersLogPath = Join-Path $ProfilesDir "counters.log"
$CountersErrLogPath = Join-Path $ProfilesDir "counters.err.log"
$NullDevice = if ($IsWindows) { "NUL" } else { "/dev/null" }

New-Item -ItemType Directory -Force -Path $ProfilesDir | Out-Null
Remove-Item -Force -ErrorAction SilentlyContinue -Path $TracePath, $SpeedscopePath, $CountersPath, $CountersLogPath, $CountersErrLogPath

Push-Location (Join-Path $RepoRoot "CpProject" "Workspace")
try {
    $traceArgs = @("collect", "--show-child-io", "--output", $TracePath, "--", "dotnet", $Dll) + $AppArgs
    $traceProc = Start-Process -FilePath "dotnet-trace" `
        -ArgumentList $traceArgs `
        -NoNewWindow -PassThru

    # dotnet-traceが起動する子プロセス(CpProject.dll)のPIDは直後には確定しないため、少し待って検出する
    $appProc = $null
    for ($i = 0; $i -lt 100; $i++) {
        $appProc = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue |
            Where-Object { $_.CommandLine -like "*$Dll*" } |
            Select-Object -First 1
        if ($appProc) { break }
        Start-Sleep -Milliseconds 100
    }

    $countersProc = $null
    if ($appProc) {
        # dotnet-counters collectは自前でキー入力(Q)を監視するため、REPLと標準入力を取り合わないよう明示的に切り離す
        $countersProc = Start-Process -FilePath "dotnet-counters" `
            -ArgumentList @("collect", "-p", $appProc.Id, "--format", "csv", "--output", $CountersPath) `
            -NoNewWindow -PassThru `
            -RedirectStandardInput $NullDevice `
            -RedirectStandardOutput $CountersLogPath `
            -RedirectStandardError $CountersErrLogPath
    }
    else {
        Write-Warning "CpProjectのPIDを検出できなかったため、GC/メモリのカウンタ収集はスキップします"
    }
    if ($countersProc) {
        Write-Output "GC/メモリの記録を開始しました (対象PID: $($appProc.Id))"
    }

    $traceProc.WaitForExit()

    if ($countersProc) {
        # 監視対象プロセスが終了すればdotnet-counters collectは自動的に終了・保存するので、少し待つだけでよい
        $exited = $countersProc.WaitForExit(10000)
        if (-not $exited) {
            Stop-Process -Id $countersProc.Id -Force -ErrorAction SilentlyContinue
        }
    }
}
finally {
    Pop-Location
}

& dotnet-trace convert --format speedscope $TracePath

Write-Output ""
Write-Output "CPUトレース: $SpeedscopePath"
if ((Test-Path $CountersPath) -and (Get-Item $CountersPath).Length -gt 0) {
    Write-Output "GC/メモリ:   $CountersPath"
}

if (Get-Command npx -ErrorAction SilentlyContinue) {
    & npx --yes speedscope $SpeedscopePath
}
else {
    Write-Output "(npxが見つからないため自動起動をスキップしました。speedscope.app にドラッグ&ドロップしてください)"
}
