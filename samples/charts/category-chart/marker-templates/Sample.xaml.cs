using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Charts;
using Infragistics.Controls;
using System;
using Microsoft.UI.Xaml.Data;

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

    private EnergyGenerationChange _energyGenerationChange = null;
    public EnergyGenerationChange EnergyGenerationChange
    {
        get
        {
            if (_energyGenerationChange == null)
            {
                _energyGenerationChange = new EnergyGenerationChange();
            }
            return _energyGenerationChange;
        }
    }

    public void CategoryChartMarkerTemplateSeriesAdded(object sender, ChartSeriesEventArgs args)
    {
        // Nothing to hand the series yet on WinUI.
        //
        // WPF looks a keyed DataTemplate up from the chart and hangs it on each series the category
        // chart makes. WinUI draws its markers immediate mode, so there is no template to hang: a
        // custom marker here is going to be a render callback or path data in the SVG sense, not a
        // XAML template, and MarkerTemplate is typed for the portable shape rather than the XAML one
        // to leave room for it.
        //
        // The handler stays wired so the sample keeps the same shape it has everywhere else, and
        // does nothing until there is something for it to do.
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

// Reads the value a marker stands for, whichever series it belongs to.
//
// A marker template is one template shared by every series, and its data context carries the record
// and the series but not the number: the number is whichever column that series draws. So this reads
// one from the other, which is what the drawn marker on the web does when it looks up
// series.valueColumn.propertyName.
//
// WinUI has no MultiBinding, so the whole data context is passed and both halves are read off it here.
public class SeriesValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var context = value as DataContext;
        if (context == null)
        {
            return null;
        }
        var item = context.Item;
        var series = context.Series as Series;
        if (item == null || series == null)
        {
            return null;
        }
        var path = series.GetType().GetProperty("ValueMemberPath");
        if (path == null)
        {
            return null;
        }
        var column = path.GetValue(series, null) as string;
        if (string.IsNullOrEmpty(column))
        {
            return null;
        }
        var property = item.GetType().GetProperty(column);
        if (property == null)
        {
            return null;
        }
        return property.GetValue(item, null);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
