using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;
using System;

namespace SamplesBrowser.Pages.Charts.CategoryChart.AxisRange;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" };
    public string[] dropDownValues1 { get; } = new string[] { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" };
    public string[] dropDownNames2 { get; } = new string[] { "100", "110", "120", "130", "140", "150", "160", "170", "180", "190", "200" };
    public string[] dropDownValues2 { get; } = new string[] { "100", "110", "120", "130", "140", "150", "160", "170", "180", "190", "200" };
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

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionChangedEventHandler
    public void EditorChangeUpdateYAxisMinimumValue(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var yAxisMinimumVal = args.NewValue;
        var chart = this.chart;
        chart.YAxisMinimumValue = Convert.ToDouble(yAxisMinimumVal);
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionChangedEventHandler
    public void EditorChangeUpdateYAxisMaximumValue(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var yAxisMaximumVal = args.NewValue;
        var chart = this.chart;
        chart.YAxisMaximumValue = Convert.ToDouble(yAxisMaximumVal);
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
