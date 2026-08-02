using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Grids;
using System.Windows;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Microsoft.UI;

namespace SamplesBrowser.Pages.Grids.DataGrid.BindingLiveData;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.DataGridLiveDataTickerOnViewInit();
        };
    }

    private PortfolioData _portfolioData = null;
    public PortfolioData PortfolioData
    {
        get
        {
            if (_portfolioData == null)
            {
                _portfolioData = new PortfolioData();
            }
            return _portfolioData;
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

    public DateTime LastUpdateTime = new DateTime();
    public int Frequency = 1000;
    public int DataVolume = 500;
    public bool IsTimerTicking = false;
    public bool IsUpdatingAllPrices = false;
    public bool IsUpdatingSomePrices = false;
    public bool UseHeatBackground = true;
    public bool UseRowGrouping = true;
    public Random Random = new Random();

    //WPF: System.Action
    public void DataGridLiveDataTickerOnViewInit()
    {
        OnGridGroupingAdd();
    }

    public void StopTicking()
    {
        if (IsTimerTicking)
        {
            IsTimerTicking = false;
        }
    }

    public void StartTicking()
    {
        if (!IsTimerTicking)
        {
            IsTimerTicking = true;
            Task.Delay(Frequency).ContinueWith((t) => OnTimerTick(), TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public void OnTimerTick()
    {
        if (!IsTimerTicking) return;

        var grid = this.grid;
        if (grid == null)
        {
            Task.Delay(Frequency).ContinueWith((t) => OnTimerTick(), TaskScheduler.FromCurrentSynchronizationContext());
            return;
        }

        var data = grid.ItemsSource as List<PortfolioDataItem>;
        if (data == null)
        {
            Task.Delay(Frequency).ContinueWith((t) => OnTimerTick(), TaskScheduler.FromCurrentSynchronizationContext());
            return;
        }

        var stillAnimating = false;
        var useClear = IsUpdatingAllPrices;
        var updateAll = IsUpdatingAllPrices;

        var toChangeIndexes = new List<bool>();
        foreach (var item in data)
        {
            toChangeIndexes.Add(false);
            if (!UseHeatBackground)
            {
                item.PriceHeat = 0;
            }
            else if (item.PriceHeat != 0)
            {
                stillAnimating = true;
            }
        }

        var toChange = (int)Math.Round(DataVolume / 10.0);
        if (updateAll)
        {
            toChange = data.Count;
        }
        else
        {
            toChange = (int)(Random.Next(2, data.Count - 1));
        }

        var sortingByPrice = false;
        for (var i = 0; i < grid.SortDescriptions.Count; i++)
        {
            if (grid.SortDescriptions[i].Field == "Price" ||
                grid.SortDescriptions[i].Field.Contains("Change"))
            {
                sortingByPrice = true;
            }
        }

        var changing = false;
        var toChangeCount = 0;

        var now = DateTime.Now;
        var elapsedTime = now.Subtract(LastUpdateTime);
        var elapsedInterval = elapsedTime.TotalMilliseconds > Frequency;
        if (elapsedInterval)
        {
            LastUpdateTime = DateTime.Now;
            for (var i = 0; i < toChange; i++)
            {
                var index = (int)(Random.Next(0, data.Count - 1));
                toChangeIndexes[index] = true;
                toChangeCount++;
                changing = true;
            }
        }

        for (var i = 0; i < data.Count; i++)
        {
            var item = data[i];
            if (toChangeIndexes[i] == true)
            {
                if (sortingByPrice && !useClear)
                {
                    grid.NotifyRemoveItem(i, item);
                    PortfolioData.RandomizeDataValues(item);
                    grid.NotifyInsertItem(i, item);
                }
                else
                {
                    var oldItem = item;
                    PortfolioData.RandomizeDataValues(item);
                    grid.NotifySetItem(i, oldItem, item);
                }

                if (UseHeatBackground)
                {
                    if (item.Change > 0)
                    {
                        item.PriceHeat = 1;
                    }
                    else
                    {
                        item.PriceHeat = -1;
                    }
                }
            }
            else
            {
                if (UseHeatBackground)
                {
                    if (item.PriceHeat > 0)
                    {
                        item.PriceHeat -= .06;
                        if (item.PriceHeat < 0) item.PriceHeat = 0;
                    }
                    if (item.PriceHeat < 0)
                    {
                        item.PriceHeat += .06;
                        if (item.PriceHeat > 0) item.PriceHeat = 0;
                    }
                }
            }
        }

        if (sortingByPrice && useClear && elapsedInterval)
        {
            grid.NotifyClearItems();
        }
        else if (useClear)
        {
            grid.NotifyClearItems();
        }

        if (!sortingByPrice || !elapsedInterval)
        {
            grid.InvalidateVisibleRows();
        }

        Task.Delay(Frequency).ContinueWith((t) => OnTimerTick(), TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void OnGridGroupingRemove()
    {
        var grid = this.grid;
        if (grid == null) return;
        grid.GroupDescriptions.Clear();
    }

    public void OnGridGroupingAdd()
    {
        var grid = this.grid;
        if (grid == null) return;

        grid.GroupDescriptions.Add(new ColumnGroupDescription { Field = "Category", SortDirection = Infragistics.Core.Controls.DataSource.ListSortDirection.Descending });
        grid.GroupDescriptions.Add(new ColumnGroupDescription { Field = "Type",     SortDirection = Infragistics.Core.Controls.DataSource.ListSortDirection.Descending });
        grid.GroupDescriptions.Add(new ColumnGroupDescription { Field = "Contract", SortDirection = Infragistics.Core.Controls.DataSource.ListSortDirection.Descending });
    }

    // LiveSomePricesDisabled / LiveAllPricesDisabled are owned by
    // DataGridToggleLiveAllPrices; IsUpdatingAllPrices, IsUpdatingSomePrices,
    // IsTimerTicking and StartTicking are owned by
    // DataGridLiveDataTickerOnViewInit. These handlers are merged into one
    // sample class and only ever used together.

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void DataGridToggleLiveSomePrices(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        if (this.LiveSomePricesDisabled) return;

        this.IsUpdatingAllPrices = false;
        this.IsUpdatingSomePrices = !this.IsUpdatingSomePrices;

        var liveSomeEditor = this.LiveSomePricesEditor;

        if (this.IsTimerTicking)
        {
            this.IsTimerTicking = false;
            if (liveSomeEditor != null) liveSomeEditor.PrimitiveValue = "Live Prices";
            this.LiveSomePricesDisabled = false;
            this.LiveAllPricesDisabled = false;
        }
        else
        {
            this.StartTicking();
            if (liveSomeEditor != null) liveSomeEditor.PrimitiveValue = "Stop Prices";
            this.LiveSomePricesDisabled = false;
            this.LiveAllPricesDisabled = true;
        }
    }

    // IsUpdatingAllPrices, IsUpdatingSomePrices, IsTimerTicking and StartTicking
    // are owned by DataGridLiveDataTickerOnViewInit; these handlers are merged
    // into one sample class and only ever used together.
    public bool LiveAllPricesDisabled = false;
    public bool LiveSomePricesDisabled = false;

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionButtonClickEventHandler
    public void DataGridToggleLiveAllPrices(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        if (LiveAllPricesDisabled) return;

        this.IsUpdatingAllPrices = !this.IsUpdatingAllPrices;
        this.IsUpdatingSomePrices = false;

        var liveAllEditor = this.LiveAllPricesEditor;
        var liveSomeEditor = this.LiveSomePricesEditor;

        if (this.IsTimerTicking)
        {
            this.IsTimerTicking = false;
            if (liveAllEditor != null) liveAllEditor.PrimitiveValue = "Live All Prices";
            LiveAllPricesDisabled = false;
            LiveSomePricesDisabled = false;
        }
        else
        {
            this.StartTicking();
            if (liveAllEditor != null) liveAllEditor.PrimitiveValue = "Stop All Prices";
            LiveAllPricesDisabled = false;
            LiveSomePricesDisabled = true;
        }
    }

    // UseRowGrouping, OnGridGroupingAdd and OnGridGroupingRemove are owned by
    // DataGridLiveDataTickerOnViewInit; these handlers are merged into one
    // sample class and only ever used together.

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionChangedEventHandler
    public void DataGridApplyLiveDataGrouping(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        this.UseRowGrouping = args.NewValue is bool b && b;
        if (this.UseRowGrouping)
            this.OnGridGroupingAdd();
        else
            this.OnGridGroupingRemove();
    }

    // UseHeatBackground is owned by DataGridLiveDataTickerOnViewInit; these
    // handlers are merged into one sample class and only ever used together.

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionChangedEventHandler
    public void DataGridToggleHeat(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        this.UseHeatBackground = args.NewValue is bool b && b;
        var grid = this.grid;
        if (grid != null) grid.InvalidateVisibleRows();
    }

    //WPF: Infragistics.Controls.Grids.CellStyleRequestedEventHandler
    public void DataGridPriceStyleKey(object sender, CellStyleRequestedEventArgs args)
    {
        var grid = this.grid;
        var row = grid.ActualDataSource.GetItemAtIndex(args.RowNumber) as PortfolioDataItem;
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
    public void DataGridPriceCellUpdating(object sender, TemplateCellUpdatingEventArgs args)
    {
        var content = args.Content;
        var item = args.CellInfo.RowItem as PortfolioDataItem;
        if (item == null) return;

        var priceShiftUp = item.Change >= 0;
        var color = priceShiftUp
            ? new SolidColorBrush(Color.FromArgb(0xFF, 0x4E, 0xB8, 0x62))
            : new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x13, 0x4A));
        var arrow = priceShiftUp ? "â†‘" : "â†“";

        StackPanel panel;
        TextBlock priceText;
        TextBlock arrowText;

        if (content.Content is StackPanel existing)
        {
            panel = existing;
            priceText = (TextBlock)panel.Children[0];
            arrowText = (TextBlock)panel.Children[1];
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
            arrowText = new TextBlock { FontFamily = new FontFamily("Verdana"), FontSize = 13, Margin = new Thickness(2, 0, 0, 0) };
            panel.Children.Add(priceText);
            panel.Children.Add(arrowText);
            content.Content = panel;
        }

        priceText.Text = "$" + System.Math.Round(System.Convert.ToDouble(args.CellInfo.Value), 2).ToString("F2");
        priceText.Foreground = color;
        arrowText.Text = arrow;
        arrowText.Foreground = color;
    }

    //WPF: Infragistics.Controls.Grids.DataBindingEventHandler
    public void DataGridPriceDataBound(object sender, DataBindingEventArgs args)
    {
        var item = args.CellInfo.RowItem as PortfolioDataItem;
        if (item == null) return;

        if (item.PriceHeat > 0)
        {
            var p = item.PriceHeat;
            const double minA = 1.0, maxA = 0.25;
            const double minR = 1.0, maxR = 0.0;
            const double minG = 1.0, maxG = 1.0;
            const double minB = 1.0, maxB = 0.0;

            var a = minA + (maxA - minA) * p;
            var r = (byte)System.Math.Round((minR + (maxR - minR) * p) * 255.0);
            var g = (byte)System.Math.Round((minG + (maxG - minG) * p) * 255.0);
            var b = (byte)System.Math.Round((minB + (maxB - minB) * p) * 255.0);
            args.CellInfo.Background = new SolidColorBrush(Color.FromArgb((byte)System.Math.Round(a * 255.0), r, g, b));
        }
        else if (item.PriceHeat < 0)
        {
            var p = item.PriceHeat * -1.0;
            const double minA = 1.0, maxA = 0.25;
            const double minR = 1.0, maxR = 1.0;
            const double minG = 1.0, maxG = 0.0;
            const double minB = 1.0, maxB = 0.0;

            var a = minA + (maxA - minA) * p;
            var r = (byte)System.Math.Round((minR + (maxR - minR) * p) * 255.0);
            var g = (byte)System.Math.Round((minG + (maxG - minG) * p) * 255.0);
            var b = (byte)System.Math.Round((minB + (maxB - minB) * p) * 255.0);
            args.CellInfo.Background = new SolidColorBrush(Color.FromArgb((byte)System.Math.Round(a * 255.0), r, g, b));
        }
        else
        {
            args.CellInfo.Background = new SolidColorBrush(Colors.White);
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
