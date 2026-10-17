namespace SamplesBrowser.Pages.Charts.CategoryChart.LineChartWithAnnotations;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Both", "Horizontal", "Vertical", "None" };
    public string[] dropDownValues1 { get; } = new string[] { "Both", "Horizontal", "Vertical", "None" };
    public string[] includedProperties1 { get; } = new string[] { "Year", "America" };
    public MarkerTypeCollection markerTypes1 { get; } = new MarkerTypeCollection { MarkerType.Circle };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private EnergyRenewableGeneration _energyRenewableGeneration = null;
    public EnergyRenewableGeneration EnergyRenewableGeneration
    {
        get
        {
            if (_energyRenewableGeneration == null)
            {
                _energyRenewableGeneration = new EnergyRenewableGeneration();
            }
            return _energyRenewableGeneration;
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
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
