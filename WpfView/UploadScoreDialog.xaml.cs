using Controller;
using Model;
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
			IsVisibleChanged += UI_IsVisibleChanged;
		}

		private void UI_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			UpdateUI();
		}

		private void UpdateUI()
		{
            TitleLabel.Content = LanguageController.GetTranslation(TranslationKey.Play_SongFinishedScreen_Title);
            YourScoreLabel.Content = LanguageController.GetTranslation(TranslationKey.Play_SongFinishedScreen_YourScore);
            YourMaxScoreLabel.Content = LanguageController.GetTranslation(TranslationKey.Play_SongFinishedScreen_MaxScore);
            UploadButton.Content = LanguageController.GetTranslation(TranslationKey.Play_SongFinishedScreen_UploadButton);
            MenuButton.Content = LanguageController.GetTranslation(TranslationKey.Play_SongFinishedScreen_MenuButton);
        }

		/// <summary>
		/// Return true for upload
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Upload_Click(object sender, RoutedEventArgs e)
        {
            //TODO check if user is logged in
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