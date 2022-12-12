using Melanchall.DryWetMidi.Multimedia;
using System;
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
        public SongSelectPage SongSelectPage { get; set; }

        public IInputDevice? InputDevice;

        public MainMenu()
        {
            InitializeComponent();
            SettingsPage = new SettingsPage(this);
            FreePlay = new FreePlayPiano(this);
            SongSelectPage = new SongSelectPage(this);
            //FreePlay.CheckInputDevice(SettingsPage.IndexInputDevice);
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
                Debug.Write("send!");
                InputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice.GetByIndex(item - 1);

                InputDevice.EventReceived += FreePlay.OnMidiEventReceived;
                if (SongSelectPage.practicePiano is not null)
                {
                    InputDevice.EventReceived += SongSelectPage.practicePiano.OnMidiEventReceived;
                }
                InputDevice.StartEventsListening();
                ComboBoxItem v = (ComboBoxItem)SettingsPage.input.Items.GetItemAt(item);
                v.IsSelected = true;
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine("No midi device found");
                Debug.WriteLine("Exception information:");
                Debug.IndentLevel = 1;
                Debug.WriteLine(ex.Message);
                Debug.IndentLevel = 0;
                InputDevice = null;
            }
        }

        private void FreePlay_Button_Click(object sender, RoutedEventArgs e)
        {

            CheckInputDevice(SettingsPage.IndexInputDevice);  // Checks if input device has been selected!
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
