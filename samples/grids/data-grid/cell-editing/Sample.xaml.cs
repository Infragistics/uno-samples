using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Grids;
using Microsoft.UI.Xaml.Controls;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "None", "Cell", "CellBatch", "Row" };
    public string[] dropDownValues1 { get; } = new string[] { "None", "Cell", "CellBatch", "Row" };
    public string[] dropDownNames2 { get; } = new string[] { "SingleClick", "DoubleClick" };
    public string[] dropDownValues2 { get; } = new string[] { "SingleClick", "DoubleClick" };

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
    public void DataGridCommitEdits(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        grid.CommitEdits();
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void DataGridUndoEdit(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        grid.Undo();
    }

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void DataGridRedoEdit(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        grid.Redo();
    }

    //WPF: Infragistics.Controls.Grids.TemplateCellUpdatingEventHandler
    public void DataGridDeleteRowButtonTemplate(object sender, TemplateCellUpdatingEventArgs args)
    {
        var content = args.Content;
        Button button;
        if (content.Content is Button existing)
        {
            button = existing;
        }
        else
        {
            button = new Button { Content = "Delete" };
            button.Click += (s, e) =>
            {
                var grid = this.grid;
                var btn = (Button)s;
                if (btn.Tag != null)
                {
                    grid.RemoveItem(btn.Tag);
                }
            };
            content.Content = button;
        }

        button.IsEnabled = !args.CellInfo.IsDeleted;
        button.Tag = args.CellInfo.RowItem;
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
