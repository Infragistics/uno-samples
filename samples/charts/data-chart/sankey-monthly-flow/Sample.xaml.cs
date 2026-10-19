using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Description;
using Infragistics.Controls.Charts;
using System;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "July", "August", "September" };
    public string[] dropDownValues1 { get; } = new string[] { "July", "August", "September" };

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

    private SankeyBudgetMonths _sankeyBudgetMonths = null;
    public SankeyBudgetMonths SankeyBudgetMonths
    {
        get
        {
            if (_sankeyBudgetMonths == null)
            {
                _sankeyBudgetMonths = new SankeyBudgetMonths();
            }
            return _sankeyBudgetMonths;
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
                DataChartCategoryCoreDescriptionModule.Register(context);
                DataChartFlowDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
                DataChartAnnotationDescriptionModule.Register(context);
                AnnotationLayerProxyDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void EditorChangeSankeyMonth(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var chart = this.chart;
        var month = args.NewValue.ToString();
        var series = chart.Series;
        for (var i = 0; i < series.Count; i++)
        {
            var sankey = series[i] as SankeySeries;
            if (sankey != null)
            {
                // The links carry one value column per month; reading another column animates the
                // flows to that month's values (ShouldAnimateOnDataSourceSwap + TransitionDuration).
                sankey.LinkValueMemberPath = month;
            }
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

