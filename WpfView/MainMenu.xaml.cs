using Melanchall.DryWetMidi.Multimedia;
using Model.DatabaseModels;
using SharpDX.Multimedia;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

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
        public AccountPage AccountPage { get; set; }
        public AdminPanel AdminPanel { get; set; }

        public User? loggedInUser { get; set; }

        //DO NOT REMOVE
        public IInputDevice? InputDevice;

        public MainMenu()
        {
            InitializeComponent();
            SettingsPage = new SettingsPage(this);
            FreePlay = new FreePlayPiano(this);
            SongSelectPage = new SongSelectPage(this);
            AccountPage = new AccountPage(this);
            AdminPanel = new(this);
            Account_ChangeIconBasedOnUser();
        }

        /// <summary>
        /// Connects MIDI-keyboard
        /// </summary>
        public void CheckInputDevice(int x)
        {
            InputDevice?.Dispose();

            if (x > 0)
            {
                if (SettingsPage.NoneSelected.IsSelected)
                {
                    SelectItem(1);
                }
                else
                {
                    SelectItem(x);
                }
            }
        }

        /// <summary>
        /// tries to select the correct input device with parameter <paramref name="item"/> for playing with a Midi keyboard otherwise throws an exception
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="item"></param>
        private void SelectItem(int item)
        {
            try
            {
                InputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice.GetByIndex(item - 1);

                InputDevice.EventReceived += FreePlay.OnMidiEventReceived;
                if (SongSelectPage.PracticePiano is not null)
                {
                    InputDevice.EventReceived += SongSelectPage.PracticePiano.OnMidiEventReceived;
                }
                InputDevice.StartEventsListening();
                ComboBoxItem v = (ComboBoxItem)SettingsPage.input.Items.GetItemAt(item);
                v.IsSelected = true;
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine(ex.Message);
                InputDevice = null;
            }
        }

        /// <summary>
        /// Goes to <see cref="FreePlayPiano"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FreePlay_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckInputDevice(SettingsPage.IndexInputDevice);  // Checks if input device has been selected!
            this.NavigationService.Navigate(FreePlay);
        }

        /// <summary>
        /// Goes to <see cref="SettingsPage"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage.GenerateInputDevices(); // Gets all input devices
            NavigationService?.Navigate(SettingsPage);
        }

        /// <summary>
        /// Goes to <see cref="PracticePlayPiano"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Practice_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(AdminPanel);
        }

        /// <summary>
        /// Navigates to <see cref="AccountPage"/> if the user is not logged in,
        /// otherwise gives the user the option to logout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Account_Button_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInUser is null) NavigationService?.Navigate(AccountPage);
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes) 
                {
                    loggedInUser= null;
                    Account_ChangeIconBasedOnUser();
                }
            }
        }

        /// <summary>
        /// Changes the image of <see cref="AccountIconImage"/> based on if the user is logged in or not.
        /// </summary>
        public void Account_ChangeIconBasedOnUser()
        {
            if (loggedInUser is null)
            {
                AccountIconImage.Source = new BitmapImage(new Uri("/Images/accountImage.png", UriKind.Relative));
                AccountIconImage.Margin = new Thickness(20, 40, 20, 40);
                WhiteRectForIcon.Visibility = Visibility.Visible;
            }
            else
            {
                WhiteRectForIcon.Visibility = Visibility.Hidden;
                AccountIconImage.Source = new BitmapImage(new Uri("/Images/log-out.png", UriKind.Relative));
            }
        }
    }
}