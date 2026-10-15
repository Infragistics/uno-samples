using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Charts;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public IndexCollection explodedSlices1 { get; } = new IndexCollection { 1, 2 };

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

    public void PieChartExplosionSliceClick(object sender, SliceClickEventArgs e)
    {
        // Explosion is state the chart keeps for each slice, so a click toggles what the slice
        // already has rather than assigning the chart's exploded list. Selection is cleared so
        // that moving out is the only thing the click does.
        e.IsExploded = !e.IsExploded;
        e.IsSelected = false;
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

