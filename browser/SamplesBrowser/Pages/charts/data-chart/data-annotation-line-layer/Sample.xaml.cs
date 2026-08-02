using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.DataChart.DataAnnotationLineLayer;

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

    private AnnotationLineData1 _annotationLineData1 = null;
    public AnnotationLineData1 AnnotationLineData1
    {
        get
        {
            if (_annotationLineData1 == null)
            {
                _annotationLineData1 = new AnnotationLineData1();
            }
            return _annotationLineData1;
        }
    }

    private AnnotationLineData2 _annotationLineData2 = null;
    public AnnotationLineData2 AnnotationLineData2
    {
        get
        {
            if (_annotationLineData2 == null)
            {
                _annotationLineData2 = new AnnotationLineData2();
            }
            return _annotationLineData2;
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
