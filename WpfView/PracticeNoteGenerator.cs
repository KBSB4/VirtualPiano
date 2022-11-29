using Model;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfView
{
    //UNDONE Work In Progress
    public static class PracticeNoteGenerator
    {
        public static Bitmap DrawNotes(Piano piano)
        {
            Bitmap bitmap = new(550, 200);

            //TODO Function: Initial screen
            bitmap = DrawInitialScreen(bitmap);
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
        /// <returns></returns>
        private static Bitmap DrawInitialScreen(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Black, 3), new Rectangle(0, 0, 550, 200));
            }
            return bitmap;
        }

        /// <summary>
        /// This will be used to draw practice notes on the screen including the movement down and deletion
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private static Bitmap DrawPracticeNotes(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {

            }
            return bitmap;
        }

        /// <summary>
        /// This will be used to draw on top of the pianokeys to show correct and incorrect presses
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
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

