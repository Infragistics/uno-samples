using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Charts;
using Infragistics.Controls.Maps;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Microsoft.UI;
using Windows.Foundation;

namespace SamplesBrowser.Pages.Maps.GeoMap.BindingDataCsv;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.MapBindingDataCsvOnViewInit();
        };
    }


    public class WorldPlaceCsv
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Population { get; set; }
    }

    //WPF: System.Action
    public async void MapBindingDataCsvOnViewInit()
    {
        var map = this.map;
        var url = "https://static.infragistics.com/xplatform/data/UsaCitiesPopulation.csv";
        var client = new HttpClient();
        var csv = await client.GetStringAsync(url);
        var csvLines = csv.Split('\n');
        var geoLocations = new List<WorldPlaceCsv>();
        for (int i = 1; i < csvLines.Length; i++)
        {
            var columns = csvLines[i].Split(',');
            if (columns.Length < 4) continue;
            geoLocations.Add(new WorldPlaceCsv
            {
                Name = columns[0],
                Latitude = double.Parse(columns[1]),
                Longitude = double.Parse(columns[2]),
                Population = double.Parse(columns[3])
            });
        }
        //var series = new GeographicHighDensityScatterSeries
        //{
        //    Name = "hdSeries",
        //    ItemsSource = geoLocations,
        //    LatitudeMemberPath = "Latitude",
        //    LongitudeMemberPath = "Longitude",
        //    HeatMaximumColor = Colors.Red,
        //    HeatMinimumColor = Colors.Black,
        //    HeatMinimum = 0,
        //    HeatMaximum = 5,
        //    PointExtent = 1,
        //    MouseOverEnabled = true
        //};
        //map.Series.Add(series);

        var geoBounds = new Rect(-130, 15, Math.Abs(-130 + 65), Math.Abs(50 - 15));
        map.ZoomToGeographic(geoBounds);
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
