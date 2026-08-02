using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] dropDownValues1 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] dropDownNames2 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] dropDownValues2 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] dropDownNames3 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] dropDownValues3 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] dropDownNames4 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] dropDownValues4 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] dropDownNames5 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] dropDownValues5 { get; } = new string[] { "gray", "darkslategray", "salmon", "cornflowerblue", "darkgreen" };
    public string[] includedProperties1 { get; } = new string[] { "Year", "Europe", "China", "America" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private CountryRenewableElectricity _countryRenewableElectricity = null;
    public CountryRenewableElectricity CountryRenewableElectricity
    {
        get
        {
            if (_countryRenewableElectricity == null)
            {
                _countryRenewableElectricity = new CountryRenewableElectricity();
            }
            return _countryRenewableElectricity;
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
                LegendDescriptionModule.Register(context);
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
