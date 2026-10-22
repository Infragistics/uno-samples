using System.Reflection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uno.UI.Hosting;

namespace WasmRuntimeRegression;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = UnoPlatformHostBuilder.Create()
            .App(() => new RegressionApp())
            .UseWebAssembly()
            .Build();
        await host.RunAsync();
    }
}

public sealed class RegressionApp : Application
{
    private Window window;

    public RegressionApp()
    {
        Resources.MergedDictionaries.Add(new XamlControlsResources());
        UnhandledException += (_, args) => Console.WriteLine("REGRESSION FAILURE: " + args.Exception);
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        try
        {
            VerifyAndShow();
        }
        catch (Exception exception)
        {
            Console.WriteLine("REGRESSION FAILURE: " + exception);
            throw;
        }
    }

    private void VerifyAndShow()
    {
        if (GetterlessAttachedProperty.ValueProperty == null)
        {
            throw new InvalidOperationException("The attached property was not registered.");
        }

        // Reproduces a getter removed by trimming while retaining the registered property.
        var helper = Type.GetType("Uno.UI.DataBinding.BindingPropertyHelper, Uno.UI", true);
        var getPropertyType = helper.GetMethod("GetPropertyType", BindingFlags.Public | BindingFlags.Static);
        var path = $"({typeof(GetterlessAttachedProperty).Namespace}:{nameof(GetterlessAttachedProperty)}.Value)";
        var actual = getPropertyType.Invoke(null, new object[] { typeof(TextBox), path, false });
        if (!Equals(actual, typeof(int)))
        {
            throw new InvalidOperationException($"Attached property type: expected System.Int32, got {actual ?? "null"}.");
        }

        Console.WriteLine("REGRESSION PASS: attached property type resolves without a CLR getter.");
        window = new Window
        {
            Content = new TextBox { Text = "Rendering and editing work", Width = 420, Height = 80 }
        };
        window.Activate();
    }
}

public static class GetterlessAttachedProperty
{
    public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
        "Value", typeof(int), typeof(GetterlessAttachedProperty), new PropertyMetadata(0));
}
