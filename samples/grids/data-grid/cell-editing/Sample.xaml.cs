using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Description;
using Infragistics.Controls.Grids;
using Infragistics.Core.Controls.DataSource;
using Infragistics.Controls.DataSource;
using Infragistics.Core.Controls.DataSource.Transactions;
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

    public void DataGridCommitEdits(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        grid.CommitEdits();
    }

    public void DataGridUndoEdit(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        grid.Undo();
    }

    public void DataGridRedoEdit(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var grid = this.grid;
        grid.Redo();
    }

    /// <summary>A cell left empty is refused with a message; anything else is taken.</summary>
    public void DataGridValidateCellEdit(object sender, GridCellValueChangingEventArgs args)
    {
        var grid = this.grid;
        // NewValue is typed as object here, where the web platforms hand back the edited text
        if (args.NewValue as string == "")
        {
            grid.SetEditError(args.EditID, "Error, cell is empty");
            grid.RejectEdit(args.EditID);
        }
        else
        {
            grid.AcceptEdit(args.EditID);
        }
    }

    /// <summary>Only an update is committed; anything else the grid proposes is refused.</summary>
    public void DataGridCommitUpdatesOnly(object sender, GridDataCommittingEventArgs args)
    {
        var grid = this.grid;
        if (args.Changes[0].TransactionType == TransactionType.Update)
        {
            grid.AcceptCommit(args.CommitID);
        }
        else
        {
            grid.RejectCommit(args.CommitID);
        }
    }

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
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

