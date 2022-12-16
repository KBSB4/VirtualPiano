using System.Windows;
namespace WpfView
{
    /// <summary>
    /// Interaction logic for UploadScoreDialog.xaml
    /// </summary>
    public partial class UploadScoreDialog : Window
    {
        public int Option { get; set; }
        public UploadScoreDialog(int score, int maxscore)
        {
            InitializeComponent();
            ScoreLabel.Content = score;
            MaxScoreLabel.Content = maxscore;
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}