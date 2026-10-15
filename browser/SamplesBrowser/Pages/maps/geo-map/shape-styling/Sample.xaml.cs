namespace SamplesBrowser.Pages.Maps.GeoMap.ShapeStyling;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using Infragistics.Controls;
using Infragistics.Controls.Maps;
using System;
using Infragistics.Controls.Charts;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Windows.UI;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.ShapeFileOnViewInit();
        };
    }

    private ShapeDataSource Data;

    public void ShapeFileOnViewInit()
    {
        var geoMap = this.geoMap;

        this.Data = new ShapeDataSource()
        {
            ShapefileSource = new Uri("https://static.infragistics.com/xplatform/shapes/world_countries_all.shp"),
            DatabaseSource = new Uri("https://static.infragistics.com/xplatform/shapes/world_countries_all.dbf")
        };

        var shapeSeries = (GeographicShapeSeries)geoMap.Series[0];
        shapeSeries.ShapefileDataSource = this.Data;
        geoMap.BackgroundContent = null;
    }

    public void ShapeFileStyling(object sender, AssigningShapeStyleEventArgs args)
    {
        // conditional styling based on the Continent field loaded from the DBF file. The event
        // covers a range of items rather than one, so the record is asked for by index.
        var record = args.GetItems(args.StartIndex, args.EndIndex)[0] as Infragistics.Controls.ShapefileRecord;
        var continent = record?.Fields["Continent"] as string;

        Color fill;
        if (continent == "Africa") {
            fill = Color.FromArgb(0xFF, 0xEC, 0xB9, 0x12);
        } else if (continent == "Asia") {
            fill = Color.FromArgb(0xFF, 0xE1, 0x1D, 0x1D);
        } else if (continent == "Europe") {
            fill = Color.FromArgb(0xFF, 0x28, 0x94, 0xEC);
        } else if (continent == "South America" || continent == "North America") {
            fill = Color.FromArgb(0xFF, 0x0C, 0xC3, 0x08);
        } else if (continent == "Australia") {
            fill = Color.FromArgb(0xFF, 0xB4, 0x2A, 0xE9);
        } else {
            fill = Colors.Gray;
        }
        args.Fill = new SolidColorBrush(fill);
        args.Stroke = new SolidColorBrush(Colors.Black);
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
