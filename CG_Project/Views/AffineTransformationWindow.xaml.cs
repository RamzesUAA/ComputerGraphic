using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ColorConverter = CG_Project.Services.ColorsService.ColorConverter;
using Color = System.Drawing.Color;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CG_Project.Views
{
    /// <summary>
    /// Interaction logic for ColorModelsWindow.xaml
    /// </summary>
    public partial class AffineTransformationWindow : Window
    {

        private Window BaseWindow;

        public AffineTransformationWindow(Window window)
        {
            InitializeComponent();
            BaseWindow = window;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }


        private void GoBack(object sender, RoutedEventArgs e)
        {
            BaseWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
