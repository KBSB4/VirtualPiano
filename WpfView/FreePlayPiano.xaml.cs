using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Microsoft.Win32;
using Model;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        readonly PracticeNotesGenerator practiceNotes;
        private MainMenu _mainMenu;
        private SettingsPage _settingsPage;

        public FreePlayPiano(MainMenu _mainMenu, SettingsPage settingsPage)
        {
            this._mainMenu = _mainMenu;
            _settingsPage = settingsPage;
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(PracticeColumnWhiteKeys, PracticeColumnBlackKeys, 28);
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;


            //Keep this here until we have a better way of connecting phyiscal devices and so we can test
            //_inputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice.GetByName("Launchkey 49");
            //_inputDevice.EventReceived += OnMidiEventReceived;
            //_inputDevice.StartEventsListening();

            //Start thread for updating practice notes
            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateVisualNotes))
            {
                IsBackground = true
            };
            updateVisualNoteThread.Start();
        }


        /// <summary>
        /// Method that constantly updates the practice notes
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateVisualNotes(object? obj)
        {
            while (true)
            {
                Thread.Sleep(25); // 30 / 120 * bpm
                try
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        practiceNotes.UpdateExampleNotes();
                    }));
                }
                catch (TaskCanceledException) //Just in case
                {
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// If note is to be played, create a new note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentSong_NotePlayed(object? sender, PianoKeyEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (e.Key.PressedDown)
                    {
                        practiceNotes.StartExampleNote(e.Key);
                    }
                    //TODO Add option to display keys live as if the piano is playing it
                    //pianoGrid.DisplayPianoKey(e.Key);
                }));
            }
            catch (TaskCanceledException) //Just in case
            {
                Environment.Exit(0);
            }
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
            if (sender is MenuItem)
            {
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

        #region MIDI
        /// <summary>
        /// Checks if everything is okay and not playing before attempting to load new file in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMIDIFileDialog(object sender, RoutedEventArgs e)
        {
            if (SongController.CurrentSong is null)
            {
                StartDialog();
            }
            else if (SongController.CurrentSong.IsPlaying)
            {
                MessageBox.Show("There is a MIDI still playing! Stop the playback of the current playing MIDI to continue",
                "MIDI is still playing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                StartDialog();
            }
        }

        /// <summary>
        /// Open dialog and prepares MIDI
        /// </summary>
        private static void StartDialog()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "MIDI Files (*.mid)|*.mid",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if ((bool)openFileDialog.ShowDialog())
            {
                //Get the path of specified file
                MIDIController.OpenMidi(openFileDialog.FileName);
                SongController.LoadSong(new MetricTimeSpan(500));
            }
        }

        /// <summary>
        /// Play MIDI File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayMIDIFile(object sender, RoutedEventArgs e)
        {
            //Boolean isisolated = IsolatedPiano.IsChecked; Planned for later
            if (MIDIController.OriginalMIDI is not null && SongController.CurrentSong is not null && !SongController.CurrentSong.IsPlaying)
            {
                SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;
                SongController.PlaySong();
            }
            else
            {
                if (MIDIController.OriginalMIDI is null)
                {
                    MessageBox.Show("Select a MIDI File first before playing",
                    "No MIDI selected", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (SongController.CurrentSong is not null && SongController.CurrentSong.IsPlaying)
                {
                    MessageBox.Show("There is a MIDI still playing! Stop the playback of the current playing MIDI to continue",
                    "MIDI is still playing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        /// <summary>
        /// Stop playing the MIDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopMIDIFile(object sender, RoutedEventArgs e)
        {
            if (SongController.CurrentSong is not null && SongController.CurrentSong.IsPlaying)
            {
                SongController.StopSong();
            }
            else
            {
                MessageBox.Show("There is no MIDI playing right now.",
                "No MIDI playing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
