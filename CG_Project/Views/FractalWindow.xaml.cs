using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
        private BarnsleyFernIFS barnsleyFern;
        private KochSnowflakeIFS kochSnowflakeIFS;
        private DragonCurveIFS dragonCurveIfs;

        private Window BaseWindow;

        public FractalWindow(Window window)
        {
            InitializeComponent();
            BaseWindow = window;
            kochSnowflake = new KochSnowflake(fractalCanvas);
            dragonCurve = new DragonCurve(fractalCanvas);
            barnsleyFern = new BarnsleyFernIFS(fractalCanvas);
            kochSnowflakeIFS = new KochSnowflakeIFS(fractalCanvas);
            dragonCurveIfs = new DragonCurveIFS(fractalCanvas);
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
                        kochSnowflake.DrawFractal(Convert.ToInt32(numberOfIterations.Text));
                        break;
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.DragonCurve)
                    {
                        dragonCurve.DrawFractal(Convert.ToInt32(numberOfIterations.Text));
                        break;
                    }
                    break;
                case 1:
                    if (checkedValue.Content.ToString() == FractalNames.KochCurve)
                    {
                        kochSnowflakeIFS.DrawFractal(Convert.ToInt32(numberOfIterations.Text));
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.DragonCurve)
                    {
                        dragonCurveIfs.DrawFractal(Convert.ToInt32(numberOfIterations.Text));
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.BarnsleyFern)
                    {
                        barnsleyFern.DrawFractal(Convert.ToInt32(numberOfIterations.Text));
                    }
                    break;
            }
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
