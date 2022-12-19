using Model.DatabaseModels;
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
        public SelectedSongControl(SongCardControl songCard, Highscore[] scores)
        {
            SongCard = songCard;
            InitializeComponent();

            //Name and difficiulty
            Title.Content = songCard.SongTitle;
            DifficultyImage.Source = songCard.DifficultyImageSource;

            foreach (Highscore score in scores)
            {
                int position = Array.FindIndex(scores, item => item.Equals(score)) + 1;
                if (score.User.Equals("Logged In User Here")) //TODO Replace with logged in user
                {
                    if (position < 10)
                    {
                        leaderBoard.Children.RemoveAt(leaderBoard.Children.Count - 1);// Remove last one in list if user is outside top 10
                    }
                    leaderBoard.Children.Add(new LeaderboardRecord(position, score.User.Name, score.Score, true));
                }
                else
                {
                    leaderBoard.Children.Add(new LeaderboardRecord(position, score.User.Name, score.Score));
                }
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