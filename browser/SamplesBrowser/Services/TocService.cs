using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SamplesBrowser.Services;

// ── TOC data model ─────────────────────────────────────────────────────────

public class Toc
{
    [JsonPropertyName("groups")]
    public List<TocGroup> Groups { get; set; } = new();
}

public class TocGroup
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("components")]
    public List<TocComponent> Components { get; set; } = new();
}

public class TocComponent
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("folder")]
    public string Folder { get; set; } = "";

    [JsonPropertyName("samples")]
    public List<TocSample> Samples { get; set; } = new();
}

public class TocSample
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("route")]
    public string Route { get; set; } = "";

    [JsonPropertyName("showLink")]
    public bool ShowLink { get; set; } = true;
}

// ── TocService ──────────────────────────────────────────────────────────────

public class TocService
{
    public Toc? Toc { get; private set; }

    /// <summary>
    /// Loads toc.json from the Assets folder next to the executable.
    /// </summary>
    public async Task LoadAsync()
    {
        try
        {
            // Read via ms-appx so it works on every Uno head. WASM has no real filesystem at
            // AppContext.BaseDirectory, so the old File.OpenRead there silently produced an
            // empty TOC (only "Home" showed in the nav).
            var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///Assets/toc.json"));
            // Strip a leading UTF-8 BOM: toc.json is written with a BOM (PowerShell Set-Content
            // -Encoding UTF8), and on WASM FileIO.ReadTextAsync does NOT strip it (desktop does).
            // JsonSerializer.Deserialize(string) treats a leading ﻿ as an invalid token, so
            // without this the TOC silently comes back empty on wasm.
            var json = (await Windows.Storage.FileIO.ReadTextAsync(file)).TrimStart('﻿');
            // Use the source-generated context (not reflection): an AOT/trimmed wasm publish
            // disables reflection-based System.Text.Json (IsReflectionEnabledByDefault=false),
            // so JsonSerializer.Deserialize<Toc>(string) throws NotSupportedException at runtime
            // → caught → empty TOC. The source-gen path is reflection-free and AOT-safe.
            Toc = JsonSerializer.Deserialize(json, TocJsonContext.Default.Toc) ?? new Toc();
            // Console.WriteLine (not Debug.WriteLine) so it's visible in Release/AOT — Debug.WriteLine
            // is compiled out without DEBUG, which is why an earlier failure here was silent.
            Console.WriteLine($"[TocService] Loaded {Toc.Groups.Count} group(s) from toc.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TocService] Failed to load toc.json: {ex}");
            Toc = new Toc();
        }
    }
}

// Source-generated (reflection-free) JSON metadata for the TOC model, so deserialization
// works under AOT/trimming on WebAssembly. Honors the [JsonPropertyName] attributes on the model.
[JsonSerializable(typeof(Toc))]
internal partial class TocJsonContext : JsonSerializerContext
{
}
