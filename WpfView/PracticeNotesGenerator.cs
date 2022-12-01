using Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfView
{
    internal class PracticeNotesGenerator
    {
        private List<Grid> practiceNoteColumns;
        private const int noteLength = 390;
        private const int noteSpeed = 10;
        public PracticeNotesGenerator(Grid whiteKeyGrid, Grid blackKeyGrid, int columnAmount)
        {
            if (columnAmount < 0)
            {
                practiceNoteColumns = new();
                return;
            }
            practiceNoteColumns = AddPracticeNoteColumns(whiteKeyGrid, blackKeyGrid, columnAmount);
        }

        public void StartExampleNote(PianoKey? key)
        {
            if (key is null) return;
            int note = (((int)key.Octave - 2) * 12) + ((int)key.Note);
            Grid currentColumn = practiceNoteColumns[note];

            double rectHeight = key.Duration.TotalSeconds * noteLength;
            Rectangle rectangle = new()
            {
                Fill = new SolidColorBrush(Colors.AliceBlue),
                Height = rectHeight,
                Width = currentColumn.ActualWidth,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, -rectHeight, 0, 0)
            };

            currentColumn.Children.Add(rectangle);
        }

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
                                column.Children.Remove(rectangle);
                            }
                        }
                    }
                }
            }
        }

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

        private static void AddKey(Grid keyGrid, List<Grid> gridColumns, int i, Grid keyColumnGrid)
        {
            Grid.SetColumn(keyColumnGrid, i);
            Grid.SetRow(keyColumnGrid, 0);
            keyGrid.Children.Add(keyColumnGrid);
            gridColumns.Add(keyColumnGrid);
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
