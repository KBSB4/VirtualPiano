using Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    internal class PianoGridGenerator
    {
        private List<Button> buttons;

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
            buttons = AddPianoKeys(whiteKeyGrid, blackKeyGrid, columnAmount);
        }

        /// <summary>
        /// Display pianokey if they are pressed down
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pressed"></param>
        public void DisplayPianoKey(PianoKey key, bool pressed)
        {
            if (key is null) return;
            int note = (((int)key.Octave - 2) * 12) + ((int)key.Note);//berekening uitleggen
            Button currentButton = buttons[note];

            if (key.Note.ToString().Contains("Sharp"))
            {
                currentButton.Background = pressed ? new SolidColorBrush(Color.FromRgb(40, 57, 149)) : new SolidColorBrush(Color.FromRgb(30, 30, 30));
            }
            else
            {
                currentButton.Background = pressed ? new SolidColorBrush(Color.FromRgb(72, 91, 190)) : new SolidColorBrush(Colors.White);
            }
        }

        /// <summary>
        /// Adds the amount of white keys specified and places black keys in between
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnAmount"></param>
        /// <returns>A list with the placed buttons</returns>
        private List<Button> AddPianoKeys(Grid whiteKeyGrid, Grid blackKeyGrid, int columnAmount)
        {
            List<Button> buttons = new();

            blackKeyGrid.ColumnDefinitions.Clear();
            whiteKeyGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < columnAmount; i++)
            {
                whiteKeyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                Button whiteKeyButton = new()
                {
                    Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    Margin = new Thickness(0, 0, 0, 0),
                    BorderThickness = new Thickness(1, 0, 1, 0),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                };

                AddKey(whiteKeyGrid, buttons, i, whiteKeyButton);

                blackKeyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                if (!(i % 7 == 2 || i % 7 == 6))
                {
                    Button blackKeyButton = new()
                    {
                        Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                        Margin = new Thickness((1920 / columnAmount) / 1.75d, 0, -15, 30),
                        BorderThickness = new Thickness(0, 0, 0, 0),
                    };
                    AddKey(blackKeyGrid, buttons, i, blackKeyButton);
                }
            }
            SetColumnWidth(whiteKeyGrid);
            SetColumnWidth(blackKeyGrid);
            return buttons;
        }

        /// <summary>
        /// Adds a key to the provided grid
        /// </summary>
        /// <param name="keyGrid"></param>
        /// <param name="buttons"></param>
        /// <param name="i"></param>
        /// <param name="keyButton"></param>
        private static void AddKey(Grid keyGrid, List<Button> buttons, int i, Button keyButton)
        {
            Grid.SetColumn(keyButton, i);
            Grid.SetRow(keyButton, 0);
            keyGrid.Children.Add(keyButton);
            buttons.Add(keyButton);
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
