using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

namespace SamplesBrowser.Pages.Charts.DataPieChart.Highlighting;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private EnergyGlobalDemand _energyGlobalDemand = null;
    public EnergyGlobalDemand EnergyGlobalDemand
    {
        get
        {
            if (_energyGlobalDemand == null)
            {
                _energyGlobalDemand = new EnergyGlobalDemand();
            }
            return _energyGlobalDemand;
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
                DataPieChartDescriptionModule.Register(context);
                ItemLegendDescriptionModule.Register(context);
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
