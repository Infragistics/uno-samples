namespace SamplesBrowser.Pages.Charts.DataChart.ChartSynchronization;

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

    private StockGoogle _stockGoogle = null;
    public StockGoogle StockGoogle
    {
        get
        {
            if (_stockGoogle == null)
            {
                _stockGoogle = new StockGoogle();
            }
            return _stockGoogle;
        }
    }

    private StockAmazon _stockAmazon = null;
    public StockAmazon StockAmazon
    {
        get
        {
            if (_stockAmazon == null)
            {
                _stockAmazon = new StockAmazon();
            }
            return _stockAmazon;
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
