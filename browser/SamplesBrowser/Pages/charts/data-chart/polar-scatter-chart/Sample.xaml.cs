namespace SamplesBrowser.Pages.Charts.DataChart.PolarScatterChart;

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

    private BoatSailingData _boatSailingData = null;
    public BoatSailingData BoatSailingData
    {
        get
        {
            if (_boatSailingData == null)
            {
                _boatSailingData = new BoatSailingData();
            }
            return _boatSailingData;
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
