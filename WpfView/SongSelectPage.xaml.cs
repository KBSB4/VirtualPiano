using SharpDX.Multimedia;
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
            PracticePiano = new PracticePlayPiano(_mainMenu);
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
            SelectedCard.Background = new SolidColorBrush(Colors.Red);

            //Show leaderboard
            //TODO Connect to database
            Leaderboard.Children.Clear();
            Leaderboard.Children.Add(new SelectedSongControl());
        }

        /// <summary>
        /// Temporary - Adds 10 random songs
        /// </summary>
        private void AddSongs()
        {
            for (int i = 0; i < 10; i++)
            {
                SongCardControl songCardControl = new(i, "Song " + (i + 1).ToString(), i % 4, this);
                SongCards.Children.Add(songCardControl);
            }
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
            if(SelectedCard is not null)
            {
                PracticePiano.PlaySelectedSong(SelectedCard.SongID);
                NavigationService?.Navigate(PracticePiano);
            } else
            {
                MessageBox.Show("Select a song from the list first before starting",
                "You can't play nothing", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}