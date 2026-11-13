using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

using Infragistics.Controls.Charts;
using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

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

    private AirplaneShape _airplaneShape = null;
    public AirplaneShape AirplaneShape
    {
        get
        {
            if (_airplaneShape == null)
            {
                AirplaneShape.Fetch().ContinueWith((t) => {_airplaneShape = t.Result;  OnPropertyChanged("AirplaneShape"); }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
            }
            return _airplaneShape;
        }
    }

    private AirplaneSeats _airplaneSeats = null;
    public AirplaneSeats AirplaneSeats
    {
        get
        {
            if (_airplaneSeats == null)
            {
                AirplaneSeats.Fetch().ContinueWith((t) => {_airplaneSeats = t.Result;  OnPropertyChanged("AirplaneSeats"); }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
            }
            return _airplaneSeats;
        }
    }

    public void AirplaneSeatFillStyling(object sender, AssigningShapeStyleEventArgs args)
    {
        // the event covers a range of items rather than one, so the record is asked for by index
        var record = args.GetItems(args.StartIndex, args.EndIndex)[0] as AirplaneSeatsItem;

        args.Opacity = 1.0;
        args.StrokeThickness = 0.5;
        args.Stroke = new SolidColorBrush(Colors.Black);
        args.Fill = new SolidColorBrush(Colors.White);
        if (record == null)
        {
            return;
        }
        // A seat is coloured by the cabin it is in, and a seat already sold is grey whichever cabin
        // that is -- so status is read after class rather than before it.
        switch (record.Class)
        {
            case "First": args.Fill = new SolidColorBrush(Colors.DodgerBlue); break;
            case "Business": args.Fill = new SolidColorBrush(Colors.LimeGreen); break;
            case "Premium": args.Fill = new SolidColorBrush(Colors.Orange); break;
            case "Economy": args.Fill = new SolidColorBrush(Colors.Red); break;
        }
        if (record.Status == "Sold")
        {
            args.Fill = new SolidColorBrush(Colors.Gray);
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

// Formats a bound value into a sentence, the way Binding.StringFormat does on WPF.
//
// WinUI's Binding has no StringFormat, so a tooltip that labels its values has nowhere to put the
// label. The format string travels as the ConverterParameter instead, and it is the same string the
// shared Xaml.xaml writes, so the two spellings of the item say the same thing.
public class StringFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var format = parameter as string;
        if (string.IsNullOrEmpty(format))
        {
            return value == null ? null : value.ToString();
        }
        return string.Format(CultureInfo.CurrentCulture, format, value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
