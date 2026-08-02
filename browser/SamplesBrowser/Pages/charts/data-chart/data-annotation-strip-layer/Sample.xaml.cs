using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.DataChart.DataAnnotationStripLayer;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] includedColumns1 { get; } = new string[] { "High", "Low", "Open", "Close" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private StockTesla _stockTesla = null;
    public StockTesla StockTesla
    {
        get
        {
            if (_stockTesla == null)
            {
                _stockTesla = new StockTesla();
            }
            return _stockTesla;
        }
    }

    private AnnotationStripData _annotationStripData = null;
    public AnnotationStripData AnnotationStripData
    {
        get
        {
            if (_annotationStripData == null)
            {
                _annotationStripData = new AnnotationStripData();
            }
            return _annotationStripData;
        }
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
