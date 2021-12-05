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


    public static class AffineTransformation
    {
        /// <summary>
        /// |a b e|   |x|
        /// |c d f| * |y|
        /// |0 0 1|   |1|
        /// </summary>
        public class AffineMatrix
        {
            public double a;
            public double b;
            public double c;
            public double d;
            public double e;
            public double f;

            public AffineMatrix(double a, double b, double c, double d, double e, double f)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
                this.e = e;
                this.f = f;
            }

            public double[,] ToArray()
            {
                return new double[,]
                {
                    { a, b, e },
                    { c, d, f },
                    { 0, 0, 1 }
                };
            }

            public double this[char index]
            {
                get
                {
                    if (index == 'a')
                        return this.a;
                    else if (index == 'b')
                        return this.b;
                    else if (index == 'c')
                        return this.c;
                    else if (index == 'd')
                        return this.d;
                    else if (index == 'e')
                        return this.e;
                    else if (index == 'f')
                        return this.f;
                    else
                        return Double.NaN;
                }
            }
        }

        public static double[,] DotProduct(double[,] matrix1, double[,] matrix2)
        {
            // cahing matrix lengths for better performance  
            var matrix1Rows = matrix1.GetLength(0);
            var matrix1Cols = matrix1.GetLength(1);
            var matrix2Rows = matrix2.GetLength(0);
            var matrix2Cols = matrix2.GetLength(1);

            // checking if product is defined  
            if (matrix1Cols != matrix2Rows)
                throw new InvalidOperationException
                  ("Product is undefined. n columns of first matrix must equal to n rows of second matrix");

            // creating the final product matrix  
            double[,] product = new double[matrix1Rows, matrix2Cols];

            // looping through matrix 1 rows  
            for (int matrix1_row = 0; matrix1_row < matrix1Rows; matrix1_row++)
            {
                // for each matrix 1 row, loop through matrix 2 columns  
                for (int matrix2_col = 0; matrix2_col < matrix2Cols; matrix2_col++)
                {
                    // loop through matrix 1 columns to calculate the dot product  
                    for (int matrix1_col = 0; matrix1_col < matrix1Cols; matrix1_col++)
                    {
                        product[matrix1_row, matrix2_col] +=
                          matrix1[matrix1_row, matrix1_col] *
                          matrix2[matrix1_col, matrix2_col];
                    }
                }
            }

            return product;
        }

        public static Point Transform(Point p, Point origin, AffineMatrix rule)
        {
            double[,] pointArray = new double[,] { { p.X }, { p.Y }, { 1.0 } };
            double angle = Math.Round(GraphicsUtils.GetAngle(origin, p));
            double angleRadians = Math.PI / 180 * angle;

            AffineMatrix translateToOrigin =
                new AffineMatrix(1, 0, 0, 1, -origin.X, -origin.Y);
            AffineMatrix translateBack =
                new AffineMatrix(1, 0, 0, 1, origin.X, origin.Y);
            AffineMatrix rotateToXAxis =
                new AffineMatrix(Math.Cos(angleRadians), -Math.Sin(angleRadians), Math.Sin(angleRadians), Math.Cos(angleRadians), 0, 0);
            AffineMatrix rotateBack =
                new AffineMatrix(Math.Cos(-angleRadians), -Math.Sin(-angleRadians), Math.Sin(-angleRadians), Math.Cos(-angleRadians), 0, 0);

            // Translate to origin
            pointArray = DotProduct(translateToOrigin.ToArray(), pointArray);

            // Rotate with respect to vector => p - origin
            pointArray = DotProduct(rotateToXAxis.ToArray(), pointArray);

            // Apply affine 'rule' transformation
            pointArray = DotProduct(rule.ToArray(), pointArray);

            // Rotate back
            pointArray = DotProduct(rotateBack.ToArray(), pointArray);

            // Translate back
            pointArray = DotProduct(translateBack.ToArray(), pointArray);

            return new Point((int)pointArray[0, 0], (int)pointArray[1, 0]);
        }

        public static System.Windows.Point Transform(System.Windows.Point p, System.Windows.Point origin, AffineMatrix rule)
        {
            double[,] pointArray = new double[,] { { p.X }, { p.Y }, { 1.0 } };
            double angle = Math.Round(GraphicsUtils.GetAngle(origin, p));
            double angleRadians = Math.PI / 180 * angle;

            AffineMatrix translateToOrigin =
                new AffineMatrix(1, 0, 0, 1, -origin.X, -origin.Y);
            AffineMatrix translateBack =
                new AffineMatrix(1, 0, 0, 1, origin.X, origin.Y);
            AffineMatrix rotateToXAxis =
                new AffineMatrix(Math.Cos(angleRadians), -Math.Sin(angleRadians), Math.Sin(angleRadians), Math.Cos(angleRadians), 0, 0);
            AffineMatrix rotateBack =
                new AffineMatrix(Math.Cos(-angleRadians), -Math.Sin(-angleRadians), Math.Sin(-angleRadians), Math.Cos(-angleRadians), 0, 0);

            // Translate to origin
            //pointArray = DotProduct(translateToOrigin.ToArray(), pointArray);

            // Rotate with respect to vector => p - origin
            //pointArray = DotProduct(rotateToXAxis.ToArray(), pointArray);

            // Apply affine 'rule' transformation
            pointArray = DotProduct(rule.ToArray(), pointArray);

            // Rotate back
            //pointArray = DotProduct(rotateBack.ToArray(), pointArray);

            // Translate back
            //pointArray = DotProduct(translateBack.ToArray(), pointArray);

            return new System.Windows.Point(pointArray[0, 0], pointArray[1, 0]);
        }
    }
}
