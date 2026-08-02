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

    private FinancialDataService _financialDataService = null;
    public FinancialDataService FinancialDataService
    {
        get
        {
            if (_financialDataService == null)
            {
                FinancialDataService.Fetch().ContinueWith((t) => {_financialDataService = t.Result;  OnPropertyChanged("FinancialDataService"); }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
            }
            return _financialDataService;
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
