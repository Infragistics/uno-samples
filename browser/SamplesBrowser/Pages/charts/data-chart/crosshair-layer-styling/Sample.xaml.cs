namespace SamplesBrowser.Pages.Charts.DataChart.CrosshairLayerStyling;

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

    private EnergyRenewableGeneration _energyRenewableGeneration = null;
    public EnergyRenewableGeneration EnergyRenewableGeneration
    {
        get
        {
            if (_energyRenewableGeneration == null)
            {
                _energyRenewableGeneration = new EnergyRenewableGeneration();
            }
            return _energyRenewableGeneration;
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
