using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.MusicTheory;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        //Score
        private int Score = 0;
        List<PianoKey> upcoming = new();
        int PressedAt;
        Dictionary<NoteName, int> playing = new();
        int ReleasedAt;

        public PracticePlayPiano(MainMenu mainMenu, int songID)
        {
            this._mainMenu = mainMenu;
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(Play2WhiteKeysGrid, Play2BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(Play2ColumnWhiteKeys, Play2ColumnBlackKeys, 28, this);
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;

            //Keep this here until we have a better way of connecting phyiscal devices and so we can test
            //_inputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice.GetByName("Launchkey 49");
            //_inputDevice.EventReceived += OnMidiEventReceived;
            //_inputDevice.StartEventsListening();

            PlaySelectedSong(songID);

            //Start thread for updating practice notes
            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateView))
            {
                IsBackground = true
            };
            updateVisualNoteThread.Start();
        }

        private void PlaySelectedSong(int songID)
        {
            //TODO In the future, this should get the song file from the database based on the songID and then play it. For now we set our own path for testing
            string path = "C:\\Users\\Harris\\Downloads\\test.mid";

            //Start song
            MIDIController.OpenMidi(path);
            SongController.LoadSong(new MetricTimeSpan(500));

            SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;
            SongController.PlaySong();
        }

        /// <summary>
        /// Method that constantly updates the view
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateView(object? obj)
        {
            var next = DateTime.Now;
            while (true)
            {
                Thread.Sleep(Math.Abs((DateTime.Now - next).Milliseconds));
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

                //Check if notes been played, delete them from the list then
                foreach (PianoKey key in upcoming)
                {
                    if (SongController.CurrentSong.TimeInSong.Milliseconds > key.TimeStamp.Milliseconds + key.Duration.Milliseconds)
                    {
                        upcoming.Remove(key);
                    }
                }

                //Go to main menu after playing
                if (!SongController.CurrentSong.IsPlaying)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        Thread.Sleep(1000);
                        NavigationService?.Navigate(_mainMenu);
                    }));
                }

                ScoreLabel.Content = "Score = " + Score;
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
                        upcoming.Add(e.Key);
                    }
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

                //SCORE FUNCTION
                PressedAt = SongController.CurrentSong.TimeInSong.Milliseconds;
                PianoKey upcomingKey = upcoming.Where(x => x.Note == key.Note && x.Octave == key.Octave).FirstOrDefault();
                if (upcomingKey != null)
                {
                    //Upcomingkey is the key that should be played next for the current PianoKey
                    if (upcomingKey.TimeStamp.Milliseconds < PressedAt + 200 && PressedAt + 200 > upcomingKey.TimeStamp.Milliseconds)
                    {
                        //played, add score
                        Score += 50;
                        if (!playing.ContainsKey(upcomingKey.Note))
                        {
                            playing.Add(key.Note, PressedAt);
                        }
                    }
                    else if (PressedAt + 200 > upcomingKey.TimeStamp.Milliseconds)
                    {
                        //Too late, no points
                        if (!playing.ContainsKey(upcomingKey.Note))
                        {
                            playing.Add(key.Note, PressedAt);
                        }
                    }
                }
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
                PianoKey upcomingKey = upcoming.Where(x => x.Note == key.Note && x.Octave == key.Octave).FirstOrDefault();
                PianoController.StopPianoSound(key);
                pianoGrid.DisplayPianoKey(key);

                //SCORE FUNCTION
                ReleasedAt = SongController.CurrentSong.TimeInSong.Milliseconds;
                if (upcomingKey is not null)
                {
                    if (upcomingKey.Duration.Milliseconds + upcomingKey.TimeStamp.Milliseconds < PressedAt + 200
                    && PressedAt + 200 > upcomingKey.TimeStamp.Milliseconds + upcomingKey.TimeStamp.Milliseconds)
                    {
                        //played, add score based on how long pressed
                        Score += ReleasedAt - playing[key.Note] * 5;
                        playing.Remove(key.Note);
                        upcoming.Remove(upcomingKey);
                    }
                }
            }
        }

        //NOTE Not used right now
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
        //NOTE Not used right now
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
