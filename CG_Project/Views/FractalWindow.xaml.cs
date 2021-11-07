using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CG_Project.Helpers;
using CG_Project.Services;
using Microsoft.Win32;

namespace CG_Project
{
    /// <summary>
    /// Interaction logic for FractalWindow.xaml
    /// </summary>
    public partial class FractalWindow : Window
    {
        private KochSnowflake kochSnowflake;
        private DragonCurve dragonCurve;
        private Window BaseWindow;

        double x = 0;
        double y = 0;

        public FractalWindow(Window window)
        {
            InitializeComponent();
            kochSnowflake = new KochSnowflake(fractalCanvas);
            dragonCurve = new DragonCurve(fractalCanvas);
            BaseWindow = window;
        }

        private void BuildFractal_OnClick(object sender, RoutedEventArgs e)
        {
            fractalCanvas.Children.Clear();
            var checkedValue = radioButtonPannel.Children.OfType<RadioButton>()
                .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            switch (fractalTypes?.SelectedIndex)
            {
                case 0:
                    if (checkedValue.Content.ToString() == FractalNames.KochCurve)
                    {
                        kochSnowflake.RunGeometricKochSnowflake(Convert.ToInt32(numberOfIterations.Text));
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.DragonCurve)
                    {
                        dragonCurve.RunGeometricDragonCurve(Convert.ToInt32(numberOfIterations.Text));
                    }
                    break;
                case 1:
                    if (checkedValue.Content.ToString() == FractalNames.KochCurve)
                    {
                        kochSnowflake = new KochSnowflake(fractalCanvas);

                        for (int i = 0; i < 10000; ++i)
                        {
                            KochSnowlakeIFS();
                            DrawPoint();
                        }
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.DragonCurve)
                    {
                        x = 1.0;
                        y = 0.0;
                        var angle45 = Math.PI * 45 / 180;
                        var angle135 = Math.PI * 135 / 180;
                        var random = new Random(1);
                        for (int i = 0; i < 10000; i++)
                        {
                            var nextNumber = random.Next(1, 3);
                            if (nextNumber == 1)
                            {
                                var x1 = (x * Math.Cos(angle45) - y * Math.Sin(angle45)) / Math.Sqrt(2);
                                var y1 = (x * Math.Sin(angle45) + y * Math.Cos(angle45)) / Math.Sqrt(2);
                                x = x1; // !!!
                                y = y1; // !!!
                            }
                            if (nextNumber == 2)
                            {
                                var x1 = (x * Math.Cos(angle135) - y * Math.Sin(angle135)) / Math.Sqrt(2) + 1;
                                var y1 = (x * Math.Sin(angle135) + y * Math.Cos(angle135)) / Math.Sqrt(2);
                                x = x1; // !!!
                                y = y1; // !!!
                            }
                            DrawPoint();
                        }
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.BarnsleyFern)
                    {
                        BarnsleyFernIFS barnsleyFern = new BarnsleyFernIFS(fractalCanvas);
                        barnsleyFern.RunBarnsleyFernIFS(Convert.ToInt32(numberOfIterations.Text));
                    }
                    break;
            }
        }

