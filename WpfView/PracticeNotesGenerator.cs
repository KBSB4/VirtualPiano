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
        private const int noteLength = 290; //120 BPM

        //private double noteSpeed = 12; //120 BPM
        //private Queue<double> tempoQueue = new();
        //private bool FirstTime = true;

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
                RadiusX = 5,
                RadiusY = 5,
            };

            currentColumn.Children.Add(rectangle);
        }

        /// <summary>
        /// Moves all notes down 1.25% of the screen, should be fired 40 times a second to move notes down completely in 2 seconds
        /// If the note is not visible on screen anymore, the note is removed from the column it is in
        /// </summary>
        public void UpdateExampleNotes()
        {
            List<Rectangle> notesToBeRemoved = new();
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
                                notesToBeRemoved.Add(rectangle);
                            }
                        }
                    }
                }
            }
            foreach (var item in notesToBeRemoved)
            {
                Grid? grid = item.Parent as Grid;
                if (grid is not null)
                {
                    grid.Children.Remove(item);
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
            whitekeycolour.GradientStops.Add(new GradientStop(Colors.Orange, 0.0));
            whitekeycolour.GradientStops.Add(new GradientStop(Colors.Yellow, 1.0));

            LinearGradientBrush blackKeyColour = new()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            blackKeyColour.GradientStops.Add(new GradientStop(Colors.DarkRed, 0.0));
            blackKeyColour.GradientStops.Add(new GradientStop(Colors.DarkOrange, 1.0));

            return pianokey.Note.ToString().Contains("Sharp") ? blackKeyColour : whitekeycolour;
        }
    }
}
