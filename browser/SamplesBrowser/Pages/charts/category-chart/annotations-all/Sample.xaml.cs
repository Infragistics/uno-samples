using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

namespace SamplesBrowser.Pages.Charts.CategoryChart.AnnotationsAll;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] includedProperties1 { get; } = new string[] { "Month", "Temperature" };
    public string[] calloutsAllowedPositions1 { get; } = new string[] {  };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private TemperatureAnnotatedData _temperatureAnnotatedData = null;
    public TemperatureAnnotatedData TemperatureAnnotatedData
    {
        get
        {
            if (_temperatureAnnotatedData == null)
            {
                _temperatureAnnotatedData = new TemperatureAnnotatedData();
            }
            return _temperatureAnnotatedData;
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
                CategoryChartDescriptionModule.Register(context);
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
