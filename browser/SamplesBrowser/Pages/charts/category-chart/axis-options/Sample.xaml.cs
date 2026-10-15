namespace SamplesBrowser.Pages.Charts.CategoryChart.AxisOptions;

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
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
