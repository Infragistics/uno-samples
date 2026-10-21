using System;

namespace SamplesBrowser.Services;

/// <summary>Resolves a browser URL to the same group/component/sample route used by other platforms.</summary>
public sealed class SampleLink
{
    public string Route { get; private set; } = "";

    public static SampleLink Parse(string address, string appBaseAddress)
    {
        var result = new SampleLink();
        if (!Uri.TryCreate(address, UriKind.Absolute, out var uri) ||
            !Uri.TryCreate(appBaseAddress, UriKind.Absolute, out var appBase) ||
            !appBase.IsBaseOf(uri)) return result;

        var basePath = appBase.AbsolutePath;
        if (!basePath.EndsWith("/", StringComparison.Ordinal)) basePath += "/";
        if (!uri.AbsolutePath.StartsWith(basePath, StringComparison.Ordinal)) return result;
        var path = Uri.UnescapeDataString(uri.AbsolutePath.Substring(basePath.Length)).Trim('/');
        if (path.StartsWith("samples/", StringComparison.Ordinal))
            path = path.Substring("samples/".Length);
        if (path != "index.html") result.Route = path;

        return result;
    }

    public static SampleLink Current()
    {
        // Checked at run time: the Skia browser target does not define __WASM__.
        if (!OperatingSystem.IsBrowser()) return new SampleLink();
        // Read this iframe's location, rather than the embedding page's URL.
        using var location = System.Runtime.InteropServices.JavaScript.JSHost.GlobalThis.GetPropertyAsJSObject("location");
        using var document = System.Runtime.InteropServices.JavaScript.JSHost.GlobalThis.GetPropertyAsJSObject("document");
        var href = location.GetPropertyAsString("href");
        var appBase = document.GetPropertyAsString("baseURI");
        // Without a <base> element (published with no WasmShellWebAppBasePath), baseURI is the page
        // URL itself, which would leave no route; the app is then served from the site root.
        if (appBase == href) appBase = location.GetPropertyAsString("origin") + "/";
        return Parse(href, appBase);
    }
}
