namespace SamplesBrowser.Pages.Charts.DataChart.ScatterPointChart;

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
