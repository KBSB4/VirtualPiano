using System.Windows;
namespace WpfView
{
    /// <summary>
    /// Interaction logic for UploadScoreDialog.xaml
    /// </summary>
    public partial class UploadScoreDialog : Window
    {

        /// <summary>
        /// Show score on screen
        /// </summary>
        /// <param name="score"></param>
        /// <param name="maxscore"></param>
        public UploadScoreDialog(int score, int maxscore)
        {
            InitializeComponent();
            ScoreLabel.Content = score;
            MaxScoreLabel.Content = maxscore;
        }

        /// <summary>
        /// Return true for upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        /// <summary>
        /// Return false for main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}