using FinancialGrid.Business;
using FinancialGrid.ViewModels;
using Infragistics.Controls.Grids;
using Infragistics.Core.Controls.DataSource;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FinancialGrid.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel
        {
            get { return (MainPageViewModel)DataContext; }
            set { DataContext = value; }
        }

        public MainPage()
        {
            this.InitializeComponent();

            ViewModel = new MainPageViewModel();

            _sliderNumOfRecords.AddHandler(Thumb.PointerReleasedEvent, new PointerEventHandler(ThumbReleased), true);

            AddGridGrouping();

            //workaround the column deifnition bug
            _grid.ActualColumnsChanged += Grid_ActualColumnsChanged;
        }

        private void ThumbReleased(object sender, PointerRoutedEventArgs e)
        {
            var expression = _sliderNumOfRecords.GetBindingExpression(Slider.ValueProperty);
            expression.UpdateSource();
        }

        private void IsGrouped_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;

            if (checkBox.IsChecked.Value)
                AddGridGrouping();
            else
                RemoveGridGrouping();
        }

        private void Grid_ActualColumnsChanged(object sender, GridColumnsChangedEventArgs args)
        {
            foreach (var column in args.Columns)
            {
                if (column.Field == "Price")
                {
                    SetCurrenyColumnProperties((NumericColumn)column);
                    column.DataBound += OpenPrice_DataBound;
                }
                else if (column.Field == "Change")
                {
                    SetNumericColumnProperties((NumericColumn)column);
                    column.DataBound += Change_DataBound;
                }
                else if (column.Field == "ChangePercent")
                {
                    SetPercentColumnProperties((NumericColumn)column);
                    column.DataBound += Change_DataBound;
                }
            }
        }

        private void Change_DataBound(object sender, DataBindingEventArgs e)
        {
            FinancialRecord record = (FinancialRecord)e.RowObject;

            e.CellInfo.BorderRightWidth = 10;

            if (record.Change > 0.0)
                e.CellInfo.Border = this.Resources["GreenBrush"] as Brush;
            else
                e.CellInfo.Border = this.Resources["RedBrush"] as Brush;
        }

        void SetCurrenyColumnProperties(NumericColumn column)
        {
            SetNumericColumnProperties(column);
            column.PositivePrefix = "$";
            column.NegativeSuffix = "$";
        }

        void SetNumericColumnProperties(NumericColumn column)
        {
            column.MaxFractionDigits = 2;
            column.MinFractionDigits = 2;
        }

        void SetPercentColumnProperties(NumericColumn column)
        {
            SetNumericColumnProperties(column);
            column.PositiveSuffix = "%";
            column.NegativeSuffix = "%";
        }

        private void OpenPrice_DataBound(object sender, DataBindingEventArgs e)
        {
            FinancialRecord record = (FinancialRecord)e.RowObject;

            if (record.Change > 0.0)
            {
                e.CellInfo.TextColor = this.Resources["GreenBrush"] as Brush;
                e.CellInfo.Background = this.Resources["GreenHeatBrush"] as Brush;
            }
            else
            {
                e.CellInfo.TextColor = this.Resources["RedBrush"] as Brush;
                e.CellInfo.Background = this.Resources["RedHeatBrush"] as Brush;
            }
        }

        public void RemoveGridGrouping()
        {
            _grid.GroupDescriptions.Clear();
            _grid.Flush();
        }

        public void AddGridGrouping()
        {
            var g = new ColumnGroupDescription();
            g.Field = "Category";
            g.SortDirection = ListSortDirection.Ascending;
            _grid.GroupDescriptions.Add(g);

            g = new ColumnGroupDescription();
            g.Field = "Type";
            g.SortDirection = ListSortDirection.Ascending;
            _grid.GroupDescriptions.Add(g);

            g = new ColumnGroupDescription();
            g.Field = "Contract";
            g.SortDirection = ListSortDirection.Ascending;
            _grid.GroupDescriptions.Add(g);
        }
    }
}
