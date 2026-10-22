<#
.SYNOPSIS
    Builds the Uno 6.5.237 WASM startup and attached-binding fixes from pinned source.
.EXAMPLE
    .\scripts\build-uno-fixes.ps1
    dotnet publish browser/SamplesBrowser/SamplesBrowser.csproj -c Release -f net9.0-browserwasm -p:UnoRuntimeFixesPath=C:/work/uno-samples/artifacts/uno-fixes/runtime
.NOTES
    Outputs stay under artifacts. Installed NuGet packages and package versions are unchanged.
#>
[CmdletBinding()]
param(
    [string]$SourceDirectory,
    [string]$OutputDirectory
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$revision = 'e4de0dcefbeeacf8686c8714e90621b014e56ff3'
$patchPath = Join-Path $PSScriptRoot 'patches/uno-6.5-wasm-startup-and-binding.patch'

if (!$SourceDirectory) {
    $SourceDirectory = Join-Path $repoRoot 'artifacts/uno-fixes/source'
}
if (!$OutputDirectory) {
    $OutputDirectory = Join-Path $repoRoot 'artifacts/uno-fixes/runtime'
}
$SourceDirectory = [IO.Path]::GetFullPath($SourceDirectory)
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)

function Invoke-Checked {
    param([string]$Command, [string[]]$Arguments)
    $executable = (Get-Command $Command -CommandType Application -ErrorAction Stop).Source
    $previousPreference = $ErrorActionPreference
    try {
        # Windows PowerShell treats native progress written to stderr as an ErrorRecord.
        $ErrorActionPreference = 'Continue'
        & $executable @Arguments 2>&1 | ForEach-Object { Write-Output $_.ToString() }
        $exitCode = $LASTEXITCODE
    }
    finally {
        $ErrorActionPreference = $previousPreference
    }
    if ($exitCode -ne 0) {
        throw "$Command failed with exit code $exitCode."
    }
}

if (!(Test-Path -LiteralPath (Join-Path $SourceDirectory '.git'))) {
    if ((Test-Path -LiteralPath $SourceDirectory) -and @(Get-ChildItem -LiteralPath $SourceDirectory -Force).Count -ne 0) {
        throw "SourceDirectory must be empty or an existing checkout of Uno at $revision."
    }
    New-Item -ItemType Directory -Path $SourceDirectory -Force | Out-Null
    Invoke-Checked git @('init', $SourceDirectory)
    Invoke-Checked git @('-C', $SourceDirectory, 'remote', 'add', 'origin', 'https://github.com/unoplatform/uno.git')
    Invoke-Checked git @('-C', $SourceDirectory, '-c', 'core.longpaths=true', 'fetch', '--depth', '1', 'origin', $revision)
    Invoke-Checked git @('-C', $SourceDirectory, '-c', 'core.longpaths=true', 'checkout', '--detach', 'FETCH_HEAD')
}

$head = $null
try {
    $head = & git -C $SourceDirectory rev-parse --verify HEAD 2>$null
}
catch {
    $head = $null
}
if (!$head -and @(Get-ChildItem -LiteralPath $SourceDirectory -Force | Where-Object Name -ne '.git').Count -eq 0) {
    Invoke-Checked git @('-C', $SourceDirectory, 'fetch', '--depth', '1', 'origin', $revision)
    Invoke-Checked git @('-C', $SourceDirectory, '-c', 'core.longpaths=true', 'checkout', '--detach', 'FETCH_HEAD')
    $head = $revision
}
if ($head -ne $revision) {
    throw "SourceDirectory must be checked out at Uno $revision; existing files were left intact."
}

$patchIsApplied = $false
try {
    & git -C $SourceDirectory apply --reverse --check $patchPath 2>$null
    $patchIsApplied = $LASTEXITCODE -eq 0
}
catch {
    $patchIsApplied = $false
}
if (!$patchIsApplied) {
    Invoke-Checked git @('-C', $SourceDirectory, 'apply', '--check', $patchPath)
    Invoke-Checked git @('-C', $SourceDirectory, 'apply', $patchPath)
}

Push-Location (Join-Path $SourceDirectory 'src')
try {
    Invoke-Checked dotnet @(
        'build', 'Uno.UI.Runtime.Skia.WebAssembly.Browser/Uno.UI.Runtime.Skia.WebAssembly.Browser.csproj',
        '--disable-build-servers', '-m:1', '-nr:false',
        '-c', 'Release', '-f', 'net9.0',
        '-p:UnoTargetFrameworkOverride=net9.0',
        '-p:GeneratePackageOnBuild=false',
        '-p:RunAnalyzers=false', '-p:EnforceCodeStyleInBuild=false'
    )
}
finally {
    Pop-Location
}

New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$outputs = @{
    'Uno.UI.dll' = 'src/Uno.UI/bin/Uno.UI.Skia/Release/net9.0/Uno.UI.dll'
    'Uno.UI.Runtime.Skia.WebAssembly.Browser.dll' = 'src/Uno.UI.Runtime.Skia.WebAssembly.Browser/bin/Release/net9.0/Uno.UI.Runtime.Skia.WebAssembly.Browser.dll'
}
foreach ($name in $outputs.Keys) {
    $assemblyPath = Join-Path $SourceDirectory $outputs[$name]
    Copy-Item -LiteralPath $assemblyPath -Destination (Join-Path $OutputDirectory $name) -Force
    $symbolsPath = [IO.Path]::ChangeExtension($assemblyPath, '.pdb')
    if (Test-Path -LiteralPath $symbolsPath) {
        Copy-Item -LiteralPath $symbolsPath -Destination (Join-Path $OutputDirectory ([IO.Path]::GetFileName($symbolsPath))) -Force
    }
}
@("revision=$revision", "patchSHA256=$((Get-FileHash -LiteralPath $patchPath -Algorithm SHA256).Hash)") |
    Set-Content -LiteralPath (Join-Path $OutputDirectory 'source-revision.txt')

Write-Host "Uno fixes built. Publish with -p:UnoRuntimeFixesPath=$OutputDirectory"
