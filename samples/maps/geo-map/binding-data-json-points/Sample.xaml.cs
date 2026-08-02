using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Charts;
using Infragistics.Controls.Maps;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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

            this.MapBindingDataJsonPointsOnViewInit();
        };
    }


    public class WorldPlaceJson
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Pop { get; set; }
        public string Country { get; set; }
        public bool Cap { get; set; }
    }

    //WPF: System.Action
    public async void MapBindingDataJsonPointsOnViewInit()
    {
        var map = this.map;
        var url = "https://static.infragistics.com/xplatform/data/WorldCities.json";
        var client = new HttpClient();
        var json = await client.GetStringAsync(url);
        var array = JArray.Parse(json);
        var geoLocations = new List<WorldPlaceJson>();
        foreach (var item in array)
        {
            if (item.Value<bool>("cap") == false) continue;
            geoLocations.Add(new WorldPlaceJson
            {
                Name = item.Value<string>("name"),
                Lat = item.Value<double>("lat"),
                Lon = item.Value<double>("lon"),
                Pop = item.Value<double>("pop"),
                Country = item.Value<string>("country"),
                Cap = item.Value<bool>("cap")
            });
        }
        var series = new GeographicSymbolSeries
        {
            ItemsSource = geoLocations,
            LatitudeMemberPath = "Lat",
            LongitudeMemberPath = "Lon",
            MarkerBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
            MarkerOutline = new SolidColorBrush(Color.FromArgb(255, 135, 5, 255)),
            Thickness = 1,
            MarkerType = MarkerType.Circle
        };
        map.Series.Add(series);
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
