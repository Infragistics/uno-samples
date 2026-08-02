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

    private DashboardGaugeDataSource _dashboardGaugeDataSource = null;
    public DashboardGaugeDataSource DashboardGaugeDataSource
    {
        get
        {
            if (_dashboardGaugeDataSource == null)
            {
                _dashboardGaugeDataSource = new DashboardGaugeDataSource();
            }
            return _dashboardGaugeDataSource;
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
