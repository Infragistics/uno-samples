namespace SamplesBrowser.Pages.Charts.DataChart.VerticalAreaChart;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;


public sealed partial class Sample : UserControl, INotifyPropertyChanged
{

    public Sample()
    {

        this.InitializeComponent();

        DataContext = this;

        this.Loaded += (s, e) => {

        };
    }

    private PopulationByAgeGroup _populationByAgeGroup = null;
    public PopulationByAgeGroup PopulationByAgeGroup
    {
        get
        {
            if (_populationByAgeGroup == null)
            {
                _populationByAgeGroup = new PopulationByAgeGroup();
            }
            return _populationByAgeGroup;
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
