// Scratch-only smoke runner: when SAMPLES_SMOKE=<report path> is set, App hands the launch over to
// this class, which instantiates every registered sample in turn, waits for it to load and settle,
// records exceptions (synchronous, dispatcher and unhandled), optionally captures a PNG, and exits.
//
// After settling it also checks that the sample's main component actually drew something (CONTENT /
// BLANK), and, when the sample has a property editor panel, drives a few of its rows through
// SimulateInput -- the path a user takes -- and checks that the edited component's pixels change
// (PERM ... CHANGED / UNCHANGED / UNHANDLED). Set SAMPLES_SMOKE_PERMS=0 to skip the permutations.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Infragistics.Controls;
using Infragistics.Controls.Layouts;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace SamplesBrowser;

public static class SmokeRunner
{
    private static string _current = "(startup)";
    private static StreamWriter _log;
    private static Window _window;
    private const int MaxPermsPerSample = 8;
    private const int PermSettleMs = 1200;

    public static bool TryStart(Application app)
    {
        var report = Environment.GetEnvironmentVariable("SAMPLES_SMOKE");
        if (string.IsNullOrEmpty(report)) return false;
        var shots = Environment.GetEnvironmentVariable("SAMPLES_SMOKE_SHOTS");
        var filter = Environment.GetEnvironmentVariable("SAMPLES_SMOKE_FILTER");
        var settleMs = int.TryParse(Environment.GetEnvironmentVariable("SAMPLES_SMOKE_SETTLE_MS"), out var ms) ? ms : 2500;
        var perms = Environment.GetEnvironmentVariable("SAMPLES_SMOKE_PERMS") != "0";

        _log = new StreamWriter(report, append: true) { AutoFlush = true };
        app.UnhandledException += (s, e) =>
        {
            _log.WriteLine($"UNHANDLED\t{_current}\t{Flatten(e.Exception)}");
            e.Handled = true;
        };
        if (Environment.GetEnvironmentVariable("SAMPLES_SMOKE_FIRSTCHANCE") == "1")
        {
            AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
            {
                try { _log.WriteLine($"FIRSTCHANCE\t{_current}\t{e.Exception.GetType().Name}: {e.Exception.Message} @ {Environment.StackTrace.Split('\n').Skip(4).Take(6).Select(l => l.Trim()).Aggregate((a, b) => a + " | " + b)}".Replace('\r', ' ').Replace('\n', ' ')); } catch { }
            };
            AppDomain.CurrentDomain.ProcessExit += (s, e) => { try { _log.WriteLine($"PROCESSEXIT\t{_current}\t{Environment.StackTrace.Replace('\r', ' ').Replace('\n', '|')}"); } catch { } };
        }
        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            _log.WriteLine($"UNOBSERVED\t{_current}\t{Flatten(e.Exception)}");
            e.SetObserved();
        };

