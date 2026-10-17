namespace SamplesBrowser.Pages.Charts.DataChart.ScatterBubbleChartSingleSource;

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

    private WorldStats _worldStats = null;
    public WorldStats WorldStats
    {
        get
        {
            if (_worldStats == null)
            {
                _worldStats = new WorldStats();
            }
            return _worldStats;
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
