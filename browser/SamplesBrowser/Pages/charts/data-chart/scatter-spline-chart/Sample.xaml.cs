using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.DataChart.ScatterSplineChart;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private HealthDataForGermany _healthDataForGermany = null;
    public HealthDataForGermany HealthDataForGermany
    {
        get
        {
            if (_healthDataForGermany == null)
            {
                _healthDataForGermany = new HealthDataForGermany();
            }
            return _healthDataForGermany;
        }
    }

    private HealthDataForFrance _healthDataForFrance = null;
    public HealthDataForFrance HealthDataForFrance
    {
        get
        {
            if (_healthDataForFrance == null)
            {
                _healthDataForFrance = new HealthDataForFrance();
            }
            return _healthDataForFrance;
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