        private double Map(double value, double istart, double istop, double ostart, double ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));

        }

        private void KochSnowlakeIFS()
        {
            double nextX;
            double nextY;

            var rnd = new Random();

            double r = rnd.NextDouble();

            if (r < 0.1)
            {
                // 1
                nextX = -1 / 6.0 * x + Math.Sqrt(3) / 6.0 * y + 1 / 6.0;
                nextY = -Math.Sqrt(3) / 6.0 * x + -1 / 6.0 * y + Math.Sqrt(3) / 6.0;
            }
            else if (r < 0.32)
            {
                // 2
                nextX = 1 / 6.0 * x + -Math.Sqrt(3) / 6.0 * y + 1 / 6.0;
                nextY = Math.Sqrt(3) / 6 * x + 1 / 6.0 * y + Math.Sqrt(3) / 6.0;
            }
            else if (r < 0.45)
            {
                // 3
                nextX = 1 / 3.0 * x + 1 / 3.0;
                nextY = 1 / 3.0 * y + Math.Sqrt(3) / 3.0;
            }
            else if (r < 0.66)
            {
                // 4
                nextX = 1 / 6.0 * x + Math.Sqrt(3) / 6.0 * y + 2 / 3.0;
                nextY = -Math.Sqrt(3) / 6.0 * x + 1 / 6.0 * y + Math.Sqrt(3) / 3.0;
            }
            else if (r < 0.78)
            {
                // 5
                nextX = 1 / 2.0 * x + -Math.Sqrt(3) / 6.0 + 1 / 3.0;
                nextY = Math.Sqrt(3) / 6.0 * x + 1 / 2.0 * y;
            }
            else if (r < 0.88)
            {
                //6
                nextX = -1 / 3.0 * x + 2 / 3.0;
                nextY = -1 / 3.0 * y;
            }
            else
            {
                //7
                nextX = 1 / 3.0 * x + 2 / 3.0;
                nextY = 1 / 3.0 * y;
            }
            x = nextX;
            y = nextY;
        }

        private void DrawPoint()
        {
            // Set the width/height boundaries
            double px = Map(x, -2.8, 3.6558, 0, fractalCanvas.Width);
            double py = Map(y, -0.5, 10.5, fractalCanvas.Height, 0);
            //MessageBox.Show($"px: {x}, py: {y}");

            Point p = new Point(px, py);

            var ellipse = new Ellipse() { Width = 3, Height = 3, Stroke = new SolidColorBrush(Colors.Black) };
            Canvas.SetLeft(ellipse, p.X);
            Canvas.SetTop(ellipse, p.Y);
            fractalCanvas.Children.Add(ellipse);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void CmdUp_OnClick(object sender, RoutedEventArgs e)
        {
            var num = Convert.ToInt32(numberOfIterations.Text);
            num += 1;
            numberOfIterations.Text = num.ToString();
        }

        private void CmdDown_OnClick(object sender, RoutedEventArgs e)
        {
            var num = Convert.ToInt32(numberOfIterations.Text);
            num -= 1;
            numberOfIterations.Text = num.ToString();
        }

        private void RadioButtonHandler_Click(object sender, RoutedEventArgs e)
        {
            var checkedValue = radioButtonPannel.Children.OfType<RadioButton>()
                .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            switch (fractalTypes?.SelectedIndex)
            {
                case 0:
                    if (checkedValue.Content.ToString() == FractalNames.KochCurve)
                    {
                        infoBlockHeader.Text = "Koch snowflake";
                        infoBlockDescription.Text =
                            "The Koch snowflake is a fractal curve and one of the earliest fractals to have been described. It is based on the Koch curve, which appeared in a 1904 paper titled \"On a Continuous Curve Without Tangents, Constructible from Elementary Geometry\" by the Swedish mathematician Helge von Koch.";
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.DragonCurve)
                    {
                        infoBlockHeader.Text = "Dragon Curve";
                        infoBlockDescription.Text =
                            "The Harter-Heighway Dragon is created by iteration of the curve process described above, and is thus a type of fractal known as iterated function systems. ... An interesting property of this curve is that although the corners of the fractal seem to touch at various points, the curve never actually crosses over itself.";
                    }
                    break;
                case 1:
                    if (checkedValue.Content.ToString() == FractalNames.KochCurve)
                    {
                        infoBlockHeader.Text = "Koch snowflake";
                        infoBlockDescription.Text =
                            "The Koch snowflake is a fractal curve and one of the earliest fractals to have been described. It is based on the Koch curve, which appeared in a 1904 paper titled \"On a Continuous Curve Without Tangents, Constructible from Elementary Geometry\" by the Swedish mathematician Helge von Koch.";
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.DragonCurve)
                    {
                        infoBlockHeader.Text = "Dragon Curve";
                        infoBlockDescription.Text =
                            "The Harter-Heighway Dragon is created by iteration of the curve process described above, and is thus a type of fractal known as iterated function systems. ... An interesting property of this curve is that although the corners of the fractal seem to touch at various points, the curve never actually crosses over itself.";
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.BarnsleyFern)
                    {
                        infoBlockHeader.Text = "Barnsley fern";
                        infoBlockDescription.Text =
                            "The Barnsley fern is a fractal named after the British mathematician Michael Barnsley who first described it in his book Fractals Everywhere.[1] He made it to resemble the black spleenwort, Asplenium adiantum-nigrum.";
                    }
                    break;
            }
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

        private void Download_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.OverwritePrompt = true;

            saveFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    Rect rect = new Rect(fractalCanvas.Margin.Left, fractalCanvas.Margin.Top, fractalCanvas.ActualWidth, fractalCanvas.ActualHeight);
                    double dpi = 96d;

                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right, (int)rect.Bottom, dpi, dpi, System.Windows.Media.PixelFormats.Default);
                    rtb.Render(fractalCanvas);
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(BitmapFrame.Create(rtb)));
                    encoder.Save(stream);
                }
            }
        }

        private void FractalTypes_OnSelectionChangedList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fractals = new List<RadioButton>();
            radioButtonPannel.Children.Clear();
            switch (fractalTypes.SelectedIndex)
            {
                case 0:
                    fractals.Add(new RadioButton() { Content = FractalNames.KochCurve, IsChecked = false });
                    fractals.Add(new RadioButton() { Content = FractalNames.DragonCurve, IsChecked = false });
                    foreach (var fractal in fractals)
                    {
                        fractal.Checked += RadioButtonHandler_Click;
                        radioButtonPannel.Children.Add(fractal);
                    }
                    break;
                case 1:
                    radioButtonPannel.Children.Clear();
                    fractals.Add(new RadioButton() { Content = FractalNames.KochCurve, IsChecked = false });
                    fractals.Add(new RadioButton() { Content = FractalNames.DragonCurve, IsChecked = false });
                    fractals.Add(new RadioButton() { Content = FractalNames.BarnsleyFern, IsChecked = false });
                    foreach (var fractal in fractals)
                    {
                        fractal.Checked += RadioButtonHandler_Click;
                        radioButtonPannel.Children.Add(fractal);
                    }
                    break;
            }
        }
    }
}
