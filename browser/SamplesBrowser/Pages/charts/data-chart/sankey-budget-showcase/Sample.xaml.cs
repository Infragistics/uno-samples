namespace SamplesBrowser.Pages.Charts.DataChart.SankeyBudgetShowcase;

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

    private SankeyHouseholdStages _sankeyHouseholdStages = null;
    public SankeyHouseholdStages SankeyHouseholdStages
    {
        get
        {
            if (_sankeyHouseholdStages == null)
            {
                _sankeyHouseholdStages = new SankeyHouseholdStages();
            }
            return _sankeyHouseholdStages;
        }
    }

    private SankeyHouseholdNodes _sankeyHouseholdNodes = null;
    public SankeyHouseholdNodes SankeyHouseholdNodes
    {
        get
        {
            if (_sankeyHouseholdNodes == null)
            {
                _sankeyHouseholdNodes = new SankeyHouseholdNodes();
            }
            return _sankeyHouseholdNodes;
        }
    }

    private SankeyHouseholdLinks _sankeyHouseholdLinks = null;
    public SankeyHouseholdLinks SankeyHouseholdLinks
    {
        get
        {
            if (_sankeyHouseholdLinks == null)
            {
                _sankeyHouseholdLinks = new SankeyHouseholdLinks();
            }
            return _sankeyHouseholdLinks;
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
