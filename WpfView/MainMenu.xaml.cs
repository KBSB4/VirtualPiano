using Melanchall.DryWetMidi.Multimedia;
using Model.DatabaseModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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

        public bool loggedIn = false;

        //public User loggedInUser = ;

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

        private void Account_Button_Click(object sender, RoutedEventArgs e)
        {
            if (loggedIn) NavigationService?.Navigate(AdminPanel);
            else NavigationService?.Navigate(AccountPage);
        }
    }
}