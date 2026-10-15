namespace SamplesBrowser.Pages.Charts.DataChart.RangeColumnChart;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

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
