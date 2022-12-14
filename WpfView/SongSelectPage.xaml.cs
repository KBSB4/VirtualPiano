using Controller;
using Model;
using Model.DatabaseModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SongSelectPage.xaml
    /// </summary>
    public partial class SongSelectPage : Page
    {
        private readonly MainMenu _mainMenu;
        public PracticePlayPiano PracticePiano { get; set; }

        private SongCardControl? SelectedCard { get; set; }

        /// <summary>
        /// Constructor for SongSelectPage
        /// </summary>
        /// <param name="mainMenu"></param>
        public SongSelectPage(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            PracticePiano = new PracticePlayPiano(_mainMenu, this);
            IsVisibleChanged += MainMenu_IsVisibleChanged;
            InitializeComponent();
            AddSongs();
            KaraokeCheckBox.Checked += KaraokeCheckBox_Checked;
            KaraokeCheckBox.Unchecked += KaraokeCheckBox_Unchecked;
        }

        /// <summary>
        /// Translate labels
        /// </summary>
        private void UpdateUI()
        {
            BackMenu.Header = LanguageController.GetTranslation(TranslationKey.Menubar_BackToMain);
            TitleLabel.Content = LanguageController.GetTranslation(TranslationKey.Menubar_SongSelect_SelectSong);
            KaraokeLabel.Content = LanguageController.GetTranslation(TranslationKey.Menubar_SongSelect_Karaoke);
            StartButton.Content = LanguageController.GetTranslation(TranslationKey.Menubar_SongSelect_Start);
        }

        /// <summary>
        /// Uncheck Karaoke => karaoke false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KaraokeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SongController.DoKaroake = false;
        }

        /// <summary>
        /// Check Karaoke => karaoke true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KaraokeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SongController.DoKaroake = true;
        }

        /// <summary>
        /// On visible changed, update UI, Songs and Leaderboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateUI();
            AddSongs();
            CreateShowLeaderboard();
        }

        /// <summary>
        /// Temporary - Starts practice play
        /// </summary>
        /// <param name="ID"></param>
        public void SongCard_Click(SongCardControl songCard)
        {
            //Deselect if there is one
            if (SelectedCard is not null)
            {
                SelectedCard.Background = null;
                //If the same songcard is clicked that is selected -> deselect and show logo
                if (SelectedCard.Equals(songCard))
                {
                    SelectedCard = null;
                    Image nothingSelectedImage = new()
                    {
                        Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Images/PianoHeroLogo.png") as ImageSource
                    };
                    Leaderboard.Children.Clear();
                    Leaderboard.Children.Add(nothingSelectedImage);
                    return;
                }
            }

            //Select the clicked card
            SelectedCard = songCard;
            SelectedCard.Background = new SolidColorBrush(Colors.OrangeRed);

            //Show leaderboard
            CreateShowLeaderboard();
        }

        /// <summary>
        /// Create and show leaderboard for the <see cref="SelectedCard"/>
        /// </summary>
        public async void CreateShowLeaderboard()
        {
            if (SelectedCard is not null)
            {
                Highscore[]? highscores = await DatabaseController.GetHighscores(SelectedCard.SongID);
                if (highscores is not null)
                {
                    Leaderboard.Children.Clear();
                    Leaderboard.Children.Add(new SelectedSongControl(SelectedCard, highscores, SelectedCard.Description, _mainMenu.LoggedInUser));
                }
            }
        }

        /// <summary>
        /// Temporary - Adds 10 random songs
        /// </summary>
        private async void AddSongs()
        {
            Song[]? songs = await DatabaseController.GetAllSongs();

            SongCards.Children.Clear();
            if (songs is null) return;
            foreach (var item in songs)
            {
                SongCardControl songCardControl = new(item.Id, item.Name, item.Description, (int)item.Difficulty, this);
                SongCards.Children.Add(songCardControl);
            }
        }

        /// <summary>
        /// Back to menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(_mainMenu);
        }

        /// <summary>
        /// Start the currently selected song, if no song selected show warning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_MouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SelectedCard is not null)
            {
                PracticePiano.PlaySelectedSong(SelectedCard.SongID);
                NavigationService?.Navigate(PracticePiano);
            }
            else
            {
                MessageBox.Show(LanguageController.GetTranslation(TranslationKey.MessageBox_SelectSongBeforeStartText),
                LanguageController.GetTranslation(TranslationKey.MessageBox_SelectSongBeforeStartCaption), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}