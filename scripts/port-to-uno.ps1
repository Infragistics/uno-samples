<#
.SYNOPSIS
    Ports the standalone WinUI sample projects under samples/ to Uno single-project
    apps, in place.

.DESCRIPTION
    Each sample folder (any directory containing Sample.xaml) is a self-contained
    WinUI app. This script replaces the WinUI app scaffolding with the Uno single-project
    scaffolding from scripts/uno-template/, while preserving the sample's own content:

        KEPT   : Sample.xaml, Sample.xaml.cs, and any data-class .cs files
        REMOVED: App.xaml(.cs), MainWindow.xaml(.cs), app.manifest, Sample.csproj, Sample.sln
        ADDED  : the full Uno template overlay (csproj, App, MainPage, GlobalUsings,
                 Platforms/, Assets/, Strings/, Properties/, manifests)

    The port is idempotent — re-running overwrites the generated scaffolding and leaves
    the sample content alone. Because the samples are auto-generated upstream, re-running
    after a fresh copy of winui-samples is the intended "regen" path.

.NOTES
    Run from the repo root or scripts/:  .\scripts\port-to-uno.ps1
#>

[CmdletBinding()]
param(
    [string]$SamplesRoot = $null,
    [string]$TemplateDir = $null
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$repoRoot  = Split-Path -Parent $scriptDir
if (-not $SamplesRoot) { $SamplesRoot = Join-Path $repoRoot "samples" }
if (-not $TemplateDir) { $TemplateDir = Join-Path $scriptDir "uno-template" }

# Files the WinUI sample carries that the Uno scaffolding replaces or drops.
$winuiScaffolding = @(
    "App.xaml", "App.xaml.cs",
    "MainWindow.xaml", "MainWindow.xaml.cs",
    "app.manifest", "Sample.csproj", "Sample.sln"
)

# Root-level template files to copy into each sample.
$templateFiles = @(
    "Sample.csproj", "App.xaml", "App.xaml.cs", "GlobalUsings.cs",
    "MainPage.xaml", "MainPage.xaml.cs", "app.manifest", "Package.appxmanifest"
)
# Template directories to copy wholesale into each sample.
$templateDirs = @("Platforms", "Assets", "Strings", "Properties")

Write-Host "=== port-to-uno.ps1 ===" -ForegroundColor Cyan
Write-Host "  Samples root : $SamplesRoot"
Write-Host "  Template dir : $TemplateDir"
Write-Host ""

$samples = Get-ChildItem -Path $SamplesRoot -Recurse -Filter "Sample.xaml" |
           Where-Object { Test-Path (Join-Path $_.DirectoryName "Sample.xaml.cs") }

Write-Host "Found $($samples.Count) sample(s)." -ForegroundColor Green

$n = 0
foreach ($x in $samples) {
    $dir = $x.DirectoryName
    $n++

    foreach ($f in $winuiScaffolding) {
        $p = Join-Path $dir $f
        if (Test-Path $p) { Remove-Item $p -Force }
    }
    foreach ($f in $templateFiles) {
        Copy-Item (Join-Path $TemplateDir $f) (Join-Path $dir $f) -Force
    }
    foreach ($d in $templateDirs) {
        $dest = Join-Path $dir $d
        if (Test-Path $dest) { Remove-Item $dest -Recurse -Force }
        Copy-Item (Join-Path $TemplateDir $d) $dest -Recurse -Force
    }

    $rel = $dir.Substring($SamplesRoot.Length).TrimStart('\','/')
    Write-Host ("  [{0,3}/{1}] {2}" -f $n, $samples.Count, $rel) -ForegroundColor Gray
}

Write-Host ""
Write-Host "Ported $n sample(s) to Uno." -ForegroundColor Cyan