using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace SamplesBrowser.Pages;

/// <summary>
/// A generic host page that lazily instantiates a sample UserControl.
/// The navigation parameter must be a <see cref="Func{Object}"/> factory
/// that creates the sample instance (provided by <see cref="SampleRegistry"/>).
/// </summary>
public sealed partial class SampleHostPage : Page
{
    public SampleHostPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        // Instantiate the sample only when this page becomes active (lazy loading).
        if (e.Parameter is Func<object> factory)
        {
            SampleContent.Content = factory();
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);

        // Release the sample content when navigating away to free resources.
        SampleContent.Content = null;
    }
}
