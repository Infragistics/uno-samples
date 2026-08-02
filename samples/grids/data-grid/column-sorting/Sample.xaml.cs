using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "SortByMultipleColumns", "SortByMultipleColumnsTriState", "SortByOneColumnOnly", "SortByOneColumnOnlyTriState" };
    public string[] dropDownValues1 { get; } = new string[] { "SortByMultipleColumns", "SortByMultipleColumnsTriState", "SortByOneColumnOnly", "SortByOneColumnOnlyTriState" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private RealEstateData _realEstateData = null;
    public RealEstateData RealEstateData
    {
        get
        {
            if (_realEstateData == null)
            {
                _realEstateData = new RealEstateData();
            }
            return _realEstateData;
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
                DataGridDescriptionModule.Register(context);
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
