namespace SamplesBrowser.Pages.Charts.CategoryChart.FormatSpecifiers;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Portable.Description;
using Infragistics.Controls;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {
            var numberFormatSpecifier2 = new NumberFormatSpecifier();
            numberFormatSpecifier2.Style = "currency";
            numberFormatSpecifier2.MaximumFractionDigits = 2;
            numberFormatSpecifier2.MinimumFractionDigits = 2;
            legend.ValueFormatSpecifiers = new object[] { numberFormatSpecifier2 };
            var numberFormatSpecifier4 = new NumberFormatSpecifier();
            numberFormatSpecifier4.Style = "currency";
            numberFormatSpecifier4.MaximumFractionDigits = 2;
            numberFormatSpecifier4.MinimumFractionDigits = 2;
            chart.DataToolTipValueFormatSpecifiers = new object[] { numberFormatSpecifier4 };
            var numberFormatSpecifier6 = new NumberFormatSpecifier();
            numberFormatSpecifier6.Style = "currency";
            numberFormatSpecifier6.MinimumFractionDigits = 0;
            chart.YAxisLabelFormatSpecifiers = new object[] { numberFormatSpecifier6 };

        };
    }

    private HighestGrossingMovies _highestGrossingMovies = null;
    public HighestGrossingMovies HighestGrossingMovies
    {
        get
        {
            if (_highestGrossingMovies == null)
            {
                _highestGrossingMovies = new HighestGrossingMovies();
            }
            return _highestGrossingMovies;
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
                DataLegendDescriptionModule.Register(context);
                CategoryChartDescriptionModule.Register(context);
                NumberFormatSpecifierDescriptionModule.Register(context);
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
