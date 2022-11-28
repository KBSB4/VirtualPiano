using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Drawing.Brush;

namespace WpfView
{
    public static class PracticeNoteGenerator
    {
        //UNDONE Work In Progress

        public static Bitmap DrawNotes()
        {
            Bitmap bitmap = new(200,200);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Black, 3), new Rectangle(0,0,200,200));
            }
           return null;
        }

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

