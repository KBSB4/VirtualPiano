using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Multimedia;
using Model;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PianoGridGenerator pianoGrid;

        private static IInputDevice _inputDevice;

        public MainWindow()
        {
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);

            //Add keydown event for the keys
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;

            SongController.LoadSong();
            SongController.PlaySong();
            SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;

            _inputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice.GetByName("Launchkey 49");
            _inputDevice.EventReceived += OnMidiEventReceived;
            _inputDevice.StartEventsListening();
        }

        private void CurrentSong_NotePlayed(object? sender, PianoKeyEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                pianoGrid.DisplayPianoKey(e.Key);
                //e.Key.PressedDown = true;

                //PlayNote(e.Key);

                //Thread t = new Thread(new ParameterizedThreadStart(PlayNote));
                //t.Start(e.Key);

                //new Thread(new ParameterizedThreadStart(PlayNote)).Start(e.Key);

                //Debug.WriteLine("EventReceived!");
                //PianoKey? pianoKey = PianoController.Piano.PianoKeys.Find(x => (e.Key.Note == x.Note) && (e.Key.Octave == x.Octave));
                //if (pianoKey is null)
                //{
                //    Debug.WriteLine("But key is null");
                //}
                //else
                //{
                //    pianoKey.PressedDown = true;
                //    Debug.WriteLine("Displaying key: " + pianoKey + " Pressed: " + pianoKey.PressedDown);
                //    pianoGrid.DisplayPianoKey(pianoKey);
                //}
            }));
        }

        private void PlayNote(object? objec)
        {
            PianoKey pianoKey = (PianoKey)objec;

            pianoGrid.DisplayPianoKey(pianoKey);

            Thread.Sleep(pianoKey.Duration);
            pianoKey.PressedDown = false;
            pianoGrid.DisplayPianoKey(pianoKey);
        }

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
        private static Action EmptyDelegate = delegate () { };

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
    }
}
