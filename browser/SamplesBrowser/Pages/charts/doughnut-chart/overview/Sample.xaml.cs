namespace SamplesBrowser.Pages.Charts.DoughnutChart.Overview;

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

    private CompanyMarketShares _companyMarketShares = null;
    public CompanyMarketShares CompanyMarketShares
    {
        get
        {
            if (_companyMarketShares == null)
            {
                _companyMarketShares = new CompanyMarketShares();
            }
            return _companyMarketShares;
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
