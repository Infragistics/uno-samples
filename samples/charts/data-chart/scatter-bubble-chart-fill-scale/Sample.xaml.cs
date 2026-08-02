using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] includedColumns1 { get; } = new string[] { "X", "Y", "Radius" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private WorldStats _worldStats = null;
    public WorldStats WorldStats
    {
        get
        {
            if (_worldStats == null)
            {
                _worldStats = new WorldStats();
            }
            return _worldStats;
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
                NumberAbbreviatorDescriptionModule.Register(context);
                DataChartCoreDescriptionModule.Register(context);
                DataChartScatterDescriptionModule.Register(context);
                DataChartScatterCoreDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
                DataChartAnnotationDescriptionModule.Register(context);
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
