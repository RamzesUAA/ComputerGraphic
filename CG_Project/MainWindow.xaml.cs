using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CG_Project.Views;

namespace CG_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private FractalWindow fractalWindow;
        private ColorModelsWindow colorModelsWindow;
        private AffineTransformationWindow affineTransformationWindow;

        private void ColorSchemes_OnClick(object sender, RoutedEventArgs e)
        {
            if (colorModelsWindow == null)
            {
                colorModelsWindow = new ColorModelsWindow(this);
            }

            this.Visibility = Visibility.Hidden;
            colorModelsWindow.Show();
        }

        private void BuildFractal_OnClick(object sender, RoutedEventArgs e)
        {
            if (fractalWindow == null)
            {
                fractalWindow = new FractalWindow(this);
            }

            this.Visibility = Visibility.Hidden;
            fractalWindow.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void AffineTransformation_Click(object sender, RoutedEventArgs e)
        {
            if (affineTransformationWindow == null)
            {
                affineTransformationWindow = new AffineTransformationWindow(this);
            }

            this.Visibility = Visibility.Hidden;
            affineTransformationWindow.Show();
        }
    }
}
