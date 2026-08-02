using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Maps;
using System;
using System.Windows;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using System.Collections.Generic;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Australia", "Caribbean", "Egypt", "European", "Hawaii", "Japan", "Poland", "SouthAfrica", "UnitedKingdom", "UnitedStates", "Uruguay" };
    public string[] dropDownValues1 { get; } = new string[] { "Australia", "Caribbean", "Egypt", "European", "Hawaii", "Japan", "Poland", "SouthAfrica", "UnitedKingdom", "UnitedStates", "Uruguay" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

            this.MapNavigationOnViewInit();
        };
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
                DataChartInteractivityDescriptionModule.Register(context);
                GeographicMapDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    //WPF: System.Action
    public void MapNavigationOnViewInit()
    {
        var map = this.map;
        var region = new Rect(-134.5, 16.0, 70.0, 37.0);
        map.ZoomToGeographic(region);
    }

    public Dictionary<string, Rect> MapRegions = new Dictionary<string, Rect>
    {
        { "Australia", new Rect(81.5, -52.0, 98.0, 56.0) },
        { "Caribbean", new Rect(-92.9, 5.4, 35.1, 25.8) },
        { "Egypt", new Rect(19.3, 19.9, 19.3, 13.4) },
        { "European", new Rect(-36.0, 31.0, 98.0, 38.0) },
        { "Hawaii", new Rect(-161.2, 18.5, 6.6, 4.8) },
        { "Japan", new Rect(122.7, 29.4, 27.5, 17.0) },
        { "Poland", new Rect(13.0, 48.0, 11.0, 9.0) },
        { "SouthAfrica", new Rect(9.0, -37.1, 26.0, 17.8) },
        { "UnitedKingdom", new Rect(-15.0, 49.5, 22.5, 8.0) },
        { "UnitedStates", new Rect(-134.5, 16.0, 70.0, 37.0) },
        { "Uruguay", new Rect(-62.1, -35.7, 10.6, 7.0) }
    };

    //WPF: Infragistics.Controls.Layouts.PropertyEditorPropertyDescriptionChangedEventHandler
    public void EditorChangeMapRegion(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var map = this.map;
        var name = args.NewValue.ToString();
        if (this.MapRegions.TryGetValue(name, out var region))
        {
            map.ZoomToGeographic(region);
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
