using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Multimedia;
using Model;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Usb.Events;
using InputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for FreePlayPiano.xaml
    /// </summary>
    public partial class FreePlayPiano : Page
    {
        private PianoGridGenerator pianoGrid;
        private static IInputDevice? _inputDevice;
        private MainMenu _mainMenu;
        private SettingsPage _settingsPage;

        public FreePlayPiano(MainMenu _mainMenu, SettingsPage settingsPage)
        {
            this._mainMenu = _mainMenu;
            _settingsPage = settingsPage;
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;

        }

       

        /// <summary>
        /// Event fired on MIDI-input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMidiEventReceived(object? sender, MidiEventReceivedEventArgs e)
        {
            PianoKey? key = PianoController.ParseMidiNote(e.Event);

            if (key is not null)
            {
                if (key.PressedDown)
                {
                    PianoController.PlayPianoSound(key);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        pianoGrid.DisplayPianoKey(key);
                    }));
                }
                else
                {
                    PianoController.StopPianoSound(key);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        pianoGrid.DisplayPianoKey(key);
                    }));
                }
            }
        }

        /// <summary>
        /// Eventhandler for when the key gets pressed. Updates key and plays the audio
        /// Updates the view
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void KeyPressed(object? source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;

            PianoKey? key = PianoController.GetPressedPianoKey(intValue);
            if (key is not null)
            {
                pianoGrid.DisplayPianoKey(key);
                PianoController.PlayPianoSound(key);
            }

            if (e.Key == Key.CapsLock)
                PianoLogic.SwapOctave(PianoController.Piano);
        }

        /// <summary>
        /// If pressed down keyboard key gets released, stop the audio playing for the pianokey and unpress it.
        /// Updates the view
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void KeyReleased(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;
            PianoKey? key = PianoController.GetReleasedKey(intValue);
            if (key is not null)
            {
                PianoController.StopPianoSound(key);
                pianoGrid.DisplayPianoKey(key);
            }

        }

        private void MainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.Navigate(_mainMenu);

        }

        private void Refresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
            MainItem.Items.Clear();
            AddInputDevices();
           
            //CheckInputDevice();
        }

       


        /// <summary>
        /// Adds all the connected MIDI-keyboards to MenuItem "Connect MIDI-keyboard" 
        /// </summary>
        private void AddInputDevices()
        {
           
            foreach (var input in InputDevice.GetAll())
            {
                var x = new MenuItem { Header = input.Name };
                MainItem.Items.Add(x);
               
                x.Click += X_Click;
            }

            
        }

        private void X_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem) {
               int x = MainItem.Items.IndexOf(sender as MenuItem);
                _settingsPage.IndexInputDevice = x;
               CheckInputDevice(x);
            }
        }



        /// <summary>
        /// Connects MIDI-keyboard
        /// </summary>
        public void CheckInputDevice(int x)
        {


            try
            {
                if (_settingsPage.IndexInputDevice >= 0)
                {
                    Debug.Write("send!");
                    _inputDevice?.Dispose();
                    _inputDevice = InputDevice.GetByIndex(x);
                    _inputDevice.EventReceived += OnMidiEventReceived;
                    _inputDevice.StartEventsListening();
                }
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine("No midi device found");
                Debug.WriteLine("Exception information:");
                Debug.IndentLevel = 1;
                Debug.WriteLine(ex.Message);
                Debug.IndentLevel = 0;
                _inputDevice = null;
            }
        }
    }
}
