using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;
using Infragistics.Controls.Description;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Auto", "AccordionFromBottom", "AccordionFromCategoryAxisMaximum", "AccordionFromCategoryAxisMinimum", "AccordionFromLeft", "AccordionFromRight", "AccordionFromTop", "AccordionFromValueAxisMaximum", "AccordionFromValueAxisMinimum", "Expand", "FromZero", "SweepFromBottom", "SweepFromCategoryAxisMaximum", "SweepFromCategoryAxisMinimum", "SweepFromCenter", "SweepFromLeft", "SweepFromRight", "SweepFromTop", "SweepFromValueAxisMaximum", "SweepFromValueAxisMinimum" };
    public string[] dropDownValues1 { get; } = new string[] { "Auto", "AccordionFromBottom", "AccordionFromCategoryAxisMaximum", "AccordionFromCategoryAxisMinimum", "AccordionFromLeft", "AccordionFromRight", "AccordionFromTop", "AccordionFromValueAxisMaximum", "AccordionFromValueAxisMinimum", "Expand", "FromZero", "SweepFromBottom", "SweepFromCategoryAxisMaximum", "SweepFromCategoryAxisMinimum", "SweepFromCenter", "SweepFromLeft", "SweepFromRight", "SweepFromTop", "SweepFromValueAxisMaximum", "SweepFromValueAxisMinimum" };
    public string[] includedProperties1 { get; } = new string[] { "Year", "Europe", "China", "America" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private EnergyRenewableGeneration _energyRenewableGeneration = null;
    public EnergyRenewableGeneration EnergyRenewableGeneration
    {
        get
        {
            if (_energyRenewableGeneration == null)
            {
                _energyRenewableGeneration = new EnergyRenewableGeneration();
            }
            return _energyRenewableGeneration;
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
                CategoryChartDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void EditorChangeReplayTransitionInDomain(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        // The editor has already written the new value onto the chart; replaying is what makes the
        // change visible, since a transition only shows while it is running.
        var chart = this.chart;
        chart.ReplayTransitionIn();
    }

    public void EditorButtonReplayTransitionInDomain(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var chart = this.chart;
        chart.ReplayTransitionIn();

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

