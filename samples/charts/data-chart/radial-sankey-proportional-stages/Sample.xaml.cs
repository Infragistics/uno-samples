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

    private SankeyIncomeStageWeights _sankeyIncomeStageWeights = null;
    public SankeyIncomeStageWeights SankeyIncomeStageWeights
    {
        get
        {
            if (_sankeyIncomeStageWeights == null)
            {
                _sankeyIncomeStageWeights = new SankeyIncomeStageWeights();
            }
            return _sankeyIncomeStageWeights;
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