        if (Environment.GetEnvironmentVariable("SAMPLES_SMOKE_EXPLICIT") != "0")
        {
            app.DispatcherShutdownMode = DispatcherShutdownMode.OnExplicitShutdown;
        }
        var window = new Window { Title = "Samples smoke" };
        // Light theme on an opaque background, so the captures look like the samples do in the browser.
        var host = new Grid
        {
            Width = 1200,
            Height = 800,
            RequestedTheme = ElementTheme.Light,
            Background = new SolidColorBrush(Microsoft.UI.Colors.White)
        };
        window.Content = host;
        window.Closed += (s, e) => { try { _log.WriteLine($"WINDOWCLOSED\t{_current}\t{Environment.StackTrace.Replace('\r', ' ').Replace('\n', '|')}"); } catch { } };
        if (Environment.GetEnvironmentVariable("SAMPLES_SMOKE_VISIBLE") == "1")
        {
            // On screen and activated, to watch the run.
            window.Activate();
        }
        else try
        {
            // Off-screen and never activated, so the run does not take focus from the desktop.
            window.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32 { X = -6000, Y = 0, Width = 1260, Height = 880 });
            window.AppWindow.Show(false);
        }
        catch
        {
            // Hosts without AppWindow placement (Uno desktop) just show the window.
            window.Activate();
        }
        _window = window;
        _ = RunAsync(host, shots, filter, settleMs, perms);
        return true;
    }

    private static async Task RunAsync(Grid host, string shots, string filter, int settleMs, bool perms)
    {
        await Task.Delay(1000);
        // Resumable: a page that took the process down left a BEGIN with nothing after it. Record it as
        // a crash and carry on from the next page instead of retrying it.
        var report = Environment.GetEnvironmentVariable("SAMPLES_SMOKE");
        var attempted = new HashSet<string>();
        string lastBegun = null;
        // The report is already open for appending, so read it through a shared stream.
        var previous = new List<string>();
        if (File.Exists(report))
        {
            using var stream = new FileStream(report, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(stream);
            string read;
            while ((read = reader.ReadLine()) != null) previous.Add(read);
        }
        foreach (var line in previous)
        {
            var parts = line.Split('\t');
            if (parts.Length < 2) continue;
            if (parts[0] == "BEGIN") { attempted.Add(parts[1]); lastBegun = parts[1]; }
            else if (parts[1] == lastBegun && (parts[0] == "OK" || parts[0] == "FAIL" || parts[0] == "CRASH")) { lastBegun = null; }
        }
        if (lastBegun != null) _log.WriteLine($"CRASH\t{lastBegun}");
        var routes = SampleRegistry.Samples.Keys.OrderBy(k => k, StringComparer.Ordinal)
            .Where(r => string.IsNullOrEmpty(filter) || r.Contains(filter))
            .Where(r => !attempted.Contains(r)).ToList();
        FrameworkElement previousSample = null;
        string previousRoute = null;
        foreach (var route in routes)
        {
            if (previousSample != null) await TeardownAsync(host, previousSample, previousRoute);
            previousSample = null;
            _current = route;
            _log.WriteLine($"BEGIN\t{route}");
            var started = DateTime.UtcNow;
            try
            {
                // Setup: construction, then until Loaded, then the first frame after it.
                var clock = System.Diagnostics.Stopwatch.StartNew();
                var sample = (FrameworkElement)SampleRegistry.Samples[route]();
                var ctorMs = clock.ElapsedMilliseconds;
                previousSample = sample;
                previousRoute = route;
                var loaded = new TaskCompletionSource<bool>();
                sample.Loaded += (s, e) => loaded.TrySetResult(true);
                host.Children.Add(sample);
                var addMs = clock.ElapsedMilliseconds;
                var finished = await Task.WhenAny(loaded.Task, Task.Delay(10000));
                if (finished != loaded.Task) _log.WriteLine($"NOLOAD\t{route}");
                var loadMs = clock.ElapsedMilliseconds;
                await NextFrameAsync();
                var frameMs = clock.ElapsedMilliseconds;
                _log.WriteLine($"{(frameMs > SlowMs ? "SLOWSETUP" : "SETUP")}\t{route}\tctor={ctorMs}\tadd={addMs}\tloaded={loadMs}\tframe={frameMs}");
                await Task.Delay(settleMs);
                if (Environment.GetEnvironmentVariable("SAMPLES_SMOKE_FIRSTCHANCE") == "1")
                    _log.WriteLine($"HOST	{route}	{host.ActualWidth}x{host.ActualHeight}	root={host.XamlRoot?.Size}	visible={_window.Visible}	contentIsHost={ReferenceEquals(_window.Content, host)}	sampleParent={VisualTreeHelper.GetParent(sample)?.GetType().Name}");
                if (!string.IsNullOrEmpty(shots)) await CaptureAsync(host, Path.Combine(shots, route.Replace("/", "__") + ".png"));

                var panels = Descendants(sample).OfType<XamPropertyEditorPanel>().ToList();
                var content = FindContent(sample, panels);
                var frame = await GrabAsync(content);
                var ink = frame == null ? -1 : Ink(frame);
                _log.WriteLine($"{(ink >= 0 && ink < 0.002 ? "BLANK" : "CONTENT")}\t{route}\t{Describe(content)}\tink={ink:F4}");

                if (perms)
                {
                    foreach (var panel in panels) await DrivePanelAsync(route, panel, content, host, shots);
                }
                _log.WriteLine($"OK\t{route}\t{(DateTime.UtcNow - started).TotalMilliseconds:F0}ms");
            }
            catch (Exception ex)
            {
                _log.WriteLine($"FAIL\t{route}\t{Flatten(ex)}");
            }
        }
        if (previousSample != null) await TeardownAsync(host, previousSample, previousRoute);
        _current = "(done)";
        _log.WriteLine("DONE");
        _log.Flush();
        Application.Current.Exit();
        // Uno's desktop host does not always leave on Exit(); the report is complete either way.
        await Task.Delay(2000);
        Environment.Exit(0);
    }

    private const int SlowMs = 3000;

    // Teardown: how long removing the sample blocks (sync), until its Unloaded fires, and until the
    // next frame. Popups still open at removal are counted, because an open WinUI popup whose host is
    // removed can make the removal very expensive.
    private static async Task TeardownAsync(Grid host, FrameworkElement sample, string route)
    {
        _current = route + " (teardown)";
        try
        {
            var popups = host.XamlRoot != null ? VisualTreeHelper.GetOpenPopupsForXamlRoot(host.XamlRoot).Count : -1;
            var unloaded = new TaskCompletionSource<bool>();
            sample.Unloaded += (s, e) => unloaded.TrySetResult(true);
            var clock = System.Diagnostics.Stopwatch.StartNew();
            host.Children.Clear();
            var syncMs = clock.ElapsedMilliseconds;
            var finished = await Task.WhenAny(unloaded.Task, Task.Delay(10000));
            var unloadMs = finished == unloaded.Task ? clock.ElapsedMilliseconds : -1;
            await NextFrameAsync();
            var frameMs = clock.ElapsedMilliseconds;
            var left = host.XamlRoot != null ? VisualTreeHelper.GetOpenPopupsForXamlRoot(host.XamlRoot).Count : -1;
            _log.WriteLine($"{(frameMs > SlowMs || unloadMs < 0 ? "SLOWTEARDOWN" : "TEARDOWN")}\t{route}\tsync={syncMs}\tunloaded={unloadMs}\tframe={frameMs}\topenPopups={popups}\tpopupsAfter={left}");
        }
        catch (Exception ex)
        {
            _log.WriteLine($"TEARDOWNFAIL\t{route}\t{Flatten(ex)}");
        }
    }

    // Completes on the next composition frame, or after a second if none comes.
    private static async Task NextFrameAsync()
    {
        var frame = new TaskCompletionSource<bool>();
        EventHandler<object> handler = (s, e) => frame.TrySetResult(true);
        CompositionTarget.Rendering += handler;
        await Task.WhenAny(frame.Task, Task.Delay(1000));
        CompositionTarget.Rendering -= handler;
    }

    // What the content check and the permutations look at: the panel's target when it is an element,
    // otherwise the sample's ContentRoot, otherwise the sample itself.
    private static FrameworkElement FindContent(FrameworkElement sample, List<XamPropertyEditorPanel> panels)
    {
        foreach (var panel in panels)
        {
            if (panel.Target is FrameworkElement target && target.ActualWidth > 0 && target.ActualHeight > 0) return target;
        }
        return Descendants(sample).OfType<FrameworkElement>().FirstOrDefault(e => e.Name == "ContentRoot") ?? sample;
    }

    private static async Task DrivePanelAsync(string route, XamPropertyEditorPanel panel, FrameworkElement fallback, Grid host, string shots)
    {
        var target = panel.Target is FrameworkElement t && t.ActualWidth > 0 && t.ActualHeight > 0 ? t : fallback;
        var rows = (panel.Properties ?? panel.ActualProperties)?.Cast<PropertyEditorPropertyDescription>().ToList()
            ?? new List<PropertyEditorPropertyDescription>();
        var done = 0;
        foreach (var row in rows)
        {
            if (done >= MaxPermsPerSample) break;
            var rowName = panel.DescriptionType + "_" + PathToKey(row.PropertyPath ?? row.Name);
            var label = row.PropertyPath ?? row.Name ?? row.Label;
            List<(string Value, string[] Path)> steps;
            switch (row.ValueType)
            {
                case PropertyEditorValueType.EnumValue:
                    var current = row.PrimitiveValue?.ToString();
                    steps = (row.DropDownValues ?? Array.Empty<string>())
                        .Where(v => !string.Equals(v, current, StringComparison.OrdinalIgnoreCase))
                        .Take(2)
                        .Select(v => (v, new[] { rowName, "Combo", "select", v }))
                        .ToList();
                    break;
                case PropertyEditorValueType.Slider:
                    var min = double.IsNaN(row.Min) ? 0 : row.Min;
                    var max = double.IsNaN(row.Max) ? 100 : row.Max;
                    steps = new[] { 0.15, 0.85 }
                        .Select(f => (min + f * (max - min)).ToString(CultureInfo.InvariantCulture))
                        .Select(v => (v, new[] { rowName, "Slider", "value", v }))
                        .ToList();
                    break;
                case PropertyEditorValueType.Boolean:
                    steps = new() { ("toggle", new[] { rowName }) };
                    break;
                case PropertyEditorValueType.Button:
                    steps = new() { ("click", new[] { rowName }) };
                    break;
                default:
                    _log.WriteLine($"NOACTION\t{route}\t{label}\t{row.ValueType}");
                    continue;
            }
            foreach (var step in steps)
            {
                if (done >= MaxPermsPerSample) break;
                done++;
                var before = await GrabAsync(target);
                var wholeBefore = await GrabAsync(host);
                string outcome;
                try
                {
                    var result = panel.SimulateInput(SimulatedInputType.Click, step.Path, 0, 0, null, null);
                    await Task.Delay(PermSettleMs);
                    var after = await GrabAsync(target);
                    if (!string.IsNullOrEmpty(shots))
                        await CaptureAsync(host, Path.Combine(shots, route.Replace("/", "__") + $"__perm{done}.png"));
                    var diff = Diff(before, after);
                    // The whole sample too, panel included: a weaker signal (the edited row itself
                    // redraws) but it does not depend on picking the right target.
                    var whole = Diff(wholeBefore, await GrabAsync(host));
                    outcome = result == null || !result.IsHandled
                        ? $"UNHANDLED\tdiff={diff:F4}\twhole={whole:F4}"
                        : (diff > 0.0005 ? "CHANGED" : whole > 0.0005 ? "WHOLEONLY" : "UNCHANGED") + $"\tdiff={diff:F4}\twhole={whole:F4}";
                }
                catch (Exception ex)
                {
                    outcome = "ERROR\t" + Flatten(ex);
                }
                _log.WriteLine($"PERM\t{route}\t{label}={step.Value}\t{outcome}");
            }
        }
    }

    private static string PathToKey(string path) => path?
        .Replace(".", "__dot__").Replace("[", "__ob__").Replace("]", "__cb__")
        .Replace(" ", "__sp__").Replace("*", "__star__");

    private static IEnumerable<DependencyObject> Descendants(DependencyObject root)
    {
        var count = VisualTreeHelper.GetChildrenCount(root);
        for (var i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(root, i);
            yield return child;
            foreach (var nested in Descendants(child)) yield return nested;
        }
    }

    private static string Describe(FrameworkElement e) =>
        e.GetType().Name + (string.IsNullOrEmpty(e.Name) ? "" : "#" + e.Name) + $"({e.ActualWidth:F0}x{e.ActualHeight:F0})";

    private sealed class Frame
    {
        public int Width, Height;
        public byte[] Pixels;
    }

    private static async Task<Frame> GrabAsync(UIElement element)
    {
        try
        {
            var rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(element);
            var pixels = await rtb.GetPixelsAsync();
            return new Frame { Width = rtb.PixelWidth, Height = rtb.PixelHeight, Pixels = pixels.ToArray() };
        }
        catch (Exception ex)
        {
            _log.WriteLine($"GRABFAIL\t{_current}\t{Flatten(ex)}");
            return null;
        }
    }

    // Fraction of pixels that differ noticeably from the most common colour: near zero for an empty
    // or single-colour surface.
    private static double Ink(Frame f)
    {
        if (f.Pixels.Length < 4) return 0;
        var counts = new Dictionary<uint, int>();
        for (var i = 0; i + 3 < f.Pixels.Length; i += 16)
        {
            var key = BitConverter.ToUInt32(f.Pixels, i) & 0xF0F0F0F0;
            counts[key] = counts.TryGetValue(key, out var c) ? c + 1 : 1;
        }
        var bg = counts.OrderByDescending(kv => kv.Value).First().Key;
        int total = 0, ink = 0;
        for (var i = 0; i + 3 < f.Pixels.Length; i += 4)
        {
            total++;
            if ((BitConverter.ToUInt32(f.Pixels, i) & 0xF0F0F0F0) != bg) ink++;
        }
        return (double)ink / total;
    }

    private static double Diff(Frame a, Frame b)
    {
        if (a == null || b == null) return -1;
        if (a.Width != b.Width || a.Height != b.Height) return 1;
        int total = 0, changed = 0;
        for (var i = 0; i + 3 < a.Pixels.Length; i += 4)
        {
            total++;
            if (Math.Abs(a.Pixels[i] - b.Pixels[i]) > 12 || Math.Abs(a.Pixels[i + 1] - b.Pixels[i + 1]) > 12 ||
                Math.Abs(a.Pixels[i + 2] - b.Pixels[i + 2]) > 12 || Math.Abs(a.Pixels[i + 3] - b.Pixels[i + 3]) > 12)
                changed++;
        }
        return total == 0 ? 0 : (double)changed / total;
    }

    private static async Task CaptureAsync(UIElement element, string path)
    {
        try
        {
            var rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(element);
            var pixels = await rtb.GetPixelsAsync();
            using var stream = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                (uint)rtb.PixelWidth, (uint)rtb.PixelHeight, 96, 96, pixels.ToArray());
            await encoder.FlushAsync();
            stream.Seek(0);
            var bytes = new byte[stream.Size];
            await stream.ReadAsync(bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);
            File.WriteAllBytes(path, bytes);
        }
        catch (Exception ex)
        {
            _log.WriteLine($"SHOTFAIL\t{_current}\t{Flatten(ex)}");
        }
    }

    private static string Flatten(Exception ex) =>
        (ex?.GetType().Name + ": " + ex?.Message + " @ " + ex?.StackTrace?.Split('\n').FirstOrDefault()?.Trim())
            .Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ');
}
