using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    internal class PianoGridGenerator
    {
        public PianoGridGenerator(Grid grid, int columnAmount)
        {
            if (columnAmount < 0)
            {
                return;
            }

            AddPianoKeys(grid, columnAmount);
            SetColumnWidth(grid);
        }

        private static void AddPianoKeys(Grid grid, int columnAmount)
        {
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < columnAmount; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                Button element = new()
                {
                    Background = Brushes.White,
                    Content = i.ToString(),
                };
                Grid.SetColumn(element, i);
                Grid.SetRow(element, 0);
                grid.Children.Add(element);
            }
        }

        private static void SetColumnWidth(Grid grid)
        {
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                grid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
            }
        }
    }
}
