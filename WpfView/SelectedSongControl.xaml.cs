using Model.DatabaseModels;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SelectedSongControl.xaml
    /// </summary>
    public partial class SelectedSongControl : UserControl
    {
        public SongCardControl SongCard { get; set; }
        public SelectedSongControl(SongCardControl songCard, Highscore[] scores, string? description)
        {
            SongCard = songCard;
            InitializeComponent();

            //Name and difficiulty
            Title.Content = songCard.SongTitle;
            DifficultyImage.Source = songCard.DifficultyImageSource;

            if (scores.Length > 0)
            {
                foreach (Highscore score in scores)
                {
                    int position = Array.FindIndex(scores, item => item.Equals(score));
                    if (score.User.Name.Equals("Harris")) //TODO Replace with logged in user
                    {
                        if (position > 10)
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
            } else
            {
                Label desc = new()
                {
                    Content = description,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 30
                };
                leaderBoard.Children.Add(desc);
            }
        }
    }
}