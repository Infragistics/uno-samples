using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.DataChart.BarChartStyling;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] allowedPositions1 { get; } = new string[] {  };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private OnlineShoppingSearches _onlineShoppingSearches = null;
    public OnlineShoppingSearches OnlineShoppingSearches
    {
        get
        {
            if (_onlineShoppingSearches == null)
            {
                _onlineShoppingSearches = new OnlineShoppingSearches();
            }
            return _onlineShoppingSearches;
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
