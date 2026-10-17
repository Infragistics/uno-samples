using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Auto", "Brighten", "BrightenSpecific", "FadeOthers", "FadeOthersSpecific", "None" };
    public string[] dropDownValues1 { get; } = new string[] { "Auto", "Brighten", "BrightenSpecific", "FadeOthers", "FadeOthersSpecific", "None" };
    public string[] dropDownNames2 { get; } = new string[] { "Auto", "DirectlyOver", "NearestItems", "NearestItemsAndSeries", "NearestItemsRetainMainShapes" };
    public string[] dropDownValues2 { get; } = new string[] { "Auto", "DirectlyOver", "NearestItems", "NearestItemsAndSeries", "NearestItemsRetainMainShapes" };
    public string[] dropDownNames3 { get; } = new string[] { "Auto", "MatchSeries", "None" };
    public string[] dropDownValues3 { get; } = new string[] { "Auto", "MatchSeries", "None" };
    public string[] includedProperties1 { get; } = new string[] { "Month", "HighNY", "HighLA" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private TemperatureRangeData _temperatureRangeData = null;
    public TemperatureRangeData TemperatureRangeData
    {
        get
        {
            if (_temperatureRangeData == null)
            {
                _temperatureRangeData = new TemperatureRangeData();
            }
            return _temperatureRangeData;
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
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

