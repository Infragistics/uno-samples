namespace SamplesBrowser.Pages.Charts.CategoryChart.HighVolume;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Portable.Description;
using Infragistics.Controls.Charts;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using System;
using System.Collections.Generic;
using System.Globalization;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public MarkerTypeCollection markerTypes1 { get; } = new MarkerTypeCollection { MarkerType.None };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.CategoryChartHighVolumeOnViewInit();
        };
    }

    private ComponentRenderer _componentRenderer = null;
    public ComponentRenderer Renderer
    {
        get
        {
            if (this._componentRenderer == null)
            {
                this._componentRenderer = ComponentRendererHelper.CreateRenderer();
                var context = this._componentRenderer.Context;
                PropertyEditorPanelDescriptionModule.Register(context);
                CategoryChartDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void CategoryChartHighVolumeOnViewInit()
    {
        var chart = this.chart;
        chart.ItemsSource = CategoryChartVolumeData.Generate(CategoryChartVolumeData.Count);
    }

    public void EditorChangeHighVolumeDataPoints(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        CategoryChartVolumeData.Count = System.Convert.ToInt32(args.NewValue);
    }

    public void EditorButtonGenerateHighVolumeData(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var chart = this.chart;
        chart.ItemsSource = CategoryChartVolumeData.Generate(CategoryChartVolumeData.Count);
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

// The data these performance samples bind is generated rather than stored, because their whole
// subject is how many points the chart will take -- the count comes from a control, so no fixed
// data item could stand in for it. Shared by the entry points that generate the first set and the
// button that generates another.
public class CategoryChartVolumeItem
{
    public string Label { get; set; }
    public double Value { get; set; }
}

public static class CategoryChartVolumeData
{
    private static Random random = new Random();

    // How many points to generate next. The slider writes it and the button reads it, because the
    // demo deliberately keeps them apart: regenerating a million points on every step of the slider
    // would make the slider unusable, which is the opposite of what a performance sample should show.
    public static int Count = 500000;

    // A random walk: each reading steps up to two either side of the one before, which gives a line
    // that looks like a signal rather than noise, at any length.
    public static List<CategoryChartVolumeItem> Generate(int count, double startValue = 0)
    {
        var data = new List<CategoryChartVolumeItem>();
        var value = startValue;
        for (var i = 0; i <= count; i++)
        {
            value += random.NextDouble() * 4.0 - 2.0;
            var item = new CategoryChartVolumeItem();
            item.Label = ToShortString(i);
            item.Value = Math.Round(value);
            data.Add(item);
        }
        return data;
    }

    // Axis labels have no room for six digits, so the count is abbreviated the way the reader of a
    // chart this size would write it.
    public static string ToShortString(double largeValue)
    {
        if (largeValue >= 1000000)
        {
            return (Math.Round(largeValue / 100000) / 10).ToString(CultureInfo.InvariantCulture) + "m";
        }
        if (largeValue >= 1000)
        {
            return (Math.Round(largeValue / 100) / 10).ToString(CultureInfo.InvariantCulture) + "k";
        }
        return Math.Round(largeValue).ToString(CultureInfo.InvariantCulture);
    }
}
