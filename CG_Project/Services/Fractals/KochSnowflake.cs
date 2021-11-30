using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CG_Project.Interfaces;

namespace CG_Project.Services
{
    public class KochSnowflake : IDrawFractal
    {
        public List<Segment> Segments;
        public Canvas FractalCanvas;
        public Polyline KochPolyline = new Polyline();

        public KochSnowflake(Canvas fractalCanvas)
        {
            FractalCanvas = fractalCanvas;
        }

        public void DrawFractal(int numberOfIterations)
        {
            InitKochSnowflake();
            for (int i = 0; i < numberOfIterations; ++i)
            {
                List<Segment> nextGeneration = new List<Segment>();

                foreach (var segment in Segments)
                {
                    Segment[] children = segment.generate();
                    nextGeneration.AddRange(children);
                }

                Segments = nextGeneration;
            }

            foreach (var segment in Segments)
            {
                Show(segment.a, segment.b);
            }
        }

        private void InitKochSnowflake()
        {
            FractalCanvas.Children.Clear();
            Segments = new List<Segment>();

            KochPolyline.Stroke = Brushes.Blue;
            Point a = new Point((FractalCanvas.Width - FractalCanvas.Height) / 2, 125);
            Point b = new Point(FractalCanvas.Width - (FractalCanvas.Width - FractalCanvas.Height) / 2, 125);
            Point c = new Point(FractalCanvas.Width / 2, FractalCanvas.Height - 10);
            Segment s1 = new Segment(a, b);
            Segment s2 = new Segment(b, c);
            Segment s3 = new Segment(c, a);
            Segments.Add(s1);
            Segments.Add(s2);
            Segments.Add(s3);
        }

        public void Show(Point a, Point b)
        {
            var line = new Line();
            line.Stroke = Brushes.Black;

            line.X1 = a.X;
            line.Y1 = a.Y;

            line.X2 = b.X;
            line.Y2 = b.Y;

            line.StrokeThickness = 2;
            FractalCanvas.Children.Add(line);
        }
    }

    public class Segment
    {
        public Point a;
        public Point b;

        public Segment(Point _a, Point _b)
        {
            a = _a;
            b = _b;
        }

        public Segment[] generate()
        {

            Segment[] children = new Segment[4];

            Vector v = Point.Subtract(b, a);
            v = Vector.Divide(v, 3);

            // Segment 0
            Point b1 = Point.Add(a, v);
            children[0] = new Segment(a, b1);

            // Segment 3
            Point a1 = Point.Subtract(b, v);
            children[3] = new Segment(a1, b);

            v = RotateRadians(v, -Math.PI / 3);
            Point c = Point.Add(b1, v);

            // Segment 1
            children[1] = new Segment(b1, c);

            // Segment 2
            children[2] = new Segment(c, a1);

            return children;
        }

        public Vector RotateRadians(Vector v, double radians)
        {
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);
            return new Vector(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
        }
    }
}
