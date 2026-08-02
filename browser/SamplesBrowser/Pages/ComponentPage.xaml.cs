using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using SamplesBrowser.Services;

namespace SamplesBrowser.Pages;

public sealed partial class ComponentPage : Page
{
    public ComponentPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is not ComponentPageParameter param) return;

        HeroTitle.Text = param.GroupName;
        HeroSubtitle.Text = $"Browse all {param.GroupName.ToLowerInvariant()} samples";

        var iconFile = ComponentIconHelper.InvertedIconFileForComponent(param.GroupName);
        HeroIconImage.Source = new SvgImageSource(new Uri($"ms-appx:///Assets/icons/components/inverted/{iconFile}"))
        {
            RasterizePixelWidth  = 112,
            RasterizePixelHeight = 112
        };


        var items = new List<SampleCardItem>();
        foreach (var comp in param.Components)
            foreach (var sample in comp.Samples)
                if (sample.ShowLink)
                    items.Add(new SampleCardItem
                    {
                        Name = sample.Name,
                        ComponentName = comp.Name,
                        Route = sample.Route,
                        Glyph = SampleGlyphMap.GlyphForRoute(sample.Route)
                    });

        SampleCards.ItemsSource = items;
    }

    private void SampleCard_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string route)
            SampleSelected?.Invoke(this, route);
    }

    public event EventHandler<string> SampleSelected;
}
