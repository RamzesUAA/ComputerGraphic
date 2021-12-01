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
    public partial class ColorModelsWindow : Window
    {
        private BitmapImage Image;
        private BitmapImage HSLImage;

        private int stride = 0;
        private int size = 0;

        byte[] pixels;
        private byte[] pixelsHSL;
        private byte[] pixelsCMYK;

        private Window BaseWindow;

        public ColorModelsWindow(Window window)
        {
            InitializeComponent();
            BaseWindow = window;
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
            ChangedImage.Visibility = Visibility.Visible;
            StartWindow.Visibility = Visibility.Hidden;
            ChangedImage.MouseMove += ChangedImage_OnMouseMove;
            stride = Image.PixelWidth * 4;
            size = Image.PixelHeight * stride;
            pixels = new byte[size];
            Image.CopyPixels(pixels, stride, 0);

            ConvertToRgbFromHsl();
            ConvertFromRgbToCmyk();

            LightnessSlider.Value = 50;
        }

        public int[] CountImagePositionChangedImage(MouseEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(ChangedImage);
            double pixelWidth = Image.PixelWidth;
            double pixelHeight = Image.PixelHeight;
            double x = pixelWidth * p.X / ChangedImage.ActualWidth;
            double y = pixelHeight * p.Y / ChangedImage.ActualHeight;
            return new[] { (int)x, (int)y };
        }

        private void ChangedImage_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Image != null)
            {
                int[] position = CountImagePositionChangedImage(e);
                int x = position[0];
                int y = position[1];
                int index = y * stride + 4 * x;
                if (index != 0 && index % 4 != 0)
                {
                    index += 4 - index % 4;
                }

                if (index + 4 < size)
                {

                    Color color = Color.FromArgb(pixelsHSL[index + 2], pixelsHSL[index + 1], pixelsHSL[index]);
                    double[] hsl = ColorConverter.RgbToHslChange(pixelsHSL[index + 2], pixelsHSL[index + 1], pixelsHSL[index]);
                    HSLLabel.Content = $"{Math.Round(hsl[0], 0)}, {Math.Round(hsl[1], 2)}, {Math.Round(hsl[2], 2)}";
                    RGBLabel.Content = $"{pixelsHSL[index + 2]}, {pixelsHSL[index + 1]}, {pixelsHSL[index]}";

                    Color color1 = Color.FromArgb(pixelsCMYK[index + 2], pixelsCMYK[index + 1], pixelsCMYK[index]);
                    double[] cmyk = ColorConverter.RgbToCmyk(color1);
                    CMYKLabel.Content = $"{Math.Round(cmyk[0] * 100, 2)}% {Math.Round(cmyk[1] * 100, 2)}% {Math.Round(cmyk[2] * 100, 2)}% {Math.Round(cmyk[3] * 100, 2)}%";
                }
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (Image != null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Image file (*.png) | *.png";
                if (saveFileDialog1.ShowDialog() == false)
                    return;

                string filename = saveFileDialog1.FileName;
                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(HSLImage));

                    encoder.Save(stream);
                }
            }
            else
            {
                MessageBox.Show("No file to save", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConvertFromRgbToCmyk()
        {
            pixelsCMYK = new byte[size];

            Image.CopyPixels(pixelsCMYK, stride, 0);
            for (int i = 0; i < size; i += 4)
            {
                if (i + 4 < pixels.Length)
                {
                    Color color = Color.FromArgb(pixelsCMYK[i + 2], pixelsCMYK[i + 1], pixelsCMYK[i]);
                    double[] cmyk = ColorConverter.RgbToCmyk(color);
                    byte[] RGB = ColorConverter.CmykToRgb(cmyk[0], cmyk[1], cmyk[2], cmyk[3]);
                    pixelsCMYK[i] = RGB[2];
                    pixelsCMYK[i + 1] = RGB[1];
                    pixelsCMYK[i + 2] = RGB[0];
                }
            }
            WriteableBitmap newImage = new WriteableBitmap(Image.PixelWidth, Image.PixelHeight, Image.DpiX, Image.DpiY, Image.Format, Image.Palette);
            newImage.WritePixels(new Int32Rect(0, 0, Image.PixelWidth, Image.PixelHeight), pixelsCMYK, stride, 0);
        }

        private void ConvertToRgbFromHsl()
        {
            pixelsHSL = new byte[size];

            Image.CopyPixels(pixelsHSL, stride, 0);
            for (int i = 0; i < size; i += 4)
            {
                if (i + 4 < pixels.Length)
                {
                    Color color = Color.FromArgb(pixelsHSL[i + 2], pixelsHSL[i + 1], pixelsHSL[i]);
                    double[] hsv = ColorConverter.RgbToHslChange(pixelsHSL[i + 2], pixelsHSL[i + 1], pixelsHSL[i]);
                    byte[] RGB = ColorConverter.HslToRgb(hsv[0], hsv[1], hsv[2]);
                    pixelsHSL[i] = RGB[2];
                    pixelsHSL[i + 1] = RGB[1];
                    pixelsHSL[i + 2] = RGB[0];
                }
            }
            WriteableBitmap newImage = new WriteableBitmap(Image.PixelWidth, Image.PixelHeight, Image.DpiX, Image.DpiY, Image.Format, Image.Palette);
            newImage.WritePixels(new Int32Rect(0, 0, Image.PixelWidth, Image.PixelHeight), pixelsHSL, stride, 0);
            HSLImage = newImage.ToBitmapImage();
            ChangedImage.Source = HSLImage;
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

        private void LightnessSlider_LostMouseCapture(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RangeBaseSaturation_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Saturation != null && e != null && Image != null)
                Saturation.Content = Math.Round(e.NewValue, 0) + "%";
            if (Image != null)
            {
                ChangeSaturationLightness();
            }
        }

        private void RangeBaseLightness_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Persents != null && e != null && Image != null)
                Persents.Content = Math.Round(e.NewValue, 0) + "%";
            if (Image != null)
            {
                ChangeSaturationLightness();
            }
        }

        private void ChangeSaturationLightnessArea()
        {
            int x = (int)Math.Min(LastPoint.X, StartPoint.X);
            int y = (int)Math.Min(LastPoint.Y, StartPoint.Y);
            int width = (int)Math.Abs(LastPoint.X - StartPoint.X) + 1;
            int height = (int)Math.Abs(LastPoint.Y - StartPoint.Y) + 1;

            byte[] pixelsData = new byte[size];

            Image.CopyPixels(pixelsData, stride, 0);


            int indxPixel1 = (int)(stride * y);
            indxPixel1 += x * 4;

            int pixel2 = indxPixel1 + (int)(stride * height);


            for (int j = indxPixel1; j < pixel2 + width * 4; j += (int)(4 * Image.Width))
            {
                for (int i = j; i < j + width * 4; i += 4)
                {
                    if (i + 4 < pixels.Length)
                    {
                        double[] hsl;
                        byte[] RGB;
                        Color color = Color.FromArgb(pixelsData[i + 2], pixelsData[i + 1], pixelsData[i]);
                        hsl = ColorConverter.RgbToHslChange(pixelsData[i + 2], pixelsData[i + 1], pixelsData[i], LightnessSlider.Value / 50d, SaturationSlider.Value / 50d);
                        RGB = ColorConverter.HslToRgb(hsl[0], hsl[1], hsl[2]);

                        pixelsData[i] = RGB[2];
                        pixelsData[i + 1] = RGB[1];
                        pixelsData[i + 2] = RGB[0];
                    }

                }
            }

            WriteableBitmap newImage = new WriteableBitmap(Image.PixelWidth, Image.PixelHeight, Image.DpiX, Image.DpiY, Image.Format, Image.Palette);
            newImage.WritePixels(new Int32Rect(0, 0, Image.PixelWidth, Image.PixelHeight), pixelsData, stride, 0);

            HSLImage = newImage.ToBitmapImage();
            ChangedImage.Source = HSLImage;
            pixelsHSL = pixelsData;
        }

        private void ChangeSaturationLightness()
        {
            if (canDraw.Children.Contains(DragRectangle))
            {
                ChangeSaturationLightnessArea();
            }
            else
            {
                byte[] pixelsLData = new byte[size];
                if (true)
                {
                    Image.CopyPixels(pixelsLData, stride, 0);
                }
                else
                {
                    return;
                }

                for (int i = 0; i < size; i += 4)
                {
                    if (i + 4 < pixels.Length)
                    {
                        double[] hsl;
                        byte[] RGB;
                        Color color = Color.FromArgb(pixelsLData[i + 2], pixelsLData[i + 1], pixelsLData[i]);

                        hsl = ColorConverter.RgbToHslChange(pixelsLData[i + 2], pixelsLData[i + 1], pixelsLData[i], LightnessSlider.Value / 50d, SaturationSlider.Value / 50d);
                        RGB = ColorConverter.HslToRgb(hsl[0], hsl[1], hsl[2]);

                        pixelsLData[i] = RGB[2];
                        pixelsLData[i + 1] = RGB[1];
                        pixelsLData[i + 2] = RGB[0];
                    }
                }
                WriteableBitmap newImage = new WriteableBitmap(Image.PixelWidth, Image.PixelHeight, Image.DpiX, Image.DpiY, Image.Format, Image.Palette);
                newImage.WritePixels(new Int32Rect(0, 0, Image.PixelWidth, Image.PixelHeight), pixelsLData, stride, 0);
                HSLImage = newImage.ToBitmapImage();
                ChangedImage.Source = HSLImage;
                pixelsHSL = pixelsLData;
            }
        }


        private Rectangle DragRectangle = default;
        private System.Windows.Point StartPoint, LastPoint;

        private void canDraw_MouseMove(object sender, MouseEventArgs e)
        {

            LastPoint = Mouse.GetPosition(canDraw);
            DragRectangle.Width = Math.Abs(LastPoint.X - StartPoint.X);
            DragRectangle.Height = Math.Abs(LastPoint.Y - StartPoint.Y);
            Canvas.SetLeft(DragRectangle, Math.Min(LastPoint.X, StartPoint.X));
            Canvas.SetTop(DragRectangle, Math.Min(LastPoint.Y, StartPoint.Y));
        }
        private void canDraw_MouseUp(object sender, MouseButtonEventArgs e)
        {
            canDraw.ReleaseMouseCapture();
            canDraw.MouseMove -= canDraw_MouseMove;
            canDraw.MouseUp -= canDraw_MouseUp;

            if (LastPoint.X < 0) LastPoint.X = 0;
            if (LastPoint.X >= canDraw.Width) LastPoint.X = canDraw.Width - 1;
            if (LastPoint.Y < 0) LastPoint.Y = 0;
            if (LastPoint.Y >= canDraw.Height) LastPoint.Y = canDraw.Height - 1;
        }


        private void ResetArea_Click(object sender, RoutedEventArgs e)
        {
            canDraw.Children.Remove(DragRectangle);
            DragRectangle = null;
        }

        private void showOrigin_Click(object sender, RoutedEventArgs e)
        {
            if (StockImage.Visibility == Visibility.Hidden)
            {
                StockImage.Visibility = Visibility.Visible;
                ChangedImage.Visibility = Visibility.Hidden;
            }
            else
            {
                StockImage.Visibility = Visibility.Hidden;
                ChangedImage.Visibility = Visibility.Visible;
            }
        }

        private void canDraw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            canDraw.Children.Remove(DragRectangle);
            DragRectangle = null;

            StartPoint = Mouse.GetPosition(canDraw);
            LastPoint = StartPoint;
            DragRectangle = new Rectangle();
            DragRectangle.Width = 1;
            DragRectangle.Height = 1;
            DragRectangle.Stroke = Brushes.Red;
            DragRectangle.StrokeThickness = 1;
            DragRectangle.Cursor = Cursors.Cross;

            canDraw.Children.Add(DragRectangle);
            Canvas.SetLeft(DragRectangle, StartPoint.X);
            Canvas.SetTop(DragRectangle, StartPoint.Y);

            canDraw.MouseMove += canDraw_MouseMove;
            canDraw.MouseUp += canDraw_MouseUp;
            canDraw.CaptureMouse();
        }
    }

    public static class ImageHelpers
    {
        public static BitmapImage ToBitmapImage(this WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }
    }
}
