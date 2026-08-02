using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;
using System;

namespace SamplesBrowser.Pages.Charts.CategoryChart.ValueLines;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownValues1 { get; } = new string[] { "Auto", "Average", "GlobalAverage", "GlobalMaximum", "GlobalMinimum", "Maximum", "Minimum" };
    public string[] dropDownNames1 { get; } = new string[] { "Auto", "Average", "GlobalAverage", "GlobalMaximum", "GlobalMinimum", "Maximum", "Minimum" };
    public string[] includedProperties1 { get; } = new string[] { "Year", "America", "Europe" };
    public string[] valueLines1 { get; } = new string[] {  };

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
    public void EditorChangeUpdateValueLines(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var item = (PropertyEditorPropertyDescription)sender;
        var value = (string)item.PrimitiveValue;
        var chart = this.chart;

        var valueLineType = (ValueLayerValueMode)Enum.Parse(typeof(ValueLayerValueMode), value);
        chart.ValueLines.Clear();
        chart.ValueLines.Add(valueLineType);
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
