using MyDartsManager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyDartsManager.UserControls
{
    /// <summary>
    /// Interaction logic for DartBoard.xaml
    /// </summary>
    public partial class DartBoard : UserControl
    {
        private static List<int> _slicesNums = new List<int>() { 3, 19, 7, 16, 8, 11, 14, 9, 12, 5, 20, 1, 18, 4, 13, 6, 10, 15, 2, 17 };

        public event EventHandler<Tuple<int, int>> DartBoardClicked;

        public DartBoard(int width, Point offset)
        {
            InitializeComponent();
            createDartBoard(width, offset);
        }


        /// <summary>
        /// Creates dart board from Paths. Dartboard has given width and is offset by the center point.
        /// </summary>
        /// <param name="width">Width of dartoard</param>
        /// <param name="center">offset of the dartboard</param>
        private void createDartBoard(double width, Point center)
        {
            Grid customGrid = new Grid();

            Path zero = GUIComponentHelper.createCircle(center, (int)width);
            zero.Fill = Brushes.Black;
            MouseButtonEventHandler zeroHandler = (sender, e) => dartBoardClick(sender, e, 0, 1);
            zero.MouseDown += zeroHandler;
            customGrid.Children.Add(zero);

            for (int i = 0; i < _slicesNums.Count; i++)
            {
                Path slice = GUIComponentHelper.createSlice(99 + i * 18, 81 + i * 18, (int)(width / 100 * 75.5), center);
                int currentIndex = i;
                MouseButtonEventHandler clickHandler = (sender, e) => dartBoardClick(sender, e, _slicesNums[currentIndex], 1);
                if (i % 2 == 0)
                {
                    slice.Fill = Brushes.Black;
                }
                else
                {
                    slice.Fill = Brushes.Beige;
                }
                slice.MouseDown += clickHandler;
                customGrid.Children.Add(slice);

                Label label = new Label();
                label.Content = _slicesNums[i].ToString();
                label.Foreground = Brushes.Silver;
                label.FontSize = 80;
                label.IsHitTestVisible = false;
                label.Margin = new Thickness(GUIComponentHelper.PolarToCartesianPoint((int)(width / 100 * 87.75), -i * 18).Y + center.Y - 45,
                    GUIComponentHelper.PolarToCartesianPoint((int)(width / 100 * 87.75), -i * 18).X + center.X - 45, 0, 0);
                customGrid.Children.Add(label);
            }

            for (int i = 0; i < _slicesNums.Count; i++)
            {
                Path curve = GUIComponentHelper.createCurve(99 + i * 18, 81 + i * 18, (int)(width / 100 * 75.5), (int)(width / 100 * 72), center);
                int currentIndex = i;
                MouseButtonEventHandler clickHandler = (sender, e) => dartBoardClick(sender, e, _slicesNums[currentIndex], 2);
                if (i % 2 == 0)
                {
                    curve.Fill = Brushes.Red;
                }
                else
                {
                    curve.Fill = Brushes.Green;
                }
                curve.MouseDown += clickHandler;
                customGrid.Children.Add(curve);
            }

            for (int i = 0; i < _slicesNums.Count; i++)
            {
                Path curve = GUIComponentHelper.createCurve(99 + i * 18, 81 + i * 18, (int)(width / 100 * 47.5), (int)(width / 100 * 44), center);
                int currentIndex = i;
                MouseButtonEventHandler clickHandler = (sender, e) => dartBoardClick(sender, e, _slicesNums[currentIndex], 3);
                if (i % 2 == 0)
                {
                    curve.Fill = Brushes.Red;
                }
                else
                {
                    curve.Fill = Brushes.Green;
                }
                curve.MouseDown += clickHandler;
                customGrid.Children.Add(curve);
            }

            Path single25 = GUIComponentHelper.createCircle(center, (int)(width / 100 * 7.1));
            Path double25 = GUIComponentHelper.createCircle(center, (int)(width / 100 * 2.8));

            MouseButtonEventHandler single25Handler = (sender, e) => dartBoardClick(sender, e, 25, 1);
            MouseButtonEventHandler double25Handler = (sender, e) => dartBoardClick(sender, e, 25, 2);

            single25.MouseDown += single25Handler;
            double25.MouseDown += double25Handler;

            single25.Fill = Brushes.Green;
            double25.Fill = Brushes.Red;

            customGrid.Children.Add(single25);
            customGrid.Children.Add(double25);

            Viewbox box = new Viewbox();
            box.Margin = new Thickness(0, 0, 0, 0);
            box.Child = customGrid;
            box.HorizontalAlignment = HorizontalAlignment.Left;
            box.VerticalAlignment = VerticalAlignment.Top;

            rootGrid.Children.Add(box);
        }

        /// <summary>
        /// Reacts on the click on dartboard and adds score to the current player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="value"> value of the throw</param>
        /// <param name="multiplier"> multiplier of the throw</param>
        private void dartBoardClick(object sender, MouseButtonEventArgs e, int value, int multiplier)
        {
            DartBoardClicked?.Invoke(this, new Tuple<int, int>(value, multiplier));
            return;
        }
    }
}
