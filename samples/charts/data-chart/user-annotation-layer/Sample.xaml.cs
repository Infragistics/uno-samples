using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml.Markup;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private CountryRenewableElectricity _countryRenewableElectricity = null;
    public CountryRenewableElectricity CountryRenewableElectricity
    {
        get
        {
            if (_countryRenewableElectricity == null)
            {
                _countryRenewableElectricity = new CountryRenewableElectricity();
            }
            return _countryRenewableElectricity;
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
                ToolbarDescriptionModule.Register(context);
                DataChartToolbarDescriptionModule.Register(context);
                DataChartCoreDescriptionModule.Register(context);
                DataChartCategoryDescriptionModule.Register(context);
                DataChartAnnotationDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
                AnnotationLayerProxyDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void EditorButtonFinishUserAnnotation(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var fields = new UserAnnotationFlowFields();
        fields.Label = this.AnnotationLabel;
        fields.Details = this.AnnotationDetails;
        fields.MainColor = this.AnnotationMainColor;
        fields.BadgeColor = this.AnnotationBadgeColor;
        UserAnnotationFlow.Finish(this.chart, fields);
    }

    public void EditorButtonCancelUserAnnotation(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        UserAnnotationFlow.Cancel(this.chart);
    }

    public void DataChartUserAnnotationRequested(object sender, UserAnnotationInformationEventArgs args)
    {
        var fields = new UserAnnotationFlowFields();
        fields.Label = this.AnnotationLabel;
        fields.Details = this.AnnotationDetails;
        fields.MainColor = this.AnnotationMainColor;
        fields.BadgeColor = this.AnnotationBadgeColor;
        UserAnnotationFlow.Begin(args.AnnotationInfo, fields);
    }

    public void DataChartUserAnnotationTooltip(object sender, UserAnnotationToolTipContentUpdatingEventArgs args)
    {
        // The layer hands over the tooltip's own content control, so the text replaces whatever it
        // is presenting rather than being appended to it.
        var container = args.Content as ContentControl;
        if (container == null)
        {
            return;
        }
        var line = new TextBlock();
        line.Text = args.AnnotationInfo.AnnotationData;
        line.Foreground = new SolidColorBrush(Colors.White);
        container.Content = line;
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

// The annotation the reader is part-way through drawing. The chart hands it over when it wants the
// label and colours filled in, and will not draw it until one of the two buttons answers -- so the
// pending annotation has to outlive the request, and all three handlers have to be looking at the
// same one.
//
// The chart and the four fields are handed in rather than looked up here. Asking for a description
// expands, where the sample is generated, to the field the component was assigned to -- which only
// means anything inside the component's own instance, so the entry points do the asking.
public class UserAnnotationFlowFields
{
    public PropertyEditorPropertyDescription Label { get; set; }
    public PropertyEditorPropertyDescription Details { get; set; }
    public PropertyEditorPropertyDescription MainColor { get; set; }
    public PropertyEditorPropertyDescription BadgeColor { get; set; }
}

public static class UserAnnotationFlow
{
    private static UserAnnotationInformation pending;

    // Seeds the fields with whatever the annotation already carries, so an edit starts from what is
    // there rather than from the placeholder text.
    public static void Begin(UserAnnotationInformation info, UserAnnotationFlowFields fields)
    {
        pending = info;
        fields.Label.PrimitiveValue = info.Label == null ? "" : info.Label;
        fields.Details.PrimitiveValue = info.AnnotationData == null ? "" : info.AnnotationData;
        fields.MainColor.PrimitiveValue = info.MainColor == null ? "black" : info.MainColor;
        fields.BadgeColor.PrimitiveValue = info.BadgeColor == null ? "black" : info.BadgeColor;
    }

    public static void Finish(XamDataChart chart, UserAnnotationFlowFields fields)
    {
        if (pending == null)
        {
            return;
        }
        pending.Label = System.Convert.ToString(fields.Label.PrimitiveValue);
        pending.AnnotationData = System.Convert.ToString(fields.Details.PrimitiveValue);
        pending.MainColor = ToBrush(fields.MainColor.PrimitiveValue);
        pending.BadgeColor = ToBrush(fields.BadgeColor.PrimitiveValue);
        chart.FinishAnnotationFlow(pending);
        pending = null;
    }

    /// <summary>The colour the editor holds as text, as the brush an annotation is drawn with.</summary>
    private static Brush ToBrush(object value)
    {
        var text = System.Convert.ToString(value);
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }
        // WinUI has no BrushConverter; the XAML type converter is what reads a colour name or hex
        // string here, the same call ColorEditorToggleSeriesBrush makes.
        return (Brush)XamlBindingHelper.ConvertValue(typeof(Brush), text);
    }

    public static void Cancel(XamDataChart chart)
    {
        if (pending != null && pending.AnnotationId != null)
        {
            chart.CancelAnnotationFlow(pending.AnnotationId);
        }
        pending = null;
    }
}
