using Controller;
using Melanchall.DryWetMidi.Interaction;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfView
{
    //UNDONE Work In Progress
    public static class PracticeNoteGenerator
    {
        //TODO Dicitonary with object for display?
        private static Dictionary<Note, Rectangle> CurrentNotesDisplaying { get; set; } = new();

        public static Bitmap DrawNotes(Piano piano)
        {
            Bitmap bitmap = new(550, 200);

            //TODO Function: Initial screen
            bitmap = DrawInitialScreen(bitmap, piano);
            //TODO Function: Notes
            bitmap = DrawPracticeNotes(bitmap);
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
                    g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black, 1), 19 * i, 0, 19 * i, 200);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// This will be used to draw practice notes on the screen including the movement down and deletion
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>The same bitmap in the parameter but edited</returns>
        private static Bitmap DrawPracticeNotes(Bitmap bitmap)
        {
            if (MIDIController.OriginalMIDI is null || MIDIController.AllNotes is null || MainWindow.t is null) { return bitmap; };

            //TODO Is this fast enough to keep up with the MIDI?
            foreach (Note note in MIDIController.AllNotes)
            {
                if (note.Time > (int)MIDIController.CurrentTick && note.Time <= (int)MIDIController.CurrentTick + 5000)
                {
                    if (!CurrentNotesDisplaying.ContainsKey(note))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            //Add new notes
                            Rectangle rect = new Rectangle(30, 20, 19, (int)note.Length/ 100); //TODO Calculate position and colour
                            g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Red), rect);
                            CurrentNotesDisplaying.Add(note, rect);
                        }
                    } else
                    {
                        //If it has been played delete, otherwise move it down
                        if (note.EndTime < (int)MIDIController.CurrentTick)
                        {
                            CurrentNotesDisplaying.Remove(note);
                        }
                        else
                        {
                            using (Graphics g = Graphics.FromImage(bitmap))
                            {
                                //Move old ones down
                                Rectangle rect = CurrentNotesDisplaying[note];
                                rect.Y += 20; //TODO This should be based on the tempo
                                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Red), rect);

                            }
                        }
                    }

                } 
            }
            return bitmap;
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

