using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.DashboardTile.PieDashboard;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private EnergyGlobalDemand _energyGlobalDemand = null;
    public EnergyGlobalDemand EnergyGlobalDemand
    {
        get
        {
            if (_energyGlobalDemand == null)
            {
                _energyGlobalDemand = new EnergyGlobalDemand();
            }
            return _energyGlobalDemand;
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
