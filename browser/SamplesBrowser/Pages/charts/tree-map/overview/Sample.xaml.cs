using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.TreeMap.Overview;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private CountyHierarchicalData _countyHierarchicalData = null;
    public CountyHierarchicalData CountyHierarchicalData
    {
        get
        {
            if (_countyHierarchicalData == null)
            {
                _countyHierarchicalData = new CountyHierarchicalData();
            }
            return _countyHierarchicalData;
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
