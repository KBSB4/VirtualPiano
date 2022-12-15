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
            for (int i = 0; i < 11; i++)
            {
                //User check
                if (i == 10) //TODO replace with if user when database gets added
                {
                    if (i > 9)
                    {
                        leaderBoard.Children.RemoveAt(leaderBoard.Children.Count - 1);// Remove last one in list if user is outside top 10
                    }
                    leaderBoard.Children.Add(new LeaderboardRecord(i, "LongnameUser" + i.ToString(), random.Next(0, 99999999), true));
                }
                else
                {
                    leaderBoard.Children.Add(new LeaderboardRecord(i, "LongnameUser" + i.ToString(), random.Next(0, 99999999)));
                }
            }
        }
    }
}