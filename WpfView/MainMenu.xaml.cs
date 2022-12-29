using Controller;
using Melanchall.DryWetMidi.Multimedia;
using Model;
using Model.DatabaseModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

        public User? LoggedInUser { get; set; }

        //DO NOT REMOVE
        public IInputDevice? InputDevice;

        public MainMenu()
        {
            InitializeComponent();
            SettingsPage = new SettingsPage(this);
            FreePlay = new FreePlayPiano(this);
            SongSelectPage = new SongSelectPage(this);
            AccountPage = new AccountPage(this, null);
            AdminPanel = new(this);
            Account_ChangeIconBasedOnUser();
            IsVisibleChanged += MainMenu_IsVisibleChanged;
        }

        /// <summary>
        /// On page visibility change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void MainMenu_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// Translate labels
        /// </summary>
		private void UpdateUI()
        {
            SettingsLabel.Content = LanguageController.GetTranslation(TranslationKey.MainMenu_Settings);
            PlayLabel.Content = LanguageController.GetTranslation(TranslationKey.MainMenu_Play);
            FreePlayLabel.Content = LanguageController.GetTranslation(TranslationKey.MainMenu_FreePlay);
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
            SettingsPage.RefreshBoxes();
            NavigationService?.Navigate(SettingsPage);
        }

        /// <summary>
        /// Goes to <see cref="PracticePlayPiano"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Practice_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(SongSelectPage);
        }

        /// <summary>
        /// Navigates to <see cref="AccountPage"/> if the user is not logged in,
        /// otherwise gives the user the option to logout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Account_Button_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedInUser is null) NavigationService?.Navigate(AccountPage);
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    LoggedInUser = null;
                    Account_ChangeIconBasedOnUser();
                }
            }
        }

        /// <summary>
        /// Changes the image of <see cref="AccountIconImage"/> based on if the user is logged in or not.
        /// </summary>
        public void Account_ChangeIconBasedOnUser()
        {
            if (LoggedInUser is null)
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