using Model.DatabaseModels;
using System;
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

        /// <summary>
        /// Create leaderboard with all information
        /// </summary>
        /// <param name="songCard"></param>
        /// <param name="scores"></param>
        /// <param name="description"></param>
        public SelectedSongControl(SongCardControl songCard, Highscore[] scores, string? description)
        {
            SongCard = songCard;
            InitializeComponent();

            //Name and difficiulty
            Title.Content = songCard.SongTitle;
            DifficultyImage.Source = songCard.DifficultyImageSource;

            //Place scores on leaderboard
            if (scores.Length > 0)
            {
                foreach (Highscore score in scores)
                {
                    int position = Array.FindIndex(scores, item => item.Equals(score)); //Find position in list
                    if (score.User.Name.Equals("Harris")) //TODO Replace with logged in user
                    {
                        if (position > 10)
                        {
                            leaderBoard.Children.RemoveAt(9); //Remove last one in list if user is outside top 10
                        }
                        leaderBoard.Children.Add(new LeaderboardRecord(position, score.User.Name, score.Score, true));
                    }
                    else
                    {
                        if (position < 10) //Prevents overwriting on leaderboard
                        {
                            leaderBoard.Children.Add(new LeaderboardRecord(position, score.User.Name, score.Score));
                        }
                    }
                }
            }
            else
            {
                //Show description if there are no scores
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