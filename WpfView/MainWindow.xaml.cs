using Controller;
using System.Windows;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Main window where all pages will be displayed
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            NavigationFrame.Navigate(new MainMenu());
            LanguageController.CreateJSON();
        }
    }
}