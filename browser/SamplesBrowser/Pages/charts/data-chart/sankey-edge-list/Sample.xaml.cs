namespace SamplesBrowser.Pages.Charts.DataChart.SankeyEdgeList;

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

    private SankeyIncomeStages _sankeyIncomeStages = null;
    public SankeyIncomeStages SankeyIncomeStages
    {
        get
        {
            if (_sankeyIncomeStages == null)
            {
                _sankeyIncomeStages = new SankeyIncomeStages();
            }
            return _sankeyIncomeStages;
        }
    }

    private SankeyIncomeFlow _sankeyIncomeFlow = null;
    public SankeyIncomeFlow SankeyIncomeFlow
    {
        get
        {
            if (_sankeyIncomeFlow == null)
            {
                _sankeyIncomeFlow = new SankeyIncomeFlow();
            }
            return _sankeyIncomeFlow;
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
