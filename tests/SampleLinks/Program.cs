using SamplesBrowser.Services;

var cases = new (string Address, string Base, string Route)[]
{
    ("https://example.com/", "https://example.com/", ""),
    ("https://example.com/index.html", "https://example.com/", ""),
    ("https://example.com/?tracking=1", "https://example.com/", ""),
    ("https://example.com/charts/category-chart/overview", "https://example.com/", "charts/category-chart/overview"),
    ("https://example.com/samples/charts/category-chart/overview/", "https://example.com/", "charts/category-chart/overview"),
    ("https://example.com/uno-demos/", "https://example.com/uno-demos/", ""),
    ("https://example.com/uno-demos/index.html", "https://example.com/uno-demos/", ""),
    ("https://example.com/uno-demos/samples/charts/category-chart/overview?tracking=1#detail", "https://example.com/uno-demos/", "charts/category-chart/overview"),
    ("https://example.com/uno-demos/gauges/radial-gauge/overview", "https://example.com/uno-demos/", "gauges/radial-gauge/overview"),
    ("https://example.com/uno-demos/charts/category-chart/%6Fverview", "https://example.com/uno-demos/", "charts/category-chart/overview"),
    ("https://example.com/uno-demos/charts/category-chart/missing", "https://example.com/uno-demos/", "charts/category-chart/missing"),
    ("https://example.com/outside/charts/category-chart/overview", "https://example.com/uno-demos/", ""),
    ("https://other.example.com/uno-demos/charts/category-chart/overview", "https://example.com/uno-demos/", ""),
    ("not a URL", "https://example.com/", ""),
};

foreach (var (address, appBase, expected) in cases)
{
    var actual = SampleLink.Parse(address, appBase).Route;
    if (actual != expected)
        throw new Exception($"{address}: expected '{expected}', got '{actual}'");
}
Console.WriteLine($"Passed {cases.Length} sample URL checks.");
