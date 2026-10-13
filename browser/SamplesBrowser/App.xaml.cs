using System;
using Infragistics.Controls.Dashboards;
using Infragistics.Controls.Grids;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Uno.Resizetizer;

namespace SamplesBrowser;

public partial class App : Application
{
    protected Window? MainWindow { get; private set; }

    public App()
    {
        // Uno uses the Skia renderer by default (see UnoFeatures), so no SkiaSharpRenderer.Use()
        // is needed — but the Infragistics platform services still have to be registered before
        // any control is created.
        Infragistics.Core.UnoPlatformRegistration.Register();
        XamDataGrid.IsCanvasModeDisabled = true;
        // The dashboard-tile feature registration types keep their WinUI* names in the Uno
        // Dashboards assembly (the shared source wasn't renamed for the Uno head).
        WinUIDataChartDashboardTileFeature.Register();
        WinUIPieChartDashboardTileFeature.Register();
        WinUIRadialGaugeDashboardTileFeature.Register();
        WinUILinearGaugeDashboardTileFeature.Register();
        WinUIGeographicMapDashboardTileFeature.Register();
        Services.ComponentAssemblyPreloader.Start();

        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new Window();
#if DEBUG
        MainWindow.UseStudio();
#endif

        if (MainWindow.Content is not Frame rootFrame)
        {
            rootFrame = new Frame();
            MainWindow.Content = rootFrame;
        }

        if (rootFrame.Content == null)
        {
            rootFrame.Navigate(typeof(MainPage), args.Arguments);
        }

        MainWindow.SetWindowIcon();
        MainWindow.Activate();
    }

    public static void InitializeLogging()
    {
#if DEBUG
        var factory = LoggerFactory.Create(builder =>
        {
#if __WASM__
            builder.AddProvider(new global::Uno.Extensions.Logging.WebAssembly.WebAssemblyConsoleLoggerProvider());
#elif __IOS__
            builder.AddProvider(new global::Uno.Extensions.Logging.OSLogLoggerProvider());
            builder.AddConsole();
#else
            builder.AddConsole();
#endif
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddFilter("Uno", LogLevel.Warning);
            builder.AddFilter("Windows", LogLevel.Warning);
            builder.AddFilter("Microsoft", LogLevel.Warning);
        });

        global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory = factory;

#if HAS_UNO
        global::Uno.UI.Adapter.Microsoft.Extensions.Logging.LoggingAdapter.Initialize();
#endif
#endif
    }
}