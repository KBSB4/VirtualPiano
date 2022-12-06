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
        private SongSelectPage _songSelect;
        private SettingsPage _adminPanel;

        public MainMenu()
        {
            InitializeComponent();

            _adminPanel = new SettingsPage(this);
            _freeplay = new FreePlayPiano(this, _adminPanel);
            _songSelect = new SongSelectPage(this);
        }

        private void FreePlay_Button_Click(object sender, RoutedEventArgs e)
        {
            _freeplay.CheckInputDevice(_adminPanel.IndexInputDevice);  // Checks if input device has been selected!
            this.NavigationService.Navigate(_freeplay);

        }

        private void Admin_Button_Click(object sender, RoutedEventArgs e)
        {
            _adminPanel.GenerateOutputDevices(); // Gets all the output devices
            _adminPanel.GenerateInputDevices(); // Gets all input devices
            NavigationService?.Navigate(_adminPanel);
        }

        private void Practice_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(_songSelect);
        }
    }
}
