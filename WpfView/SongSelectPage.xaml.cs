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

        public SongSelectPage(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            PracticePiano = new PracticePlayPiano(_mainMenu, this);
            _mainMenu.IsVisibleChanged += _mainMenu_IsVisibleChanged;
            InitializeComponent();
            AddSongs();
            KaraokeCheckBox.Checked += KaraokeCheckBox_Checked;
            KaraokeCheckBox.Unchecked += KaraokeCheckBox_Unchecked;
        }

        private void KaraokeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SongController.DoKaroake = false;
        }

        private void KaraokeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SongController.DoKaroake = true;
        }

        private void _mainMenu_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AddSongs();
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
                        Source = new ImageSourceConverter().ConvertFromString("../../../../WpfView/Images/PianoHeroLogo.png") as ImageSource
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

        public async void CreateShowLeaderboard()
        {
            if (SelectedCard is not null)
            {
                Leaderboard.Children.Clear();
                Highscore[] highscores = await DatabaseController.GetHighscores(SelectedCard.SongID);
                Leaderboard.Children.Add(new SelectedSongControl(SelectedCard, highscores, SelectedCard.Description));
            }
        }

        /// <summary>
        /// Temporary - Adds 10 random songs
        /// </summary>
        private async void AddSongs()
        {
            SongCards.Children.Clear();
            Song[] songs = await DatabaseController.GetAllSongs();

            foreach (var item in songs)
            {
                SongCardControl songCardControl = new(item.Id, item.Name, item.Description, (int)item.Difficulty, this);
                SongCards.Children.Add(songCardControl);
            }
        }

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
                MessageBox.Show("Select a song from the list first before starting",
                "You can't play nothing", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}