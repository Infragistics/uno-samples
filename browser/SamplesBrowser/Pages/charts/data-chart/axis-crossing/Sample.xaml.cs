namespace SamplesBrowser.Pages.Charts.DataChart.AxisCrossing;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Portable.Description;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private ScatterWaveData _scatterWaveData = null;
    public ScatterWaveData ScatterWaveData
    {
        get
        {
            if (_scatterWaveData == null)
            {
                _scatterWaveData = new ScatterWaveData();
            }
            return _scatterWaveData;
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
                DataChartCoreDescriptionModule.Register(context);
                DataChartScatterCoreDescriptionModule.Register(context);
                DataChartScatterDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
            }
        return this._componentRenderer;
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
