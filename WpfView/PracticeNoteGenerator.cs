using Controller;
using Melanchall.DryWetMidi.MusicTheory;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfView
{
    //UNDONE Work In Progress
    public static class PracticeNoteGenerator
    {
        //TODO Dicitonary with object for display?
        private static Dictionary<PianoKey, Rectangle> CurrentNotesDisplaying { get; set; } = new();

        public static Bitmap DrawNotes(Piano piano, PianoKey key)
        {
            Bitmap bitmap = new(550, 200);

            bitmap = DrawInitialScreen(bitmap, piano);
            bitmap = DrawPracticeNotes(bitmap, piano, key);
            //TODO Function: Key visualisation (show if correct pressed or not)
            bitmap = DrawKeyVisualisation(bitmap);

            return bitmap;
        }

        /// <summary>
        /// This could be used to draw lines to better visualise the 'lanes' for the practice notes
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>The same bitmap in the parameter but edited</returns>
        private static Bitmap DrawInitialScreen(Bitmap bitmap, Piano piano)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                //g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Black, 3), new Rectangle(0, 0, 550, 200));
                for (int i = 1; i <= 48; i++)
                {
                    //TODO Fix line positions
                    g.DrawLine(new System.Drawing.Pen(new SolidBrush(System.Drawing.Color.Black)), 19 * i, 0, 19 * i, 200);
                    //TODO add sharps
                }
            }
            return bitmap;
        }

        /// <summary>
        /// This will be used to draw practice notes on the screen including the movement down and deletion
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>The same bitmap in the parameter but edited</returns>
        private static Bitmap DrawPracticeNotes(Bitmap bitmap, Piano piano, PianoKey? pianokey)
        {
            if (MIDIController.OriginalMIDI is null || MIDIController.AllNotes is null || MainWindow.t is null) { return bitmap; };

            if (pianokey is not null && !CurrentNotesDisplaying.ContainsKey(pianokey))
            {
                    //Add new notes
                    //Set x and size
                    int x = 0;
                    int size = (int)pianokey.Duration / 100;
                    //TODO SHARPS
                    x = piano.PianoKeys.FindIndex(x => x.Note == pianokey.Note)*19;
                   
                    //Create a new rectangle that visualises the note
                    //Offset by its size so it plays at the start of the note and not at the end
                    Rectangle rect = new Rectangle(x, 0 - size, 18, size);
                    CurrentNotesDisplaying.Add(pianokey, rect);
            } else
            {
                UpdateExistingNotes(bitmap);
            }

            return bitmap;
        }

        private static void UpdateExistingNotes(Bitmap bitmap)
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
                        rect.Y += 5; //TODO This should be based on the tempo
                        g.FillRectangle(GetPianoKeyColour(pk), rect);
                        //Save new position
                        CurrentNotesDisplaying[pk] = rect;
                    }
                }
            }
        }

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
                throw new ArgumentNullException(nameof(bitmap));

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

