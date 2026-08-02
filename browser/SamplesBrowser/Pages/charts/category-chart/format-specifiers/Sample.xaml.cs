using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls;

namespace SamplesBrowser.Pages.Charts.CategoryChart.FormatSpecifiers;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    //private NumberFormatSpecifier[] _numberFormatSpecifier1 = null;
    //public NumberFormatSpecifier[] numberFormatSpecifier1
    //{
    //    get
    //    {
    //        if (_numberFormatSpecifier1 == null)
    //        {
    //            var numberFormatSpecifier1 = new System.Collections.Generic.List<NumberFormatSpecifier>();

    //            numberFormatSpecifier1.Add(numberFormatSpecifier2);
    //            _numberFormatSpecifier1 = numberFormatSpecifier1.ToArray();
    //        }
    //        return _numberFormatSpecifier1;
    //    }
    //}
    //private NumberFormatSpecifier[] _numberFormatSpecifier3 = null;
    //public NumberFormatSpecifier[] numberFormatSpecifier3
    //{
    //    get
    //    {
    //        if (_numberFormatSpecifier3 == null)
    //        {
    //            var numberFormatSpecifier3 = new System.Collections.Generic.List<NumberFormatSpecifier>();

    //            numberFormatSpecifier3.Add(numberFormatSpecifier4);
    //            _numberFormatSpecifier3 = numberFormatSpecifier3.ToArray();
    //        }
    //        return _numberFormatSpecifier3;
    //    }
    //}
    //private NumberFormatSpecifier[] _numberFormatSpecifier5 = null;
    //public NumberFormatSpecifier[] numberFormatSpecifier5
    //{
    //    get
    //    {
    //        if (_numberFormatSpecifier5 == null)
    //        {
    //            var numberFormatSpecifier5 = new System.Collections.Generic.List<NumberFormatSpecifier>();

    //            numberFormatSpecifier5.Add(numberFormatSpecifier6);
    //            _numberFormatSpecifier5 = numberFormatSpecifier5.ToArray();
    //        }
    //        return _numberFormatSpecifier5;
    //    }
    //}

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

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
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
