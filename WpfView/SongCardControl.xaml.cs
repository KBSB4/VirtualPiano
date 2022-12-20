using BusinessLogic;
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
        readonly SongSelectPage? SongSelectPage;
        public SongCardControl(int id) : this(id, "No name") { }
        public SongCardControl(int id, string songTitle) : this(id, songTitle, null, 0, null) { }
        public SongCardControl(int id, string songTitle, string? description, int difficulty, SongSelectPage? songSelectPage)
        {
            InitializeComponent();
            SongID = id;
            SongTitle = songTitle;
            //If no description, show nothing
            Description = description ?? "";
            Difficulty = difficulty;

            //Set difficulty icon
            DifficultyImageSource = Difficulty switch
            {
                0 => new BitmapImage(new Uri(ProjectSettings.GetPath(PianoHeroPath.ImagesFolder) + "DifficultyIconEZ.png", UriKind.Relative)),
                1 => new BitmapImage(new Uri(ProjectSettings.GetPath(PianoHeroPath.ImagesFolder) + "DifficultyIconMedium.png", UriKind.Relative)),
                2 => new BitmapImage(new Uri(ProjectSettings.GetPath(PianoHeroPath.ImagesFolder) + "DifficultyIconHard.png", UriKind.Relative)),
                3 => new BitmapImage(new Uri(ProjectSettings.GetPath(PianoHeroPath.ImagesFolder) + "DifficultyIconHero.png", UriKind.Relative)),
                _ => new BitmapImage(new Uri(ProjectSettings.GetPath(PianoHeroPath.ImagesFolder) + "DifficultyIconEZ.png", UriKind.Relative)),
            };

            //For event click
            SongSelectPage = songSelectPage;
        }

        public string SongTitle
        {
            get { return (string)GetValue(SongTitleProperty); }
            set { SetValue(SongTitleProperty, value); }
        }
        public static readonly DependencyProperty SongTitleProperty =
            DependencyProperty.Register("SongTitle", typeof(string), typeof(SongCardControl), new PropertyMetadata("no name"));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(SongCardControl), new PropertyMetadata(""));

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

        /// <summary>
        /// Select the card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button is not null && SongSelectPage is not null) { SongSelectPage.SongCard_Click(this); }
        }
    }
}