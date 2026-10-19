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

    private FeatureSatisfactionScores _featureSatisfactionScores = null;
    public FeatureSatisfactionScores FeatureSatisfactionScores
    {
        get
        {
            if (_featureSatisfactionScores == null)
            {
                _featureSatisfactionScores = new FeatureSatisfactionScores();
            }
            return _featureSatisfactionScores;
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

