using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.Sparkline.DisplayWinloss;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private SparklineMixedData _sparklineMixedData = null;
    public SparklineMixedData SparklineMixedData
    {
        get
        {
            if (_sparklineMixedData == null)
            {
                _sparklineMixedData = new SparklineMixedData();
            }
            return _sparklineMixedData;
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
