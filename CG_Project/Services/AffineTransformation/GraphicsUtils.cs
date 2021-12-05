using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace CG_Project.Services.AffineTransformation
{

    class GraphicsUtils
    {
        public static IEnumerable<Point> GetPointsOnLine(Point p1, Point p2)
        {
            bool steep = Math.Abs(p2.Y - p1.Y) > Math.Abs(p2.X - p1.X);
            if (steep)
            {
                int t;
                t = p1.X; // swap x0 and y0
                p1.X = p1.Y;
                p1.Y = t;
                t = p2.X; // swap x1 and y1
                p2.X = p2.Y;
                p2.Y = t;
            }
            if (p1.X > p2.X)
            {
                int t;
                t = p1.X; // swap x0 and x1
                p1.X = p2.X;
                p2.X = t;
                t = p1.Y; // swap y0 and y1
                p1.Y = p2.Y;
                p2.Y = t;
            }
            int dx = p2.X - p1.X;
            int dy = Math.Abs(p2.Y - p1.Y);
            int error = dx / 2;
            int ystep = (p1.Y < p2.Y) ? 1 : -1;
            int y = p1.Y;
            for (int x = p1.X; x <= p2.X; x++)
            {
                yield return new Point((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }

        public static float GetLength(Point p1, Point p2)
        {
            return (float)Math.Sqrt(Math.Pow((p2.X - p1.X), 2.0) +
                    Math.Pow((p2.Y - p1.Y), 2.0));
        }

        public static float GetLength(System.Windows.Point p1, System.Windows.Point p2)
        {
            return (float)Math.Sqrt(Math.Pow((p2.X - p1.X), 2.0) +
                    Math.Pow((p2.Y - p1.Y), 2.0));
        }

        public static Point GetEndpoint(double angle, Point start, float lenght)
        {
            double radians = Math.PI / 180 * angle;
            double cosine = Math.Cos(radians);
            double x2 = start.X + (lenght * Math.Cos(radians));
            double y2 = start.Y - (lenght * Math.Sin(radians));
            cosine = Math.Cos(radians);

            return new Point((int)Math.Round(x2), (int)Math.Round(y2));
        }

        public static float GetAngle(Point p1, Point p2)
        {
            Point c = new Point(p2.X, p1.Y);
            float leg = GetLength(p1, c);
            float hupotenuse = GetLength(p1, p2);
            if (hupotenuse == 0) return 0;
            float angle = (float)Math.Acos(leg / hupotenuse) * 180.0f / (float)Math.PI;

            if (p2.X >= p1.X && p2.Y <= p1.Y)
                angle = angle;
            else if (p2.X < p1.X && p2.Y <= p1.Y)
                angle = 180 - angle;
            else if (p2.X <= p1.X && p2.Y > p1.Y)
                angle = 180 + angle;
            else if (p2.X > p1.X && p2.Y > p1.Y)
                angle = 0 - angle;

            return angle;
        }

        public static float GetAngle(System.Windows.Point p1, System.Windows.Point p2)
        {
            System.Windows.Point c = new System.Windows.Point(p2.X, p1.Y);
            float leg = GetLength(p1, c);
            float hupotenuse = GetLength(p1, p2);
            if (hupotenuse == 0) return 0;
            float angle = (float)Math.Acos(leg / hupotenuse) * 180.0f / (float)Math.PI;

            if (p2.X >= p1.X && p2.Y <= p1.Y)
                angle = angle;
            else if (p2.X < p1.X && p2.Y <= p1.Y)
                angle = 180 - angle;
            else if (p2.X <= p1.X && p2.Y > p1.Y)
                angle = 180 + angle;
            else if (p2.X > p1.X && p2.Y > p1.Y)
                angle = 0 - angle;

            return angle;
        }

        public static void DrawLine(Bitmap bmp, IEnumerable<Point> line, System.Drawing.Color color)
        {
            foreach (Point pixel in line)
            {
                bmp.SetPixel(pixel.X, pixel.Y, color);
            }
        }

        public static void DrawLine(Bitmap bmp, Point p1, Point p2, System.Drawing.Color color)
        {
            var line = GetPointsOnLine(p1, p2);
            foreach (Point pixel in line)
            {
                bmp.SetPixel(pixel.X, pixel.Y, color);
            }
        }

        public static bool IsFit(Point p1, int screenWidth, int screenHeight)
        {
            if (p1.X < 0 || p1.X > screenWidth || p1.Y < 0 || p1.Y > screenHeight)
                return false;
            return true;
        }

        public static bool CheckIfPointInRange(Bitmap bmp, Point point)
        {
            if (point.X >= bmp.Width || point.X <= 0 || point.Y >= bmp.Height || point.Y <= 0)
            {
                return false;
            }
            return true;
        }
        public static bool IsPointOnLine(System.Windows.Point lStart, System.Windows.Point lEnd, System.Windows.Point p)
        {
            Point s = new Point((int)lStart.X, (int)lStart.Y);
            Point e = new Point((int)lEnd.X, (int)lEnd.Y);
            Point newP = new Point((int)p.X, (int)p.Y);

            return IsPointOnLine(s, e, newP);
        }

        public static bool IsPointOnLine(Point lStart, Point lEnd, Point p)
        {
            var line = GetPointsOnLine(lStart, lEnd);
            if (line.Contains(p))
                return true;
            else
                return false;
        }

        public static WriteableBitmap ConvertToWriteableBitmap(Bitmap bmp)
        {
            if (bmp == null)
                return null;
            BitmapSource bitmapSource =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());


            return new WriteableBitmap(bitmapSource);
        }

        public static WriteableBitmap RotateImage(WriteableBitmap bmp, float angle)
        {
            return new WriteableBitmap(new TransformedBitmap(bmp, new RotateTransform(angle)));
        }
    }
}
