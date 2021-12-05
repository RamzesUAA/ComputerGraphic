using System;
using System.Windows;

namespace CG_Project.Services.AffineTransformation
{
    class AffineTransformator
    {
        public double[,] Transform(double[,] inputParallelogram, double coeffA, double coeffB)
        {
            var radians = Math.Atan(coeffA);

            var simplfyToB = ReturnSimplifierToB(coeffB);
            var turnedParallelogramAtCenter = Multiply(simplfyToB, ReturnTurnTransformationMatrix(radians));
            var mirrorMatrix = Multiply(turnedParallelogramAtCenter, ReturnMirrorTransformationMatrix());

            var backTransformation = Multiply(mirrorMatrix, ReturnBackTurnTransformationMatrix(radians));   
            var backTransformationToPlusB = Multiply(backTransformation, ReturnSimplifierToPlusB(coeffB));

            return Multiply(inputParallelogram, backTransformationToPlusB);
        }

        public double[,] ReturnSimplifierToB(double b)
        {
            double[,] matr = {{ 1,  0, 0 },
                              { 0,  1, 0 },
                              { 0, -b, 1 }};
            return matr;
        }

        public double[,] ReturnTurnTransformationMatrix(double a)
        {
            double[,] matr = {{  Math.Cos(a), -Math.Sin(a), 0 },
                              {  Math.Sin(a),  Math.Cos(a), 0 },
                              {            0,            0, 1 }};
            return matr;
        }

        public double[,] ReturnMirrorTransformationMatrix()
        {
            double[,] matr = {{  1,   0,   0 },
                              {  0,   -1,  0 },
                              {  0,   0,  1 }};
            return matr;
        }

        public double[,] ReturnBackTurnTransformationMatrix(double a)
        {
            double[,] matr = {{   Math.Cos(a),  Math.Sin(a), 0 },
                              {  -Math.Sin(a),  Math.Cos(a), 0 },
                              {             0,            0, 1 }};
            return matr;
        }

        public double[,] ReturnSimplifierToPlusB(double b)
        {
            double[,] matr = {{ 1, 0, 0 },
                              { 0, 1, 0 },
                              { 0, b, 1 }};
            return matr;
        }

        public static double[,] Multiply(double[,] matrix1, double[,] matrix2)
        {
            var matrix1Rows = matrix1.GetLength(0);
            var matrix1Cols = matrix1.GetLength(1);
            var matrix2Rows = matrix2.GetLength(0);
            var matrix2Cols = matrix2.GetLength(1);

            if (matrix1Cols != matrix2Rows)
                throw new InvalidOperationException
                  ("Product is undefined. n columns of first matrix must equal to n rows of second matrix");

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
    }
}
