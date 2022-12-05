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
        PianoGridGenerator pianoGrid;
        PracticeNotesGenerator practiceNotes;

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

            //_inputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice.GetByName("Launchkey 49");
            //_inputDevice.EventReceived += OnMidiEventReceived;
            //_inputDevice.StartEventsListening();

            new Thread(new ParameterizedThreadStart(UpdateVisualNotes)).Start();
        }

        private void UpdateVisualNotes(object? obj)
        {
            while (true)
            {
                Thread.Sleep(25);
                try
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        practiceNotes.UpdateExampleNotes();
                    }));
                } catch (TaskCanceledException ex)
                {
                    //ignore
                }
            }
        }

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
                    //pianoGrid.DisplayPianoKey(e.Key);
                }));
            } catch (TaskCanceledException ex)
            {
                //TODO Does not work check: https://developercommunity.visualstudio.com/t/taskcanceledexception-during-application-shutdown/284294
                //On closure stop all events?
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
        /// Opens the dialog to select a MIDI file and open it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMIDIFileDialog(object sender, RoutedEventArgs e)
        {
            if (t is null)
            {
                var openFileDialog = new OpenFileDialog();

                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "MIDI Files (*.mid)|*.mid";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if ((bool)openFileDialog.ShowDialog())
                {
                    //Get the path of specified file
                    MIDIController.OpenMidi(openFileDialog.FileName);
                    SongController.LoadSong(new MetricTimeSpan(500));
                }
            }
            else
            {
                MessageBox.Show("There is a MIDI still playing! Stop the playback of the current playing MIDI to continue",
                    "MIDI is still playing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //To play MIDIs without hogging the main thread

        /// <summary>
        /// Play MIDI File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        Thread t;
        private void PlayMIDIFile(object sender, RoutedEventArgs e)
        {
            Boolean isisolated = IsolatedPiano.IsChecked;
            if (MIDIController.OriginalMIDI is not null && t is null)
            {
                ////Play MIDI when program starts
                //t = new Thread(() =>
                //{
                //    MIDIController.PlayMidi(isisolated);
                //    t = null;
                //});
                ////Makes the thread close when application close
                //t.IsBackground = true;
                //t.Start();

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

                if (t is not null)
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
            if (t is not null)
            {
                t.Interrupt();
                t = null;
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
