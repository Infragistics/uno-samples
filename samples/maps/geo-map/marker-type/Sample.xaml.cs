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

    private WorldCities _worldCities = null;
    public WorldCities WorldCities
    {
        get
        {
            if (_worldCities == null)
            {
                _worldCities = new WorldCities();
            }
            return _worldCities;
        }
    }

    private WorldCapitals _worldCapitals = null;
    public WorldCapitals WorldCapitals
    {
        get
        {
            if (_worldCapitals == null)
            {
                _worldCapitals = new WorldCapitals();
            }
            return _worldCapitals;
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
