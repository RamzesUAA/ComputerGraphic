using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ColorConverter = CG_Project.Services.ColorsService.ColorConverter;
using Color = System.Drawing.Color;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot.Annotations;

namespace CG_Project.Views
{
    /// <summary>
    /// Interaction logic for ColorModelsWindow.xaml
    /// </summary>
    public partial class AffineTransformationWindow : Window
    {

        private Window BaseWindow;
        PlotModel plotModel;
        LineSeries line;
        LineSeries paralellogram;

        public AffineTransformationWindow(Window window)
        {
            InitializeComponent();
            BaseWindow = window;
            plotModel = SetPlot();
            buildParalelogram_Click(this, null);
            buildLineBtn_Click(this, null);
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
                Text = String.Format($"{name}' ({points.X}; {points.Y})"),
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
            line.Points.Add(new DataPoint(x1, (-Y)));
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
    }
}
