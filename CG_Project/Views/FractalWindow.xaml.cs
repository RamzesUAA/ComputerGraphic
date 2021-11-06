using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CG_Project.Helpers;
using CG_Project.Services;

namespace CG_Project
{
    /// <summary>
    /// Interaction logic for FractalWindow.xaml
    /// </summary>
    public partial class FractalWindow : Window
    {
        private KochSnowflake kochSnowflake;
        public FractalWindow()
        {
            InitializeComponent();
            kochSnowflake = new KochSnowflake(fractalCanvas);

            for (int i = 0; i < 100; ++i)
            {
                KochSnowlakeFern();
                DrawPoint();
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

        private void BuildFractal_OnClick(object sender, RoutedEventArgs e)
        {
            //fractalCanvas.Children.Clear();
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

                    }
                    break;
                case 1:
                    if (checkedValue.Content.ToString() == FractalNames.KochCurve)
                    {
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.DragonCurve)
                    {

                    }
                    else if (checkedValue.Content.ToString() == FractalNames.BarnsleyFern)
                    {

                        for (int i = 0; i < Convert.ToInt32(numberOfIterations.Text); ++i)
                        {
                            DrawPointFern();
                            BarnsleyFern();
                        }
                    }
                    break;
            }


        }

        private double Map(double value, double istart, double istop, double ostart, double ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));

        }
        double x = 0;
        double y = 0;

        private void BarnsleyFern()
        {
            double nextX;
            double nextY;

            var rnd = new Random();

            double r = rnd.NextDouble();

            if (r < 0.01)
            {
                nextY = 0.16 * y;
                nextX = 0;
            }
            else if (r < 0.86)
            {
                nextX = 0.85 * x + 0.04 * y;
                nextY = -0.04 * x + 0.85 * y + 1.6;
            }
            else if (r < 0.93)
            {
                nextX = 0.2 * x + -0.26 * y;
                nextY = 0.23 * x + 0.22 * y + 1.6;
            }
            else
            {
                nextX = -0.15 * x + 0.28 * y;
                nextY = 0.26 * x + 0.24 * y + 0.44;
            }
            x = nextX;
            y = nextY;
        }

        private void KochSnowlakeFern()
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

            Point p = new Point(px * 2, py);

            var ellipse = new Ellipse() { Width = 3, Height = 3, Stroke = new SolidColorBrush(Colors.Black) };
            Canvas.SetLeft(ellipse, p.X);
            Canvas.SetTop(ellipse, p.Y);
            fractalCanvas.Children.Add(ellipse);

        }
        private void PhonesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fractals = new List<RadioButton>();
            switch (fractalTypes.SelectedIndex)
            {
                case 0:
                    radioButtonPannel.Children.Clear();
                    fractals.Add(new RadioButton() { Content = FractalNames.KochCurve, IsChecked = false });
                    fractals.Add(new RadioButton() { Content = FractalNames.DragonCurve, IsChecked = false });
                    foreach (var fractal in fractals)
                    {
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
                        radioButtonPannel.Children.Add(fractal);
                    }
                    break;
            }
        }

        private void DrawPointFern()
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
    }





}
