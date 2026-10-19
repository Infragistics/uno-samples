namespace SamplesBrowser.Pages.Charts.DataChart.VerticalSplineChart;

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

    private AtmosphereTemperatureByAltitude _atmosphereTemperatureByAltitude = null;
    public AtmosphereTemperatureByAltitude AtmosphereTemperatureByAltitude
    {
        get
        {
            if (_atmosphereTemperatureByAltitude == null)
            {
                _atmosphereTemperatureByAltitude = new AtmosphereTemperatureByAltitude();
            }
            return _atmosphereTemperatureByAltitude;
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
