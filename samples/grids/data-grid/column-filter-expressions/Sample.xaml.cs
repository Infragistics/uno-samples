using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Description;
using Infragistics.Controls.Grids;
#if !TESTING
using Infragistics.Core.Controls.DataSource;
using Infragistics.Controls.DataSource;
#endif

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Name", "Street", "City", "Country" };
    public string[] dropDownValues1 { get; } = new string[] { "Name", "Street", "City", "Country" };
    public string[] dropDownNames2 { get; } = new string[] { "Contains", "StartsWith", "EndsWith" };
    public string[] dropDownValues2 { get; } = new string[] { "Contains", "StartsWith", "EndsWith" };

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

        public void DataGridApplyFilterExpressions(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
        {
    #if !TESTING
            var factory = new FilterFactory();
            var grid = this.grid;
            var columnEditor = this.FilterColumnEditor;
            var modeEditor = this.FilterModeEditor;
            var textEditor = this.FilterTextEditor;

            var filterColumn = columnEditor.PrimitiveValue as string;
            var filterMode = modeEditor.PrimitiveValue as string;
            var filterText = (textEditor.PrimitiveValue as string) ?? "";

            grid.FilterExpressions.Clear();
            if (filterText == "")
            {
                return;
            }

            var expression = filterText.ToUpper();
            var column = factory.Property(filterColumn).ToUpper();

            FilterExpression filter;
            switch (filterMode)
            {
                case "Contains":   filter = column.Contains(expression);   break;
                case "StartsWith": filter = column.StartsWith(expression); break;
                case "EndsWith":   filter = column.EndsWith(expression);   break;
                default:           filter = column.Contains(expression);   break;
            }

            grid.FilterExpressions.Add(filter);
    #endif
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

