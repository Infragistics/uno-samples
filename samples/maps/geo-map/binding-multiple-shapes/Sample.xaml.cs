using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Maps;
using Infragistics.Controls;
using System;
using Infragistics.Controls.Charts;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Foundation;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.MapMultipleShapesLoad();
        };
    }

    private WorldCities _worldCities = null;
    public WorldCities WorldCities
    {
        get
        {
            if (_worldCities == null)
            {
                _worldCities = new WorldCities();
            }
            return _worldCities;
        }
    }

    private MapMultipleShapesReaders mapMultipleShapesReaders = new MapMultipleShapesReaders();

    /// <summary>
    /// Three shapefiles, each read into the series that draws it. A shapefile is fetched and parsed
    /// asynchronously, so each source is given the method that turns its records into data when it
    /// arrives — those methods belong to the supporting item this one requires.
    /// </summary>
    public void MapMultipleShapesLoad()
    {
        var root = "https://static.infragistics.com/xplatform/shapes/";
        var readers = this.mapMultipleShapesReaders;

        var sdsPolygons = new ShapeDataSource();
        sdsPolygons.ImportCompleted += (s, e) => readers.ReadPolygons(s, e, this.geoMap);
        sdsPolygons.ShapefileSource = new Uri(root + "WorldCountries.shp");
        sdsPolygons.DatabaseSource = new Uri(root + "WorldCountries.dbf");

        var sdsPolylines = new ShapeDataSource();
        sdsPolylines.ImportCompleted += (s, e) => readers.ReadPolylines(s, e, this.geoMap);
        sdsPolylines.ShapefileSource = new Uri(root + "WorldCableRoutes.shp");
        sdsPolylines.DatabaseSource = new Uri(root + "WorldCableRoutes.dbf");

        var sdsLocations = new ShapeDataSource();
        sdsLocations.ImportCompleted += (s, e) => readers.ReadPoints(s, e, this.geoMap);
        sdsLocations.ShapefileSource = new Uri(root + "WorldCities.shp");
        sdsLocations.DatabaseSource = new Uri(root + "WorldCities.dbf");

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

/// <summary>
/// What each of the three shapefiles becomes once it has loaded: records turned into data, bound to
/// the series that draws them.
///
/// These are handlers for a shapefile converter's ImportCompleted, and the converter is created in
/// code by the item that requires this one — so there is nothing in a description to bind them to,
/// and they are not initializers either. A supporting item is what they are: a type asked for by
/// name, whose methods the loader wires up.
/// </summary>
public class MapMultipleShapesReaders
{
    /// <summary>Country shapes, with the fields the tooltip shows taken from the .DBF file.</summary>
    public void ReadPolygons(object? sender, AsyncCompletedEventArgs e, XamGeographicMap map)
    {
        if (e.Error != null || sender is not ShapeDataSource sds) return;

        var geoPolygons = new List<object>();
        for (int i = 0; i < sds.Count; i++)
        {
            var record = sds[i];
            // using field/column names from .DBF file
            geoPolygons.Add(new
            {
                Points = record.Points,
                Name = record.Fields["NAME"],
                Gdp = record.Fields["GDP"],
                Population = record.Fields["POPULATION"]
            });
        }

        var polygonSeries = (GeographicShapeSeries)map.Series[0];
        polygonSeries.ItemsSource = geoPolygons;
    }

    /// <summary>Connections between places, each record one path.</summary>
    public void ReadPolylines(object? sender, AsyncCompletedEventArgs e, XamGeographicMap map)
    {
        if (e.Error != null || sender is not ShapeDataSource sds) return;

        var geoPolylines = new List<object>();
        for (int i = 0; i < sds.Count; i++)
        {
            var record = sds[i];
            geoPolylines.Add(new
            {
                Points = record.Points,
                Name = record.Fields["Name"],
                Capacity = record.Fields["CapacityG"],
                Distance = record.Fields["DistanceKM"],
                // a route whose in-service year is blank has no InService field at all
                Service = record.Fields.ContainsKey("InService") ? record.Fields["InService"] : null
            });
        }

        var lineSeries = (GeographicPolylineSeries)map.Series[1];
        lineSeries.ItemsSource = geoPolylines;
    }

    /// <summary>Cities with a known population, one point per record.</summary>
    public void ReadPoints(object? sender, AsyncCompletedEventArgs e, XamGeographicMap map)
    {
        if (e.Error != null || sender is not ShapeDataSource sds) return;

        var geoLocations = new List<object>();
        for (int i = 0; i < sds.Count; i++)
        {
            var record = sds[i];
            var pop = Convert.ToDouble(record.Fields["POPULATION"]);
            if (pop > 0)
            {
                // each of these records holds a single point
                geoLocations.Add(new
                {
                    Latitude = record.Points[0][0].Y,
                    Longitude = record.Points[0][0].X,
                    City = record.Fields["NAME"],
                    Population = pop
                });
            }
        }

        var symbolSeries = (GeographicSymbolSeries)map.Series[2];
        symbolSeries.ItemsSource = geoLocations;
    }
}
