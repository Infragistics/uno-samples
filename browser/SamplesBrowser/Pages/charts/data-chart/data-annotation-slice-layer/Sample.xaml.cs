using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace SamplesBrowser.Pages.Charts.DataChart.DataAnnotationSliceLayer;

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

    private AnnotationSliceStockSplitData _annotationSliceStockSplitData = null;
    public AnnotationSliceStockSplitData AnnotationSliceStockSplitData
    {
        get
        {
            if (_annotationSliceStockSplitData == null)
            {
                _annotationSliceStockSplitData = new AnnotationSliceStockSplitData();
            }
            return _annotationSliceStockSplitData;
        }
    }

    private AnnotationSliceEarningsMissData _annotationSliceEarningsMissData = null;
    public AnnotationSliceEarningsMissData AnnotationSliceEarningsMissData
    {
        get
        {
            if (_annotationSliceEarningsMissData == null)
            {
                _annotationSliceEarningsMissData = new AnnotationSliceEarningsMissData();
            }
            return _annotationSliceEarningsMissData;
        }
    }

    private AnnotationSliceEarningsBeatData _annotationSliceEarningsBeatData = null;
    public AnnotationSliceEarningsBeatData AnnotationSliceEarningsBeatData
    {
        get
        {
            if (_annotationSliceEarningsBeatData == null)
            {
                _annotationSliceEarningsBeatData = new AnnotationSliceEarningsBeatData();
            }
            return _annotationSliceEarningsBeatData;
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
