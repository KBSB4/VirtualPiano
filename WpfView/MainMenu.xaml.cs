using System.Windows;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private FreePlayPiano _freeplay;
        private AdminPanel _adminPanel;
        public MainMenu()
        {
            InitializeComponent();
            _freeplay = new FreePlayPiano(this);
            _adminPanel = new AdminPanel(this);

        }

        private void FreePlay_Button_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("FreePlayPiano.xaml", UriKind.Relative));
            //Also possible (no string usage) but might make a new one every time
            this.NavigationService.Navigate(_freeplay);
        }

        private void Admin_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(_adminPanel);
        }
    }
}
