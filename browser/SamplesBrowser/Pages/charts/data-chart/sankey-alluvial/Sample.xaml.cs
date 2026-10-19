namespace SamplesBrowser.Pages.Charts.DataChart.SankeyAlluvial;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Controls.Charts;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dimensionMemberPaths1 { get; } = new string[] { "Region", "Product", "Channel" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {
            sankeyLayoutAdapter1.LinkItemsSource = this.SankeyOrderChannels;
            sankeyLayoutAdapter1.DimensionMemberPaths = this.dimensionMemberPaths1;

        };
    }

    private SankeyOrderStages _sankeyOrderStages = null;
    public SankeyOrderStages SankeyOrderStages
    {
        get
        {
            if (_sankeyOrderStages == null)
            {
                _sankeyOrderStages = new SankeyOrderStages();
            }
            return _sankeyOrderStages;
        }
    }

    private SankeyOrderChannels _sankeyOrderChannels = null;
    public SankeyOrderChannels SankeyOrderChannels
    {
        get
        {
            if (_sankeyOrderChannels == null)
            {
                _sankeyOrderChannels = new SankeyOrderChannels();
            }
            return _sankeyOrderChannels;
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
