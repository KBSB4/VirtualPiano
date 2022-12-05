using Melanchall.DryWetMidi.MusicTheory;
using Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace WpfView
{
    internal class PracticeNotesGenerator
    {
        private List<Grid> practiceNoteColumns;
        private const int noteLength = 390;
        //TODO do this base do tempomap
        private const int noteSpeed = 10;

        /// <summary>
        /// Prepare grids for practice notes
        /// </summary>
        /// <param name="whiteKeyGrid"></param>
        /// <param name="blackKeyGrid"></param>
        /// <param name="columnAmount"></param>
        public PracticeNotesGenerator(Grid whiteKeyGrid, Grid blackKeyGrid, int columnAmount)
        {
            if (columnAmount < 0)
            {
                practiceNoteColumns = new();
                return;
            }
            practiceNoteColumns = AddPracticeNoteColumns(whiteKeyGrid, blackKeyGrid, columnAmount);
        }

        /// <summary>
        /// Adds upcoming note
        /// </summary>
        /// <param name="key"></param>
        public void StartExampleNote(PianoKey? key)
        {
            if (key is null) return;
            int note = (((int)key.Octave - 2) * 12) + ((int)key.Note);

            Grid currentColumn = practiceNoteColumns[0];
			if (practiceNoteColumns.Count > note)
            {
				currentColumn = practiceNoteColumns[note];
            }

            double rectHeight = key.Duration.TotalSeconds * noteLength;
            Rectangle rectangle = new()
            {
                Fill = GetPianoKeyColour(key),
                Height = rectHeight,
                Width = currentColumn.ActualWidth,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, -rectHeight, 0, 0)
            };

            currentColumn.Children.Add(rectangle);
        }

        /// <summary>
        /// Update each note to fall down
        /// </summary>
        public void UpdateExampleNotes()
        {
            foreach (var column in practiceNoteColumns)
            {
                if (column.Children.Count > 0)
                {
                    foreach (var note in column.Children)
                    {
                        Rectangle? rectangle = note as Rectangle;
                        if (rectangle is not null)
                        {
                            rectangle.Margin = new Thickness(0, rectangle.Margin.Top + noteSpeed, 0, 0);
                            if (rectangle.Margin.Top > column.Height)
                            {
                                //Remove
                                column.Children.Remove(rectangle);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add columns to each grid
        /// </summary>
        /// <param name="whiteKeyGrid"></param>
        /// <param name="blackKeyGrid"></param>
        /// <param name="columnAmount"></param>
        /// <returns></returns>
        private List<Grid> AddPracticeNoteColumns(Grid whiteKeyGrid, Grid blackKeyGrid, int columnAmount)
        {
            List<Grid> columns = new();

            blackKeyGrid.ColumnDefinitions.Clear();
            whiteKeyGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < columnAmount; i++)
            {
                whiteKeyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                Grid whiteColumn = new()
                {
                    Margin = new Thickness(0, 0, 0, 0),
                };
                AddKey(whiteKeyGrid, columns, i, whiteColumn);

                blackKeyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                if (!(i % 7 == 2 || i % 7 == 6))
                {
                    Grid blackColumn = new()
                    {
                        Margin = new Thickness((1920 / columnAmount) / 1.75d, 0, -15, 0),
                    };
                    AddKey(blackKeyGrid, columns, i, blackColumn);
                }
            }
            SetColumnWidth(whiteKeyGrid);
            SetColumnWidth(blackKeyGrid);

            return columns;
        }

        /// <summary>
        /// Add key to grid
        /// </summary>
        /// <param name="keyGrid"></param>
        /// <param name="gridColumns"></param>
        /// <param name="i"></param>
        /// <param name="keyColumnGrid"></param>
        private static void AddKey(Grid keyGrid, List<Grid> gridColumns, int i, Grid keyColumnGrid)
        {
            Grid.SetColumn(keyColumnGrid, i);
            Grid.SetRow(keyColumnGrid, 0);
            keyGrid.Children.Add(keyColumnGrid);
            gridColumns.Add(keyColumnGrid);
        }

        /// <summary>
        /// Set width of column
        /// </summary>
        /// <param name="grid"></param>
        private static void SetColumnWidth(Grid grid)
        {
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                grid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
            }
        }

        /// <summary>
        /// Finds the right colour for the right note
        /// </summary>
        /// <param name="pianokey"></param>
        /// <returns>SolidBrush</returns>
        private static SolidColorBrush GetPianoKeyColour(PianoKey pianokey)
        {
            SolidColorBrush solidBrush;
            switch (pianokey.Note)
            {
                case NoteName.C:
                    solidBrush = new SolidColorBrush(Colors.Red);
                    break;
                case NoteName.CSharp:
                    solidBrush = new SolidColorBrush(Colors.DarkRed);
                    break;
                case NoteName.D:
                    solidBrush = new SolidColorBrush(Colors.Green);
                    break;
                case NoteName.DSharp:
                    solidBrush = new SolidColorBrush(Colors.DarkGreen);
                    break;
                case NoteName.E:
                    solidBrush = new SolidColorBrush(Colors.DeepSkyBlue);
                    break;
                case NoteName.F:
                    solidBrush = new SolidColorBrush(Colors.Blue);
                    break;
                case NoteName.FSharp:
                    solidBrush = new SolidColorBrush(Colors.DarkBlue);
                    break;
                case NoteName.G:
                    solidBrush = new SolidColorBrush(Colors.Yellow);
                    break;
                case NoteName.GSharp:
                    solidBrush = new SolidColorBrush(Colors.LightYellow);
                    break;
                case NoteName.A:
                    solidBrush = new SolidColorBrush(Colors.Purple);
                    break;
                case NoteName.ASharp:
                    solidBrush = new SolidColorBrush(Colors.MediumPurple);
                    break;
                case NoteName.B:
                    solidBrush = new SolidColorBrush(Colors.DarkTurquoise);
                    break;
                default:
                    solidBrush = new SolidColorBrush(Colors.DarkSeaGreen);
                    break;
            }
            return solidBrush;
        }
    }
}
