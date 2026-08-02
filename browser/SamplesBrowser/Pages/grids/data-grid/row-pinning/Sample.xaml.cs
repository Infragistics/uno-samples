using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using System;
using System.Collections;
using System.Threading;
using Infragistics.Controls;
using Infragistics.Controls.Grids;

namespace SamplesBrowser.Pages.Grids.DataGrid.RowPinning;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.DataGridPinSampleEmployeesOnViewInit();
        };
    }

    private EmployeesSalesData _employeesSalesData = null;
    public EmployeesSalesData EmployeesSalesData
    {
        get
        {
            if (_employeesSalesData == null)
            {
                _employeesSalesData = new EmployeesSalesData();
            }
            return _employeesSalesData;
        }
    }


    private Timer _timer;

    //WPF: System.Action
    public void DataGridPinSampleEmployeesOnViewInit()
    {
        _timer = new Timer((state) =>
        {
            PinRows();
            _timer.Dispose();
        }, null, 100, Timeout.Infinite);
    }

    private void PinRows()
    {
        var grid = this.grid;
        var data = this.EmployeesSalesData;
        grid.PinnedItems.Add(data[2]);
        grid.PinnedItems.Add(data[4]);
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
