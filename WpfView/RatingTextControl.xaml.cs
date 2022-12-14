using Model;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for RatingTextControl.xaml
    /// </summary>
    public partial class RatingTextControl : UserControl
    {
        private static int amounExisting = 0;
        public static int AmounExisting { get => amounExisting; set => amounExisting = value; }
        //Control properties
        public static readonly DependencyProperty RotationProperty =
        DependencyProperty.Register("Rotation", typeof(int), typeof(RatingTextControl), new PropertyMetadata(0));

        public static readonly DependencyProperty RatingTextProperty =
            DependencyProperty.Register("RatingText", typeof(string), typeof(RatingTextControl), new PropertyMetadata("no name"));

        public string RatingText
        {
            get { return (string)GetValue(RatingTextProperty); }
            set { SetValue(RatingTextProperty, value); }
        }

        public int Rotation
        {
            get { return (int)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        /// <summary>
        /// Creates the rating text to display
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="rotation"></param>
        public RatingTextControl(Rating rating, int rotation)
        {
            AmounExisting++;
            RatingText = rating.ToString();
            Rotation = rotation;
            InitializeComponent();
            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateRatingText));
            updateVisualNoteThread.Start();
        }

        /// <summary>
        /// Updates text
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateRatingText(object? obj)
        {
            Thread.Sleep(1000);
            AmounExisting--;
            Dispatcher.Invoke(new Action(() =>
            {
                ((Grid)Parent).Children.Remove(this);
            }));
        }
    }
}