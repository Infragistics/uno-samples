using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SamplesBrowser.Pages;
using SamplesBrowser.Services;

namespace SamplesBrowser;

/// <summary>Item bound to each card in the home "Browse by component" grid.</summary>
internal sealed class HomeComponentCardItem
{
    public string Name        { get; init; } = "";
    public string GroupName   { get; init; } = "";
    public string Description { get; init; } = "";
    /// <summary>ms-appx URI for the inverted SVG card icon.</summary>
    public Uri?   IconUri     { get; init; }
    /// <summary>The component: tag used by NavView_ItemInvoked logic.</summary>
    public string Tag         { get; init; } = "";
}

public sealed partial class MainPage : Page
{
    private readonly TocService _tocService = new();
    private readonly Dictionary<NavigationViewItem, string> _sampleRoutes = new();
    private readonly Dictionary<NavigationViewItem, Services.TocGroup> _groupItems = new();
    private readonly SampleLink _launchLink = SampleLink.Current();
    private bool _initialized;

    // Segoe MDL2 glyphs used for nav items (component header / sample leaf).
    private static readonly string ComponentGlyph = ((char)0xE9F9).ToString();
    private static readonly string SampleGlyph = ((char)0xE712).ToString();

    public MainPage()
    {
        this.InitializeComponent();
    }

    private async void NavView_Loaded(object sender, RoutedEventArgs e)
    {
        if (_initialized) return;
        _initialized = true;
        NavView.IsEnabled = false;
        try
        {
            if (_launchLink.Route.Length > 0)
            {
                await ComponentAssemblyPreloader.Ready;
                OpenSample(_launchLink.Route, embed: true);
                return;
            }
            await System.Threading.Tasks.Task.WhenAll(
                _tocService.LoadAsync(), ComponentAssemblyPreloader.Ready);
            await ComponentControlWarmup.RunAsync(WarmupHost);
            BuildNavigation();
            ShowHome();
        }
        finally
        {
            StartupProgress.IsActive = false;
            StartupOverlay.Visibility = Visibility.Collapsed;
            NavView.IsEnabled = true;
        }
    }

    private bool OpenSample(string route, bool embed = false)
    {
        if (!SampleRegistry.Samples.TryGetValue(route, out var factory))
        {
            if (embed && route.Length > 0)
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
                NavView.Visibility = Visibility.Collapsed;
                EmbedError.Text = "Sample not found: " + route;
                EmbedError.Visibility = Visibility.Visible;
                return true;
            }
            return false;
        }

