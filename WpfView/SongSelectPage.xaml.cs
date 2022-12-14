using System.Windows;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SongSelectPage.xaml
    /// </summary>
    public partial class SongSelectPage : Page
    {
        private readonly MainMenu _mainMenu;
        public PracticePlayPiano PracticePiano { get; set; }

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
        public void SongCard_Click(int ID)
        {
            PracticePiano.PlaySelectedSong(ID);
            NavigationService?.Navigate(PracticePiano);
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
    }
}