using System;

namespace SamplesBrowser.Pages;

public sealed class SampleHostParameter
{
    public string Route { get; init; } = "";
    public Func<object> Factory { get; init; }
    public bool IsEmbedded { get; init; }
}
