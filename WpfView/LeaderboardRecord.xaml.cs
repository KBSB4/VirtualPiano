using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for LeaderboardRecord.xaml
    /// </summary>
    public partial class LeaderboardRecord : UserControl
    {
        public LeaderboardRecord(int position, string? userName, int? score) : this(position, userName, score, false) { }

        public LeaderboardRecord(int position, string? userName, int? score, bool CurrentUser)
        {
            Position = (position + 1).ToString();
            UserName = userName;
            Score = score.ToString();

            InitializeComponent();

            //Change colour of top 3 and add trophy image
            SolidColorBrush firstPosition = new(Colors.Gold);
            SolidColorBrush secondPosition = new(Colors.Silver);
            SolidColorBrush thirdPosition = new(Colors.SaddleBrown);
            SolidColorBrush otherPosition = new(Colors.White);

            TrophyImage.Height = 40;
            TrophyImage.Width = 40;

            switch (position)
            {
                case 0:
                    PositionLabel.Foreground = firstPosition;
                    UserLabel.Foreground = firstPosition;
                    ScoreLabel.Foreground = firstPosition;
                    TrophyImage.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Images/TrophyGold.png") as ImageSource;
                    break;
                case 1:
                    PositionLabel.Foreground = secondPosition;
                    UserLabel.Foreground = secondPosition;
                    ScoreLabel.Foreground = secondPosition;
                    TrophyImage.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Images/ThrophySilver.png") as ImageSource;
                    break;
                case 2:
                    PositionLabel.Foreground = thirdPosition;
                    UserLabel.Foreground = thirdPosition;
                    ScoreLabel.Foreground = thirdPosition;
                    TrophyImage.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Images/ThropyBronze.png") as ImageSource;
                    break;
                default:
                    PositionLabel.Foreground = otherPosition;
                    UserLabel.Foreground = otherPosition;
                    ScoreLabel.Foreground = otherPosition;
                    TrophyImage.Source = new ImageSourceConverter().ConvertFromString("pack://application:,,,/Images/NoTrophy.png") as ImageSource;
                    break;
            }

            //If logged in, show
            if (CurrentUser)
            {
                Background = new SolidColorBrush(Colors.OrangeRed);
            }
        }
        public string Position
        {
            get { return (string)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(string), typeof(LeaderboardRecord), new PropertyMetadata("0"));

        public string? UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(LeaderboardRecord), new PropertyMetadata("no name"));

        public string? Score
        {
            get { return (string)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(string), typeof(LeaderboardRecord), new PropertyMetadata("0"));
    }
}