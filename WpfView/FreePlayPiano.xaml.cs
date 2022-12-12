using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Core;
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
using System.Windows.Media.Imaging;
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

        //Just for testing, can be removed, make sure to also remove line {var rating = (Rating)rnd.Next(Enum.GetNames(typeof(Rating)).Length);}
        Random rnd = new Random();

        public FreePlayPiano(MainMenu _mainMenu)
        {
            this._mainMenu = _mainMenu;
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(PracticeColumnWhiteKeys, PracticeColumnBlackKeys, 28);
            KeyDown += KeyPressed;
            KeyUp += KeyReleased;
            SongLogic.startCountDown += StartCountDown;

            //Start thread for updating practice notes
            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateVisualNotes))
            {
                IsBackground = true
            };
            updateVisualNoteThread.Start();
        }

        private void StartCountDown(object? sender, EventArgs e)
        {
            Thread countDownThread = new(new ParameterizedThreadStart(CountDown));
            countDownThread.Start();
        }

        private void CountDown(object? obj)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                CountDownImage.Visibility = Visibility.Visible;
                CountDownImage.Source = new BitmapImage(new Uri("/Images/CountdownReady.png", UriKind.Relative));
                Debug.WriteLine("Image updated");
            }));
            Thread.Sleep(2500);
            Dispatcher.Invoke(new Action(() =>
            {
                CountDownImage.Source = new BitmapImage(new Uri("/Images/CountdownSet.png", UriKind.Relative));
            }));
            Thread.Sleep(2500);
            Dispatcher.Invoke(new Action(() =>
            {
                CountDownImage.Source = new BitmapImage(new Uri("/Images/CountdownGo.png", UriKind.Relative));
            }));
            Thread.Sleep(2500);
            Dispatcher.Invoke(new Action(() =>
            {
                CountDownImage.Visibility = Visibility.Hidden;
            }));
        }


        /// <summary>
        /// Thread that updates the visual position of already placed notes
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateVisualNotes(object? obj)
        {
            var next = DateTime.Now;
            while (true)
            {
                Thread.Sleep(Math.Abs((DateTime.Now - next).Milliseconds));
                next = DateTime.Now.AddMilliseconds(25);// 25 milliseconds equals 40 frames per second
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
                var rating = (Rating)rnd.Next(Enum.GetNames(typeof(Rating)).Length);
                practiceNotes.DisplayNoteFeedBack(key, rating);
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
            _mainMenu.SettingsPage.GenerateInputDevices();
            NavigationService?.Navigate(_mainMenu.SettingsPage);
        }

        /// <summary>
        /// Connects MIDI-keyboard
        /// </summary>
        public void CheckInputDevice(int x)
        {
            _inputDevice?.Dispose();

            if (_mainMenu.SettingsPage.NoneSelected.IsSelected)
            {
                SelectItem(1);
            }
            else
            {
                SelectItem(x);
            }
        }

        private void SelectItem(int item)
        {
            try
            {
                Debug.Write("send!");
                _inputDevice = InputDevice.GetByIndex(item - 1);
                _inputDevice.EventReceived += OnMidiEventReceived;
                _inputDevice.StartEventsListening();
                ComboBoxItem v = (ComboBoxItem)_mainMenu.SettingsPage.input.Items.GetItemAt(item);
                v.IsSelected = true;
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
                MidiController.OpenMidi(openFileDialog.FileName);
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
            MidiFile currentMidiFile = MidiController.GetMidiFile();


            if (currentMidiFile is not null && SongController.CurrentSong is not null && !SongController.CurrentSong.IsPlaying)
            {
                SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;
                SongController.PlaySong();
            }
            else
            {
                if (currentMidiFile is null)
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
