using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CG_Project.Interfaces;

namespace CG_Project.Services
{
    public class DragonCurveIFS : IDrawFractal
    {
        private Canvas FractalCanvas;
        public DragonCurveIFS(Canvas fractalCanvas)
        {
            FractalCanvas = fractalCanvas;
        }
        public void DrawFractal(int numberOfIterations)
        {
            double x = 1.0;
            double y = 0.0;
            var angle45 = Math.PI * 45 / 180;
            var angle135 = Math.PI * 135 / 180;
            var random = new Random(1);
            for (int i = 0; i < numberOfIterations; i++)
            {
                var nextNumber = random.Next(1, 3);
                if (nextNumber == 1)
                {
                    var x1 = (x * Math.Cos(angle45) - (y * Math.Sin(angle45))) / Math.Sqrt(2);
                    var y1 = (x * Math.Sin(angle45) + (y * Math.Cos(angle45))) / Math.Sqrt(2);
                    x = x1; // !!!
                    y = y1; // !!!
                }
                if (nextNumber == 2)
                {
                    var x1 = (x * Math.Cos(angle135) - (y * Math.Sin(angle135))) / Math.Sqrt(2) + 1;
                    var y1 = (x * Math.Sin(angle135) + (y * Math.Cos(angle135))) / Math.Sqrt(2);
                    x = x1; // !!!
                    y = y1; // !!!
                }
                DrawPoint(x, y);
            }
        }

        private double Map(double value, double istart, double istop, double ostart, double ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
        }

        private void DrawPoint(double x, double y)
        {
            // Set the width/height boundaries
            double px = Map(x, 1.3, -0.5, 0, FractalCanvas.Width);
            double py = Map(y, -0.6, 0.9, FractalCanvas.Height, 0);
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
    }
}
