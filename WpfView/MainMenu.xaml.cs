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
        public FreePlayPiano FreePlay { get; set; }
        public SettingsPage SettingsPage { get; set; }
        public MainMenu()
        {
            InitializeComponent();
            
            SettingsPage = new SettingsPage(this);
            FreePlay = new FreePlayPiano(this);

        }

        private void FreePlay_Button_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("FreePlayPiano.xaml", UriKind.Relative));
            //Also possible (no string usage) but might make a new one every time
           
            
            FreePlay.CheckInputDevice(SettingsPage.IndexInputDevice);  // Checks if input device has been selected!
            this.NavigationService.Navigate(FreePlay);
            
        }

        private void Admin_Button_Click(object sender, RoutedEventArgs e)
        {
             
            SettingsPage.GenerateInputDevices(); // Gets all input devices
            NavigationService?.Navigate(SettingsPage);
        }
    }
}
