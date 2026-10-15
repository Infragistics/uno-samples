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

    private CompanyIncomeStatement _companyIncomeStatement = null;
    public CompanyIncomeStatement CompanyIncomeStatement
    {
        get
        {
            if (_companyIncomeStatement == null)
            {
                _companyIncomeStatement = new CompanyIncomeStatement();
            }
            return _companyIncomeStatement;
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
                DataChartCoreDescriptionModule.Register(context);
                DataChartCategoryCoreDescriptionModule.Register(context);
                DataChartCategoryDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
                DataChartAnnotationDescriptionModule.Register(context);
                CalloutLayerDescriptionModule.Register(context);
                NumberAbbreviatorDescriptionModule.Register(context);
                LegendDescriptionModule.Register(context);
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

