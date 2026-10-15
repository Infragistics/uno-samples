namespace SamplesBrowser.Pages.Charts.CategoryChart.MarkerOptions;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;
using Infragistics.Controls.Description;
using System;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownValues1 { get; } = new string[] { "Circle", "Checkmark", "Automatic", "Triangle", "Pyramid", "Square", "Diamond", "Pentagon", "Hexagon", "Tetragram", "Pentagram", "Hexagram", "None" };
    public string[] dropDownNames1 { get; } = new string[] { "Circle", "Checkmark", "Automatic", "Triangle", "Pyramid", "Square", "Diamond", "Pentagon", "Hexagon", "Tetragram", "Pentagram", "Hexagram", "None" };
    public MarkerTypeCollection markerTypes1 { get; } = new MarkerTypeCollection { MarkerType.Circle };

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
                CategoryChartDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void EditorChangeUpdateMarkerType(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var item = (PropertyEditorPropertyDescription)sender;
        var value = (string)item.PrimitiveValue;
        var chart = this.chart;

        var markerVal = (MarkerType)Enum.Parse(typeof(MarkerType), value);
        chart.MarkerTypes.Clear();
        chart.MarkerTypes.Add(markerVal);
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
