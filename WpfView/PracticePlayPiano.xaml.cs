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
        private Stopwatch stopWatch = new Stopwatch();

        //Score
        private int Score = 0;
        Dictionary<NoteName, int> playing = new();
        List<PianoKey> playedNotes = new();

        public PracticePlayPiano(MainMenu mainMenu, int songID)
        {
            this._mainMenu = mainMenu;
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(Play2WhiteKeysGrid, Play2BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(Play2ColumnWhiteKeys, Play2ColumnBlackKeys, 28, this);

            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;

            PlaySelectedSong(songID);

            //Start thread for updating practice notes
            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateView))
            {
                IsBackground = true
            };
            updateVisualNoteThread.Start();
            stopWatch.Start();
        }

        private void PlaySelectedSong(int songID)
        {
            //TODO In the future, this should get the song file from the database based on the songID and then play it. For now we set our own path for testing
            string path = "../../../../WpfView/test.mid";

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
                        ScoreLabel.Content = "Score = " + Score;
                    }));
                }
                catch (TaskCanceledException) //Just in case
                {
                    Environment.Exit(0);
                }

                //Go to main menu after playing
                if (!SongController.CurrentSong.IsPlaying)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        //TODO Sometimes hangs
                        Thread.Sleep(1000);
                        NavigationService?.Navigate(_mainMenu);
                    }));
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
                }));
            }
            catch (TaskCanceledException) //Just in case
            {
                Environment.Exit(0);
            }
        }

        public void DeletedPressedKey(object? sender, PianoKeyEventArgs e)
        {
            if (playing.ContainsKey(e.Key.Note))
            {
                int ReleasedAt = (int)(e.Key.TimeStamp.TotalMilliseconds + e.Key.Duration.TotalMilliseconds);
                Score += ((ReleasedAt - playing[e.Key.Note]) / 10);
                playing.Remove(e.Key.Note);
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
            int PressedAt;
            int intValue = (int)e.Key;

            PianoKey? key = PianoController.GetPressedPianoKey(intValue);
            if (key is not null && stopWatch.Elapsed.TotalSeconds >= 2)
            {
                stopWatch.Stop();
                pianoGrid.DisplayPianoKey(key);
                PianoController.PlayPianoSound(key);

                //SCORE FUNCTION
                PressedAt = (int)SongController.CurrentSong.TimeInSong.TotalMilliseconds;

                PianoKey upcomingKey = practiceNotes.upcoming.Where(x => x.Note == key.Note && x.Octave == key.Octave).FirstOrDefault();

                if (upcomingKey != null && !playedNotes.Contains(upcomingKey))
                {
                    //Upcomingkey is the key that should be played next for the current PianoKey
                    //todo TWEAK VALUES
                    if (PressedAt > upcomingKey.TimeStamp.TotalMilliseconds - 150 && PressedAt < upcomingKey.TimeStamp.TotalMilliseconds + 150)
                    {
                        //played, add score
                        if (!playing.ContainsKey(upcomingKey.Note))
                        {
                            Score += 50;
                            playing.Add(key.Note, PressedAt);
                            playedNotes.Add(upcomingKey);
                        }
                    }
                    else if (PressedAt > upcomingKey.TimeStamp.TotalMilliseconds)
                    {
                        //Too late, no points
                        if (!playing.ContainsKey(upcomingKey.Note))
                        {
                            playing.Add(key.Note, PressedAt);
                            Debug.WriteLine("Added NO points with " + key.Note);
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
            int ReleasedAt;
            int intValue = (int)e.Key;
            PianoKey? key = PianoController.GetReleasedKey(intValue);
            if (key is not null)
            {
                PianoKey upcomingKey = practiceNotes.upcoming.Where(x => x.Note == key.Note && x.Octave == key.Octave).FirstOrDefault();
                PianoController.StopPianoSound(key);
                pianoGrid.DisplayPianoKey(key);

                //SCORE FUNCTION
                ReleasedAt = (int)SongController.CurrentSong.TimeInSong.TotalMilliseconds;
                if (upcomingKey is not null)
                {
                    //todo TWEAK VALUES
                    if (ReleasedAt < (upcomingKey.TimeStamp.TotalMilliseconds + upcomingKey.Duration.TotalMilliseconds) + 50)
                    {
                        //played, add score based on how long pressed
                        if (playing.ContainsKey(key.Note))
                        {
                            Score += (ReleasedAt - playing[key.Note]) / 10;
                        }
                    }
                }
                playing.Remove(key.Note);
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