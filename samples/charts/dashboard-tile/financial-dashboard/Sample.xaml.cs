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

    private MultipleStocks _multipleStocks = null;
    public MultipleStocks MultipleStocks
    {
        get
        {
            if (_multipleStocks == null)
            {
                MultipleStocks.Fetch().ContinueWith((t) => {_multipleStocks = t.Result;  OnPropertyChanged("MultipleStocks"); }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
            }
            return _multipleStocks;
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
