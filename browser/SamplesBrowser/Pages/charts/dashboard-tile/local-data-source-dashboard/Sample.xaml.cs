using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.DashboardTile.LocalDataSourceDashboard;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private RetailSalesPerformanceLocalDataSource _retailSalesPerformanceLocalDataSource = null;
    public RetailSalesPerformanceLocalDataSource RetailSalesPerformanceLocalDataSource
    {
        get
        {
            if (_retailSalesPerformanceLocalDataSource == null)
            {
                _retailSalesPerformanceLocalDataSource = new RetailSalesPerformanceLocalDataSource();
            }
            return _retailSalesPerformanceLocalDataSource;
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
