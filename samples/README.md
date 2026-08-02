# WinUI Samples

This folder contains individual WinUI samples. Each sub-folder is an independently buildable WinUI 3 application.

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
- `WinUIApp.csproj` – standalone project file
- `App.xaml` / `App.xaml.cs` – application entry point
- `MainWindow.xaml` / `MainWindow.xaml.cs` – window that hosts the sample
- `Sample.xaml` / `Sample.xaml.cs` – the sample UserControl (the actual content)
- `README.md` – sample-specific documentation

## Running a Sample Independently

1. Open the `WinUIApp.csproj` file in Visual Studio 2022
2. Press F5 to build and run

## Adding Samples to the Browser

Run the ingest script from the `scripts/` folder to add all samples to the combined browser app:

```powershell
cd scripts
.\ingest-samples.ps1
```

Then open and build `browser/SamplesBrowser/SamplesBrowser.csproj`.
