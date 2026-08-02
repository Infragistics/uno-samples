using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Grids;

namespace SamplesBrowser.Pages.Grids.DataGrid.LoadSaveLayout;

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
                DataGridToolbarDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public string SavedLayout = "";

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void DataGridLoadLayout(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        if (string.IsNullOrEmpty(this.SavedLayout)) return;
        var grid = this.grid;
        if (grid == null) return;
        grid.LoadLayout(this.SavedLayout);
    }

    
    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void DataGridSaveLayout(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        if (grid == null) return;
        SavedLayout = grid.SaveLayout();
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
