using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Microsoft.UI;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Gauges;

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
                RadialGaugeDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void RadialGaugeAnimateToGauge1(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var gauge = this.gauge;
        if (gauge == null) return;

        gauge.TransitionDuration = 1000;
        gauge.MinimumValue = 0;
        gauge.MaximumValue = 10;
        gauge.Value = 7.5;

        gauge.ScaleStartAngle = 180;
        gauge.ScaleEndAngle = 270;
        gauge.ScaleBrush = new SolidColorBrush(Colors.Transparent);
        gauge.ScaleSweepDirection = SweepDirection.Clockwise;

        gauge.BackingOutline = new SolidColorBrush(Colors.White);
        gauge.BackingBrush = new SolidColorBrush(Colors.White);
        gauge.BackingShape = RadialGaugeBackingShape.Fitted;

        gauge.NeedleEndExtent = 0.8;
        gauge.NeedleShape = RadialGaugeNeedleShape.Triangle;
        gauge.NeedlePivotShape = RadialGaugePivotShape.Circle;
        gauge.NeedlePivotWidthRatio = 0.1;
        gauge.NeedleBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));
        gauge.NeedleOutline = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));

        gauge.TickBrush = new SolidColorBrush(Colors.Transparent);
        gauge.MinorTickBrush = new SolidColorBrush(Colors.Transparent);

        gauge.LabelInterval = 5;
        gauge.LabelExtent = 0.915;
        gauge.Font = "15px Verdana,Arial";

        gauge.Ranges.Clear();
        gauge.Ranges.Add(new RadialGaugeRange { StartValue = 0, EndValue = 5,  Brush = new SolidColorBrush(Color.FromArgb(0xFF, 0xA4, 0xBD, 0x29)), Outline = new SolidColorBrush(Color.FromArgb(0xFF, 0xA4, 0xBD, 0x29)), InnerStartExtent = 0.3, InnerEndExtent = 0.3, OuterStartExtent = 0.9, OuterEndExtent = 0.9 });
        gauge.Ranges.Add(new RadialGaugeRange { StartValue = 5, EndValue = 10, Brush = new SolidColorBrush(Color.FromArgb(0xFF, 0xF8, 0x62, 0x32)), Outline = new SolidColorBrush(Color.FromArgb(0xFF, 0xF8, 0x62, 0x32)), InnerStartExtent = 0.3, InnerEndExtent = 0.3, OuterStartExtent = 0.9, OuterEndExtent = 0.9 });
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void RadialGaugeAnimateToGauge2(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var gauge = this.gauge;
        if (gauge == null) return;

        gauge.TransitionDuration = 1000;
        gauge.MinimumValue = 100;
        gauge.MaximumValue = 200;
        gauge.Value = 125;

        gauge.ScaleStartAngle = 180;
        gauge.ScaleEndAngle = 0;
        gauge.ScaleBrush = new SolidColorBrush(Colors.Transparent);
        gauge.ScaleSweepDirection = SweepDirection.Clockwise;

        gauge.BackingOutline = new SolidColorBrush(Colors.White);
        gauge.BackingBrush = new SolidColorBrush(Colors.White);
        gauge.BackingShape = RadialGaugeBackingShape.Fitted;

        gauge.NeedleEndExtent = 0.8;
        gauge.NeedleShape = RadialGaugeNeedleShape.Triangle;
        gauge.NeedlePivotShape = RadialGaugePivotShape.Circle;
        gauge.NeedlePivotWidthRatio = 0.1;
        gauge.NeedleBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));
        gauge.NeedleOutline = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));

        gauge.TickBrush = new SolidColorBrush(Colors.Transparent);
        gauge.MinorTickBrush = new SolidColorBrush(Colors.Transparent);

        gauge.LabelInterval = 50;
        gauge.LabelExtent = 0.935;
        gauge.Font = "13px Verdana,Arial";

        gauge.Ranges.Clear();
        gauge.Ranges.Add(new RadialGaugeRange { StartValue = 100, EndValue = 150, Brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x32, 0xF8, 0x45)), Outline = new SolidColorBrush(Color.FromArgb(0xFF, 0x32, 0xF8, 0x45)), InnerStartExtent = 0.3, InnerEndExtent = 0.3, OuterStartExtent = 0.9, OuterEndExtent = 0.9 });
        gauge.Ranges.Add(new RadialGaugeRange { StartValue = 150, EndValue = 200, Brush = new SolidColorBrush(Color.FromArgb(0xFF, 0xBF, 0x32, 0xF8)), Outline = new SolidColorBrush(Color.FromArgb(0xFF, 0xBF, 0x32, 0xF8)), InnerStartExtent = 0.3, InnerEndExtent = 0.3, OuterStartExtent = 0.9, OuterEndExtent = 0.9 });
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void RadialGaugeAnimateToGauge3(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var gauge = this.gauge;
        if (gauge == null) return;

        gauge.TransitionDuration = 1000;
        gauge.MinimumValue = 0;
        gauge.MaximumValue = 80;
        gauge.Value = 10;
        gauge.Interval = 10;

        gauge.LabelExtent = 0.6;
        gauge.LabelInterval = 10;
        gauge.Font = "15px Verdana,Arial";

        gauge.ScaleStartAngle = 135;
        gauge.ScaleEndAngle = 45;
        gauge.ScaleBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x0B, 0x8F, 0xED));
        gauge.ScaleOversweepShape = RadialGaugeScaleOversweepShape.Auto;
        gauge.ScaleSweepDirection = SweepDirection.Clockwise;
        gauge.ScaleEndExtent = 0.825;
        gauge.ScaleStartExtent = 0.775;

        gauge.MinorTickStartExtent = 0.7;
        gauge.MinorTickEndExtent = 0.75;
        gauge.TickStartExtent = 0.675;
        gauge.TickEndExtent = 0.75;

        gauge.BackingShape = RadialGaugeBackingShape.Fitted;
        gauge.BackingBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFC, 0xFC, 0xFC));
        gauge.BackingOutline = new SolidColorBrush(Color.FromArgb(0xFF, 0xD6, 0xD6, 0xD6));
        gauge.BackingOversweep = 5;
        gauge.BackingCornerRadius = 10;
        gauge.BackingOuterExtent = 0.9;

        gauge.NeedleShape = RadialGaugeNeedleShape.NeedleWithBulb;
        gauge.NeedlePivotShape = RadialGaugePivotShape.CircleOverlay;
        gauge.NeedleEndExtent = 0.5;
        gauge.NeedlePointFeatureExtent = 0.3;
        gauge.NeedlePivotWidthRatio = 0.2;
        gauge.NeedleBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x9F, 0x9F, 0xA0));
        gauge.NeedleOutline = new SolidColorBrush(Color.FromArgb(0xFF, 0x9F, 0x9F, 0xA0));
        gauge.NeedlePivotBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x9F, 0x9F, 0xA0));
        gauge.NeedlePivotOutline = new SolidColorBrush(Color.FromArgb(0xFF, 0x9F, 0x9F, 0xA0));

        gauge.TickBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x33, 0x33));
        gauge.MinorTickBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x49, 0x49, 0x49));
        gauge.MinorTickCount = 6;

        gauge.Ranges.Clear();
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void RadialGaugeAnimateToGauge4(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var gauge = this.gauge;
        if (gauge == null) return;

        gauge.TransitionDuration = 1000;
        gauge.MinimumValue = 0;
        gauge.MaximumValue = 50;
        gauge.Value = 25;
        gauge.Interval = 5;

        gauge.LabelInterval = 5;
        gauge.LabelExtent = 0.71;
        gauge.Font = "15px Verdana,Arial";

        gauge.IsNeedleDraggingEnabled = true;
        gauge.NeedleEndExtent = 0.5;
        gauge.NeedleShape = RadialGaugeNeedleShape.Triangle;
        gauge.NeedleEndWidthRatio = 0.03;
        gauge.NeedleStartWidthRatio = 0.05;
        gauge.NeedlePivotShape = RadialGaugePivotShape.CircleOverlay;
        gauge.NeedlePivotWidthRatio = 0.15;
        gauge.NeedleBaseFeatureWidthRatio = 0.15;
        gauge.NeedleBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));
        gauge.NeedleOutline = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));
        gauge.NeedlePivotBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));
        gauge.NeedlePivotOutline = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));

        gauge.MinorTickCount = 4;
        gauge.MinorTickEndExtent = 0.625;
        gauge.MinorTickStartExtent = 0.6;
        gauge.MinorTickStrokeThickness = 1;
        gauge.MinorTickBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));
        gauge.TickStartExtent = 0.6;
        gauge.TickEndExtent = 0.65;
        gauge.TickStrokeThickness = 2;
        gauge.TickBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0x79, 0x7A));

        gauge.ScaleStartAngle = 120;
        gauge.ScaleEndAngle = 60;
        gauge.ScaleBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xD6, 0xD6, 0xD6));
        gauge.ScaleOversweepShape = RadialGaugeScaleOversweepShape.Fitted;
        gauge.ScaleSweepDirection = SweepDirection.Clockwise;
        gauge.ScaleEndExtent = 0.57;
        gauge.ScaleStartExtent = 0.5;

        gauge.BackingBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFC, 0xFC, 0xFC));
        gauge.BackingOutline = new SolidColorBrush(Color.FromArgb(0xFF, 0xD6, 0xD6, 0xD6));
        gauge.BackingStrokeThickness = 5;
        gauge.BackingShape = RadialGaugeBackingShape.Circular;

        gauge.Ranges.Clear();
        gauge.Ranges.Add(new RadialGaugeRange { StartValue = 5,  EndValue = 15, Brush = new SolidColorBrush(Color.FromArgb(0xFF, 0xF8, 0x62, 0x32)), Outline = new SolidColorBrush(Color.FromArgb(0xFF, 0xF8, 0x62, 0x32)), InnerStartExtent = 0.5, InnerEndExtent = 0.5, OuterStartExtent = 0.57, OuterEndExtent = 0.57 });
        gauge.Ranges.Add(new RadialGaugeRange { StartValue = 15, EndValue = 35, Brush = new SolidColorBrush(Color.FromArgb(0xFF, 0xDC, 0x3F, 0x76)), Outline = new SolidColorBrush(Color.FromArgb(0xFF, 0xDC, 0x3F, 0x76)), InnerStartExtent = 0.5, InnerEndExtent = 0.5, OuterStartExtent = 0.57, OuterEndExtent = 0.57 });
        gauge.Ranges.Add(new RadialGaugeRange { StartValue = 35, EndValue = 45, Brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0x46, 0xB9)), Outline = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0x46, 0xB9)), InnerStartExtent = 0.5, InnerEndExtent = 0.5, OuterStartExtent = 0.57, OuterEndExtent = 0.57 });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
