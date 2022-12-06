using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SongSelectPage.xaml
    /// </summary>
    public partial class SongSelectPage : Page
    {
        public SongSelectPage()
        {
            InitializeComponent();

            AddSongs();
        }

        private void AddSongs()
        {
            for (int i = 0; i < 10; i++)
            {
                SongCardControl songCardControl = new("Song " + i.ToString(), i % 4);

                SongCards.Children.Add(songCardControl);
            }
        }
    }
}
