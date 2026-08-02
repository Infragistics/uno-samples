using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls;
using Infragistics.Controls.Charts;
using System.Collections;
using System.Threading;

namespace SamplesBrowser.Pages.Charts.CategoryChart.SelectionMatcher;

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

    private ComponentRenderer _componentRenderer = null;
    public ComponentRenderer Renderer
    {
        get
        {
            if (this._componentRenderer == null)
            {
                this._componentRenderer = ComponentRendererHelper.CreateRenderer();
                var context = this._componentRenderer.Context;
                LegendDescriptionModule.Register(context);
                CategoryChartDescriptionModule.Register(context);
                DataChartAnnotationDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
                DataChartCoreDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    private Timer _timer;

    //WPF: System.Action
    public void SelectionMatcherOnViewInit()
    {
    	_timer = new Timer((state) =>
    	{
    		AddSelection();
    		_timer.Dispose();
    	}, null, 100, Timeout.Infinite);
    }

    private void AddSelection()
    {
    	var chart = this.chart;

    	//ChartSelection selection = new ChartSelection();
    	//var s = chart.Series.Where(s => ((AnchoredCategorySeries)s).ValueMemberPath == "Hydro").FirstOrDefault();
    	//selection.Item = ((IList)s.ItemsSource)[1];
    	//SeriesMatcher matcher = new SeriesMatcher();
    	//matcher.MemberPath = "Hydro";
    	//matcher.MemberPathType = "ValueMemberPath";
    	//selection.Matcher = matcher;

    	//chart.SelectedSeriesItems.Add(selection);

    	//SeriesMatcher matcher2 = new SeriesMatcher();
    	//ChartSelection selection2 = new ChartSelection();
    	//selection2 = new ChartSelection();
    	//s = chart.Series.Where(s => ((AnchoredCategorySeries)s).ValueMemberPath == "Wind").FirstOrDefault();
    	//selection2.Item = ((IList)s.ItemsSource)[1];
    	//matcher2.MemberPath = "Wind";
    	//matcher2.MemberPathType = "ValueMemberPath";

    	//selection.Matcher = matcher2;

    	//chart.SelectedSeriesItems.Add(selection2);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
