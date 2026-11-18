using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;
using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Maps;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private MapAzureImageryApiKey mapAzureImageryApiKey = new MapAzureImageryApiKey();

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
                AzureMapsImageryDescriptionModule.Register(context);
            }
        return this._componentRenderer;
        }
    }

    public void EditorChangeAzureImageryApiKey(object sender, PropertyEditorPropertyDescriptionChangedEventArgs args)
    {
        var apiKey = this.mapAzureImageryApiKey;
        apiKey.Key = args.NewValue?.ToString()?.Trim() ?? "";
    }

    public void EditorButtonAzureImagerySetApiKey(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var apiKey = this.mapAzureImageryApiKey;
        var map = this.map;
        var imagery = map.BackgroundContent as AzureMapsImagery;
        if (imagery != null && !string.IsNullOrEmpty(apiKey.Key))
        {
            imagery.ApiKey = apiKey.Key;
        }
    }

    public void EditorButtonAzureImageryRoad(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var map = this.map;
        var imagery = map.BackgroundContent as AzureMapsImagery;
        if (imagery != null)
        {
            imagery.ImageryStyle = AzureMapsImageryStyle.Road;
        }
    }

    public void EditorButtonAzureImagerySatellite(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var map = this.map;
        var imagery = map.BackgroundContent as AzureMapsImagery;
        if (imagery != null)
        {
            imagery.ImageryStyle = AzureMapsImageryStyle.Satellite;
        }
    }

    public void EditorButtonAzureImageryDarkGrey(object sender, PropertyEditorPropertyDescriptionButtonClickEventArgs args)
    {
        var map = this.map;
        var imagery = map.BackgroundContent as AzureMapsImagery;
        if (imagery != null)
        {
            imagery.ImageryStyle = AzureMapsImageryStyle.DarkGrey;
        }
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
/// The Azure Maps key typed into the property editor, held until the Set Key button applies it.
///
/// The text box and the button are separate editors with separate handlers, so the value has to
/// live somewhere both can reach. Applying the key on every keystroke would send tile requests with
/// partial keys, which is why the text box only records it here.
/// </summary>
public class MapAzureImageryApiKey
{
    public string Key { get; set; } = "";
}
