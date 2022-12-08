using Model;
using System;
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
        private PracticePlayPiano PracticePlayPianoPage { get; set; }
        private const int noteLength = 290;
        public Dictionary<Rectangle, PianoKey> keyValuePairs = new();
        public List<PianoKey> upcoming = new();
        public event EventHandler<PianoKeyEventArgs> NoteDeleted;

        /// <summary>
        /// Prepare grids for practice notes
        /// </summary>
        /// <param name="whiteKeyGrid"></param>
        /// <param name="blackKeyGrid"></param>
        /// <param name="columnAmount"></param>
        public PracticeNotesGenerator(Grid whiteKeyGrid, Grid blackKeyGrid, int columnAmount, PracticePlayPiano? ppp)
        {
            PracticePlayPianoPage = ppp;
            if (columnAmount < 0)
            {
                practiceNoteColumns = new();
                return;
            }
            practiceNoteColumns = AddPracticeNoteColumns(whiteKeyGrid, blackKeyGrid, columnAmount);

            if(ppp is not null)
            {
                NoteDeleted += ppp.DeletedPressedKey;
            }
        }
        public PracticeNotesGenerator(Grid whiteKeyGrid, Grid blackKeyGrid, int columnAmount) : this(whiteKeyGrid, blackKeyGrid, columnAmount, null) { }

        /// <summary>
        /// Adds upcoming note
        /// </summary>
        /// <param name="key"></param>
        public void StartExampleNote(PianoKey? key)
        {
            if (key is null) return;
            int note = (((int)key.Octave - 2) * 12) + ((int)key.Note);

            Grid currentColumn = practiceNoteColumns[0];
            if (practiceNoteColumns.Count > note && 0 < note)
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
                Margin = new Thickness(0, -rectHeight, 0, 0),
                RadiusX = 10,
                RadiusY = 10,
            };

            currentColumn.Children.Add(rectangle);
            keyValuePairs.Add(rectangle, key);
            upcoming.Add(key);
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
                            //Use margin to move down
                            rectangle.Margin = new Thickness(0, rectangle.Margin.Top + (column.ActualHeight / 100 * 1.25F), 0, 0);
                            if (rectangle.Margin.Top > column.ActualHeight)
                            {
                                //Remove
                                //column.Children.Remove(rectangle); //TODO DOES NOT WORK, BREAKS ENUMERATOR

                                //Remove key so it can not be scored
                                if (PracticePlayPianoPage is not null)
                                {
                                    NoteDeleted.Invoke(this, new PianoKeyEventArgs(keyValuePairs[rectangle]));
                                    upcoming.Remove(keyValuePairs[rectangle]);
                                }
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
        private static LinearGradientBrush GetPianoKeyColour(PianoKey pianokey)
        {
            LinearGradientBrush whitekeycolour = new()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            whitekeycolour.GradientStops.Add(
                new GradientStop(Colors.Red, 0.0));
            whitekeycolour.GradientStops.Add(
                new GradientStop(Colors.Yellow, 1.0));

            return pianokey.Note.ToString().Contains("Sharp") ? whitekeycolour : whitekeycolour;
        }
    }
}
