namespace SamplesBrowser.Pages.Charts.DoughnutChart.Selection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;
using System.Collections;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.DoughnutChartSelectionOnViewInit();
        };
    }

    private EnergyGlobalDemand _energyGlobalDemand = null;
    public EnergyGlobalDemand EnergyGlobalDemand
    {
        get
        {
            if (_energyGlobalDemand == null)
            {
                _energyGlobalDemand = new EnergyGlobalDemand();
            }
            return _energyGlobalDemand;
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
                ItemLegendDescriptionModule.Register(context);
                DoughnutChartDescriptionModule.Register(context);
                RingSeriesDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void DoughnutChartSelectionOnViewInit()
    {
        // The readout opens with the first slice's figures, so it has something to say before the
        // reader clicks. It is not selected as well, the way the web platforms select it: a ring
        // series has no slice selection to ask for here -- the collection is on the chart and holds
        // slices rather than their indexes -- so what the topic teaches, the click and the styling
        // it applies, is what this shows. The rest waits for the API to match.
        var series = this.ringSeries;
        DoughnutChartSelectionReadout.Show(
            series,
            this.SelectedSlice,
            0);
    }

    public void DoughnutChartSelectionSliceClick(object sender, SliceClickEventArgs e)
    {
        // A click that selects reports which slice; a click that clears the selection reports that.
        DoughnutChartSelectionReadout.Show(
            this.ringSeries,
            this.SelectedSlice,
            e.IsSelected ? e.Index : -1);
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

// What the sample reports about the selection, shared by the two entry points: the one that selects
// a slice to start with, and the one that answers a click. Both write the same readout, so the text
// is written in one place.
//
// The series and the field are handed in rather than looked up here. Asking for a description
// expands, where the sample is generated, to the field the component was assigned to -- which only
// means anything inside the component's own instance, so the entry points do the asking.
public static class DoughnutChartSelectionReadout
{
    public static void Show(RingSeries series, PropertyEditorPropertyDescription field, int index)
    {
        var data = series.ItemsSource as IList;
        if (index < 0 || data == null || index >= data.Count)
        {
            field.PrimitiveValue = "No Selection";
            return;
        }
        var item = (EnergyGlobalDemandItem)data[index];
        field.PrimitiveValue = item.Category + " — " + item.Value + "%";
    }
}
