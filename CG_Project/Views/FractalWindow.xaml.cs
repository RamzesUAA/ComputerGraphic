using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CG_Project.Services;

namespace CG_Project
{
    /// <summary>
    /// Interaction logic for FractalWindow.xaml
    /// </summary>
    public partial class FractalWindow : Window
    {
        private KochSnowflake kochSnowflake = default;
        public FractalWindow()
        {
            InitializeComponent();
            // determine the size of the snowflake:
            kochSnowflake = new KochSnowflake(fractalCanvas.Height, fractalCanvas.Width);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void CmdUp_OnClick(object sender, RoutedEventArgs e)
        {
            var num = Convert.ToInt32(numberOfIterations.Text);
            num += 1;
            numberOfIterations.Text = num.ToString();
        }

        private void CmdDown_OnClick(object sender, RoutedEventArgs e)
        {
            var num = Convert.ToInt32(numberOfIterations.Text);
            num -= 1;
            numberOfIterations.Text = num.ToString();
        }

        private void BuildFractal_OnClick(object sender, RoutedEventArgs e)
        {
            fractalCanvas.Children.Clear();
            var checkedValue = radioButtonPannel.Children.OfType<RadioButton>()
                .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            switch (fractalTypes?.SelectedIndex)
            {
                case 0:
                    if (checkedValue.Content.ToString() == FractalNames.KochCurve)
                    {
                        RunGeometricKochSnowflake();
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.DragonCurve)
                    {

                    }
                    break;
                case 1:
                    if (checkedValue.Content.ToString() == FractalNames.KochCurve)
                    {
                    }
                    else if (checkedValue.Content.ToString() == FractalNames.DragonCurve)
                    {

                    }
                    else if (checkedValue.Content.ToString() == FractalNames.BarnsleyFern)
                    {

                    }
                    break;
            }

        }

        private void RunGeometricKochSnowflake()
        {
            kochSnowflake.numberOfIterations = Convert.ToInt32(numberOfIterations.Text);
            kochSnowflake.i = 0;
            kochSnowflake.II = 0;
            fractalCanvas.Children.Add(kochSnowflake.pl);
            CompositionTarget.Rendering += kochSnowflake.StartAnimation;
        }

        private void PhonesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fractals = new List<RadioButton>();
            switch (fractalTypes.SelectedIndex)
            {
                case 0:
                    radioButtonPannel.Children.Clear();
                    fractals.Add(new RadioButton() { Content = FractalNames.KochCurve, IsChecked = false });
                    fractals.Add(new RadioButton() { Content = FractalNames.DragonCurve, IsChecked = false });
                    foreach (var fractal in fractals)
                    {
                        radioButtonPannel.Children.Add(fractal);
                    }
                    break;
                case 1:
                    radioButtonPannel.Children.Clear();
                    fractals.Add(new RadioButton() { Content = FractalNames.KochCurve, IsChecked = false });
                    fractals.Add(new RadioButton() { Content = FractalNames.DragonCurve, IsChecked = false });
                    fractals.Add(new RadioButton() { Content = FractalNames.BarnsleyFern, IsChecked = false });
                    foreach (var fractal in fractals)
                    {
                        radioButtonPannel.Children.Add(fractal);
                    }
                    break;
            }
        }
    }

    public static class FractalNames
    {
        public const string KochCurve = "Koch curve";
        public const string DragonCurve = "Dragon Curve";
        public const string BarnsleyFern = "Barnsley fern";
    }
}
