using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using InputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice;


namespace WpfView
{
    /// <summary>
    /// Interaction logic for PracticePlayPiano.xaml
    /// </summary>
    public partial class PracticePlayPiano : Page
    {
        //TODO Is there a way to reuse the code from FreePlay without copy and paste?
        private PianoGridGenerator pianoGrid;
        private static IInputDevice? _inputDevice;
        readonly PracticeNotesGenerator practiceNotes;
        private MainMenu _mainMenu;

        private const int MinScore = 500;

        //TODO use getset to keep it at max 500
        private int Score = 0;
        public PracticePlayPiano(MainMenu mainMenu, int songID)
        {
            this._mainMenu = mainMenu;
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(Play2WhiteKeysGrid, Play2BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(Play2ColumnWhiteKeys, Play2ColumnBlackKeys, 28);
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

            PlaySelectedSong(songID);
        }

        private void PlaySelectedSong(int songID)
        {
            //TODO In the future, this should get the song file from the database based on the songID and then play it. For now we set our own path for testing
            string path = "C:\\Users\\Harris\\Downloads\\sm64.mid";

            //Start song
            MIDIController.OpenMidi(path);
            SongController.LoadSong(new MetricTimeSpan(500));

            SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;
            SongController.PlaySong();
        }

        /// <summary>
        /// Method that constantly updates the practice notes
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateVisualNotes(object? obj)
        {
            var next = DateTime.Now;
            while (true)
            {
                //Debug.WriteLine(Math.Abs((DateTime.Now - next).Milliseconds));
                Thread.Sleep(Math.Abs((DateTime.Now - next).Milliseconds)); // 30 / 120 * bpm
                next = DateTime.Now.AddMilliseconds(25);
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

        Queue<PianoKey> upcomingKeys = new();
        private void CurrentSong_NotePlayed(object? sender, PianoKeyEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (e.Key.PressedDown)
                    {
                        practiceNotes.StartExampleNote(e.Key);
                        upcomingKeys.Enqueue(e.Key);
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
        int PressedAt;
        public void KeyPressed(object? source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;

            PianoKey? key = PianoController.GetPressedPianoKey(intValue);
            if (key is not null)
            {
                pianoGrid.DisplayPianoKey(key);
                PianoController.PlayPianoSound(key);
                //SCORE FUNCTION

                //Check if its to be played in 1 second
                PressedAt = SongController.CurrentSong.TimeInSong.Seconds;
                if (upcomingKeys.Peek().TimeStamp.Seconds < (PressedAt + 1))
                {
                    PianoKey playing = upcomingKeys.Dequeue();
                    if(playing.TimeStamp.Seconds < PressedAt + 1 && PressedAt + 1 > playing.TimeStamp.Seconds)
                    {
                        //played, add score
                        Score += 50;

                        //start counting for how long
                    } else
                    {
                        //too late
                        Score -= 50;
                    } 
                }
            }

            if (e.Key == Key.CapsLock)
                PianoLogic.SwapOctave(PianoController.Piano);

            Debug.WriteLine(Score);
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
                //SCORE FUNCTION
                //check how long is pressed
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

            if (!_mainMenu.SettingsPage.NoneSelected.IsSelected)
            {
                try
                {


                    Debug.Write("send!");
                    ;
                    _inputDevice = InputDevice.GetByIndex(x - 1);
                    _inputDevice.EventReceived += OnMidiEventReceived;
                    _inputDevice.StartEventsListening();


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
}
