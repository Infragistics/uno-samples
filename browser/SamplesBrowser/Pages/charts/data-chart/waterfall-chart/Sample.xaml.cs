namespace SamplesBrowser.Pages.Charts.DataChart.WaterfallChart;

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

    private CompanyIncomeData _companyIncomeData = null;
    public CompanyIncomeData CompanyIncomeData
    {
        get
        {
            if (_companyIncomeData == null)
            {
                _companyIncomeData = new CompanyIncomeData();
            }
            return _companyIncomeData;
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
