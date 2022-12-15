using System;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SelectedSongControl.xaml
    /// </summary>
    public partial class SelectedSongControl : UserControl
    {
        public SongCardControl SongCard { get; set; }
        public SelectedSongControl(SongCardControl songCard)
        {
            SongCard = songCard;
            InitializeComponent();
            //Name and difficiulty
            Title.Content = songCard.SongTitle;
            DifficultyImage.Source = songCard.DifficultyImageSource;

            //TODO database and get people
            Random random = new();
            for (int i = 0; i < 10; i++)
            {
                leaderBoard.Children.Add(new LeaderboardRecord(i, "LongnameUser" + i.ToString(), random.Next(0, 99999999)));
            }
            //TODO change top 3

            //TODO get logged in user
        }
    }
}
