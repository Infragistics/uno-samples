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

    private WorldFlights _worldFlights = null;
    public WorldFlights WorldFlights
    {
        get
        {
            if (_worldFlights == null)
            {
                _worldFlights = new WorldFlights();
            }
            return _worldFlights;
        }
    }

    private WorldGridlines _worldGridlines = null;
    public WorldGridlines WorldGridlines
    {
        get
        {
            if (_worldGridlines == null)
            {
                _worldGridlines = new WorldGridlines();
            }
            return _worldGridlines;
        }
    }

    private WorldAirports _worldAirports = null;
    public WorldAirports WorldAirports
    {
        get
        {
            if (_worldAirports == null)
            {
                _worldAirports = new WorldAirports();
            }
            return _worldAirports;
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

