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

    private WebsiteVisitsByHour _websiteVisitsByHour = null;
    public WebsiteVisitsByHour WebsiteVisitsByHour
    {
        get
        {
            if (_websiteVisitsByHour == null)
            {
                _websiteVisitsByHour = new WebsiteVisitsByHour();
            }
            return _websiteVisitsByHour;
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

