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

    private MonthlyRainfallByCity _monthlyRainfallByCity = null;
    public MonthlyRainfallByCity MonthlyRainfallByCity
    {
        get
        {
            if (_monthlyRainfallByCity == null)
            {
                _monthlyRainfallByCity = new MonthlyRainfallByCity();
            }
            return _monthlyRainfallByCity;
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

