using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CG_Project.Interfaces;

namespace CG_Project.Services
{
    public class KochSnowflakeIFS : IDrawFractal
    {
        private Canvas FractalCanvas;
        public KochSnowflakeIFS(Canvas fractalCanvas)
        {
            FractalCanvas = fractalCanvas;
        }

        public void DrawFractal(int numberOfIterations)
        {
            double x = 0;
            double y = 0;
            for (int i = 0; i < numberOfIterations; ++i)
            {
                KochSnowlakeIFS(ref x, ref y);
                DrawPoint(ref x, ref y);
            }
        }
        private void KochSnowlakeIFS(ref double x, ref double y)
        {
            double nextX;
            double nextY;

            var rnd = new Random();

            double r = rnd.NextDouble();

            if (r < 0.2)
            {
                // 1
                nextX = -1 / 6.0 * x + Math.Sqrt(3) / 6.0 * y + 1 / 6.0;
                nextY = -Math.Sqrt(3) / 6.0 * x + (-1 / 6.0 * y) + Math.Sqrt(3) / 6.0;
            }
            else if (r < 0.45)
            {
                // 2
                nextX = 1 / 6.0 * x + -Math.Sqrt(3) / 6.0 * y + 1 / 6.0;
                nextY = Math.Sqrt(3) / 6 * x + 1 / 6.0 * y + Math.Sqrt(3) / 6.0;
            }
            else if (r < 0.55)
            {
                // 3
                nextX = 1 / 3.0 * x + 1 / 3.0;
                nextY = 1 / 3.0 * y + Math.Sqrt(3) / 3.0;
            }
            else if (r < 0.73)
            {
                // 4
                nextX = 1 / 6.0 * x + Math.Sqrt(3) / 6.0 * y + 2 / 3.0;
                nextY = (-Math.Sqrt(3) / 6.0 * x) + 1 / 6.0 * y + Math.Sqrt(3) / 3.0;
            }
            else if (r < 0.88)
            {
                //5
                nextX = -1 / 3.0 * x + 2 / 3.0;
                nextY = -1 / 3.0 * y;
            }
            else
            {
                //6
                nextX = 1 / 3.0 * x + 2 / 3.0;
                nextY = 1 / 3.0 * y;
            }
            x = nextX;
            y = nextY;
        }

        private void DrawPoint(ref double x, ref double y)
        {
            // Set the width/height boundaries
            double px = Map(x, 1.2, -0.2, 0, FractalCanvas.Width);
            double py = Map(y, -0.33, 0.9, FractalCanvas.Height, 0);
            //MessageBox.Show($"px: {x}, py: {y}");
            //MessageBox.Show($"x: {x}, y: {y}");
            Trace.WriteLine($"px: {px}, py: {py}");
            Trace.WriteLine($"x: {x}, y: {y}");

            Point p = new Point(px, py);

            var ellipse = new Ellipse() { Width = 3, Height = 3, Stroke = new SolidColorBrush(Colors.Black) };
            Canvas.SetLeft(ellipse, p.X);
            Canvas.SetTop(ellipse, p.Y);
            FractalCanvas.Children.Add(ellipse);
        }

        private double Map(double value, double istart, double istop, double ostart, double ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
        }

    }
}
