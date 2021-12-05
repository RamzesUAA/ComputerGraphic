using System;
using System.Windows;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot.Annotations;
using System.Threading;
using CG_Project.Services.AffineTransformation;
using System.Threading.Tasks;

namespace CG_Project.Views
{
    public partial class AffineTransformationWindow : Window
    {
        private Window BaseWindow;
        private AffineTransformator _transformator;
        PlotModel plotModel;
        LineSeries line;
        LineSeries paralellogram;
        private CancellationTokenSource token;
        private bool buttonStopWasClicked = false;

        public AffineTransformationWindow(Window window)
        {
            InitializeComponent();
            BaseWindow = window;
            plotModel = SetPlot();
            _transformator = new AffineTransformator();
            buildParalelogram_Click(this, null);
            buildLineBtn_Click(this, null);
        }

        private void PaintFunction(CancellationToken token)
        {
            try
            {
                #region start
                int x1 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_1.Text; }));
                int y1 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_1.Text; }));
                int x2 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_2.Text; }));
                int y2 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_2.Text; }));
                int x3 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_3.Text; }));
                int y3 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_3.Text; }));
                int x4 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_4.Text; }));
                int y4 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_4.Text; }));
                #endregion

                //Point chosenVertexAsPoint;
                //chosenVertexAsPoint.X = x2;
                //chosenVertexAsPoint.Y = y2;
                //double[,] points = { { x1, y1, 1 }, { x2, y2, 1 }, { x3, y3, 1 }, { x4, y4, 1 } };


                double coofA = Double.Parse(cooficient_A.Dispatcher.Invoke(() => { return cooficient_A.Text; }));
                double coofB = Double.Parse(cooficient_B.Dispatcher.Invoke(() => { return cooficient_B.Text; }));


                double angle = Math.Atan(coofA);
                // - translate -b
                // - rotate -angle
                // - invert y
                // - rotate angle
                // - transtlate b
                AffineTransformation.AffineMatrix translationMatrix =
                    new AffineTransformation.AffineMatrix(
                        1,
                        0,
                        0,
                        1,
                        0,
                        -coofB);

                AffineTransformation.AffineMatrix rotationMatrix =
                    new AffineTransformation.AffineMatrix(
                        Math.Cos(-angle),
                        -Math.Sin(-angle),
                        Math.Sin(-angle),
                        Math.Cos(-angle),
                        0,
                        0);

                AffineTransformation.AffineMatrix scalingMatrix =
                    new AffineTransformation.AffineMatrix(
                        1,
                        0,
                        0,
                        -1,
                        0,
                        0);

                AffineTransformation.AffineMatrix rotationBackMatrix =
                    new AffineTransformation.AffineMatrix(
                        Math.Cos(angle),
                        -Math.Sin(angle),
                        Math.Sin(angle),
                        Math.Cos(angle),
                        0,
                        0);

                AffineTransformation.AffineMatrix translationBackMatrix =
                new AffineTransformation.AffineMatrix(
                    1,
                    0,
                    0,
                    1,
                    0,
                    coofB);

                Point origin = new Point(0, 0);
                Point p1 = new Point(x1, y1);
                Point p2 = new Point(x2, y2);
                Point p3 = new Point(x3, y3);
                Point p4 = new Point(x4, y4);


                p1 = AffineTransformation.Transform(p1, origin, translationMatrix);
                p2 = AffineTransformation.Transform(p2, origin, translationMatrix);
                p3 = AffineTransformation.Transform(p3, origin, translationMatrix);
                p4 = AffineTransformation.Transform(p4, origin, translationMatrix);

                p1 = AffineTransformation.Transform(p1, origin, rotationMatrix);
                p2 = AffineTransformation.Transform(p2, origin, rotationMatrix);
                p3 = AffineTransformation.Transform(p3, origin, rotationMatrix);
                p4 = AffineTransformation.Transform(p4, origin, rotationMatrix);

                p1 = AffineTransformation.Transform(p1, origin, scalingMatrix);
                p2 = AffineTransformation.Transform(p2, origin, scalingMatrix);
                p3 = AffineTransformation.Transform(p3, origin, scalingMatrix);
                p4 = AffineTransformation.Transform(p4, origin, scalingMatrix);

                p1 = AffineTransformation.Transform(p1, origin, rotationBackMatrix);
                p2 = AffineTransformation.Transform(p2, origin, rotationBackMatrix);
                p3 = AffineTransformation.Transform(p3, origin, rotationBackMatrix);
                p4 = AffineTransformation.Transform(p4, origin, rotationBackMatrix);

                p1 = AffineTransformation.Transform(p1, origin, translationBackMatrix);
                p2 = AffineTransformation.Transform(p2, origin, translationBackMatrix);
                p3 = AffineTransformation.Transform(p3, origin, translationBackMatrix);
                p4 = AffineTransformation.Transform(p4, origin, translationBackMatrix);

                double[,] newPoints =
                {
                    { p1.X, p1.Y },
                    { p2.X, p2.Y },
                    { p3.X, p3.Y },
                    { p4.X, p4.Y }
                };

                PaintParallelogram(newPoints);
                //while (true)
                //{
                //    points = _transformator.Transform(points, coofA, coofB);
                //    PaintParallelogram(points);
                //    token.ThrowIfCancellationRequested();
                //    Thread.Sleep(paintTimeout);
                //}
            }
            catch (OperationCanceledException e)
            {
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("Uncorrect input data. Try again \n" + e.Message, "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            //try
            //{
            //    #region start
            //    int x1 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_1.Text; }));
            //    int y1 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_1.Text; }));
            //    int x2 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_2.Text; }));
            //    int y2 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_2.Text; }));
            //    int x3 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_3.Text; }));
            //    int y3 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_3.Text; }));
            //    int x4 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_4.Text; }));
            //    int y4 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_4.Text; }));
            //    #endregion

            //    //Point chosenVertexAsPoint;
            //    //chosenVertexAsPoint.X = x2;
            //    //chosenVertexAsPoint.Y = y2;
            //    double[,] points = { { x1, y1, 1 }, { x2, y2, 1 }, { x3, y3, 1 }, { x4, y4, 1 } };


            //    int coofA = Int32.Parse(cooficient_A.Dispatcher.Invoke(() => { return cooficient_A.Text; }));
            //    int coofB = Int32.Parse(cooficient_B.Dispatcher.Invoke(() => { return cooficient_B.Text; }));
            //    points = _transformator.Transform(points, coofA, coofB);
            //    PaintParallelogram(points);
            //    //while (true)
            //    //{
            //    //    points = _transformator.Transform(points, coofA, coofB);
            //    //    PaintParallelogram(points);
            //    //    token.ThrowIfCancellationRequested();
            //    //    Thread.Sleep(paintTimeout);
            //    //}
            //}
            //catch (OperationCanceledException e)
            //{
            //    return;
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Uncorrect input data. Try again \n" + e.Message, "Input Error",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            buttonStopWasClicked = !buttonStopWasClicked;
            token = new CancellationTokenSource();
            Task.Run(() => PaintFunction(token.Token), token.Token);
        }

        private double[,] CalculateFourthParallelogramVertex(double[,] vertexes)
        {
            double center_M_x = (vertexes[0, 0] + vertexes[2, 0]) / 2d;
            double center_M_y = (vertexes[0, 1] + vertexes[2, 1]) / 2d;

            double vertex_D_x = 2d * center_M_x - vertexes[1, 0];
            double vertex_D_y = 2d * center_M_y - vertexes[1, 1];

            double[,] vertex_D = { { vertex_D_x }, { vertex_D_y }, { 1 } };

            return vertex_D;
        }

        private void PaintParallelogram(double[,] points)
        {
            ResetParalelogramPlot();

            if (AffinePlot != null)
            {
                DataPoint A = new DataPoint(points[0, 0], points[0, 1]);
                DataPoint B = new DataPoint(points[1, 0], points[1, 1]);
                DataPoint C = new DataPoint(points[2, 0], points[2, 1]);
                DataPoint D = new DataPoint(points[3, 0], points[3, 1]);


                paralellogram = new LineSeries();
                paralellogram.Points.Add(A);
                paralellogram.Points.Add(B);
                paralellogram.Points.Add(C);
                paralellogram.Points.Add(D);
                paralellogram.Points.Add(A);

                paralellogram.Color = OxyColor.FromRgb(0, 0, 0);

                plotModel.Series.Add(paralellogram);

                SetAnnotaionForPoint(plotModel, A, "A");
                SetAnnotaionForPoint(plotModel, B, "B");
                SetAnnotaionForPoint(plotModel, C, "C");
                SetAnnotaionForPoint(plotModel, D, "D");

                double scale = plotModel.Axes[0].Scale;

                this.Dispatcher.Invoke(() =>
                {
                    AffinePlot.Model = plotModel;
                    AffinePlot.InvalidatePlot(true);
                });
            }
        }

        private void SetAnnotaionForPoint(PlotModel plotModel, DataPoint points, string name)
        {
            plotModel.Annotations.Add(new PointAnnotation()
            {
                X = Convert.ToDouble(points.X),
                Y = Convert.ToDouble(points.Y),
                Text = String.Format($"{name}' ({Math.Round(points.X, 2)}; {Math.Round(points.Y, 2)})"),
                FontSize = 18,
                FontWeight = 2
            });
        }

        private PlotModel SetPlot()
        {
            const int maxScope = 10;
            var pm = new PlotModel();
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -maxScope,
                Maximum = maxScope,
                PositionAtZeroCrossing = true,
                ExtraGridlines = new[] { 0.0 },
                AxislineColor = OxyColor.FromRgb(196, 196, 196),
                AxislineThickness = 3,
                Font = "Oswald",
                TicklineColor = OxyColor.FromRgb(196, 196, 196),

            });
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = -maxScope,
                Maximum = maxScope,
                PositionAtZeroCrossing = true,
                ExtraGridlines = new[] { 0.0 },
                AxislineColor = OxyColor.FromRgb(196, 196, 196),
                AxislineThickness = 3,
                Font = "Oswald",
                TicklineColor = OxyColor.FromRgb(196, 196, 196)
            });

            //pm.Updated += ModelOnUpdated;
            //pm.PlotType = PlotType.Cartesian;
            return pm;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Image file (*.png) | *.png";
            if (saveFileDialog1.ShowDialog() == false)
                return;
            string filename = saveFileDialog1.FileName;
            var exporter = new PngExporter() { Width = 384, Height = 384 };
            exporter.ExportToFile(AffinePlot.ActualModel, filename);
        }

        private void buildParalelogram_Click(object sender, RoutedEventArgs e)
        {
            ResetParalelogramPlot();

            if (x_1 != null && y_1 != null &&
               x_2 != null && y_2 != null &&
               x_3 != null && y_3 != null &&
               x_4 != null && y_4 != null)
            {
                try
                {
                    int x1 = Int32.Parse(x_1.Text);
                    int y1 = Int32.Parse(y_1.Text);
                    int x2 = Int32.Parse(x_2.Text);
                    int y2 = Int32.Parse(y_2.Text);
                    int x3 = Int32.Parse(x_3.Text);
                    int y3 = Int32.Parse(y_3.Text);

                    double[,] vertex_A = { { x1 }, { y1 }, { 1 } };
                    double[,] vertex_B = { { x2 }, { y2 }, { 1 } };
                    double[,] vertex_C = { { x3 }, { y3 }, { 1 } };

                    double[,] points =
                    {
                              { vertex_A[0, 0], vertex_A[1, 0] },
                              { vertex_B[0, 0], vertex_B[1, 0] },
                              { vertex_C[0, 0], vertex_C[1, 0] },
                    };

                    double[,] vertex_D = CalculateFourthParallelogramVertex(points);


                    double[,] newPoints =
                    {
                              { vertex_A[0, 0], vertex_A[1, 0] },
                              { vertex_B[0, 0], vertex_B[1, 0] },
                              { vertex_C[0, 0], vertex_C[1, 0] },
                              { vertex_D[0, 0], vertex_D[1, 0] }
                    };

                    x_4.Text = vertex_D[0, 0].ToString();
                    y_4.Text = vertex_D[1, 0].ToString();

                    PaintParallelogram(newPoints);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void ResetParalelogramPlot()
        {
            var isContains = plotModel.Series.Contains(paralellogram);
            if (isContains)
            {
                plotModel.Series.Remove(paralellogram);
            }
            plotModel.Annotations.Clear();
        }

        private void buildLineBtn_Click(object sender, RoutedEventArgs e)
        {
            var isContains = plotModel.Series.Contains(line);
            if (isContains)
            {
                plotModel.Series.Remove(line);
            }
            line = new LineSeries();
            int coofA = Int32.Parse(cooficient_A.Text);
            int coofB = Int32.Parse(cooficient_B.Text);
            const int Y = 500;
            double x1 = (-coofB + (-Y)) / coofA;
            double x2 = (-coofB + Y) / coofA;

            line.SeriesGroupName = "Lines";
            line.Points.Add(new DataPoint(x1, -Y));
            line.Points.Add(new DataPoint(x2, Y));
            plotModel.Series.Add(line);
            line.Color = OxyColor.FromRgb(0, 0, 0);

            this.Dispatcher.Invoke(() =>
            {
                AffinePlot.Model = plotModel;
                AffinePlot.InvalidatePlot(true);
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            BaseWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }


        private void resetParalelogram_Click(object sender, RoutedEventArgs e)
        {
            buildParalelogram_Click(this, null);
        }
    }
}
