using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Portable.Description;

namespace SamplesBrowser.Pages.Grids.DataGrid.CellMerging;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] dropDownNames1 { get; } = new string[] { "Always", "Never", "OnlyWhenSorted" };
    public string[] dropDownValues1 { get; } = new string[] { "Always", "Never", "OnlyWhenSorted" };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private FinancialDataService _financialDataService = null;
    public FinancialDataService FinancialDataService
    {
        get
        {
            if (_financialDataService == null)
            {
                FinancialDataService.FetchData().ContinueWith((t) => {_financialDataService = t.Result;  OnPropertyChanged("FinancialDataService"); }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
            }
            return _financialDataService;
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
