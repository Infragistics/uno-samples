using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ColorExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            DataContext = this;
            this.InitializeComponent();
        }

        private void Grid_PointerMoved(object sender, PointerRoutedEventArgs args)
        {
        }

        private ObservableCollection<ColorData> _hoveredColors = new ObservableCollection<ColorData>(); 
        public ObservableCollection<ColorData> HoveredColors
        {
            get
            {
                return _hoveredColors;
            }

        }

        private void PixelHovered(object sender, PixelHoveredEventArgs args)
        {
            var color = args.Color;
            _hoveredColors.Add(new ColorData(color));
            if (_hoveredColors.Count > 50000)
            {
                _hoveredColors.RemoveAt(0);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }

    public class ColorData
    {
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set;  }

        public double Alpha { get; set; }

        public DateTime Date { get; set; }


        public ColorData(Windows.UI.Color color)
        {
            this.Alpha = color.A;
            this.Red = color.R;
            this.Blue = color.B;
            this.Green = color.G;
            this.Date = DateTime.Now;
        }
    }
}
