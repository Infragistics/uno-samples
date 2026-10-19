namespace SamplesBrowser.Pages.Charts.DataChart.VerticalLineChart;

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

    private OceanTemperatureByDepth _oceanTemperatureByDepth = null;
    public OceanTemperatureByDepth OceanTemperatureByDepth
    {
        get
        {
            if (_oceanTemperatureByDepth == null)
            {
                _oceanTemperatureByDepth = new OceanTemperatureByDepth();
            }
            return _oceanTemperatureByDepth;
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
