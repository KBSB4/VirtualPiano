using System;
using System.Linq;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SelectedSongControl.xaml
    /// </summary>
    public partial class SelectedSongControl : UserControl
    {
        public SongCardControl SongCard { get; set; }
        Random random = new();
        public SelectedSongControl(SongCardControl songCard)
        {
            SongCard = songCard;
            InitializeComponent();

            //Name and difficiulty
            Title.Content = songCard.SongTitle;
            DifficultyImage.Source = songCard.DifficultyImageSource;

            int user = 2;

            //TODO database and get people
            for (int i = 0; i <= 9; i++)
            {
                //User check
                if (i == (user - 1)) //TODO replace with if user when database gets added
                {
                    leaderBoard.Children.Add(new LeaderboardRecord(i, "LoggedInUser", random.Next(0, 99999999), true));
                }
                else
                {
                    leaderBoard.Children.Add(new LeaderboardRecord(i, RandomString(random.Next(4, 24)), random.Next(0, 99999999)));
                }
            }
            if ((user - 1) > 9)
            {
                leaderBoard.Children.RemoveAt(leaderBoard.Children.Count - 1);// Remove last one in list if user is outside top 10
                leaderBoard.Children.Add(new LeaderboardRecord((user - 1), "LoggedInUser", random.Next(0, 99999999), true));
            }
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}