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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace CG_Project.Views
{
    /// <summary>
    /// Interaction logic for ColorModelsWindow.xaml
    /// </summary>
    public partial class ColorModelsWindow : Window
    {
        private BitmapImage Image;
        private BitmapImage HSLImage;
        private BitmapImage CMYKImage;
        
        private Window BaseWindow;

        public ColorModelsWindow(Window window)
        {
            InitializeComponent();
            BaseWindow = window;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ColorModelImage_MouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Upload_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a Picture",
                Filter = "Images (*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF;*.jpeg",
                Multiselect = false
            };
            if (op.ShowDialog() == true)
            {
                Image = new BitmapImage(new Uri(op.FileName));
            }
            else
            {
                return;
            }

            StockImage.Source = Image;
            //ChangedImage.MouseMove += ChangedImage_OnMouseMove;
            //stride = Image.PixelWidth * 4;
            //size = Image.PixelHeight * stride;
            //pixels = new byte[size];
            //Image.CopyPixels(pixels, stride, 0);
            //if (HSL.IsChecked == true)
            //{
            //    ConvertFromRgbToCmyk();
            //    ConvertToRgbFromHsl();
            //}
            //else
            //{
            //    ConvertToRgbFromHsl();
            //    ConvertFromRgbToCmyk();
            //}

            //Slider.Value = 50;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            BaseWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void Info_OnClick(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Add(new CustomModalWindow());
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

    }
}
