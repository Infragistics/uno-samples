using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace Sample;

public sealed partial class Sample : UserControl, INotifyPropertyChanged
{
    public string[] allowedPositions1 { get; } = new string[] {  };
    public string[] allowedPositions2 { get; } = new string[] {  };

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private RoadblocksToSuccess _roadblocksToSuccess = null;
    public RoadblocksToSuccess RoadblocksToSuccess
    {
        get
        {
            if (_roadblocksToSuccess == null)
            {
                _roadblocksToSuccess = new RoadblocksToSuccess();
            }
            return _roadblocksToSuccess;
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
