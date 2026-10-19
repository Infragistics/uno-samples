using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

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

    private PolicyRatesByQuarter _policyRatesByQuarter = null;
    public PolicyRatesByQuarter PolicyRatesByQuarter
    {
        get
        {
            if (_policyRatesByQuarter == null)
            {
                _policyRatesByQuarter = new PolicyRatesByQuarter();
            }
            return _policyRatesByQuarter;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

