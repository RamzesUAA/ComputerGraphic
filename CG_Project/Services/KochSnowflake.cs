using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CG_Project.Services
{
    public class KochSnowflake
    {
        public int numberOfIterations { get; set; }
        public Polyline pl = new Polyline();
        public int II { get; set; }
        public int i { get; set; }

        private double[] dTheta;
        private double distanceScale;
        private double SnowflakeSize = default;
        private Point SnowflakePoint = new Point();
        private double Height { get; set; }
        private double Width { get; set; }

        public KochSnowflake(double height, double width) : this()
        {
            Height = height;
            Width = width;
            double ysize = 0.8 * height /
                           (Math.Sqrt(3) * 4 / 3);
            double xsize = 0.8 * width / 2;
            double size = 0;
            if (ysize < xsize)
                size = ysize;
            else
                size = xsize;
            SnowflakeSize = 2 * size;
            pl.Stroke = Brushes.Blue;
        }

        private KochSnowflake()
        {
            distanceScale = 1.0 / 3;
            dTheta = new double[4] { 0, Math.PI / 3,
                -2 * Math.PI / 3, Math.PI / 3 };
        }

        public void StartAnimation(object sender,
       EventArgs e)
        {
            i += 1;
            if (i % 60 == 0)
            {
                pl.Points.Clear();
                DrawSnowFlake(SnowflakeSize, II);
                string str = "Snow Flake - Depth = " +
                II.ToString();
                II += 1;
                if (II > numberOfIterations)
                {
                    CompositionTarget.Rendering -=
                    StartAnimation;
                }
            }
        }
        private void SnowFlakeEdge(int depth, double theta, double distance)
        {
            Point pt = new Point();
            if (depth <= 0)
            {
                pt.X = SnowflakePoint.X +
                distance * Math.Cos(theta);
                pt.Y = SnowflakePoint.Y +
                distance * Math.Sin(theta);
                pl.Points.Add(pt);
                SnowflakePoint = pt;
                return;
            }
            distance *= distanceScale;
            for (int j = 0; j < 4; j++)
            {
                theta += dTheta[j];
                SnowFlakeEdge(depth - 1,
                theta, distance);
            }
        }

        private void DrawSnowFlake(
        double length, int depth)
        {
            double xmid = Width / 2;
            double ymid = Height / 2;
            Point[] pta = new Point[4];
            pta[0] = new Point(xmid, ymid + length / 2 *
            Math.Sqrt(3) * 2 / 3);
            pta[1] = new Point(xmid + length / 2,
            ymid - length / 2 * Math.Sqrt(3) / 3);
            pta[2] = new Point(xmid - length / 2,
            ymid - length / 2 * Math.Sqrt(3) / 3);
            pta[3] = pta[0];
            pl.Points.Add(pta[0]);
            for (int j = 1; j < pta.Length; j++)
            {
                double x1 = pta[j - 1].X;
                double y1 = pta[j - 1].Y;
                double x2 = pta[j].X;
                double y2 = pta[j].Y;
                double dx = x2 - x1;
                double dy = y2 - y1;
                double theta = Math.Atan2(dy, dx);
                SnowflakePoint = new Point(x1, y1);
                SnowFlakeEdge(depth, theta, length);
            }
        }
    }
}
