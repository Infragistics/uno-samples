namespace SamplesBrowser.Pages.Charts.DataChart.VerticalWaterfallChart;

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

    private ProfitBridge _profitBridge = null;
    public ProfitBridge ProfitBridge
    {
        get
        {
            if (_profitBridge == null)
            {
                _profitBridge = new ProfitBridge();
            }
            return _profitBridge;
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
