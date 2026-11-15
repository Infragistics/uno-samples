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
    public static async Task RunAsync(Panel host, Func<bool> stop = null)
    {
        // Keep this content in the visual tree, hidden behind the opaque cover in MainPage.xaml.
        // Collapsed/transparent controls would skip the rendering work we want to warm.
        var data = new[]
        {
            new WarmupPoint { Label = "A", Value = 10 },
            new WarmupPoint { Label = "B", Value = 20 },
            new WarmupPoint { Label = "C", Value = 15 },
        };
        Func<FrameworkElement>[] factories =
        [
            // Only the controls whose first creation is expensive (measured 0.2-1.6 s each).
            () => new XamCategoryChart { ItemsSource = data, ChartType = CategoryChartType.Line },
            () => new Infragistics.Controls.Grids.XamDataGrid { ItemsSource = data },
            () => new Infragistics.Controls.Inputs.XamColorEditor(),
            () => new XamDataPieChart { ItemsSource = data },
        ];
        var total = Stopwatch.StartNew();
        try
        {
            foreach (var factory in factories)
            {
                if (stop?.Invoke() == true) break;
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
                    // Let at least one frame render the control behind the cover.
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

    private static void Log(string message)
    {
        Debug.WriteLine(message);
        Console.WriteLine(message);
    }
}