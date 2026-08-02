using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

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

    private AnnotationSliceMultiOverlayData _annotationSliceMultiOverlayData = null;
    public AnnotationSliceMultiOverlayData AnnotationSliceMultiOverlayData
    {
        get
        {
            if (_annotationSliceMultiOverlayData == null)
            {
                _annotationSliceMultiOverlayData = new AnnotationSliceMultiOverlayData();
            }
            return _annotationSliceMultiOverlayData;
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
