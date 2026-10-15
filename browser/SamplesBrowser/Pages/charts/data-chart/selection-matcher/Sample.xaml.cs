namespace SamplesBrowser.Pages.Charts.DataChart.SelectionMatcher;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Controls;
using Infragistics.Controls.Charts;
using System.Collections;
using System.Threading;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.SelectionMatcherOnViewInit();
        };
    }

    private EnergyRenewableConsumption _energyRenewableConsumption = null;
    public EnergyRenewableConsumption EnergyRenewableConsumption
    {
        get
        {
            if (_energyRenewableConsumption == null)
            {
                _energyRenewableConsumption = new EnergyRenewableConsumption();
            }
            return _energyRenewableConsumption;
        }
    }

    private Timer _timer;

    public void SelectionMatcherOnViewInit()
    {
    	// The timer fires on a pool thread; the chart can only be touched from the UI thread.
    	var dispatcher = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
    	_timer = new Timer((state) =>
    	{
    		dispatcher.TryEnqueue(() => AddSelection());
    		_timer.Dispose();
    	}, null, 100, Timeout.Infinite);
    }

    private void AddSelection()
    {
    	var chart = this.chart;
    	// Shared by the category chart and data chart samples; only the category chart has ItemsSource.
    	var data = (IList)((object)chart is XamCategoryChart categoryChart
    		? categoryChart.ItemsSource
    		: ((XamDataChart)(object)chart).Series[0].ItemsSource);

    	ChartSelection selection = new ChartSelection();
    	selection.Item = data[1];
    	SeriesMatcher matcher = new SeriesMatcher();
    	matcher.MemberPath = "Hydro";
    	matcher.MemberPathType = "ValueMemberPath";
    	selection.Matcher = matcher;

    	chart.SelectedSeriesItems.Add(selection);

    	SeriesMatcher matcher2 = new SeriesMatcher();
    	ChartSelection selection2 = new ChartSelection();
    	selection2 = new ChartSelection();
    	selection2.Item = data[1];
    	matcher2.MemberPath = "Wind";
    	matcher2.MemberPathType = "ValueMemberPath";

    	selection2.Matcher = matcher2;

    	chart.SelectedSeriesItems.Add(selection2);
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
