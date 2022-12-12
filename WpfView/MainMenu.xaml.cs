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
        public SongSelectPage SongSelectPage { get; set; }
        public MainMenu()
        {
            InitializeComponent();
            SettingsPage = new SettingsPage(this);
            FreePlay = new FreePlayPiano(this);
            SongSelectPage = new SongSelectPage(this);
            FreePlay.CheckInputDevice(SettingsPage.IndexInputDevice);
        }

        private void FreePlay_Button_Click(object sender, RoutedEventArgs e)
        {
            FreePlay.CheckInputDevice(SettingsPage.IndexInputDevice);  // Checks if input device has been selected!
            this.NavigationService.Navigate(FreePlay);

        }

        private void Admin_Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage.GenerateInputDevices(); // Gets all input devices
            NavigationService?.Navigate(SettingsPage);
        }

        private void Practice_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(SongSelectPage);
        }
    }
}
