using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SongCardControl.xaml
    /// </summary>
    public partial class SongCardControl : UserControl
    {
        public SongCardControl() : this("No name") { }

        public SongCardControl(string songTitle) : this(songTitle, 0) { }

        public SongCardControl(string songTitle, int difficulty)
        {
            InitializeComponent();
            SongTitle = songTitle;
            Difficulty = difficulty;
            DifficultyImageSource = Difficulty switch
            {
                0 => new BitmapImage(new Uri("/Images/DifficultyIconEZ.png", UriKind.Relative)),
                1 => new BitmapImage(new Uri("/Images/DifficultyIconMedium.png", UriKind.Relative)),
                _ => new BitmapImage(new Uri("/Images/DifficultyIconEZ.png", UriKind.Relative)),
            };
        }

        public string SongTitle
        {
            get { return (string)GetValue(SongTitleProperty); }
            set { SetValue(SongTitleProperty, value); }
        }
        public static readonly DependencyProperty SongTitleProperty =
            DependencyProperty.Register("SongTitle", typeof(string), typeof(SongCardControl), new PropertyMetadata("no name"));

        public int Difficulty
        {
            get { return (int)GetValue(DifficultyProperty); }
            set { SetValue(DifficultyProperty, value); }
        }
        public static readonly DependencyProperty DifficultyProperty =
            DependencyProperty.Register("Difficulty", typeof(int), typeof(SongCardControl), new PropertyMetadata(default(int)));

        public ImageSource DifficultyImageSource
        {
            get { return (ImageSource)GetValue(DifficultyImageSourceProperty); }
            set { SetValue(DifficultyImageSourceProperty, value); }
        }
        public static readonly DependencyProperty DifficultyImageSourceProperty =
            DependencyProperty.Register("DifficultyImageSource", typeof(ImageSource), typeof(SongCardControl), new PropertyMetadata(default(ImageSource)));

    }
}
