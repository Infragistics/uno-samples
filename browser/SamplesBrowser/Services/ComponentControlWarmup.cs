using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Infragistics.Controls.Charts;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SamplesBrowser.Services;

/// <summary>Exercises control creation, templates, layout and rendering on the UI thread.</summary>
public static class ComponentControlWarmup
{
    public sealed class WarmupPoint
    {
        public string Label { get; set; } = "";
        public double Value { get; set; }
    }

    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(WarmupPoint))]
    public static async Task RunAsync(Panel host)
    {
        // Keep this content in the visual tree behind the opaque startup overlay.
        // Collapsed/transparent controls would skip the rendering work we want to warm.
        var data = new[]
        {
            new WarmupPoint { Label = "A", Value = 10 },
            new WarmupPoint { Label = "B", Value = 20 },
            new WarmupPoint { Label = "C", Value = 15 },
        };
        Func<FrameworkElement>[] factories =
        [
            () => new XamCategoryChart { ItemsSource = data, ChartType = CategoryChartType.Line },
            () => CreateDataChart(data),
            () => new XamPieChart { ItemsSource = data, LabelMemberPath = "Label", ValueMemberPath = "Value" },
            () => new XamDataPieChart { ItemsSource = data },
            () => CreateDoughnutChart(data),
            () => new XamSparkline { ItemsSource = data, ValueMemberPath = "Value" },
            () => new XamTreemap { ItemsSource = data, IdMemberPath = "Label", LabelMemberPath = "Label", ValueMemberPath = "Value" },
            () => new Infragistics.Controls.Gauges.XamRadialGauge { Value = 50 },
            () => new Infragistics.Controls.Gauges.XamLinearGauge { Value = 50 },
            () => new Infragistics.Controls.Gauges.XamBulletGraph { Value = 50 },
            () => new Infragistics.Controls.Grids.XamDataGrid { ItemsSource = data },
            () => new Infragistics.Controls.Inputs.XamColorEditor(),
            () => new Infragistics.Controls.Layouts.XamPropertyEditorPanel(),
            () => new Infragistics.Controls.Layouts.XamToolbar(),
            // Avoid maps/dashboard instances: they can start network requests or background work.
        ];
        var total = Stopwatch.StartNew();
        try
        {
            foreach (var factory in factories)
            {
                var timer = Stopwatch.StartNew();
                FrameworkElement control = null;
                RoutedEventHandler loadedHandler = null;
                try
                {
                    control = factory();
                    var loaded = new TaskCompletionSource<bool>();
                    loadedHandler = (_, _) => loaded.TrySetResult(true);
                    control.Loaded += loadedHandler;
                    host.Children.Add(control);
                    if (control.IsLoaded) loaded.TrySetResult(true);
                    host.UpdateLayout();
                    if (await Task.WhenAny(loaded.Task, Task.Delay(2000)) != loaded.Task)
                        throw new TimeoutException("Control did not enter the visual tree.");
                    // Yield to the compositor while the control remains visible behind the overlay.
                    await Task.Delay(50);
                    Log($"[ComponentWarmup] {control.GetType().Name}: {timer.ElapsedMilliseconds} ms");
                }
                catch (Exception ex)
                {
                    // Warmup is optional and must not prevent browsing.
                    Log($"[ComponentWarmup] {control?.GetType().Name ?? "construction"}: {ex.Message}");
                }
                finally
                {
                    if (control != null && loadedHandler != null) control.Loaded -= loadedHandler;
                    host.Children.Clear();
                }
            }
        }
        finally
        {
            host.Children.Clear();
            host.Visibility = Visibility.Collapsed;
            Log($"[ComponentWarmup] Total: {total.ElapsedMilliseconds} ms");
        }
    }

    private static XamDoughnutChart CreateDoughnutChart(WarmupPoint[] data)
    {
        var chart = new XamDoughnutChart();
        chart.Series.Add(new RingSeries
        {
            ItemsSource = data, LabelMemberPath = "Label", ValueMemberPath = "Value",
        });
        return chart;
    }

    private static XamDataChart CreateDataChart(WarmupPoint[] data)
    {
        var chart = new XamDataChart();
        var xAxis = new CategoryXAxis { ItemsSource = data, Label = "Label" };
        var yAxis = new NumericYAxis();
        chart.Axes.Add(xAxis);
        chart.Axes.Add(yAxis);
        chart.Series.Add(new LineSeries
        {
            ItemsSource = data, ValueMemberPath = "Value", XAxis = xAxis, YAxis = yAxis,
        });
        return chart;
    }

    private static void Log(string message)
    {
        Debug.WriteLine(message);
        Console.WriteLine(message);
    }
}