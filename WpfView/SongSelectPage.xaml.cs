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
            InitializeComponent();
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
                //TODO Connect to database and send current user through to the control
                Highscore[] highscores = await DatabaseController.GetHighscores(SelectedCard.SongID);
                Leaderboard.Children.Add(new SelectedSongControl(SelectedCard, highscores));
            }
        }

        /// <summary>
        /// Temporary - Adds 10 random songs
        /// </summary>
        private async void AddSongs()
        {
            Song[] songs = await DatabaseController.GetAllSongs();

            foreach (var item in songs)
            {
                SongCardControl songCardControl = new(item.Id, item.Name, (int)item.Difficulty, this);
                SongCards.Children.Add(songCardControl);
            }

            //for (int i = 0; i < 10; i++)
            //{
            //    SongCardControl songCardControl = new(i, "Song " + (i + 1).ToString(), i % 4, this);
            //    SongCards.Children.Add(songCardControl);
            //}
        }

        /// <summary>
        /// Return to main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(_mainMenu);
        }

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