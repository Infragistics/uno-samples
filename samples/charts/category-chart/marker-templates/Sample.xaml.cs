using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Charts;
using Infragistics.Controls;
using System;
using Infragistics.Portable.Components.Data;
using PortableDataTemplate = Infragistics.Portable.Components.Data.DataTemplate;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private EnergyGenerationChange _energyGenerationChange = null;
    public EnergyGenerationChange EnergyGenerationChange
    {
        get
        {
            if (_energyGenerationChange == null)
            {
                _energyGenerationChange = new EnergyGenerationChange();
            }
            return _energyGenerationChange;
        }
    }

    public void CategoryChartMarkerTemplateSeriesAdded(object sender, ChartSeriesEventArgs args)
    {
        // On Uno the chart draws its markers immediate mode, so MarkerTemplate is the portable
        // render-callback template (Infragistics.Portable.Components.Data.DataTemplate), not a XAML
        // DataTemplate. It is the same Measure/Render model the web side uses: Measure says how big
        // the marker is, Render draws it at (XPosition, YPosition).
        if (args.Series is MarkerSeries markerSeries)
        {
            markerSeries.MarkerTemplate = CreateValueMarkerTemplate(markerSeries);
        }
    }

    // The value this marker stands for above the point, and a white dot with a dark outline on it.
    // One template per series, so the series' own ValueMemberPath says which column to read.
    private static PortableDataTemplate CreateValueMarkerTemplate(MarkerSeries series)
    {
        const double DotSize = 12;
        const double LabelGap = 2;
        const double LabelGapBelow = 5;
        const double FontSize = 11;

        var font = new RendererFont("Verdana", FontSize, "normal", "normal");
        var black = RendererBrush.FromArgb(255, 0, 0, 0);
        var white = RendererBrush.FromArgb(255, 255, 255, 255);

        return new PortableDataTemplate
        {
            Measure = info =>
            {
                info.Width = DotSize;
                info.Height = DotSize;
            },
            Render = info =>
            {
                var renderer = info.Renderer;
                if (renderer == null)
                {
                    return;
                }

                double cx = info.XPosition;
                double cy = info.YPosition;
                var data = info.RendererData;

                // During the hit-test pass the chart identifies markers by colour, so the shape
                // has to be filled with the item brush it hands us, and nothing else is drawn.
                var fill = white;
                if (info.IsHitTestRender)
                {
                    if (data?.ItemBrush == null)
                    {
                        return;
                    }
                    fill = data.ItemBrush;
                }

                renderer.RenderPath(new RendererPath
                {
                    Data = new RendererEllipseGeometry
                    {
                        Center = new RendererPoint(cx, cy),
                        RadiusX = DotSize / 2,
                        RadiusY = DotSize / 2,
                    },
                    Fill = fill,
                    Stroke = info.IsHitTestRender ? fill : black,
                    StrokeThickness = 1,
                });

                if (info.IsHitTestRender)
                {
                    return;
                }

                var value = ReadValue(data?.Item, (series as AnchoredCategorySeries)?.ValueMemberPath);
                if (value == null)
                {
                    return;
                }
                var text = value + "%";

                // Negative columns grow downward, so their label goes below the dot, pointing the
                // same way as the column; zero and positive values keep it above.
                bool below = IsNegative(value);
                double textHeight = FontSize * 1.25;

                renderer.SetFont(font);
                double width = renderer.MeasureTextWidth(text);
                renderer.RenderTextBlock(new RendererTextBlock
                {
                    Text = text,
                    Fill = black,
                    CanvasLeft = cx - width / 2,
                    CanvasTop = below
                        ? cy + DotSize / 2 + LabelGapBelow
                        : cy - DotSize / 2 - LabelGap - textHeight,
                });
            },
        };
    }

    private static object ReadValue(object item, string column)
    {
        if (item == null || string.IsNullOrEmpty(column))
        {
            return null;
        }
        return item.GetType().GetProperty(column)?.GetValue(item, null);
    }

    private static bool IsNegative(object value)
    {
        try
        {
            return value is IConvertible && System.Convert.ToDouble(value) < 0;
        }
        catch (FormatException)
        {
            return false;
        }
        catch (InvalidCastException)
        {
            return false;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
