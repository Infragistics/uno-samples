using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using System.Windows;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml.Shapes;
using Infragistics.Controls.Grids;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Documents;

namespace SamplesBrowser.Pages.Grids.DataGrid.Overview;

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


    //WPF: Infragistics.Controls.Grids.TemplateCellUpdatingEventHandler
    public void DataGridSalesGaugeTemplate(object sender, TemplateCellUpdatingEventArgs args)
    {
        var content = args.Content;
        var item = (EmployeesSalesDataItem)args.CellInfo.RowItem;
        var sales = item.Sales;

        StackPanel panel;
        Rectangle gaugeTrack;
        Rectangle gaugeBar;
        TextBlock gaugeValue;

        if (content.Content is StackPanel existing)
        {
            panel = existing;
            var trackGrid = (Grid)panel.Children[0];
            gaugeTrack = (Rectangle)trackGrid.Children[0];
            gaugeBar = (Rectangle)trackGrid.Children[1];
            gaugeValue = (TextBlock)panel.Children[1];
        }
        else
        {
            panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(16, 0, 16, 0)
            };

            var trackGrid = new Grid { Height = 6, Margin = new Thickness(0, 8, 0, 0) };
            gaugeTrack = new Rectangle { Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD)), Height = 4, VerticalAlignment = VerticalAlignment.Center };
            gaugeBar = new Rectangle { Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x7F, 0x7F, 0x7F)), Height = 6, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center };
            trackGrid.Children.Add(gaugeTrack);
            trackGrid.Children.Add(gaugeBar);

            gaugeValue = new TextBlock
            {
                FontFamily = new FontFamily("Verdana"),
                FontSize = 13,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 2, 0, 0)
            };

            panel.Children.Add(trackGrid);
            panel.Children.Add(gaugeValue);
            content.Content = panel;
        }

        Brush activeBrush;
        if (sales < 400000) activeBrush = new SolidColorBrush(Color.FromArgb(0xFF, 211, 17, 3));
        else if (sales < 650000) activeBrush = new SolidColorBrush(Colors.Orange);
        else activeBrush = new SolidColorBrush(Color.FromArgb(0xFF, 21, 190, 6));

        gaugeValue.Foreground = activeBrush;
        gaugeBar.Fill = activeBrush;

        var gaugeFraction = System.Math.Min(1.0, sales / 990000.0);
        gaugeBar.Width = double.IsNaN(content.ActualWidth) || content.ActualWidth <= 0 ? 0 : content.ActualWidth * gaugeFraction;

        gaugeValue.Text = "$" + (sales / 1000) + ",000";
    }

    //WPF: Infragistics.Controls.Grids.TemplateCellUpdatingEventHandler
    public void DataGridAddressLinesTemplate(object sender, TemplateCellUpdatingEventArgs args)
    {
        var content = args.Content;
        var item = (EmployeesSalesDataItem)args.CellInfo.RowItem;
        var street = item.Street;
        var city = item.City;
        var country = item.Country;

        StackPanel panel;
        TextBlock line1;
        TextBlock line2;

        if (content.Content is StackPanel existing)
        {
            panel = existing;
            line1 = (TextBlock)panel.Children[0];
            line2 = (TextBlock)panel.Children[1];
        }
        else
        {
            panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Center
            };
            line1 = new TextBlock { FontFamily = new FontFamily("Verdana"), FontSize = 13, Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 24, 29, 31)) };
            line2 = new TextBlock { FontFamily = new FontFamily("Verdana"), FontSize = 13, Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 24, 29, 31)) };
            panel.Children.Add(line1);
            panel.Children.Add(line2);
            content.Content = panel;
        }

        line1.Text = street;
        line2.Text = city + ", " + country;
    }

    //WPF: Infragistics.Controls.Grids.TemplateCellUpdatingEventHandler
    public void DataGridPhoneLinkTemplate(object sender, TemplateCellUpdatingEventArgs args)
    {
        var content = args.Content;
        var item = (EmployeesSalesDataItem)args.CellInfo.RowItem;
        var phone = item.Phone;

        TextBlock textBlock;
        Hyperlink hyperlink;
        Run run;

        if (content.Content is TextBlock existing && existing.Inlines.Count > 0 && existing.Inlines[0] is Hyperlink hl)
        {
            textBlock = existing;
            hyperlink = hl;
            run = (Run)hyperlink.Inlines[0];
        }
        else
        {
            run = new Run();
            hyperlink = new Hyperlink { Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x42, 0x86, 0xF4)) };
            hyperlink.Inlines.Add(run);
            textBlock = new TextBlock
            {
                FontFamily = new FontFamily("Verdana"),
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            textBlock.Inlines.Add(hyperlink);
            content.Content = textBlock;
        }

        hyperlink.NavigateUri = new System.Uri("tel:" + phone);
        run.Text = phone;
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
