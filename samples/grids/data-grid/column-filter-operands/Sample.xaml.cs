using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Grids;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.DataGridRegisterCountryFilterOnViewInit();
            this.DataGridRegisterCustomAgeFilterOnViewInit();
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


    //WPF: System.Action
    public void DataGridRegisterCountryFilterOnViewInit()
    {
        var grid = this.grid;
        var operand = new FilterOperand
        {
            EditorType = EditorType.Text,
            DisplayName = "(Custom) In Code Filter"
        };
        operand.FilterRequested += OnFilter;
        grid.ActualColumns[0].FilterOperands.Add(operand);
    }

    private void OnFilter(object sender, GridCustomFilterRequestedEventArgs args)
    {
        var prop = args.FilterFactory.Property(args.Column.Field);
        args.Expression = prop.IsEqualTo("France");
    }

    //WPF: System.Action
    public void DataGridRegisterCustomAgeFilterOnViewInit()
    {
        var grid = this.grid;
        grid.ActualColumns[1].FilterOperands.Add(new CustomAgeFilter());
    }

    public class CustomAgeFilter : FilterOperand
    {
        public CustomAgeFilter()
        {
            DisplayName = "Filter As Class";
            IsInputRequired = false;
            EditorType = EditorType.Numeric;
            FilterRequested += OnFilter;
        }

        private void OnFilter(object sender, GridCustomFilterRequestedEventArgs args)
        {
            var prop = args.FilterFactory.Property(args.Column.Field);
            args.Expression = prop.IsEqualTo(30);
        }
    }

    //WPF: Infragistics.Controls.Grids.GridCustomFilterRequestedEventHandler
    public void DataGridFilterSalesLessThanOrEqual300k(object sender, GridCustomFilterRequestedEventArgs args)
    {
        args.Expression = args.FilterFactory.Property("Sales").IsLessThanOrEqualTo(300000);
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
