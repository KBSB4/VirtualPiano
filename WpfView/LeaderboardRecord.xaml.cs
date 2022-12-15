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
        public LeaderboardRecord(int position, string userName, int score) : this(position, userName, score, false) { }

        public LeaderboardRecord(int position, string userName, int score, bool CurrentUser)
        {
            Position = (position + 1).ToString();
            UserName = userName;
            Score = score.ToString();
            InitializeComponent();

            //Change colour of top 3 and add trophy image
            //TODO Change images to trophies
            SolidColorBrush firstPosition = new SolidColorBrush(Colors.Gold);
            SolidColorBrush secondPosition = new SolidColorBrush(Colors.Silver);
            SolidColorBrush thirdPosition = new SolidColorBrush(Colors.Brown);
            SolidColorBrush otherPosition = new SolidColorBrush(Colors.White);

            TrophyImage.Height = 40;
            TrophyImage.Width = 40;
            switch (position)
            {
                case 0:
                    PositionLabel.Foreground = firstPosition;
                    UserLabel.Foreground = firstPosition;
                    ScoreLabel.Foreground = firstPosition;
                    TrophyImage.Source = new ImageSourceConverter().ConvertFromString("../../../../WpfView/Images/DifficultyIconHero.png") as ImageSource;
                    break;
                case 1:
                    PositionLabel.Foreground = secondPosition;
                    UserLabel.Foreground = secondPosition;
                    ScoreLabel.Foreground = secondPosition;
                    TrophyImage.Source = new ImageSourceConverter().ConvertFromString("../../../../WpfView/Images/DifficultyIconMedium.png") as ImageSource;
                    break;
                case 2:
                    PositionLabel.Foreground = thirdPosition;
                    UserLabel.Foreground = thirdPosition;
                    ScoreLabel.Foreground = thirdPosition;
                    TrophyImage.Source = new ImageSourceConverter().ConvertFromString("../../../../WpfView/Images/DifficultyIconEZ.png") as ImageSource;
                    break;
                default:

                    PositionLabel.Foreground = otherPosition;
                    UserLabel.Foreground = otherPosition;
                    ScoreLabel.Foreground = otherPosition;
                    TrophyImage.Source = new ImageSourceConverter().ConvertFromString("../../../../WpfView/Images/NoTrophy.png") as ImageSource;
                    if (CurrentUser)
                    {
                        Background = new SolidColorBrush(Colors.OrangeRed);
                    }
                    break;
            }
        }

        public string Position
        {
            get { return (string)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(string), typeof(LeaderboardRecord), new PropertyMetadata("0"));

        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(LeaderboardRecord), new PropertyMetadata("no name"));

        public string Score
        {
            get { return (string)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(string), typeof(LeaderboardRecord), new PropertyMetadata("0"));
    }
}
