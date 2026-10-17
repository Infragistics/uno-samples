using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Description;
using Infragistics.Controls.Charts;
using System;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "3", "5", "7", "10", "12", "15", "20", "25", "30" };
    public string[] dropDownValues1 { get; } = new string[] { "3", "5", "7", "10", "12", "15", "20", "25", "30" };
    public string[] dropDownValues2 { get; } = new string[] { "Circle", "Checkmark", "Triangle", "Pyramid", "Square", "Diamond", "Pentagon", "Hexagon", "Tetragram", "Pentagram", "Hexagram", "Hidden", "Automatic", "None" };
    public string[] dropDownNames2 { get; } = new string[] { "Circle", "Checkmark", "Triangle", "Pyramid", "Square", "Diamond", "Pentagon", "Hexagon", "Tetragram", "Pentagram", "Hexagram", "Hidden", "Automatic", "None" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private CountryDemographicEurope _countryDemographicEurope = null;
    public CountryDemographicEurope CountryDemographicEurope
    {
        get
        {
            if (_countryDemographicEurope == null)
            {
                _countryDemographicEurope = new CountryDemographicEurope();
            }
            return _countryDemographicEurope;
        }
    }

    private CountryDemographicAfrican _countryDemographicAfrican = null;
    public CountryDemographicAfrican CountryDemographicAfrican
    {
        get
        {
            if (_countryDemographicAfrican == null)
            {
                _countryDemographicAfrican = new CountryDemographicAfrican();
            }
            return _countryDemographicAfrican;
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
                DataChartCoreDescriptionModule.Register(context);
                DataChartScatterDescriptionModule.Register(context);
                DataChartScatterCoreDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
                DataChartAnnotationDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void EditorChangeUpdateDataChartMarkerSize(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var chart = this.chart;
        var markerSizeVal = Convert.ToInt32(args.NewValue);
        var series = chart.Series;
        for (var i = 0; i < series.Count; i++)
        {
            var markerSeries = series[i] as MarkerSeries;
            if (markerSeries != null)
            {
                markerSeries.MarkerSize = markerSizeVal;
            }
        }
    }

    public void EditorChangeUpdateDataChartMarkerType(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var chart = this.chart;
        var markerTypeVal = (MarkerType)Enum.Parse(typeof(MarkerType), args.NewValue.ToString());
        var series = chart.Series;
        for (var i = 0; i < series.Count; i++)
        {
            var markerSeries = series[i] as MarkerSeries;
            if (markerSeries != null)
            {
                markerSeries.MarkerType = markerTypeVal;
            }
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

