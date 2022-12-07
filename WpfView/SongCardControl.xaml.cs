using System;
using System.Diagnostics;
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
        SongSelectPage SongSelectPage;

        public SongCardControl(int id) : this(id, "No name") { }
        public SongCardControl(int id, string songTitle) : this(id, songTitle, 0) { }
        public SongCardControl(int id, string songTitle, int difficulty) : this(id, songTitle, difficulty, null) { }
        public SongCardControl(int id, string songTitle, int difficulty, SongSelectPage songSelectPage)
        {
            InitializeComponent();
            SongTitle = songTitle;
            SongSelectPage = songSelectPage;
            Difficulty = difficulty;
            DifficultyImageSource = Difficulty switch
            {
                0 => new BitmapImage(new Uri("/Images/DifficultyIconEZ.png", UriKind.Relative)),
                1 => new BitmapImage(new Uri("/Images/DifficultyIconMedium.png", UriKind.Relative)),
                2 => new BitmapImage(new Uri("/Images/DifficultyIconHard.png", UriKind.Relative)),
                3 => new BitmapImage(new Uri("/Images/DifficultyIconHero.png", UriKind.Relative)),
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

        public int SongID
        {
            get { return (int)GetValue(SongIDProperty); }
            set { SetValue(SongIDProperty, value); }
        }
        public static readonly DependencyProperty SongIDProperty =
            DependencyProperty.Register("SongID", typeof(int), typeof(SongCardControl), new PropertyMetadata(default(int)));

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button is not null)
            {
                SongSelectPage.SongCard_Click(SongID);
            }
        }
    }
}
