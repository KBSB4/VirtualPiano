using System.Diagnostics;
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
        private SettingsPage _adminPanel;

        public MainMenu()
        {
            InitializeComponent();
            
            _adminPanel = new SettingsPage(this);
            _freeplay = new FreePlayPiano(this,_adminPanel);
        }

        private void FreePlay_Button_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("FreePlayPiano.xaml", UriKind.Relative));
            //Also possible (no string usage) but might make a new one every time
           
            _freeplay.CheckInputDevice(_adminPanel.IndexInputDevice);  // Checks if input device has been selected!
            this.NavigationService.Navigate(_freeplay);
            
        }

        private void Admin_Button_Click(object sender, RoutedEventArgs e)
        {
            _adminPanel.GenerateOutputDevices(); // Gets all the output devices
            _adminPanel.GenerateInputDevices(); // Gets all input devices
            NavigationService?.Navigate(_adminPanel);
        }
    }
}
