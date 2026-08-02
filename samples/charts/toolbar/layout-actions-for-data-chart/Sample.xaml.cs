using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Description;
using Infragistics.Controls.Layouts;
using Infragistics.Controls.Charts;

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
    public void ToolbarToggleAnnotations(object sender, ToolCommandEventArgs e)
    {
    	var target = (XamDataChart)((XamToolbar)sender).Target;
    	bool enable = false;
        switch (e.Command.CommandId)
    	{
    		case "EnableTooltips":
    			enable = (bool)e.Command.ArgumentsList[0].Value;
    			if (enable)
    			{
    				target.Series.Add(new DataToolTipLayer());
    			}
    			else
    			{
    				Series toRemove = null;
    				foreach (var s in target.Series)
    				{
    					if (s is DataToolTipLayer)
    					{
    						toRemove = s;
    					}
    				}
    				if (toRemove != null)
    				{
    					target.Series.Remove(toRemove);
    				}
    			}
    			break;
    		case "EnableCrosshairs":
    			enable = (bool)e.Command.ArgumentsList[0].Value;
    			if (enable)
    			{
    				target.Series.Add(new CrosshairLayer());
    			}
    			else
    			{
    				Series toRemove = null;
    				foreach (var s in target.Series)
    				{
    					if (s is CrosshairLayer)
    					{
    						toRemove = s;
    					}
    				}
    				if (toRemove != null)
    				{
    					target.Series.Remove(toRemove);
    				}
    			}
    			break;
    		case "EnableFinalValues":
    			enable = (bool)e.Command.ArgumentsList[0].Value;
    			if (enable)
    			{
    				target.Series.Add(new FinalValueLayer());
    			}
    			else
    			{
    				Series toRemove = null;
    				foreach (var s in target.Series)
    				{
    					if (s is FinalValueLayer)
    					{
    						toRemove = s;
    					}
    				}
    				if (toRemove != null)
    				{
    					target.Series.Remove(toRemove);
    				}
    			}
    			break;
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
