namespace SamplesBrowser.Pages.Charts.DataChart.VerticalChartCombined;

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

    private RegionalSalesTargets _regionalSalesTargets = null;
    public RegionalSalesTargets RegionalSalesTargets
    {
        get
        {
            if (_regionalSalesTargets == null)
            {
                _regionalSalesTargets = new RegionalSalesTargets();
            }
            return _regionalSalesTargets;
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
