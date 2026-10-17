using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Description;
using Infragistics.Controls.Charts;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Stacked Column Series", "Stacked 100 Column Series", "Stacked Area Series", "Stacked 100 Area Series", "Stacked Line Series", "Stacked 100 Line Series", "Stacked Spline Series", "Stacked 100 Spline Series", "Stacked Bar Series", "Stacked 100 Bar Series" };
    public string[] dropDownValues1 { get; } = new string[] { "Stacked Column Series", "Stacked 100 Column Series", "Stacked Area Series", "Stacked 100 Area Series", "Stacked Line Series", "Stacked 100 Line Series", "Stacked Spline Series", "Stacked 100 Spline Series", "Stacked Bar Series", "Stacked 100 Bar Series" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private EnergyProductionByCountry _energyProductionByCountry = null;
    public EnergyProductionByCountry EnergyProductionByCountry
    {
        get
        {
            if (_energyProductionByCountry == null)
            {
                _energyProductionByCountry = new EnergyProductionByCountry();
            }
            return _energyProductionByCountry;
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
                PropertyEditorPanelDescriptionModule.Register(context);
                DataChartCoreDescriptionModule.Register(context);
                DataChartCategoryDescriptionModule.Register(context);
                DataChartCategoryCoreDescriptionModule.Register(context);
                DataChartStackedDescriptionModule.Register(context);
                StackedFragmentSeriesDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
                LegendDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    // Every stacked type carries the same five fragments, one per fuel, so they are built once and
    // handed to whichever stack the reader picks.
    private StackedFragmentSeries[] Fragments()
    {
        var fuels = new string[] { "Coal", "Hydro", "Nuclear", "Gas", "Oil" };
        var made = new StackedFragmentSeries[fuels.Length];
        for (var i = 0; i < fuels.Length; i++)
        {
            var fragment = new StackedFragmentSeries();
            fragment.ValueMemberPath = fuels[i];
            fragment.Title = fuels[i];
            made[i] = fragment;
        }
        return made;
    }

    private StackedSeriesBase Make(string name)
    {
        switch (name)
        {
            case "Stacked Area Series": return new StackedAreaSeries();
            case "Stacked 100 Area Series": return new Stacked100AreaSeries();
            case "Stacked Bar Series": return new StackedBarSeries();
            case "Stacked 100 Bar Series": return new Stacked100BarSeries();
            case "Stacked 100 Column Series": return new Stacked100ColumnSeries();
            case "Stacked Line Series": return new StackedLineSeries();
            case "Stacked 100 Line Series": return new Stacked100LineSeries();
            case "Stacked Spline Series": return new StackedSplineSeries();
            case "Stacked 100 Spline Series": return new Stacked100SplineSeries();
        }
        return new StackedColumnSeries();
    }

    public void EditorChangeStackedSeriesType(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var chart = this.chart;
        var name = args.NewValue.ToString();

        // A bar stack grows along the other axis, so it reads the category down the side and the
        // numbers across the bottom; every other type is the other way round. Both pairs of axes
        // are declared on the chart, and the swap is which pair this stack is given.
        var bar = name.IndexOf("Bar") >= 0;
        var stack = Make(name);
        // Which axis is the category one separates the two stacked bases, so each is reached through
        // the base that declares the pair it has rather than through the one they share.
        if (bar)
        {
            var vertical = (VerticalStackedSeriesBase)stack;
            vertical.XAxis = this.numXAxis;
            vertical.YAxis = this.catYAxis;
        }
        else
        {
            var horizontal = (HorizontalStackedSeriesBase)stack;
            horizontal.XAxis = this.catXAxis;
            horizontal.YAxis = this.numYAxis;
        }

        foreach (var fragment in Fragments())
        {
            stack.Series.Add(fragment);
        }

        chart.Series.Clear();
        chart.Series.Add(stack);
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

