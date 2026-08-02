using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Charts;
using System.Collections;

namespace SamplesBrowser.Pages.Charts.CategoryChart.CustomSelection;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private SelectableData _selectableData = null;
    public SelectableData SelectableData
    {
        get
        {
            if (_selectableData == null)
            {
                _selectableData = new SelectableData();
            }
            return _selectableData;
        }
    }


    //WPF: Infragistics.Controls.Charts.DomainChartSeriesPointerEventHandler
    public void CategoryChartCustomSelectionPointerDown(object sender, DomainChartSeriesPointerEventArgs args)
    {
        var chart = this.chart;
        var selectableData = (SelectableData)chart.ItemsSource;
        var selectedItem = args.Item as SelectableDataItem;
        if (selectedItem == null) return;

        var selectedIndex = -1;
        for (var i = 0; i < selectableData.Count; i++)
        {
            if (selectedItem.Category == selectableData[i].Category)
            {
                selectedIndex = i; break;
            }
        }

        if (selectedItem.SelectedValue == selectedItem.DataValue)
            selectedItem.SelectedValue = double.NaN;
        else
            selectedItem.SelectedValue = selectedItem.DataValue;

        chart.NotifySetItem(selectableData, selectedIndex, selectedItem, selectedItem);
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
