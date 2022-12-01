using Controller;
using Melanchall.DryWetMidi.MusicTheory;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfView
{
    public static class PracticeNoteGenerator
    {
        private static Dictionary<PianoKey, Rectangle> CurrentNotesDisplaying { get; set; } = new();

        /// <summary>
        /// Main function that calls the rest
        /// </summary>
        /// <param name="piano"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Bitmap DrawNotes(Piano piano, PianoKey key)
        {
            Bitmap bitmap = new(550, 200);

            bitmap = DrawInitialScreen(bitmap, piano);

            //Prevents flicker
            if (key is not null)
            {
                AddNewNotes(piano, key);
                return null;
            }

            bitmap = UpdateView(bitmap);
            //TODO Function: Key visualisation (show if correct pressed or not)
            bitmap = DrawKeyVisualisation(bitmap);

            return bitmap;
        }

        /// <summary>
        /// Draw lines that act as lanes for the practice notes
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="piano"></param>
        /// <returns><param name="bitmap"></param></returns>
        private static Bitmap DrawInitialScreen(Bitmap bitmap, Piano piano)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                //g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Black, 3), new Rectangle(0, 0, 550, 200));
                for (int i = 1; i <= 48; i++)
                {
                    //TODO Fix line positions
                    g.DrawLine(new System.Drawing.Pen(new SolidBrush(System.Drawing.Color.Black)), 20 * i, -500, 20 * i, 500);
                    //TODO add sharps
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Adds new notes to display to the player
        /// </summary>
        /// <param name="piano"></param>
        /// <param name="pianokey"></param>
        private static void AddNewNotes(Piano piano, PianoKey? pianokey)
        {
            if (MIDIController.OriginalMIDI is null || MIDIController.AllNotes is null || MainWindow.t is null) { return; };

            if (pianokey is not null && !CurrentNotesDisplaying.ContainsKey(pianokey))
            {
                //Add new notes
                //Set x and size
                int x = 0;
                int size = (int)pianokey.Duration / 100;
                //TODO SHARPS
                x = piano.PianoKeys.FindIndex(x => x.Note == pianokey.Note) * 20 + 1;

                //Create a new rectangle that visualises the note
                //Offset by its size so it plays at the start of the note and not at the end
                Rectangle rect = new Rectangle(x, 0 - size, 18, size);
                CurrentNotesDisplaying.Add(pianokey, rect);
            }
        }

        /// <summary>
        /// Updates the bitmap to show all notes that are placed
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns><param name="bitmap"></param></returns>
        private static Bitmap UpdateView(Bitmap bitmap)
        {
            foreach (PianoKey pk in CurrentNotesDisplaying.Keys)
            {
                //If it has been played -> delete, otherwise move it down
                if (pk.TimeStamp >= SongController.CurrentSong.SongTimer.ElapsedMilliseconds)
                {
                    CurrentNotesDisplaying.Remove(pk);
                }
                else
                {
                    //Move down?
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        Rectangle rect = CurrentNotesDisplaying[pk];
                        //TODO This should be based on the tempo, however this might need to be done with a faster timer. Unfortunately a faster timer also crashes the program
                        rect.Y += 5;
                        g.FillRectangle(GetPianoKeyColour(pk), rect);
                        //Save new position
                        CurrentNotesDisplaying[pk] = rect;
                    }
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Finds the right colour for the right note
        /// </summary>
        /// <param name="pianokey"></param>
        /// <returns>SolidBrush</returns>
        private static SolidBrush GetPianoKeyColour(PianoKey pianokey)
        {
            SolidBrush solidBrush;
            switch (pianokey.Note)
            {
                case NoteName.C:
                    solidBrush = new SolidBrush(System.Drawing.Color.Red);
                    break;
                case NoteName.CSharp:
                    solidBrush = new SolidBrush(System.Drawing.Color.DarkRed);
                    break;
                case NoteName.D:
                    solidBrush = new SolidBrush(System.Drawing.Color.Green);
                    break;
                case NoteName.DSharp:
                    solidBrush = new SolidBrush(System.Drawing.Color.DarkGreen);
                    break;
                case NoteName.E:
                    solidBrush = new SolidBrush(System.Drawing.Color.DeepSkyBlue);
                    break;
                case NoteName.F:
                    solidBrush = new SolidBrush(System.Drawing.Color.Blue);
                    break;
                case NoteName.FSharp:
                    solidBrush = new SolidBrush(System.Drawing.Color.DarkBlue);
                    break;
                case NoteName.G:
                    solidBrush = new SolidBrush(System.Drawing.Color.Yellow);
                    break;
                case NoteName.GSharp:
                    solidBrush = new SolidBrush(System.Drawing.Color.LightYellow);
                    break;
                case NoteName.A:
                    solidBrush = new SolidBrush(System.Drawing.Color.Purple);
                    break;
                case NoteName.ASharp:
                    solidBrush = new SolidBrush(System.Drawing.Color.MediumPurple);
                    break;
                case NoteName.B:
                    solidBrush = new SolidBrush(System.Drawing.Color.DarkTurquoise);
                    break;
                default:
                    solidBrush = new SolidBrush(System.Drawing.Color.DarkSeaGreen);
                    break;
            }
            return solidBrush;
        }

        /// <summary>
        /// This will be used to draw on top of the pianokeys to show correct and incorrect presses
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>The same bitmap in the parameter but edited</returns>
        private static Bitmap DrawKeyVisualisation(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {

            }
            return bitmap;
        }

        /// <summary>
        /// Creates a BitmapSource from a bitmap for the WPF
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}

