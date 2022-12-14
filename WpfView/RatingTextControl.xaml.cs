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
        public RatingTextControl(Rating rating)
        {
            RatingText = rating.ToString();
            Rotation = new Random().Next(-15, 15);
            InitializeComponent();
            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateRatingText));
            updateVisualNoteThread.Start();
        }

        private void UpdateRatingText(object? obj)
        {
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() =>
            {
                ((Grid)Parent).Children.Remove(this);
            }));
        }

        public string RatingText
        {
            get { return (string)GetValue(RatingTextProperty); }
            set { SetValue(RatingTextProperty, value); }
        }
        public static readonly DependencyProperty RatingTextProperty =
            DependencyProperty.Register("RatingText", typeof(string), typeof(RatingTextControl), new PropertyMetadata("no name"));

        public int Rotation
        {
            get { return (int)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(int), typeof(RatingTextControl), new PropertyMetadata(0));
    }
}
