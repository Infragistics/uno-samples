using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

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

    private OlympicMedalsTopCountriesWithTotals _olympicMedalsTopCountriesWithTotals = null;
    public OlympicMedalsTopCountriesWithTotals OlympicMedalsTopCountriesWithTotals
    {
        get
        {
            if (_olympicMedalsTopCountriesWithTotals == null)
            {
                _olympicMedalsTopCountriesWithTotals = new OlympicMedalsTopCountriesWithTotals();
            }
            return _olympicMedalsTopCountriesWithTotals;
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
                LegendDescriptionModule.Register(context);
                NumberAbbreviatorDescriptionModule.Register(context);
                DataChartCoreDescriptionModule.Register(context);
                DataChartCategoryDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
                DataLegendDescriptionModule.Register(context);
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
