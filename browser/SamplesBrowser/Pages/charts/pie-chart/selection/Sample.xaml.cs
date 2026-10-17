namespace SamplesBrowser.Pages.Charts.PieChart.Selection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Controls.Charts;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private LocalData _localData = null;
    public LocalData LocalData
    {
        get
        {
            if (_localData == null)
            {
                _localData = new LocalData();
            }
            return _localData;
        }
    }

    public void PieChartSelectionSliceClick(object sender, SliceClickEventArgs e)
    {
        // Selecting a slice also moves it out of the pie, so that the selection the chart is
        // tracking is visible from the shape as well as from the styling.
        e.IsExploded = !e.IsExploded;
        e.IsSelected = true;
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
