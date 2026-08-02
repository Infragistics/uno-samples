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

    private CalendarSeasons _calendarSeasons = null;
    public CalendarSeasons CalendarSeasons
    {
        get
        {
            if (_calendarSeasons == null)
            {
                _calendarSeasons = new CalendarSeasons();
            }
            return _calendarSeasons;
        }
    }

    private CalendarMonths _calendarMonths = null;
    public CalendarMonths CalendarMonths
    {
        get
        {
            if (_calendarMonths == null)
            {
                _calendarMonths = new CalendarMonths();
            }
            return _calendarMonths;
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
