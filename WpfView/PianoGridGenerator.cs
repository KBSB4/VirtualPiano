using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfView
{
    internal class PianoGridGenerator
    {
        public PianoGridGenerator(Grid whiteKeyGrid, Grid blackKeyGrid, int columnAmount)
        {
            if (columnAmount < 0)
            {
                return;
            }

            AddWhitePianoKeys(whiteKeyGrid, columnAmount);
            AddBlackPianoKeys(blackKeyGrid, columnAmount);
        }

        /// <summary>
        /// columnAmount is the amount of white keys
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnAmount"></param>
        private static void AddWhitePianoKeys(Grid grid, int columnAmount)
        {
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < columnAmount; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                Button element = new()
                {
                    Background = CreateColor()
                };
                //Rectangle rect = new()
                //{
                //    Fill = CreateColor(),
                //};
                element.IsEnabled = false;
                Grid.SetColumn(element, i);
                Grid.SetRow(element, 0);
                grid.Children.Add(element);
            }
            SetColumnWidth(grid);
        }

        /// <summary>
        /// columnAmount is the amount of black keys
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnAmount"></param>
        private static void AddBlackPianoKeys(Grid grid, int columnAmount)
        {
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < columnAmount; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                Rectangle rect = new()
                {
                    Fill = CreateColor(),
                    //Width = 
                };
                Grid.SetColumn(rect, i);
                Grid.SetRow(rect, 0);
                grid.Children.Add(rect);
            }
            SetColumnWidth(grid);
        }

        private static void SetColumnWidth(Grid grid)
        {
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                grid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
            }
        }

        private static SolidColorBrush CreateColor()
        {
            var random = new Random();

            var r = Convert.ToByte(random.Next(0, 255));
            var g = Convert.ToByte(random.Next(0, 255));
            var b = Convert.ToByte(random.Next(0, 255));

            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }
    }
}
