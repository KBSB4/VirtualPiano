using System.Windows;
namespace WpfView
{
    /// <summary>
    /// Interaction logic for UploadScoreDialog.xaml
    /// </summary>
    public partial class UploadScoreDialog : Window
    {
        public int Option { get; set; }
        public UploadScoreDialog()
        {
            InitializeComponent();
            //TODO Show score, maybe some more info?
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            //TODO check if user is logged in
            this.DialogResult = true;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}