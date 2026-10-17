namespace SamplesBrowser.Pages.Charts.CategoryChart.HighFrequency;

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
using Microsoft.UI.Xaml;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public MarkerTypeCollection markerTypes1 { get; } = new MarkerTypeCollection { MarkerType.None };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.CategoryChartHighFrequencyOnViewInit();
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

    public void CategoryChartHighFrequencyOnViewInit()
    {
        CategoryChartFrequency.Generate(this.chart);
        CategoryChartFrequency.RestartTimer(this.chart);
    }

    public void EditorButtonToggleHighFrequency(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        CategoryChartFrequency.Toggle();
    }

    public void EditorChangeHighFrequencyRefresh(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        CategoryChartFrequency.RefreshMilliseconds = System.Convert.ToInt32(args.NewValue);
        CategoryChartFrequency.RestartTimer(this.chart);
    }

    public void EditorChangeHighFrequencyPoints(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        CategoryChartFrequency.Points = System.Convert.ToInt32(args.NewValue);
    }

    public void EditorButtonGenerateHighFrequencyData(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        CategoryChartFrequency.Generate(this.chart);
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

// A window onto a series that keeps arriving: every tick appends one reading and drops the oldest,
// so the line scrolls without the collection growing. The chart is told about each end separately --
// an insert at the tail and a remove at the head -- which is what lets it move a chart of this size
// without rebuilding it.
//
// All of it lives here because five controls drive one running chart between them: start and stop,
// how often it ticks, how many points it holds, and generating a fresh set. Each of those is a
// separate entry point, and they have to be talking about the same data and the same timer.
//
// The chart is handed in rather than looked up here. Asking for a description resolves, where the
// sample is generated, to the field the component was assigned to -- which only means anything
// inside the component's own instance, so the entry points do the asking and pass the answer on.
public class CategoryChartFrequencyItem
{
    public string Label { get; set; }
    public double Value { get; set; }
}

public static class CategoryChartFrequency
{
    public static int Points = 100000;
    public static int RefreshMilliseconds = 10;
    public static bool Running = false;

    private static XamCategoryChart chart;
    private static List<CategoryChartFrequencyItem> data = new List<CategoryChartFrequencyItem>();
    private static int index = 0;
    private static DispatcherTimer timer;
    private static Random random = new Random();

    public static void Generate(XamCategoryChart target)
    {
        chart = target;
        data = new List<CategoryChartFrequencyItem>();
        var value = 100.0;
        for (var i = 0; i <= Points; i++)
        {
            value += random.NextDouble() * 4.0 - 2.0;
            var item = new CategoryChartFrequencyItem();
            item.Label = i.ToString(CultureInfo.InvariantCulture);
            item.Value = Math.Round(value);
            data.Add(item);
        }
        index = data.Count;
        chart.ItemsSource = data;
    }

    public static void Toggle()
    {
        Running = !Running;
    }

    // Restarted rather than adjusted, because the interval is fixed when the timer is created.
    public static void RestartTimer(XamCategoryChart target)
    {
        StopTimer();
        chart = target;
        // Leaving the sample stops the timer, so it does not keep ticking against a chart that is gone.
        chart.Unloaded += Chart_Unloaded;
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(RefreshMilliseconds);
        timer.Tick += (s, e) => Tick();
        timer.Start();
    }

    public static void StopTimer()
    {
        if (timer != null)
        {
            timer.Stop();
            timer = null;
        }
        if (chart != null)
        {
            chart.Unloaded -= Chart_Unloaded;
        }
    }

    private static void Chart_Unloaded(object sender, RoutedEventArgs e)
    {
        StopTimer();
        Running = false;
        chart = null;
    }

    private static void Tick()
    {
        if (!Running)
        {
            return;
        }
        if (chart == null)
        {
            return;
        }

        var previous = data[data.Count - 1];
        var arrived = new CategoryChartFrequencyItem();
        arrived.Label = (++index).ToString(CultureInfo.InvariantCulture);
        arrived.Value = previous.Value + random.NextDouble() * 4.0 - 2.0;

        var leaving = data[0];
        data.Add(arrived);
        chart.NotifyInsertItem(data, data.Count - 1, arrived);
        data.RemoveAt(0);
        chart.NotifyRemoveItem(data, 0, leaving);
    }
}
