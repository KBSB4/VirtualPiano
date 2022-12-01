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

            SongController.LoadSong();
            SongController.PlaySong();
            SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;

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
                Dispatcher.Invoke(new Action(() =>
                {
                    practiceNotes.UpdateExampleNotes();
                }));
            }
        }

        private void CurrentSong_NotePlayed(object? sender, PianoKeyEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (e.Key.PressedDown)
                {
                    practiceNotes.StartExampleNote(e.Key);
                }
                //pianoGrid.DisplayPianoKey(e.Key);
            }));
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
    }
}
