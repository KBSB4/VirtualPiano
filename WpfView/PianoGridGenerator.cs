using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    internal class PianoGridGenerator
    {

        public PianoGridGenerator(Grid grid, int rowAmount, int columnAmount)
        {
            if (rowAmount < 0 || columnAmount < 0)
            {
                return;
            }

            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear(); ;

            for (int i = 0; i < rowAmount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }
            for (int i = 0; i < columnAmount; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            Button element = new Button() { Background = Brushes.Aqua, Width =  };
            Grid.SetColumn(element, 4);
            grid.Children.Add(element);
        }
    }
}
