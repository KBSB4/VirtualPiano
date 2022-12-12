using System.Windows;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SongSelectPage.xaml
    /// </summary>
    public partial class SongSelectPage : Page
    {
        MainMenu _mainMenu;
        public PracticePlayPiano practicePiano;

        public SongSelectPage(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            practicePiano = new PracticePlayPiano(_mainMenu);
            InitializeComponent();

            AddSongs();
        }
        public void SongCard_Click(int ID)
        {
            practicePiano.PlaySelectedSong(ID);
            NavigationService?.Navigate(practicePiano);
        }

        private void AddSongs()
        {
            for (int i = 0; i < 10; i++)
            {

                SongCardControl songCardControl = new(i, "Song " + (i + 1).ToString(), i % 4, this);

                SongCards.Children.Add(songCardControl);
            }
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(_mainMenu);
        }
    }
}
