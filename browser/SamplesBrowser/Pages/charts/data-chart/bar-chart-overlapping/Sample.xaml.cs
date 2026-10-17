namespace SamplesBrowser.Pages.Charts.DataChart.BarChartOverlapping;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Controls.Charts;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public CalloutPlacementPositionsCollection allowedPositions1 { get; } = new CalloutPlacementPositionsCollection { CalloutPlacementPositions.Right };
    public CalloutPlacementPositionsCollection allowedPositions2 { get; } = new CalloutPlacementPositionsCollection { CalloutPlacementPositions.Right };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private RoadblocksToSuccess _roadblocksToSuccess = null;
    public RoadblocksToSuccess RoadblocksToSuccess
    {
        get
        {
            if (_roadblocksToSuccess == null)
            {
                _roadblocksToSuccess = new RoadblocksToSuccess();
            }
            return _roadblocksToSuccess;
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
