namespace SamplesBrowser.Pages.Charts.PieChart.Animation;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Charts;
using Infragistics.Controls.Layouts;
using System;
using Microsoft.UI.Xaml;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.PieChartAnimationOnViewInit();
        };
    }

    private LocalData _localData = null;
    public LocalData LocalData
    {
        get
        {
            if (_localData == null)
            {
                _localData = new LocalData();
            }
            return _localData;
        }
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
                PieChartDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void PieChartAnimationOnViewInit()
    {
        PieChartAnimation.Start(this.chart);
    }

    public void EditorButtonPieChartAnimation(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        PieChartAnimation.Toggle(this.chart);
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

// The animation is the pie growing from nothing to full size while it turns once, stepped by a
// timer. It lives here rather than in either entry point because both of them drive the same
// animation: the sample starts it once when the view is ready, and the button stops and restarts
// it. Two items each holding their own timer would leave the button unable to stop the one that
// started on load.
//
// The chart is handed in rather than looked up here. Asking for a description resolves, where the
// sample is generated, to the field the component was assigned to -- which only means anything
// inside the component's own instance, so the entry points do the asking and pass the answer on.
public static class PieChartAnimation
{
    private static XamPieChart chart;
    private static DispatcherTimer timer;

    public static bool Running
    {
        get { return timer != null; }
    }

    public static void Toggle(XamPieChart target)
    {
        if (Running)
        {
            Stop();
        }
        else
        {
            Start(target);
        }
    }

    public static void Start(XamPieChart target)
    {
        Stop();
        chart = target;
        // Leaving the sample stops the animation, so the timer does not outlive the chart.
        chart.Unloaded += Chart_Unloaded;
        chart.StartAngle = 0;
        chart.RadiusFactor = 0.1;
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(15);
        timer.Tick += (s, e) => Tick();
        timer.Start();
    }

    public static void Stop()
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
        Stop();
        chart = null;
    }

    private static void Tick()
    {
        if (chart == null)
        {
            return;
        }
        if (chart.RadiusFactor < 1.0)
        {
            chart.RadiusFactor += 0.0025;
        }
        if (chart.StartAngle < 360)
        {
            chart.StartAngle++;
        }
        // Both have arrived, so there is nothing left to step.
        if (chart.RadiusFactor >= 1.0 && chart.StartAngle >= 360)
        {
            Stop();
        }
    }
}
