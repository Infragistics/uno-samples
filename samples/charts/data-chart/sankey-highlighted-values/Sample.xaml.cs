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

    private SankeyBudgetStages _sankeyBudgetStages = null;
    public SankeyBudgetStages SankeyBudgetStages
    {
        get
        {
            if (_sankeyBudgetStages == null)
            {
                _sankeyBudgetStages = new SankeyBudgetStages();
            }
            return _sankeyBudgetStages;
        }
    }

    private SankeyBudgetNodes _sankeyBudgetNodes = null;
    public SankeyBudgetNodes SankeyBudgetNodes
    {
        get
        {
            if (_sankeyBudgetNodes == null)
            {
                _sankeyBudgetNodes = new SankeyBudgetNodes();
            }
            return _sankeyBudgetNodes;
        }
    }

    private SankeyBudgetLinks _sankeyBudgetLinks = null;
    public SankeyBudgetLinks SankeyBudgetLinks
    {
        get
        {
            if (_sankeyBudgetLinks == null)
            {
                _sankeyBudgetLinks = new SankeyBudgetLinks();
            }
            return _sankeyBudgetLinks;
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

