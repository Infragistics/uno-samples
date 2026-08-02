using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Maps.GeoMap.GeoSymbolMapWithCallouts;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private WorldCapitals2M _worldCapitals2M = null;
    public WorldCapitals2M WorldCapitals2M
    {
        get
        {
            if (_worldCapitals2M == null)
            {
                _worldCapitals2M = new WorldCapitals2M();
            }
            return _worldCapitals2M;
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
