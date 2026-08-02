using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infragistics.Controls.Grids;
using System.Windows;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Microsoft.UI;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.DataGridPerformanceTickerOnViewInit();
        };
    }

    private SalesPersonsData _salesPersonsData = null;
    public SalesPersonsData SalesPersonsData
    {
        get
        {
            if (_salesPersonsData == null)
            {
                SalesPersonsData.Fetch().ContinueWith((t) => {_salesPersonsData = t.Result;  OnPropertyChanged("SalesPersonsData"); }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
            }
            return _salesPersonsData;
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
                DataGridDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public DateTime LastDataUpdate = DateTime.Now;
    public int Interval = 100;
    public int TimerStep = 16;
    public bool IsTimerTicking = false;
    public Random Random = new Random();
    public int ToChangePerInterval = 200;

    //WPF: System.Action
    public void DataGridPerformanceTickerOnViewInit()
    {
        StartTicking();
    }

    public void StartTicking()
    {
        if (!IsTimerTicking)
        {
            IsTimerTicking = true;
            Task.Delay(TimerStep).ContinueWith((t) => OnTimerTick(), TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public void OnTimerTick()
    {
        if (!IsTimerTicking) return;

        var grid = this.grid;
        if (grid == null)
        {
            Task.Delay(TimerStep).ContinueWith((t) => OnTimerTick(), TaskScheduler.FromCurrentSynchronizationContext());
            return;
        }

        var data = grid.ItemsSource as List<SalesPerson>;
        if (data == null)
        {
            Task.Delay(TimerStep).ContinueWith((t) => OnTimerTick(), TaskScheduler.FromCurrentSynchronizationContext());
            return;
        }

        var now = DateTime.Now;
        var intervalElapsed = (now - LastDataUpdate).TotalMilliseconds > Interval;

        var toChangeIndexes = new Dictionary<int, bool>();
        var useClear = false;

        var sortingByAvgSale = false;
        for (var i = 0; i < grid.SortDescriptions.Count; i++)
        {
            if (grid.SortDescriptions[i].Field == "AvgSale" ||
                grid.SortDescriptions[i].Field.Contains("Change"))
            {
                sortingByAvgSale = true;
            }
        }

        if (intervalElapsed)
        {
            LastDataUpdate = now;
            for (var i = 0; i < ToChangePerInterval; i++)
            {
                var index = (int)Math.Round(Random.NextDouble() * data.Count - 1);
                while (toChangeIndexes.ContainsKey(index))
                {
                    index = (int)Math.Round(Random.NextDouble() * data.Count - 1);
                }
                toChangeIndexes[index] = true;
            }
        }

        for (var i = 0; i < data.Count; i++)
        {
            var item = data[i];
            if (toChangeIndexes.ContainsKey(i))
            {
                if (sortingByAvgSale && !useClear)
                {
                    grid.NotifyRemoveItem(i, item);
                    var oldItem = item;
                    RandomizeItem(item);
                    grid.NotifyInsertItem(i, item);
                }
                else
                {
                    var oldItem = item;
                    RandomizeItem(item);
                    grid.NotifySetItem(i, oldItem, item);
                }

                if (item.Change > 0)
                {
                    item.AvgSaleHeat = 1;
                }
                else
                {
                    item.AvgSaleHeat = -1;
                }
            }
            else
            {
                if (item.AvgSaleHeat > 0)
                {
                    item.AvgSaleHeat -= .06;
                    if (item.AvgSaleHeat < 0) item.AvgSaleHeat = 0;
                }
                if (item.AvgSaleHeat < 0)
                {
                    item.AvgSaleHeat += .06;
                    if (item.AvgSaleHeat > 0) item.AvgSaleHeat = 0;
                }
            }
        }

        if (!sortingByAvgSale || !intervalElapsed)
        {
            grid.InvalidateVisibleRows();
        }

        Task.Delay(TimerStep).ContinueWith((t) => OnTimerTick(), TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void RandomizeItem(SalesPerson item)
    {
        item.Change = Random.NextDouble() * 40.0 - 20.0;
        var prevSale = item.AvgSale;
        item.AvgSale += item.Change;
        item.PercentChange = (item.AvgSale / prevSale) * 100.0;
    }

    //WPF: Infragistics.Controls.Grids.CellStyleRequestedEventHandler
    public void DataGridPerformanceAvgSaleStyleKey(object sender, CellStyleRequestedEventArgs args)
    {
        var grid = this.grid;
        var row = grid.ActualDataSource.GetItemAtIndex(args.RowNumber) as SalesPerson;
        if (row != null && row.Change >= 0)
        {
            args.StyleKey = "priceShiftUp";
        }
        else
        {
            args.StyleKey = "priceShiftDown";
        }
    }

    //WPF: Infragistics.Controls.Grids.TemplateCellUpdatingEventHandler
    public void DataGridPerformanceAvgSaleCellUpdating(object sender, TemplateCellUpdatingEventArgs args)
    {
        var content = args.Content;
        var item = args.CellInfo.RowItem as SalesPerson;
        if (item == null) return;

        var priceShiftUp = item.Change >= 0;
        var color = priceShiftUp
            ? new SolidColorBrush(Color.FromArgb(0xFF, 0x4E, 0xB8, 0x62))
            : new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x13, 0x4A));

        StackPanel panel;
        TextBlock priceText;

        if (content.Content is StackPanel existing)
        {
            panel = existing;
            priceText = (TextBlock)panel.Children[0];
        }
        else
        {
            panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            priceText = new TextBlock { FontFamily = new FontFamily("Verdana"), FontSize = 13 };
            panel.Children.Add(priceText);
            content.Content = panel;
        }

        priceText.Text = "$" + System.Math.Round(System.Convert.ToDouble(args.CellInfo.Value), 2).ToString("F2");
        priceText.Foreground = color;
    }

    //WPF: Infragistics.Controls.Grids.DataBindingEventHandler
    public void DataGridPerformanceAvgSaleDataBound(object sender, DataBindingEventArgs args)
    {
        var item = args.CellInfo.RowItem as SalesPerson;
        if (item == null) return;

        if (item.AvgSaleHeat > 0)
        {
            var p = item.AvgSaleHeat;
            var r = (byte)System.Math.Round((1.0 + (0.0 - 1.0) * p) * 255.0);
            var g = (byte)System.Math.Round(1.0 * 255.0);
            var b = (byte)System.Math.Round((1.0 + (0.0 - 1.0) * p) * 255.0);
            args.CellInfo.Background = new SolidColorBrush(Color.FromArgb(0xFF, r, g, b));
        }
        else if (item.AvgSaleHeat < 0)
        {
            var p = item.AvgSaleHeat * -1.0;
            var r = (byte)System.Math.Round(1.0 * 255.0);
            var g = (byte)System.Math.Round((1.0 + (0.0 - 1.0) * p) * 255.0);
            var b = (byte)System.Math.Round((1.0 + (0.0 - 1.0) * p) * 255.0);
            args.CellInfo.Background = new SolidColorBrush(Color.FromArgb(0xFF, r, g, b));
        }
        else
        {
            args.CellInfo.Background = new SolidColorBrush(Colors.White);
        }
    }

    //WPF: Infragistics.Controls.Grids.CellStyleRequestedEventHandler
    public void DataGridPerformanceChangeStyleKey(object sender, CellStyleRequestedEventArgs args)
    {
        var value = System.Convert.ToDouble(args.ResolvedValue);
        if (value >= 0)
        {
            args.StyleKey = "priceAmountUp";
        }
        else
        {
            args.StyleKey = "priceAmountDown";
        }
    }

    //WPF: Infragistics.Controls.Grids.TemplateCellUpdatingEventHandler
    public void DataGridPerformanceChangeCellUpdating(object sender, TemplateCellUpdatingEventArgs args)
    {
        var content = args.Content;
        var value = System.Convert.ToDouble(args.CellInfo.Value);
        var priceShiftUp = value >= 0;
        var color = priceShiftUp
            ? new SolidColorBrush(Color.FromArgb(0xFF, 0x4E, 0xB8, 0x62))
            : new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x13, 0x4A));

        Border border;
        TextBlock text;

        if (content.Content is Border existing)
        {
            border = existing;
            text = (TextBlock)border.Child;
        }
        else
        {
            text = new TextBlock { FontFamily = new FontFamily("Verdana"), FontSize = 13 };
            border = new Border
            {
                Child = text,
                BorderThickness = new Thickness(0, 0, 4, 0),
                Padding = new Thickness(0, 0, 5, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            content.Content = border;
        }

        text.Text = value.ToString("F2");
        border.BorderBrush = color;
    }

    //WPF: Infragistics.Controls.Grids.CellStyleRequestedEventHandler
    public void DataGridPerformancePercentStyleKey(object sender, CellStyleRequestedEventArgs args)
    {
        var value = System.Convert.ToDouble(args.ResolvedValue);
        if (value >= 0)
        {
            args.StyleKey = "pricePercentUp";
        }
        else
        {
            args.StyleKey = "pricePercentDown";
        }
    }

    //WPF: Infragistics.Controls.Grids.TemplateCellUpdatingEventHandler
    public void DataGridPerformancePercentCellUpdating(object sender, TemplateCellUpdatingEventArgs args)
    {
        var content = args.Content;
        var value = System.Convert.ToDouble(args.CellInfo.Value);
        var priceShiftUp = value >= 0;
        var color = priceShiftUp
            ? new SolidColorBrush(Color.FromArgb(0xFF, 0x4E, 0xB8, 0x62))
            : new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x13, 0x4A));

        Border border;
        TextBlock text;

        if (content.Content is Border existing)
        {
            border = existing;
            text = (TextBlock)border.Child;
        }
        else
        {
            text = new TextBlock { FontFamily = new FontFamily("Verdana"), FontSize = 13 };
            border = new Border
            {
                Child = text,
                BorderThickness = new Thickness(0, 0, 4, 0),
                Padding = new Thickness(0, 0, 5, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            content.Content = border;
        }

        text.Text = value.ToString("F2") + "%";
        border.BorderBrush = color;
    }

    //WPF: Infragistics.Controls.Grids.CellStyleRequestedEventHandler
    public void DataGridPerformanceKpiStyleKey(object sender, CellStyleRequestedEventArgs args)
    {
        var value = System.Convert.ToDouble(args.ResolvedValue);
        if (value < 20.0)
        {
            args.StyleKey = "kpi_red";
        }
        else if (value > 80.0)
        {
            args.StyleKey = "kpi_green";
        }
    }

    //WPF: Infragistics.Controls.Grids.DataBindingEventHandler
    public void DataGridPerformanceKpiDataBound(object sender, DataBindingEventArgs args)
    {
        var value = System.Convert.ToDouble(args.ResolvedValue);
        if (value < 20.0)
        {
            args.CellInfo.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x13, 0x4A));
        }
        else if (value > 80.0)
        {
            args.CellInfo.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x4E, 0xB8, 0x62));
        }
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
