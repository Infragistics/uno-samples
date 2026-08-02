using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Root", "Groups", "Both", "None" };
    public string[] dropDownValues1 { get; } = new string[] { "Root", "Groups", "Both", "None" };
    public string[] dropDownNames2 { get; } = new string[] { "List", "Cells", "RowTop", "RowBottom", "None" };
    public string[] dropDownValues2 { get; } = new string[] { "List", "Cells", "RowTop", "RowBottom", "None" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private ProductOrders _productOrders = null;
    public ProductOrders ProductOrders
    {
        get
        {
            if (_productOrders == null)
            {
                _productOrders = new ProductOrders();
            }
            return _productOrders;
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
