using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;

namespace SamplesBrowser.Pages;

/// <summary>Hosts a sample and lazily loads its original, Shiki-highlighted source.</summary>
public sealed partial class SampleHostPage : Page
{
    private SampleHostParameter _sample;
    private WebView2 _webView;

    public SampleHostPage()
    {
        InitializeComponent();
        ActualThemeChanged += OnActualThemeChanged;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is not SampleHostParameter sample) return;
        _sample = sample;
        ViewSelector.Visibility = sample.IsEmbedded ? Visibility.Collapsed : Visibility.Visible;
        if (sample.IsEmbedded) SampleBorder.Padding = new Thickness(0);
        SampleContent.Content = sample.Factory();
    }

    private void ShowSample_Click(object sender, RoutedEventArgs e)
    {
        SampleButton.IsChecked = true;
        SourceButton.IsChecked = false;
        SampleBorder.Visibility = Visibility.Visible;
        SourceContainer.Visibility = Visibility.Collapsed;
    }

    private void ShowSource_Click(object sender, RoutedEventArgs e)
    {
        SampleButton.IsChecked = false;
        SourceButton.IsChecked = true;
        SampleBorder.Visibility = Visibility.Collapsed;
        SourceContainer.Visibility = Visibility.Visible;
        if (_webView != null || _sample == null) return;

        SourceStatus.Text = "Loading source…";
        SourceStatus.Visibility = Visibility.Visible;
        var sample = _sample;
        var view = _webView = new WebView2();
        view.NavigationCompleted += async (_, args) =>
        {
            if (_webView != view) return;
            if (!args.IsSuccess)
            {
                SourceStatus.Text = "Unable to display source.";
                SourceStatus.Visibility = Visibility.Visible;
                return;
            }
            SourceStatus.Visibility = Visibility.Collapsed;
            await ApplySourceThemeAsync(view);
        };
        RoutedEventHandler loaded = null;
        loaded = async (_, _) =>
        {
            view.Loaded -= loaded;
            await LoadSourceAsync(view, sample);
        };
        view.Loaded += loaded;
        SourceViewHost.Children.Add(view);
    }

    private async Task LoadSourceAsync(WebView2 view, SampleHostParameter sample)
    {
        try
        {
#if HAS_UNO
            var file = await StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///Assets/Source/" + sample.Route + ".html"));
            var html = await FileIO.ReadTextAsync(file);
#else
            // The WinUI browser is unpackaged; read content beside the executable.
            var html = await System.IO.File.ReadAllTextAsync(System.IO.Path.Combine(
                AppContext.BaseDirectory, "Assets", "Source", sample.Route + ".html"));
#endif
            if (_webView != view || _sample != sample) return;
            await view.EnsureCoreWebView2Async();
            if (_webView != view || _sample != sample) return;
            view.NavigateToString(html.TrimStart('\uFEFF'));
        }
        catch (Exception ex)
        {
            if (_webView != view) return;
            SourceStatus.Text = "Unable to load source for this sample. Regenerate the source assets and check WebView availability.";
            SourceStatus.Visibility = Visibility.Visible;
            Console.WriteLine($"[SourceViewer] {sample.Route}: {ex}");
            SourceViewHost.Children.Clear();
            _webView = null;
        }
    }

    private async void OnActualThemeChanged(FrameworkElement sender, object args)
    {
        if (_webView != null) await ApplySourceThemeAsync(_webView);
    }

    private async Task ApplySourceThemeAsync(WebView2 view)
    {
        try
        {
            var theme = ActualTheme == ElementTheme.Dark ? "dark" : "light";
            await view.ExecuteScriptAsync("document.documentElement.dataset.theme = '" + theme + "';");
        }
        catch (Exception ex)
        {
            // A theme change can race the WebView's initial navigation or teardown.
            Console.WriteLine($"[SourceViewer] Theme: {ex.Message}");
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        _sample = null;
#if !HAS_UNO
        _webView?.Close();
#endif
        _webView = null;
        SourceViewHost.Children.Clear();
        SampleContent.Content = null;
    }
}
