#Requires -Version 7.0
param(
    [switch]$IncludeTrace
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

Write-Output "タスク開始"

New-Item -ItemType Directory -Force -Path $ProfilesDir | Out-Null
Remove-Item -Force -ErrorAction SilentlyContinue -Path $TracePath, $SpeedscopePath, $CountersPath, $CountersLogPath, $CountersErrLogPath

# VS Codeはこのタスクの準備完了シグナル(下の行)を見てから実際にCpProjectを起動するため、
# 先に準備完了を通知してからCpProjectの起動を待つ (先に起動を検出しようとすると待ち合わせが成立しない)
Write-Output "アタッチ準備完了。CpProjectの起動を待ちます..."
$appProc = $null
for ($i = 0; $i -lt 150; $i++) {
    $appProc = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue |
        Where-Object { $_.CommandLine -like "*$Dll*" } |
        Select-Object -First 1
    if ($appProc) { break }
    Start-Sleep -Milliseconds 200
}

if (-not $appProc) {
    Write-Error "CpProjectの起動を検出できませんでした。"
    exit 1
}

Write-Output "対象PID: $($appProc.Id)"

# CSV保存用にcollectをサイレントで並行実行 (自前でキー入力(Q)を監視するため標準入力は切り離しておく)
$countersProc = Start-Process -FilePath "dotnet-counters" `
    -ArgumentList @("collect", "-p", $appProc.Id, "--format", "csv", "--output", $CountersPath) `
    -NoNewWindow -PassThru `
    -RedirectStandardInput $NullDevice `
    -RedirectStandardOutput $CountersLogPath `
    -RedirectStandardError $CountersErrLogPath
Write-Output "GC/メモリの記録を開始しました: $CountersPath"

# このタスクは(デバッグ対象のREPLとは別の)専用ターミナルで動くため、monitorのライブ表示をそのまま出せる
$countersMonitorProc = Start-Process -FilePath "dotnet-counters" `
    -ArgumentList @("monitor", "-p", $appProc.Id) `
    -NoNewWindow -PassThru

$traceProc = $null
if ($IncludeTrace) {
    $traceProc = Start-Process -FilePath "dotnet-trace" `
        -ArgumentList @("collect", "-p", $appProc.Id, "--output", $TracePath) `
        -NoNewWindow -PassThru `
        -RedirectStandardInput $NullDevice
    Write-Output "CPUトレースの記録を開始しました: $TracePath"
}

# 対象プロセス(CpProject)の終了を待つ。終了すればdotnet-trace/dotnet-countersも自動的に終了・保存する
try {
    $appProc.WaitForExit()
}
catch {
    # 既に終了している場合は無視
}

if ($countersProc -and -not $countersProc.HasExited) {
    $exited = $countersProc.WaitForExit(10000)
    if (-not $exited) {
        Stop-Process -Id $countersProc.Id -Force -ErrorAction SilentlyContinue
    }
}

if ($countersMonitorProc -and -not $countersMonitorProc.HasExited) {
    $exited = $countersMonitorProc.WaitForExit(10000)
    if (-not $exited) {
        Stop-Process -Id $countersMonitorProc.Id -Force -ErrorAction SilentlyContinue
    }
}

if ($traceProc) {
    if (-not $traceProc.HasExited) {
        $exited = $traceProc.WaitForExit(10000)
        if (-not $exited) {
            Stop-Process -Id $traceProc.Id -Force -ErrorAction SilentlyContinue
        }
    }

    & dotnet-trace convert --format speedscope $TracePath
    Write-Output ""
    Write-Output "CPUトレース: $SpeedscopePath"

    if (Get-Command npx -ErrorAction SilentlyContinue) {
        & npx --yes speedscope $SpeedscopePath
    }
    else {
        Write-Output "(npxが見つからないため自動起動をスキップしました。speedscope.app にドラッグ&ドロップしてください)"
    }
}

if ((Test-Path $CountersPath) -and (Get-Item $CountersPath).Length -gt 0) {
    Write-Output "GC/メモリ:   $CountersPath"
}
