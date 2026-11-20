namespace SamplesBrowser.Pages.Charts.DataChart.UserAnnotationLayer;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;
using Infragistics.Controls.Charts;
using Microsoft.UI;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    // The annotation the user is part-way through drawing. The chart hands it over when it wants the
    // label and colours filled in, and will not draw it until Done or Cancel answers the request.
    private UserAnnotationInformation annotationInfo;

    public Sample()
    {
        this.InitializeComponent();

        DataContext = this;

        this.annotationBadgeColorEditor.Value = new SolidColorBrush(Colors.Black);
        this.annotationMainColorEditor.Value = new SolidColorBrush(Colors.Black);
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

    public void DataChartUserAnnotationRequested(object sender, UserAnnotationInformationEventArgs args)
    {
        this.annotationInfo = args.AnnotationInfo;
        this.ToggleDialogState(true);
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

    private void OnDoneButtonClick(object sender, RoutedEventArgs e)
    {
        if (this.annotationInfo != null)
        {
            this.annotationInfo.Label = this.annotationInput.Text;
            this.annotationInfo.AnnotationData = this.annotationDetails.Text;
            this.annotationInfo.MainColor = this.annotationMainColorEditor.Value;
            this.annotationInfo.BadgeColor = this.annotationBadgeColorEditor.Value;

            this.chart.FinishAnnotationFlow(this.annotationInfo);
            this.annotationInfo = null;
        }
        this.ToggleDialogState(false);
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        if (this.annotationInfo != null && this.annotationInfo.AnnotationId != null)
        {
            this.chart.CancelAnnotationFlow(this.annotationInfo.AnnotationId);
        }
        this.annotationInfo = null;
        this.ToggleDialogState(false);
    }

    private void ToggleDialogState(bool open)
    {
        this.annotationPopup.Visibility = open ? Visibility.Visible : Visibility.Collapsed;
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
