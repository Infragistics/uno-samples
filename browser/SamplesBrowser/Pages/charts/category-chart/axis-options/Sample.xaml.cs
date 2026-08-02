using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.CategoryChart.AxisOptions;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private OlympicMedalsTopCountries _olympicMedalsTopCountries = null;
    public OlympicMedalsTopCountries OlympicMedalsTopCountries
    {
        get
        {
            if (_olympicMedalsTopCountries == null)
            {
                _olympicMedalsTopCountries = new OlympicMedalsTopCountries();
            }
            return _olympicMedalsTopCountries;
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
