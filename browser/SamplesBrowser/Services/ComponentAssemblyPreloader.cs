using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

namespace SamplesBrowser.Services;

/// <summary>Loads the component assemblies during startup without constructing UI controls.</summary>
public static class ComponentAssemblyPreloader
{
    private static readonly Lazy<Task> Loading = new(LoadAsync);

    public static Task Ready => Loading.Value;

    public static void Start() => _ = Ready;

    private static async Task LoadAsync()
    {
#if __WASM__
        // WASM may have no worker threads; yield to the startup flow before resolving assemblies.
        await Task.Yield();
        LoadAssemblies();
#else
        await Task.Run(LoadAssemblies);
#endif
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "Best-effort warmup of statically referenced controls. References removed by trimming need no preloading.")]
    private static void LoadAssemblies()
    {
        // Static type references preserve the roots for trimming/AOT. Resolve each root
        // separately so a missing optional component cannot prevent the browser opening.
        Func<Assembly>[] roots =
        [
            () => typeof(Infragistics.Controls.Charts.XamCategoryChart).Assembly,
            () => typeof(Infragistics.Controls.Charts.XamDataChart).Assembly,
            () => typeof(Infragistics.Controls.Charts.XamPieChart).Assembly,
            () => typeof(Infragistics.Controls.Charts.XamDoughnutChart).Assembly,
            () => typeof(Infragistics.Controls.Charts.XamDataPieChart).Assembly,
            () => typeof(Infragistics.Controls.Charts.XamSparkline).Assembly,
            () => typeof(Infragistics.Controls.Charts.XamTreemap).Assembly,
            () => typeof(Infragistics.Controls.Gauges.XamRadialGauge).Assembly,
            () => typeof(Infragistics.Controls.Gauges.XamLinearGauge).Assembly,
            () => typeof(Infragistics.Controls.Gauges.XamBulletGraph).Assembly,
            () => typeof(Infragistics.Controls.Grids.XamDataGrid).Assembly,
            () => typeof(Infragistics.Controls.Inputs.XamColorEditor).Assembly,
            () => typeof(Infragistics.Controls.Layouts.XamPropertyEditorPanel).Assembly,
            () => typeof(Infragistics.Controls.Layouts.XamToolbar).Assembly,
            () => typeof(Infragistics.Controls.Maps.XamGeographicMap).Assembly,
            () => typeof(Infragistics.Controls.Dashboards.XamDashboardTile).Assembly,
        ];
        var timer = Stopwatch.StartNew();
        var visited = new HashSet<string>(StringComparer.Ordinal);
        var pending = new Queue<Assembly>();
        foreach (var root in roots)
        {
            try { pending.Enqueue(root()); }
            catch (Exception ex) { Console.WriteLine($"[ComponentPreload] {ex.Message}"); }
        }
        while (pending.Count > 0)
        {
            var assembly = pending.Dequeue();
            if (!visited.Add(assembly.FullName ?? assembly.GetName().Name ?? "")) continue;
            try
            {
                foreach (var reference in assembly.GetReferencedAssemblies())
                {
                    if (reference.Name?.StartsWith("Infragistics.", StringComparison.Ordinal) != true ||
                        visited.Contains(reference.FullName)) continue;
                    try { pending.Enqueue(Assembly.Load(reference)); }
                    catch (Exception ex) { Console.WriteLine($"[ComponentPreload] {reference.Name}: {ex.Message}"); }
                }
            }
            catch (Exception ex) { Console.WriteLine($"[ComponentPreload] {assembly.GetName().Name}: {ex.Message}"); }
        }
        Console.WriteLine($"[ComponentPreload] Loaded {visited.Count} assemblies in {timer.ElapsedMilliseconds} ms.");
    }
}
