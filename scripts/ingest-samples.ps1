<#
.SYNOPSIS
    Ingests individual WinUI samples from the samples/ folder into the combined
    SamplesBrowser application.

.DESCRIPTION
    This script:
      1. Scans samples/ for all sample folders (those containing Sample.xaml).
      2. Copies Sample.xaml and Sample.xaml.cs into the browser's Pages/ directory,
         updating namespaces and x:Class attributes.
      3. Generates Assets/toc.json with the full group/component/sample hierarchy.
      4. Generates SampleRegistry.cs that maps routes to UserControl factories.
      5. Updates SamplesBrowser.csproj to include all copied page files.

.NOTES
    Run from the scripts/ directory:
        .\ingest-samples.ps1

    Or from the repository root:
        .\scripts\ingest-samples.ps1
#>

[CmdletBinding()]
param(
    [string]$SamplesRoot  = $null,
    [string]$BrowserRoot  = $null
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# ---- Resolve paths ----

$scriptDir   = Split-Path -Parent $MyInvocation.MyCommand.Definition
$repoRoot    = Split-Path -Parent $scriptDir

if (-not $SamplesRoot) { $SamplesRoot = Join-Path $repoRoot "samples" }
if (-not $BrowserRoot) { $BrowserRoot = Join-Path $repoRoot "browser\SamplesBrowser" }

$pagesDir    = Join-Path $BrowserRoot "Pages"
$assetsDir   = Join-Path $BrowserRoot "Assets"
$tocPath     = Join-Path $assetsDir   "toc.json"
$registryPath = Join-Path $BrowserRoot "SampleRegistry.cs"

Write-Host ""
Write-Host "=== ingest-samples.ps1 ===" -ForegroundColor Cyan
Write-Host "  Samples root : $SamplesRoot"
Write-Host "  Browser root : $BrowserRoot"
Write-Host ""

# ---- Helper: convert a hyphenated folder segment to PascalCase ----

function ConvertTo-PascalCase([string]$segment) {
    $parts = @(($segment -split '-') | ForEach-Object {
        if ($_.Length -gt 0) { $_.Substring(0,1).ToUpper() + $_.Substring(1) }
    })
    return ($parts -join "")
}

# ---- Helper: convert a hyphenated folder name to space-separated title case ----
# e.g. "category-chart" -> "Category Chart"

function ConvertTo-TitleCase([string]$segment) {
    $parts = @(($segment -split '-') | ForEach-Object {
        if ($_.Length -gt 0) { $_.Substring(0,1).ToUpper() + $_.Substring(1) }
    })
    return ($parts -join " ")
}

# ---- Helper: convert a folder-relative route to a .NET namespace suffix ----

function Get-Namespace([string]$route) {
    # route: "charts/category-chart/overview"
    $parts = @(($route -split '/') | ForEach-Object { ConvertTo-PascalCase $_ })
    return ($parts -join ".")
}

# ---- Clean previously ingested sample pages ----

Write-Host "Cleaning previously ingested pages ..." -ForegroundColor Yellow

# Remove only the dynamically-generated sub-folders (not HomePage / SampleHostPage)
$staticPages = @("HomePage.xaml", "HomePage.xaml.cs", "SampleHostPage.xaml", "SampleHostPage.xaml.cs")

Get-ChildItem -Path $pagesDir -Recurse -Directory | Sort-Object FullName -Descending | ForEach-Object {
    Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Host "  Done." -ForegroundColor Green

# ---- Discover samples ----

Write-Host ""
Write-Host "Scanning for samples in $SamplesRoot ..." -ForegroundColor Yellow

# A sample folder is any directory that contains both Sample.xaml and Sample.xaml.cs
$sampleFiles = Get-ChildItem -Path $SamplesRoot -Recurse -Filter "Sample.xaml" |
               Where-Object { Test-Path (Join-Path $_.DirectoryName "Sample.xaml.cs") }

if ($sampleFiles.Count -eq 0) {
    Write-Warning "No samples found. Make sure each sample folder contains Sample.xaml and Sample.xaml.cs."
    exit 0
}

Write-Host "  Found $($sampleFiles.Count) sample(s)." -ForegroundColor Green

# ---- Build internal sample list ----

$sampleList = [System.Collections.Generic.List[PSCustomObject]]::new()

foreach ($xamlFile in $sampleFiles) {
    $sampleDir = $xamlFile.DirectoryName

    # Compute route relative to samples root: "charts/category-chart/overview"
    $route = ($sampleDir -replace [regex]::Escape($SamplesRoot), "").TrimStart('\', '/')
    $route = $route -replace '\\', '/'

    # Extract path segments
    $segments = $route -split '/'
    if ($segments.Count -lt 3) {
        Write-Warning "Skipping '$route' - expected at least 3 path segments (group/component/sample)."
        continue
    }

    $group     = $segments[0]                         # e.g. "charts"
    $component = $segments[1]                         # e.g. "category-chart"
    $sampleSeg = $segments[-1]                        # e.g. "overview"

    # Human-readable names (capitalize first letter of each hyphenated word)
    $groupName     = ConvertTo-TitleCase $group
    $componentName = ConvertTo-TitleCase $component
    $sampleName    = ConvertTo-TitleCase $sampleSeg

    # .NET namespace suffix inside SamplesBrowser.Pages
    $nsSuffix = Get-Namespace $route    # e.g. "Charts.CategoryChart.Overview"
    $fullNs   = "SamplesBrowser.Pages.$nsSuffix"

    $sampleList.Add([PSCustomObject]@{
        Route         = $route
        Group         = $group
        GroupName     = $groupName
        Component     = $component
        ComponentName = $componentName
        SampleSeg     = $sampleSeg
        SampleName    = $sampleName
        Namespace     = $fullNs
        SrcDir        = $sampleDir
        XamlFile      = $xamlFile.FullName
        CsFile        = Join-Path $sampleDir "Sample.xaml.cs"
    })
}

Write-Host "  Processed $($sampleList.Count) sample(s)." -ForegroundColor Green

# ---- Copy sample files into browser/Pages/ ----

Write-Host ""
Write-Host "Copying sample files ..." -ForegroundColor Yellow

$copiedItems = [System.Collections.Generic.List[string]]::new()

# Per-sample template files (Sample/SampleViewModel) live in the sample's Pages
# subdir under the browser. Anything else .cs in the sample folder is treated as
# a shared data class (POCO in the global namespace) — it gets deduped to a
# single Services/ copy in the browser. Mirrors the Blazor/WC pattern where
# Data.cs / Data.ts files are pulled out of per-sample folders and shared.
$servicesDir = Join-Path $BrowserRoot "Services"
New-Item -ItemType Directory -Path $servicesDir -Force | Out-Null
$copiedServicesByName = @{}

# Per-sample boilerplate that the standalone sample app needs but the browser
# doesn't (App.xaml.cs etc. — the browser has its own App/MainWindow).
$skipPerSampleCs = @(
    'App.xaml.cs',
    'MainWindow.xaml.cs'
)

foreach ($s in $sampleList) {

    $destDir = Join-Path $pagesDir ($s.Route -replace '/', '\')
    New-Item -ItemType Directory -Path $destDir -Force | Out-Null

    # ---- Sample.xaml ----
    $xamlContent = Get-Content $s.XamlFile -Raw

    # Replace x:Class="WinUIApp.Sample" with the browser namespace
    $xamlContent = $xamlContent -replace 'x:Class="[^"]*"', "x:Class=`"$($s.Namespace).Sample`""

    # Normalise trailing whitespace: ensure exactly one newline at end of file
    $xamlContent = $xamlContent.TrimEnd() + "`n"

    $destXaml = Join-Path $destDir "Sample.xaml"
    Set-Content -Path $destXaml -Value $xamlContent -NoNewline -Encoding UTF8
    $copiedItems.Add($destXaml)

    # ---- Sample.xaml.cs ----
    $csContent = Get-Content $s.CsFile -Raw

    # Replace namespace declaration
    $csContent = $csContent -replace 'namespace\s+\S+\s*;', "namespace $($s.Namespace);"
    $csContent = $csContent -replace 'namespace\s+\S+\s*\{', "namespace $($s.Namespace) {"

    # Normalise trailing whitespace: ensure exactly one newline at end of file
    $csContent = $csContent.TrimEnd() + "`n"

    $destCs = Join-Path $destDir "Sample.xaml.cs"
    Set-Content -Path $destCs -Value $csContent -NoNewline -Encoding UTF8
    $copiedItems.Add($destCs)

    # ---- Shared data classes -> Services/ (deduped by filename) ----
    # Anything else .cs in the sample folder is a POCO data class (global
    # namespace). Multiple samples typically share the same data class — first
    # one wins; later samples with the same filename are assumed identical and
    # skipped. If a later sample produces a different file with the same name
    # the difference is silently lost, same trade-off Blazor's ingest accepts.
    Get-ChildItem -Path $s.SrcDir -Filter "*.cs" -File | ForEach-Object {
        $name = $_.Name
        if ($name -eq "Sample.xaml.cs") { return }
        if ($skipPerSampleCs -contains $name) { return }

        if ($copiedServicesByName.ContainsKey($name)) { return }

        $destData = Join-Path $servicesDir $name
        $dataContent = Get-Content $_.FullName -Raw
        $dataContent = $dataContent.TrimEnd() + "`n"
        Set-Content -Path $destData -Value $dataContent -NoNewline -Encoding UTF8
        $copiedServicesByName[$name] = $true
        $copiedItems.Add($destData)
    }

    Write-Host "  + $($s.Route)" -ForegroundColor Gray
}

Write-Host "  Copied $($sampleList.Count) sample(s)." -ForegroundColor Green
Write-Host "  Copied $($copiedServicesByName.Count) shared data class(es) to Services/" -ForegroundColor Green

# ---- Generate toc.json ----

Write-Host ""
Write-Host "Generating toc.json ..." -ForegroundColor Yellow

# Build a hierarchy: groups -> components -> samples
$groups = [ordered]@{}

foreach ($s in $sampleList) {
    if (-not $groups.Contains($s.Group)) {
        $groups[$s.Group] = [PSCustomObject]@{
            name       = $s.GroupName
            components = [ordered]@{}
        }
    }

    $g = $groups[$s.Group]
    if (-not $g.components.Contains($s.Component)) {
        $g.components[$s.Component] = [PSCustomObject]@{
            name    = $s.ComponentName
            folder  = $s.Component
            samples = [System.Collections.Generic.List[PSCustomObject]]::new()
        }
    }

    $g.components[$s.Component].samples.Add([PSCustomObject]@{
        name     = $s.SampleName
        route    = $s.Route
        showLink = $true
    })
}

# Convert to a serialisable structure, forcing arrays with @() so that
# ConvertTo-Json never collapses a single-element collection to an object.
$tocGroups = [System.Collections.Generic.List[PSCustomObject]]::new()

foreach ($g in $groups.Values) {
    $compList = [System.Collections.Generic.List[PSCustomObject]]::new()
    foreach ($c in $g.components.Values) {
        $compList.Add([PSCustomObject]@{
            name    = $c.name
            folder  = $c.folder
            samples = @($c.samples.ToArray())
        })
    }
    $tocGroups.Add([PSCustomObject]@{
        name       = $g.name
        components = @($compList.ToArray())
    })
}

$toc = [PSCustomObject]@{ groups = @($tocGroups.ToArray()) }
$tocJson = $toc | ConvertTo-Json -Depth 10
Set-Content -Path $tocPath -Value $tocJson -Encoding UTF8

Write-Host "  Written to $tocPath" -ForegroundColor Green

# ---- Generate SampleRegistry.cs ----

Write-Host ""
Write-Host "Generating SampleRegistry.cs ..." -ForegroundColor Yellow

$entries = foreach ($s in $sampleList) {
    "        { `"$($s.Route)`", () => new Pages.$($s.Namespace.Replace('SamplesBrowser.Pages.', '')).Sample() },"
}

$registryContent = @"
// AUTO-GENERATED by scripts/ingest-samples.ps1 -- DO NOT EDIT MANUALLY.
// Run the ingest script to regenerate this file after adding or removing samples.

using System;
using System.Collections.Generic;

namespace SamplesBrowser;

public static class SampleRegistry
{
    /// <summary>
    /// Maps a sample route (e.g. "charts/category-chart/overview") to a factory
    /// that creates the corresponding sample UserControl.
    /// Populated by scripts/ingest-samples.ps1.
    /// </summary>
    public static readonly Dictionary<string, Func<object>> Samples = new()
    {
$($entries -join "`n")
    };
}
"@

Set-Content -Path $registryPath -Value $registryContent -Encoding UTF8

Write-Host "  Written to $registryPath" -ForegroundColor Green

# Note: SamplesBrowser.csproj is NOT modified here. The SDK's implicit Page/
# Compile globs pick up everything under Pages/ automatically, so the only
# work this script needs to do for the csproj is leave it alone.

# ---- Summary ----

Write-Host ""
Write-Host "=== Done ===" -ForegroundColor Cyan
Write-Host "  $($sampleList.Count) sample(s) ingested into the browser."
Write-Host ""
Write-Host "Next steps:" -ForegroundColor White
Write-Host "  1. Open browser\SamplesBrowser\SamplesBrowser.csproj in Visual Studio 2022"
Write-Host "  2. Build (Ctrl+Shift+B) and run (F5)"
Write-Host ""
