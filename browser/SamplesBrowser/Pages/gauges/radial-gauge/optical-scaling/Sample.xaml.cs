namespace SamplesBrowser.Pages.Gauges.RadialGauge.OpticalScaling;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Gauges;
using System;


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

    public void RadialGaugeToggleOpticalScaling(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var gauge = this.gauge;
        if (gauge == null) return;
        gauge.OpticalScalingEnabled = args.NewValue is bool b && b;
    }

    public void RadialGaugeSetSize(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var gauge = this.gauge;
        if (gauge == null) return;
        var size = Convert.ToDouble(args.NewValue) * 5;
        gauge.Width = size;
        gauge.Height = size;
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
