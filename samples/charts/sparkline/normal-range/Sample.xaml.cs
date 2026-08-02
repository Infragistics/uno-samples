using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Visible", "Collapsed" };
    public string[] dropDownValues1 { get; } = new string[] { "Visible", "Collapsed" };
    public string[] dropDownNames2 { get; } = new string[] { "0", "10", "15", "20", "25", "30" };
    public string[] dropDownValues2 { get; } = new string[] { "0", "10", "15", "20", "25", "30" };
    public string[] dropDownNames3 { get; } = new string[] { "0", "10", "15", "20", "25", "30" };
    public string[] dropDownValues3 { get; } = new string[] { "0", "10", "15", "20", "25", "30" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private SparklineMixedData _sparklineMixedData = null;
    public SparklineMixedData SparklineMixedData
    {
        get
        {
            if (_sparklineMixedData == null)
            {
                _sparklineMixedData = new SparklineMixedData();
            }
            return _sparklineMixedData;
        }
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
                SparklineDescriptionModule.Register(context);
            }
        return this._componentRenderer;
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
