namespace SamplesBrowser.Pages.Charts.CategoryChart.DataAggregations;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls;
using Infragistics.Controls.Charts;
using Infragistics.Controls.Description;
using System;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Country", "Product", "Month", "Year" };
    public string[] dropDownValues1 { get; } = new string[] { "Country", "Product", "Month", "Year" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.PropertyEditorInitAggregationsOnViewInit();
        };
    }

    private SalesData _salesData = null;
    public SalesData SalesData
    {
        get
        {
            if (_salesData == null)
            {
                _salesData = new SalesData();
            }
            return _salesData;
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
                LegendDescriptionModule.Register(context);
                CategoryChartDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void PropertyEditorInitAggregationsOnViewInit()
    {
    	var editor = this.editor;

    	var initialSummariesDropdown = new PropertyEditorPropertyDescription();
    	var sortGroupsDropdown = new PropertyEditorPropertyDescription();
    	initialSummariesDropdown.Label = "Initial Summaries";
    	initialSummariesDropdown.ValueType = PropertyEditorValueType.EnumValue;
    	initialSummariesDropdown.ShouldOverrideDefaultEditor = true;
    	initialSummariesDropdown.DropDownNames = new string[] { "Sum(Sales) as Sales", "Avg(Sales) as Sales", "Min(Sales) as Sales", "Max(Sales) as Sales", "Count(Sales) as Sales" };
    	initialSummariesDropdown.DropDownValues = new string[] { "Sum(Sales) as Sales", "Avg(Sales) as Sales", "Min(Sales) as Sales", "Max(Sales) as Sales", "Count(Sales) as Sales" };
    	sortGroupsDropdown.Label = "Sort Groups";
    	sortGroupsDropdown.ValueType = PropertyEditorValueType.EnumValue;
    	sortGroupsDropdown.ShouldOverrideDefaultEditor = true;
    	sortGroupsDropdown.DropDownNames = new string[] { "Sales Asc", "Sales Desc" };
    	sortGroupsDropdown.DropDownValues = new string[] { "Sales Asc", "Sales Desc" };

    	editor.Properties.Add(initialSummariesDropdown);
    	editor.Properties.Add(sortGroupsDropdown);

    	initialSummariesDropdown.Changed += this.EditorChangeUpdateInitialSummaries;
    	sortGroupsDropdown.Changed += this.EditorChangeUpdateGroupSorts;
    }

    public void EditorChangeUpdateInitialSummaries(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
    	var chart = this.chart;
    	var intialSummaryVal = args.NewValue.ToString();
    	chart.InitialSummaries = intialSummaryVal;
    }

    public void EditorChangeUpdateGroupSorts(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
    	var chart = this.chart;
    	var groupSortsVal = args.NewValue.ToString();
    	chart.GroupSorts = groupSortsVal;
    }

    public void EditorChangeUpdateInitialGroups(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var chart = this.chart;
        var intialGroupVal = args.NewValue.ToString();
        chart.InitialGroups = null;
        chart.InitialGroups = intialGroupVal;
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
