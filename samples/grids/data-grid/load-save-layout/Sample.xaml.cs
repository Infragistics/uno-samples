using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Grids;

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

    private DataGridLayoutState dataGridLayoutState = new DataGridLayoutState();

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

    public void DataGridLoadLayout(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var state = this.dataGridLayoutState;
        if (string.IsNullOrEmpty(state.SavedLayout)) return;
        var grid = this.grid;
        if (grid == null) return;
        grid.LoadLayout(state.SavedLayout);
    }

    public void DataGridSaveLayout(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        if (grid == null) return;
        this.dataGridLayoutState.SavedLayout = grid.SaveLayout();
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

/// <summary>
/// The layout the save button stores and the load button restores. The two handlers are separate
/// items, so the value they share is a supporting item each asks for, rather than a field each
/// declares and a sample that uses both would declare twice.
/// </summary>
public class DataGridLayoutState
{
    public string SavedLayout = "";
}
