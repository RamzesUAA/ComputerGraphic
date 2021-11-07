using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            //for (int i = 0; i < 10000; ++i)
            //{
            //    KochSnowlakeFern();
            //    DrawPoint();
            //}
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

                        // Find the first control points.
                        float dx = (float)(Math.Min(
                            fractalCanvas.Width / 1.5f,
                            fractalCanvas.Height) / 3f);

                        // Try to center it a bit.
                        float x0 = (float)((fractalCanvas.Width - dx * 1.5f) / 2f + dx / 3f);
                        float y0 = (float)((fractalCanvas.Height - dx) / 2f + dx / 3f);

                        // Recursively draw the lines.
                        int level = Convert.ToInt32(numberOfIterations.Text);
                        DrawDragonLine(level, Direction.Right, x0, y0, 2 * dx, 0);

                    }
                    break;
                case 1:
                    if (checkedValue.Content.ToString() == FractalNames.KochCurve)
                    {
                        kochSnowflake = new KochSnowflake(fractalCanvas);

                        for (int i = 0; i < 10000; ++i)
                        {
                            KochSnowlakeFern();
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

        // The direction the curve should turn next.
        private enum Direction
        {
            Left,
            Right
        }

        private void DrawDragonLine( int level, Direction turn_towards, float x1, float y1, float dx, float dy)
        {
            if (level <= 0)
            {

                var line = new Line();
                line.Stroke = Brushes.Black;

                line.X1 = x1;
                line.Y1 = y1;

                line.X2 = x1 + dx;
                line.Y2 = y1 + dy;

                line.StrokeThickness = 2;
                fractalCanvas.Children.Add(line);

#if DRAW_POINTS
                gr.DrawEllipse(Pens.Blue, x1 - 2, y1 - 2, 4, 4);
                gr.DrawEllipse(Pens.Blue, x1 + dx - 2, y1 + dy - 2, 4, 4);
#endif
            }
            else
            {
#if DRAW_LEVEL_MINUS_1
                if (level == 1)
                {
                    gr.DrawLine(Pens.Silver, x1, y1, x1 + dx, y1 + dy);
                }
#endif
                float nx = (float)(dx / 2);
                float ny = (float)(dy / 2);
                float dx2 = -ny + nx;
                float dy2 = nx + ny;
                if (turn_towards == Direction.Right)
                {
                    // Turn to the right.
                    DrawDragonLine(level - 1, Direction.Right, x1, y1, dx2, dy2);
                    float x2 = x1 + dx2;
                    float y2 = y1 + dy2;
                    DrawDragonLine(level - 1, Direction.Left, x2, y2, dy2, -dx2);
                }
                else
                {
                    // Turn to the left.
                    DrawDragonLine(level - 1, Direction.Right, x1, y1, dy2, -dx2);
                    float x2 = x1 + dy2;
                    float y2 = y1 - dx2;
                    DrawDragonLine(level - 1, Direction.Left, x2, y2, dx2, dy2);
                }
            }
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
