using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for ScoreCardControl.xaml
    /// </summary>
    public partial class ScoreCardControl : UserControl
    {
        public ScoreCardControl(string username, int score, int position)
        {
            UsernameProp = username;
            ScoreProp = score;
            PositionProp = position;
        }

        public string UsernameProp
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(ScoreCardControl), new PropertyMetadata("no name"));

        public int ScoreProp
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(ScoreCardControl), new PropertyMetadata(default(int)));

        public int PositionProp
        {
            get { return (int)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(int), typeof(ScoreCardControl), new PropertyMetadata(default(int)));

        public ImageSource TrophyImageSourceProp
        {
            get { return (ImageSource)GetValue(TrophyImageSourceProperty); }
            set { SetValue(TrophyImageSourceProperty, value); }
        }
        public static readonly DependencyProperty TrophyImageSourceProperty =
            DependencyProperty.Register("TrophyImageSource", typeof(ImageSource), typeof(ScoreCardControl), new PropertyMetadata(default(ImageSource)));
    }
}