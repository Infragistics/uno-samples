using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private CountryStatsAfrica _countryStatsAfrica = null;
    public CountryStatsAfrica CountryStatsAfrica
    {
        get
        {
            if (_countryStatsAfrica == null)
            {
                _countryStatsAfrica = new CountryStatsAfrica();
            }
            return _countryStatsAfrica;
        }
    }

    private CountryStatsEurope _countryStatsEurope = null;
    public CountryStatsEurope CountryStatsEurope
    {
        get
        {
            if (_countryStatsEurope == null)
            {
                _countryStatsEurope = new CountryStatsEurope();
            }
            return _countryStatsEurope;
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
