using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

internal static class Program
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = false };
    private static readonly HashSet<string> SkippedDirectories = new(StringComparer.OrdinalIgnoreCase)
        { "bin", "obj", "Platforms", "Properties", "Strings", ".git" };
    private static readonly HashSet<string> Scaffolding = new(StringComparer.OrdinalIgnoreCase)
        { "App.xaml", "App.xaml.cs", "MainWindow.xaml", "MainWindow.xaml.cs", "MainPage.xaml", "MainPage.xaml.cs", "GlobalUsings.cs", "AssemblyInfo.cs" };
    private static readonly Dictionary<string, string> Languages = new(StringComparer.OrdinalIgnoreCase)
        { [".cs"] = "csharp", [".xaml"] = "xml", [".xml"] = "xml", [".json"] = "json", [".csv"] = "text" };
    private static readonly EnumerationOptions Enumeration = new()
        { RecurseSubdirectories = true, AttributesToSkip = FileAttributes.ReparsePoint, IgnoreInaccessible = false };

    public static int Main(string[] args)
    {
        try
        {
            if (args.Length == 1 && args[0] == "--self-test") { SelfTest(); return 0; }
            if (args.Length != 2) throw new ArgumentException("Usage: SourceViewerGenerator <samples-root> <browser-root>");
            Generate(Path.GetFullPath(args[0]), Path.GetFullPath(args[1]));
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }

    private static string Template()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ViewerTemplate")
            ?? throw new InvalidOperationException("Viewer template missing.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static IEnumerable<string> Files(string root) =>
        Directory.EnumerateFiles(root, "*", Enumeration).Where(file =>
            !Path.GetRelativePath(root, file).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .SkipLast(1).Any(SkippedDirectories.Contains));

    private static string Slash(string value) => value.Replace(Path.DirectorySeparatorChar, '/');
    private static int Rank(string file) => Path.GetFileName(file) switch
        { "Sample.xaml" => 0, "Sample.xaml.cs" => 1, _ => 2 };

    private static string Render(string template, string route, SourceFile[] files) =>
        template.Replace("{{ROUTE_TITLE}}", WebUtility.HtmlEncode(route))
            .Replace("/*SOURCE_FILES*/[]", JsonSerializer.Serialize(files, JsonOptions));

    private static bool WriteIfChanged(string path, string content)
    {
        if (File.Exists(path) && File.ReadAllText(path) == content) return false;
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content, new UTF8Encoding(false));
        return true;
    }

    private static void Generate(string samplesRoot, string browserRoot)
    {
        var template = Template();
        var outputRoot = Path.Combine(browserRoot, "Assets", "Source");
        var manifest = new List<object>();
        var regenerated = 0;
        foreach (var xaml in Files(samplesRoot).Where(f => Path.GetFileName(f) == "Sample.xaml" && File.Exists(f + ".cs")).Order(StringComparer.Ordinal))
        {
            var dir = Path.GetDirectoryName(xaml)!;
            var route = Slash(Path.GetRelativePath(samplesRoot, dir));
            var files = Files(dir).Where(f => Languages.ContainsKey(Path.GetExtension(f)) && !Scaffolding.Contains(Path.GetFileName(f)))
                .OrderBy(Rank).ThenBy(f => Slash(Path.GetRelativePath(dir, f)), StringComparer.Ordinal)
                .Select(f => new SourceFile(Slash(Path.GetRelativePath(dir, f)), Languages[Path.GetExtension(f)], File.ReadAllText(f))).ToArray();
            var html = Render(template, route, files);
            if (Encoding.UTF8.GetByteCount(html) > 2 * 1024 * 1024)
                throw new InvalidOperationException($"Source document exceeds WebView2's 2 MB limit: {route}");
            if (WriteIfChanged(Path.Combine(outputRoot, route + ".html"), html)) regenerated++;
            var digest = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(html)));
            manifest.Add(new { route, digest, files = files.Select(f => new { f.Name, f.Language }) });
        }
        WriteIfChanged(Path.Combine(outputRoot, "index.json"), JsonSerializer.Serialize(new { version = 2, samples = manifest }, JsonOptions) + "\n");
        Console.WriteLine($"Source viewer: {manifest.Count} samples, {regenerated} regenerated from original files.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static SourceFile[] ReadSource(string html)
    {
        const string prefix = "const files = ";
        var start = html.IndexOf(prefix, StringComparison.Ordinal) + prefix.Length;
        var end = html.IndexOf(";\n", start, StringComparison.Ordinal);
        return JsonSerializer.Deserialize<SourceFile[]>(html[start..end], JsonOptions)!;
    }

    private static void SelfTest()
    {
        var template = Template();
        var source = "<UserControl x:Class=\"Original.Sample\"/>\r\n";
        var malicious = "// </script><script>window.injected=true</script>\n";
        var rendered = Render(template, "<route>", [new("Sample.xaml", "xml", source), new("Data.cs", "csharp", malicious)]);
        Check(!rendered.Contains("</script><script>window.injected"), "Source escaped into script markup.");
        Check(rendered.Contains("Sample source: &lt;route&gt;"), "Route title was not HTML-encoded.");
        Check(ReadSource(rendered)[0].Text == source && ReadSource(rendered)[1].Text == malicious, "Original source text changed.");
        Check(!rendered.Contains("/*SOURCE_FILES*/[]"), "Source placeholder remains.");
        Check(rendered.Contains("https://esm.sh/shiki@4.5.0"), "Pinned Shiki CDN missing.");

        var tempBase = Path.GetFullPath(Path.GetTempPath());
        var temp = Path.Combine(tempBase, "source-viewer-test-" + Guid.NewGuid().ToString("N"));
        var samplesRoot = Path.Combine(temp, "samples");
        var browserRoot = Path.Combine(temp, "browser");
        try
        {
            foreach (var name in new[] { "one", "two" })
            {
                var dir = Path.Combine(samplesRoot, "charts", "test", name);
                Directory.CreateDirectory(Path.Combine(dir, "obj"));
                File.WriteAllText(Path.Combine(dir, "Sample.xaml"), source);
                File.WriteAllText(Path.Combine(dir, "Sample.xaml.cs"), "namespace Original; public class Sample {}\n");
                File.WriteAllText(Path.Combine(dir, "Data.cs"), "// data belonging to " + name + "\n");
                File.WriteAllText(Path.Combine(dir, "MainPage.xaml.cs"), "// exclude scaffolding");
                File.WriteAllText(Path.Combine(dir, "obj", "Generated.cs"), "// exclude build output");
            }
            Generate(samplesRoot, browserRoot);
            foreach (var name in new[] { "one", "two" })
            {
                var html = File.ReadAllText(Path.Combine(browserRoot, "Assets", "Source", "charts", "test", name + ".html"));
                var captured = ReadSource(html);
                Check(captured.Select(f => f.Name).SequenceEqual(new[] { "Sample.xaml", "Sample.xaml.cs", "Data.cs" }), "Scaffolding included or file order incorrect.");
                Check(captured[0].Text == source && captured[2].Text == "// data belonging to " + name + "\n", "Per-sample source not preserved.");
            }
            var output = Path.Combine(browserRoot, "Assets", "Source", "charts", "test", "one.html");
            var timestamp = File.GetLastWriteTimeUtc(output);
            Generate(samplesRoot, browserRoot);
            Check(File.GetLastWriteTimeUtc(output) == timestamp, "Unchanged source rewritten.");
            File.WriteAllText(Path.Combine(samplesRoot, "charts", "test", "one", "Data.cs"), "// changed");
            Generate(samplesRoot, browserRoot);
            Check(ReadSource(File.ReadAllText(output))[2].Text == "// changed", "Changed source not regenerated.");
        }
        finally
        {
            var relative = Path.GetRelativePath(tempBase, Path.GetFullPath(temp));
            Check(relative.StartsWith("source-viewer-test-", StringComparison.Ordinal) && !relative.Contains(Path.DirectorySeparatorChar), "Unsafe temporary cleanup path.");
            if (Directory.Exists(temp)) Directory.Delete(temp, true);
        }
        Console.WriteLine("Passed source capture, escaping, file ordering, isolation, incremental generation and CDN configuration checks.");
    }

    private sealed record SourceFile(string Name, string Language, string Text);
}
