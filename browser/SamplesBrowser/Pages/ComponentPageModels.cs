using System.Collections.Generic;
using SamplesBrowser.Services;

namespace SamplesBrowser.Pages;

public sealed class SampleCardItem
{
    public string Name { get; init; } = "";
    public string ComponentName { get; init; } = "";
    public string Route { get; init; } = "";
    public string Glyph { get; init; } = "\uE9F9";
}

public sealed class ComponentPageParameter
{
    public string GroupName { get; init; } = "";
    public List<TocComponent> Components { get; init; } = new();
}

/// <summary>
/// Maps a sample route string to a meaningful Segoe Fluent / MDL2 icon glyph.
/// </summary>
public static class SampleGlyphMap
{
    public static string GlyphForRoute(string route) => route.ToLowerInvariant() switch
    {
        // ── Annotations ───────────────────────────────────────────────
        var r when r.Contains("annotations-all")              => "\uF406",  // all annotation types
        var r when r.Contains("annotations-callouts")         => "\uE8BD",  // speech bubble callout
        var r when r.Contains("annotations-crosshairs")       => "\uE81D",  // crosshair target
        var r when r.Contains("annotations-custom")           => "\uE70F",  // pencil / custom edit
        var r when r.Contains("annotations-final-value")      => "\uE8D0",  // flag / end marker
        var r when r.Contains("annotations-highlighting")     => "\uE81A",  // highlight lamp

        // data-chart annotation layers
        var r when r.Contains("data-annotation-band")         => "\uE14C",  // horizontal band / grid
        var r when r.Contains("data-annotation-line")         => "\uE9D3",  // line layer
        var r when r.Contains("data-annotation-rect")         => "\uE9B1",  // rectangle layer
        var r when r.Contains("data-annotation-slice")        => "\uF4A5",  // slice / pie wedge
        var r when r.Contains("data-annotation-strip")        => "\uE14C",  // strip / band
        var r when r.Contains("data-annotation-multiple")     => "\uF406",  // multiple annotations
        var r when r.Contains("callout-layer")                => "\uE8BD",  // callout bubble

        // ── Axis ──────────────────────────────────────────────────────
        var r when r.Contains("axis-annotations-corner")      => "\uE9B1",  // corner radius
        var r when r.Contains("axis-label-rotation")          => "\uE13E",  // rotate / flip
        var r when r.Contains("axis-min-max")                 => "\uEA18",  // range / min-max
        var r when r.Contains("axis-gap")                     => "\uEA19",  // spacing / gap
        var r when r.Contains("axis-gridlines")               => "\uE14C",  // grid lines
        var r when r.Contains("axis-inverted")                => "\uE13E",  // swap / invert axes
        var r when r.Contains("axis-labels")                  => "\uE8D2",  // tag / label
        var r when r.Contains("axis-locations")               => "\uE81D",  // pin / location
        var r when r.Contains("axis-options")                 => "\uE9E9",  // settings cog
        var r when r.Contains("axis-overlap")                 => "\uE8AB",  // layers overlap
        var r when r.Contains("axis-range")                   => "\uEA18",  // range slider
        var r when r.Contains("axis-tickmarks")               => "\uE8A1",  // tick / check marks
        var r when r.Contains("axis-titles")                  => "\uE8D2",  // label / title

        // ── Area series ───────────────────────────────────────────────
        var r when r.Contains("area-chart")                   => "\uE9D2",  // area chart
        var r when r.Contains("spline-area")                  => "\uE9D2",  // spline area
        var r when r.Contains("step-area")                    => "\uE9D2",  // step area
        var r when r.Contains("range-area")                   => "\uE9D2",  // range area
        var r when r.Contains("stacked-100-area")             => "\uE9D2",
        var r when r.Contains("stacked-area")                 => "\uE9D2",
        var r when r.Contains("radial-area")                  => "\uE9CE",  // radial / radar
        var r when r.Contains("polar-area")                   => "\uE9CE",

        // ── Line series ───────────────────────────────────────────────
        var r when r.Contains("line-chart")                   => "\uE9D3",  // line chart
        var r when r.Contains("spline-multiple")              => "\uE9D3",
        var r when r.Contains("spline-single")                => "\uE9D3",
        var r when r.Contains("spline-styling")               => "\uE9D3",
        var r when r.Contains("step-line")                    => "\uE9D3",
        var r when r.Contains("scatter-line")                 => "\uE9D3",
        var r when r.Contains("scatter-spline")               => "\uE9D3",
        var r when r.Contains("stacked-100-line")             => "\uE9D3",
        var r when r.Contains("stacked-100-spline-chart")     => "\uE9D3",
        var r when r.Contains("stacked-line")                 => "\uE9D3",
        var r when r.Contains("stacked-spline-chart")         => "\uE9D3",
        var r when r.Contains("polar-line")                   => "\uE9CE",
        var r when r.Contains("polar-spline-chart")           => "\uE9CE",
        var r when r.Contains("radial-line")                  => "\uE9CE",
        var r when r.Contains("value-lines")                  => "\uE9D3",  // reference lines
        var r when r.Contains("trendline")                    => "\uE9D3",  // trend line

        // ── Column series ─────────────────────────────────────────────
        var r when r.Contains("column-chart-with-tooltips")   => "\uE8BD",  // column + tooltip
        var r when r.Contains("column-chart")                 => "\uE9F9",  // column / bar chart
        var r when r.Contains("range-column")                 => "\uE9F9",
        var r when r.Contains("stacked-100-column")           => "\uE9F9",
        var r when r.Contains("stacked-column")               => "\uE9F9",
        var r when r.Contains("radial-column")                => "\uE9CE",

        // ── Bar series (horizontal) ────────────────────────────────────
        var r when r.Contains("bar-chart")                    => "\uF246",  // horizontal bar
        var r when r.Contains("stacked-100-bar")              => "\uF246",
        var r when r.Contains("stacked-bar")                  => "\uF246",

        // ── Point / Scatter / Bubble ───────────────────────────────────
        var r when r.Contains("point-chart")                  => "\uF138",  // scatter dots
        var r when r.Contains("scatter-bubble")               => "\uF138",
        var r when r.Contains("scatter-point")                => "\uF138",
        var r when r.Contains("polar-scatter")                => "\uE9CE",

        // ── Pie / Doughnut ────────────────────────────────────────────
        var r when r.Contains("radial-pie")                   => "\uE9CE",  // radial pie → radar
        var r when r.Contains("data-pie")                     => "\uE9F5",  // data pie chart
        var r when r.Contains("doughnut")                     => "\uF4A5",  // doughnut ring
        var r when r.Contains("pie")                          => "\uE9F5",  // pie chart
        var r when r.Contains("rings")                        => "\uF4A5",  // doughnut rings

        // ── Financial / Waterfall ──────────────────────────────────────
        var r when r.Contains("financial-price")              => "\uE8C8",  // candlestick / stock
        var r when r.Contains("financial-dashboard")          => "\uE8C8",
        var r when r.Contains("waterfall")                    => "\uE9F0",  // waterfall steps

        // ── Highlight / Filter / Selection ────────────────────────────
        var r when r.Contains("chart-highlight-filter-datasource") => "\uEF29", // filter + source
        var r when r.Contains("chart-highlight-filter-multiple")   => "\uE71C", // multi filter
        var r when r.Contains("chart-highlight-filter")            => "\uE71C", // filter funnel
        var r when r.Contains("highlight-filter")                  => "\uE71C",
        var r when r.Contains("highlighting-behavior")             => "\uE81A", // behavior lamp
        var r when r.Contains("highlighting-mode")                 => "\uE81A",
        var r when r.Contains("highlighting")                      => "\uE81A",
        var r when r.Contains("selection-multiple")                => "\uE8CB", // multi select
        var r when r.Contains("selection-matcher")                 => "\uE762", // cursor / pointer
        var r when r.Contains("selection-mode")                    => "\uE762",
        var r when r.Contains("custom-selection")                  => "\uE762",
        var r when r.Contains("selection")                         => "\uE762",
        var r when r.Contains("legend-highlighting")               => "\uE81A",

        // ── Legend ────────────────────────────────────────────────────
        var r when r.Contains("data-legend-grouping-and-highlighting") => "\uE81A",
        var r when r.Contains("data-legend-grouping")             => "\uE9F3",
        var r when r.Contains("data-legend-styling")              => "\uE790",
        var r when r.Contains("data-legend-formatting")           => "\uE8D2",
        var r when r.Contains("data-legend")                      => "\uE9F3",  // legend list
        var r when r.Contains("legend")                           => "\uE9F3",

        // ── Tooltip ───────────────────────────────────────────────────
        var r when r.Contains("data-tooltip-grouping-and-highlighting") => "\uE81A",
        var r when r.Contains("data-tooltip-grouping")            => "\uE8BD",
        var r when r.Contains("data-tooltip-styling")             => "\uE790",
        var r when r.Contains("data-tooltip-formatting")          => "\uE8D2",
        var r when r.Contains("data-tooltip-positioning")         => "\uE81D",
        var r when r.Contains("data-tooltip")                     => "\uE8BD",  // tooltip bubble

        // ── Data / Aggregation / Format ───────────────────────────────
        var r when r.Contains("data-aggregations")                => "\uF246",  // aggregate / sum
        var r when r.Contains("data-filter")                      => "\uE71C",  // filter
        var r when r.Contains("format-specifiers")                => "\uE8D2",  // formatting label

        // ── Markers / Styling ─────────────────────────────────────────
        var r when r.Contains("marker-options")                   => "\uE9CE",  // circle marker
        var r when r.Contains("markers")                          => "\uE9CE",
        var r when r.Contains("styling")                          => "\uE790",  // paint brush

        // ── Sparkline display types ────────────────────────────────────
        var r when r.Contains("display-area")                     => "\uE9D2",
        var r when r.Contains("display-column")                   => "\uE9F9",
        var r when r.Contains("display-lines")                    => "\uE9D3",
        var r when r.Contains("display-winloss")                  => "\uE8C8",  // win/loss = financial
        var r when r.Contains("normal-range")                     => "\uEA18",  // range band
        var r when r.Contains("unknown-values")                   => "\uE9CE",  // question / missing

        // ── Tree Map ──────────────────────────────────────────────────
        var r when r.Contains("highlighting-percent")             => "\uE81A",
        var r when r.Contains("tree-map")                         => "\uE9F3",

        // ── Toolbar ───────────────────────────────────────────────────
        var r when r.Contains("actions-built-in-category")        => "\uE9F9",  // category chart actions
        var r when r.Contains("actions-built-in-data")            => "\uE9D2",  // data chart actions
        var r when r.Contains("color-editor-support")             => "\uE790",  // color / palette
        var r when r.Contains("custom-tool")                      => "\uE70F",  // custom / pencil
        var r when r.Contains("layout-actions")                   => "\uE14C",  // layout grid
        var r when r.Contains("layout-in-vertical")               => "\uE14C",  // vertical layout
        var r when r.Contains("theming")                          => "\uE790",  // theme / paint

        // ── Dashboard ─────────────────────────────────────────────────
        var r when r.Contains("gauge-dashboard")                  => "\uE9CE",  // gauge / dial
        var r when r.Contains("map-dashboard")                    => "\uE81D",  // map pin
        var r when r.Contains("pie-dashboard")                    => "\uE9F5",  // pie
        var r when r.Contains("local-data-source")                => "\uE8F1",  // database / local
        var r when r.Contains("dashboard")                        => "\uE9D9",  // dashboard grid

        // ── Animation / Transition ────────────────────────────────────
        var r when r.Contains("animation-replay")                 => "\uE72C",  // replay / repeat
        var r when r.Contains("animation")                        => "\uE768",  // play / animate
        var r when r.Contains("transition-event")                 => "\uE768",

        // ── Radial (catch remaining) ───────────────────────────────────
        var r when r.Contains("radial-label-mode")                => "\uE8D2",
        var r when r.Contains("radial-proportional")              => "\uE9CE",
        var r when r.Contains("radial")                           => "\uE9CE",

        // ── Color Editor ──────────────────────────────────────────────
        var r when r.Contains("color-editor")                     => "\uE790",

        // ── Generic ───────────────────────────────────────────────────
        var r when r.Contains("overview")                         => "\uE7C4",  // eye / overview
        var r when r.Contains("layout")                           => "\uE14C",  // layout grid
        var r when r.Contains("others")                           => "\uE712",  // more / ellipsis
        _                                                         => "\uE9F9"
    };
}
