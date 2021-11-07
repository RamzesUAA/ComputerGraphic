using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CG_Project.Services
{
    public class BarnsleyFernIFS
    {
        private Canvas FractalCanvas;

        public BarnsleyFernIFS(Canvas fractalCanvas)
        {
            FractalCanvas = fractalCanvas;
        }

        public void RunBarnsleyFernIFS(int numberOfIterations)
        {
            double x = 0;
            double y = 0;

            for (int i = 0; i < numberOfIterations; ++i)
            {
                DrawPointFern(x, y);
                BarnsleyFernIteration(ref x, ref y);
            }
        }

        private void BarnsleyFernIteration(ref double x, ref double y)
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

        private void DrawPointFern(double x, double y)
        {
            // Set the width/height boundaries
            double px = Map(x, -2.8, 3.6558, 0, FractalCanvas.Width);
            double py = Map(y, -0.5, 10.5, FractalCanvas.Height, 0);
            //MessageBox.Show($"px: {x}, py: {y}");
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
