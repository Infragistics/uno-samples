using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Markup;

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

    private CountryRenewableElectricity _countryRenewableElectricity = null;
    public CountryRenewableElectricity CountryRenewableElectricity
    {
        get
        {
            if (_countryRenewableElectricity == null)
            {
                _countryRenewableElectricity = new CountryRenewableElectricity();
            }
            return _countryRenewableElectricity;
        }
    }


    //WPF: Infragistics.Controls.Layouts.ToolCommandEventHandler
    public void ColorEditorToggleSeriesBrush(object sender, ToolCommandEventArgs e)
    {
    	var target = (XamDataChart)((XamToolbar)sender).Target;
    	var color = e.Command.ArgumentsList[0].Value;
    	if (e.Command.CommandId == "ToggleSeriesBrush" && target.Series.Count != 0)
    	{
    		Series series = target.Series[0];
    		series.Brush = (Brush)XamlBindingHelper.ConvertValue(typeof(Brush), color.ToString());
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
