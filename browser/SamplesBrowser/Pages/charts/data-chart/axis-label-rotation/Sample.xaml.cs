using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

namespace SamplesBrowser.Pages.Charts.DataChart.AxisLabelRotation;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "None", "SizeChanging", "SizeChangingAndZoom" };
    public string[] dropDownValues1 { get; } = new string[] { "None", "SizeChanging", "SizeChangingAndZoom" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private TemperatureAverageDataLongLabels _temperatureAverageDataLongLabels = null;
    public TemperatureAverageDataLongLabels TemperatureAverageDataLongLabels
    {
        get
        {
            if (_temperatureAverageDataLongLabels == null)
            {
                _temperatureAverageDataLongLabels = new TemperatureAverageDataLongLabels();
            }
            return _temperatureAverageDataLongLabels;
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
                DataChartCategoryDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
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
