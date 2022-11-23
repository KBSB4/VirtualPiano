using Model;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void DisplayPianoKey(PianoKey key, bool pressed)
        {
            int note = (((int)key.Octave) * 12) + ((int)key.Note);

            Button test = buttons[note];
            Debug.WriteLine(test.Background);

            switch (key.Note)
            {
                case Melanchall.DryWetMidi.MusicTheory.NoteName.C:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.D:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.E:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.F:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.G:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.A:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.B:

                    test.Background = pressed ? new SolidColorBrush(Color.FromRgb(90, 120, 255)) : new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    break;
                case Melanchall.DryWetMidi.MusicTheory.NoteName.CSharp:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.DSharp:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.FSharp:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.GSharp:
                case Melanchall.DryWetMidi.MusicTheory.NoteName.ASharp:
                    test.Background = pressed ? new SolidColorBrush(Color.FromRgb(50, 50, 150)) : new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    break;
                default:
                    test.Background = pressed ? new SolidColorBrush(Color.FromRgb(90, 100, 255)) : new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    break;
            }

        }

        /// <summary>
        /// columnAmount is the amount of white keys
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnAmount"></param>
        private static List<Button> AddPianoKeys(Grid whiteKeyGrid, Grid blackKeyGrid, int columnAmount)
        {
            List<Button> buttons = new();


            blackKeyGrid.ColumnDefinitions.Clear();
            whiteKeyGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < columnAmount; i++)
            {
                //Create key
                whiteKeyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                Button whiteKeyButton = new()
                {
                    Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)), //new SolidColorBrush(new Random().Next(8) == 1 ? Color.FromRgb(90, 100, 255) : Color.FromRgb(255, 255, 255)),
                    Margin = new Thickness(0, 0, 0, 0),
                };

                //Add key
                Grid.SetColumn(whiteKeyButton, i);
                Grid.SetRow(whiteKeyButton, 0);
                whiteKeyGrid.Children.Add(whiteKeyButton);
                buttons.Add(whiteKeyButton);


                blackKeyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                if (!(i % 7 == 2 || i % 7 == 6))
                {
                    Button blackKeyButton = new()
                    {
                        Background = Brushes.Black,
                        Margin = new Thickness((1920 / columnAmount) / 1.75d, 0, -15, 30) //68.57 is the exact pixel width of one key 68.57 x 28 = 1920 pixels
                    };

                    //Add key
                    Grid.SetColumn(blackKeyButton, i);
                    Grid.SetRow(blackKeyButton, 0);
                    blackKeyGrid.Children.Add(blackKeyButton);
                    buttons.Add(blackKeyButton);
                }
            }
            SetColumnWidth(whiteKeyGrid);
            SetColumnWidth(blackKeyGrid);
            return buttons;
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
