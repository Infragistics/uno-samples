# Uno Platform Samples

This folder contains individual Uno Platform samples. Each sub-folder is an independently buildable Uno Platform application.

## Structure

```
samples/
├── charts/
│   └── category-chart/
│       ├── overview/
│       └── animation/
├── gauges/
│   └── radial-gauge/
│       └── overview/
└── ...
```

Each sample folder contains:
- `Sample.csproj` – standalone Uno Platform project file
- `App.xaml` / `App.xaml.cs` – application entry point
- `MainPage.xaml` / `MainPage.xaml.cs` – page that hosts the sample
- `Sample.xaml` / `Sample.xaml.cs` – the sample UserControl (the actual content)
- `Platforms/` – platform-specific application assets and entry points

## Running a Sample Independently

1. Open the `Sample.csproj` file in Visual Studio with Uno Platform support.
2. Select the desired target and press F5 to build and run.

## Adding Samples to the Browser

Run the ingest script from the `scripts/` folder to add all samples to the combined browser app:

```powershell
cd scripts
.\ingest-samples.ps1
```

Then open and build `browser/SamplesBrowser/SamplesBrowser.csproj`.
