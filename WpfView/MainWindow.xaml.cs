using System.Windows;
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
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            _NavigationFrame.Navigate(new MainMenu());
        readonly PianoGridGenerator pianoGrid;
        readonly PracticeNotesGenerator practiceNotes;

        //Note Keep until plug and play + settings are implemented
        private static IInputDevice _inputDevice;

        public MainWindow()
        {
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(PracticeColumnWhiteKeys, PracticeColumnBlackKeys, 28);

            //Add keydown event for the keys
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
                Thread.Sleep(33);
                try
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        practiceNotes.UpdateExampleNotes();
                    }));
                }
                catch (TaskCanceledException ex) //Just in case
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
            catch (TaskCanceledException ex) //Just in case
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
            Debug.WriteLine($"{e.Event}");
            PianoKey? key = PianoController.ParseMidiNote(e.Event);

            if (key is not null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    pianoGrid.DisplayPianoKey(key);
                }));
                if (key.PressedDown)
                {
                    PianoController.PlayPianoSound(key);
                }
                else
                {
                    PianoController.StopPianoSound(key);
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
            {
                PianoLogic.SwapOctave(PianoController.Piano);
            }
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
            else if(SongController.CurrentSong.IsPlaying)
            {
                MessageBox.Show("There is a MIDI still playing! Stop the playback of the current playing MIDI to continue",
                "MIDI is still playing", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
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
