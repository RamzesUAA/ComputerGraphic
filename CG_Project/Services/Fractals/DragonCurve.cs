using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CG_Project.Interfaces;

namespace CG_Project.Services
{
    class DragonCurve : IDrawFractal
    {
        private Canvas FractalCanvas;

        public DragonCurve(Canvas fractalCanvas)
        {
            FractalCanvas = fractalCanvas;
        }
        public void DrawFractal(int numberOfIterations)
        {
            // Find the first control points.
            float dx = (float)(Math.Min(
                FractalCanvas.Width / 1.5f,
                FractalCanvas.Height) / 3f);

            // Try to center it a bit.
            float x0 = (float)((FractalCanvas.Width - dx * 2f) / 2f + dx / 3f);
            float y0 = (float)((FractalCanvas.Height - dx) / 2f + dx / 3f);

            // Recursively draw the lines.
            int level = numberOfIterations;
            DrawDragonLine(level, Direction.Right, x0, y0, (float)(2 * dx), 0);
        }

        // The direction the curve should turn next.
        private enum Direction
        {
            Left,
            Right
        }

        private void DrawDragonLine(int level, Direction turn_towards, float x1, float y1, float dx, float dy)
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
                FractalCanvas.Children.Add(line);
            }
            else
            {
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

    }
}
