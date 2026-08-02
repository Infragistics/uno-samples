using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Maps;
using System;
using Infragistics.Controls;

namespace SamplesBrowser.Pages.Maps.GeoMap.ShapeStyling;

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

    //WPF: System.Action
    public void ShapeFileOnViewInit()
    {
        var geoMap = this.geoMap;

        this.Data = new ShapeDataSource()
        {
            ShapefileSource = new Uri("https://static.infragistics.com/xplatform/shapes/world_countries_all.shp"),
            DatabaseSource = new Uri("https://static.infragistics.com/xplatform/shapes/world_countries_all.dbf")
        };

        var shapeSeries = (GeographicShapeSeries)geoMap.Series[0];
        shapeSeries.ItemsSource = this.Data;
        geoMap.BackgroundContent = null;
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