        if (embed)
        {
            AppTitleBar.Visibility = Visibility.Collapsed;
            NavView.Visibility = Visibility.Collapsed;
            EmbeddedFrame.Visibility = Visibility.Visible;
            EmbeddedFrame.Navigate(typeof(Pages.SampleHostPage), new SampleHostParameter { Route = route, Factory = factory, IsEmbedded = true });
        }
        else
        {
            HomePanel.Visibility = Visibility.Collapsed;
            ContentFrame.Visibility = Visibility.Visible;
            ContentFrame.Navigate(typeof(Pages.SampleHostPage), new SampleHostParameter { Route = route, Factory = factory });
            foreach (var pair in _sampleRoutes)
                if (pair.Value == route) NavView.SelectedItem = pair.Key;
        }
        return true;
    }

    private void BuildNavigation()
    {
        if (_tocService.Toc == null) return;

        while (NavView.MenuItems.Count > 1)
            NavView.MenuItems.RemoveAt(1);
        _sampleRoutes.Clear();
        _groupItems.Clear();

        foreach (var group in _tocService.Toc.Groups)
        {
            // Group header (e.g. "Charts", "Gauges")
            NavView.MenuItems.Add(new NavigationViewItemHeader { Content = group.Name });

            foreach (var component in group.Components)
            {
                // Component item (e.g. "Category Chart") - expands to show samples
                var compItem = new NavigationViewItem
                {
                    Content = component.Name,
                    SelectsOnInvoked = true,
                    Tag = $"component:{group.Name}:{component.Name}",
                    Icon = ComponentIconHelper.NavIconForComponent(component.Name)
                };
                _groupItems[compItem] = group;

                foreach (var sample in component.Samples)
                {
                    if (!sample.ShowLink) continue;

                    var sampleItem = new NavigationViewItem
                    {
                        Content = sample.Name,
                        Tag     = sample.Route,
                    };
                    compItem.MenuItems.Add(sampleItem);
                    _sampleRoutes[sampleItem] = sample.Route;
                }

                NavView.MenuItems.Add(compItem);
            }
        }
    }

    private void ShowHome()
    {
        NavView.Header = null;
        HomePanel.Visibility    = Visibility.Visible;
        ContentFrame.Visibility = Visibility.Collapsed;

        if (_tocService.Toc == null || HomeComponentCards.ItemsSource != null) return;

        var featured = new[]
        {
            ("Category Chart",  "Charts", "Visualize trends with lines, splines, steps, waterfalls and more across a shared category axis."),
            ("Data Chart",      "Charts", "A highly composable chart supporting unlimited series combinations, annotations, and interactions."),
            ("Pie Chart",       "Charts", "Display proportional data as slices of a circle, with explode, labels, and legend support."),
            ("Sparkline",       "Charts", "Compact inline charts perfect for embedding rich data trends directly within grids and lists."),
        };

        var items = new List<HomeComponentCardItem>();
        foreach (var (compName, groupName, description) in featured)
        {
            items.Add(new HomeComponentCardItem
            {
                Name        = compName,
                GroupName   = groupName,
                Description = description,
                IconUri     = new Uri($"ms-appx:///Assets/icons/components/inverted/{ComponentIconHelper.InvertedIconFileForComponent(compName)}"),
                Tag         = $"component:{groupName}:{compName}"
            });
        }
        HomeComponentCards.ItemsSource = items;
    }

    private void HomeComponent_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not string tag) return;
        NavigateToComponent(tag);
    }

    private void NavigateToComponent(string tag)
    {
        if (_tocService.Toc == null) return;
        var parts = tag.Split(':', 3);
        if (parts.Length < 3) return;
        var group = _tocService.Toc.Groups.Find(g => g.Name == parts[1]);
        var comp  = group?.Components.Find(c => c.Name == parts[2]);
        if (comp == null) return;

        HomePanel.Visibility    = Visibility.Collapsed;
        ContentFrame.Visibility = Visibility.Visible;
        var param = new ComponentPageParameter
        {
            GroupName  = comp.Name,
            Components = new List<TocComponent> { comp }
        };
        ContentFrame.Navigate(typeof(Pages.ComponentPage), param);
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked) return;

        if (args.InvokedItemContainer is NavigationViewItem item)
        {
            if (item.Tag is string tag && tag == "home")
            {
                ShowHome();
                return;
            }

            if (item.Tag is string tag2 && tag2.StartsWith("component:") &&
                _groupItems.TryGetValue(item, out var grp))
            {
                var compName = tag2.Split(':')[2];
                var comp = grp.Components.Find(c => c.Name == compName);
                if (comp != null)
                {
                    HomePanel.Visibility    = Visibility.Collapsed;
                    ContentFrame.Visibility = Visibility.Visible;
                    var param = new Pages.ComponentPageParameter
                    {
                        GroupName  = comp.Name,
                        Components = new List<Services.TocComponent> { comp }
                    };
                    ContentFrame.Navigate(typeof(Pages.ComponentPage), param);
                }
                return;
            }

            if (item.Tag is string route) OpenSample(route);
        }
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        NavView.IsBackButtonVisible = ContentFrame.CanGoBack
            ? NavigationViewBackButtonVisible.Visible
            : NavigationViewBackButtonVisible.Collapsed;

        if (e.Content is Pages.SampleHostPage sample && e.Parameter is Pages.SampleHostParameter parameter)
            SetHeader(sample, parameter.Route);
        else
            NavView.Header = null;

        if (e.Content is Pages.ComponentPage cp)
            cp.SampleSelected += OnComponentPageSampleSelected;
    }

    private void OnComponentPageSampleSelected(object? sender, string route)
    {
        OpenSample(route);
    }

    private void SetHeader(Pages.SampleHostPage sample, string route)
    {
        NavView.Header = sample.CreateHeader(GetSampleTitle(route),
            (Style)Application.Current.Resources["NavHeaderTextStyle"]);
    }

    private string GetSampleTitle(string route)
    {
        if (_tocService.Toc == null) return route;
        foreach (var group in _tocService.Toc.Groups)
            foreach (var comp in group.Components)
                foreach (var sample in comp.Samples)
                    if (sample.Route == route)
                        return comp.Name + " - " + sample.Name;
        return route;
    }
}
