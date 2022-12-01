using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Microsoft.Win32;
using Model;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PianoGridGenerator pianoGrid;
        //Note: Needs to be this high to prevent exception in xaudio
        Timer drawtimer = new(500);

        private static IInputDevice _inputDevice;
        public static Thread t = null;

        public MainWindow()
        {
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);

            //Add keydown event for the keys
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;

            drawtimer.Elapsed += ElapsedMethod;
            drawtimer.AutoReset = false;
            drawtimer.Start();
        }
        //TODO Move functions to MIDIController?
        #region MIDI
        /// <summary>
        /// Event fired on MIDI-input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMidiEventReceived(object? sender, EventArgs e)
        {
            if (e is MidiEventReceivedEventArgs a)
            {

                Debug.WriteLine($"{a.Event}");
                PianoKey? key = PianoController.ParseMidiNote(a.Event);

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
                    SongController.LoadSong((MidiTimeSpan)500);
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
        private void PlayMIDIFile(object sender, RoutedEventArgs e)
        {
            Boolean isisolated = IsolatedPiano.IsChecked;
            if (MIDIController.OriginalMIDI is not null && t is null)
            {
                //Play MIDI when program starts
                t = new Thread(() =>
                {
                    MIDIController.PlayMidi(isisolated);
                    t = null;
                });
                //Makes the thread close when application close
                t.IsBackground = true;
                t.Start();
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

        //TODO Can this be done nicer?
        private void ElapsedMethod(object? sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateMainImage(this, null);
        }
        private void CurrentSong_NotePlayed(object? sender, PianoKeyEventArgs e)
        {
            UpdateMainImage(this, e);
        }


        /// <summary>
        /// Updates MainImage in the WPF with new practice notes. rememberSource is used to prevent flickering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        BitmapSource rememberSource;
        private void UpdateMainImage(object sender, PianoKeyEventArgs e)
        {
            this.MainImage.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.MainImage.Source = null;
                    PianoKey pk = null;
                    //This is done so when we add a new key, it will not update the view as well
                    //TODO Can this be done nicer?
                    if (e is not null)
                    {
                        pk = e.Key;
                    }
                    BitmapSource bitmapSource = PracticeNoteGenerator.CreateBitmapSourceFromGdiBitmap(PracticeNoteGenerator.DrawNotes(PianoController.Piano, pk));

                    if (bitmapSource is not null)
                    {
                        this.MainImage.Source = bitmapSource;
                        rememberSource = bitmapSource;
                    }
                    else
                    {
                        this.MainImage.Source = rememberSource;
                    }

                }));
            drawtimer.Start();
        }
    }
}
