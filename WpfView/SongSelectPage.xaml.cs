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

        private SongCardControl SelectedCard { get; set; }

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
                SelectedCard.BorderThickness = new Thickness(0);
                //TODO Alternatively change background to prevent changing size
            }

            //Select the clicked card
            SelectedCard = songCard;
            SelectedCard.BorderThickness = new Thickness(1);
            SelectedCard.BorderBrush = new SolidColorBrush(Colors.Red);
            //TODO Alternatively change background to prevent changing size
            
            //Get leaderboard and display
            //TODO Database query here


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
            }
        }
    }
}