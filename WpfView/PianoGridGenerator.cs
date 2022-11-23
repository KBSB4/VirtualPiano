using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    internal class PianoGridGenerator
    {
        /// <summary>
        /// Generate whitekeys and blackkeys for the WPF
        /// </summary>
        /// <param name="whiteKeyGrid"></param>
        /// <param name="blackKeyGrid"></param>
        /// <param name="columnAmount"></param>
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
        private static void AddWhitePianoKeys(Grid whiteKeyGrid, int columnAmount)
        {
            whiteKeyGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < columnAmount; i++)
            {
                //Create key
                whiteKeyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                Button rect = new()
                {
                    Background = new SolidColorBrush(new Random().Next(8) == 1 ? Color.FromRgb(90, 100, 255) : Color.FromRgb(255, 255, 255)),
                    Margin = new Thickness(0, 0, 0, 0),
                };

                //Add key
                Grid.SetColumn(rect, i);
                Grid.SetRow(rect, 0);
                whiteKeyGrid.Children.Add(rect);
            }
            SetColumnWidth(whiteKeyGrid);
        }

        /// <summary>
        /// columnAmount is the amount of black keys
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnAmount"></param>
        private static void AddBlackPianoKeys(Grid blackKeyGrid, int columnAmount)
        {
            blackKeyGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < columnAmount; i++)
            {
                //Create Key
                blackKeyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                if (!(i % 7 == 2 || i % 7 == 6))
                {
                    Button rect = new()
                    {
                        Background = Brushes.Black,
                        Margin = new Thickness((1920 / columnAmount) / 1.75d, 0, -15, 30) //68.57 is the exact pixel width of one key 68.57 x 28 = 1920 pixels
                    };

                    //Add key
                    Grid.SetColumn(rect, i);
                    Grid.SetRow(rect, 0);
                    blackKeyGrid.Children.Add(rect);
                }
            }
            SetColumnWidth(blackKeyGrid);
        }

        /// <summary>
        /// Selfexplanatory
        /// </summary>
        /// <param name="grid"></param>
        private static void SetColumnWidth(Grid grid)
        {
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                grid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
            }
        }
    }
}
