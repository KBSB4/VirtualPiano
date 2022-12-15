using System.Windows;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for LeaderboardRecord.xaml
    /// </summary>
    public partial class LeaderboardRecord : UserControl
    {
        public LeaderboardRecord()
        {
            InitializeComponent();
            Position = "1";
            UserName = "testRecord";
            Score = "69420";
        }

        public LeaderboardRecord(int position, string userName, int score)
        {
            Position = position.ToString();
            UserName = userName;
            Score = score.ToString();
            InitializeComponent();
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
