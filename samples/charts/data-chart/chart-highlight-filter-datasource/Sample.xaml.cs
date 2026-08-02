using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

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

    private OnlineTrafficHighlightTotals _onlineTrafficHighlightTotals = null;
    public OnlineTrafficHighlightTotals OnlineTrafficHighlightTotals
    {
        get
        {
            if (_onlineTrafficHighlightTotals == null)
            {
                _onlineTrafficHighlightTotals = new OnlineTrafficHighlightTotals();
            }
            return _onlineTrafficHighlightTotals;
        }
    }

    private OnlineTrafficHighlightDesktopOnly _onlineTrafficHighlightDesktopOnly = null;
    public OnlineTrafficHighlightDesktopOnly OnlineTrafficHighlightDesktopOnly
    {
        get
        {
            if (_onlineTrafficHighlightDesktopOnly == null)
            {
                _onlineTrafficHighlightDesktopOnly = new OnlineTrafficHighlightDesktopOnly();
            }
            return _onlineTrafficHighlightDesktopOnly;
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
                DataChartCoreDescriptionModule.Register(context);
                DataChartCategoryDescriptionModule.Register(context);
                DataChartInteractivityDescriptionModule.Register(context);
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
