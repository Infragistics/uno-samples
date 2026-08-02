using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Grids;

namespace SamplesBrowser.Pages.Grids.DataGrid.ColumnPinningPicker;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

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
                DataGridDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void DataGridPinIdAndNameColumnsLeft(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        grid.PinColumn(grid.ActualColumns[0], PinnedPositions.Left);
        grid.PinColumn(grid.ActualColumns[1], PinnedPositions.Left);
        grid.PinColumn(grid.ActualColumns[2], PinnedPositions.Left);
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void DataGridPinAddressColumnsRight(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        grid.PinColumn(grid.ActualColumns[6], PinnedPositions.Right);
        grid.PinColumn(grid.ActualColumns[7], PinnedPositions.Right);
        grid.PinColumn(grid.ActualColumns[8], PinnedPositions.Right);
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void DataGridUnpinNameAndAddressColumns(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        grid.PinColumn(grid.ActualColumns[0], PinnedPositions.None);
        grid.PinColumn(grid.ActualColumns[1], PinnedPositions.None);
        grid.PinColumn(grid.ActualColumns[2], PinnedPositions.None);
        grid.PinColumn(grid.ActualColumns[6], PinnedPositions.None);
        grid.PinColumn(grid.ActualColumns[7], PinnedPositions.None);
        grid.PinColumn(grid.ActualColumns[8], PinnedPositions.None);
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
