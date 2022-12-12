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


namespace WpfView
{
    /// <summary>
    /// Interaction logic for PracticePlayPiano.xaml
    /// </summary>
    public partial class PracticePlayPiano : Page
    {
        private PianoGridGenerator pianoGrid;
        private static IInputDevice? _inputDevice;
        readonly PracticeNotesGenerator practiceNotes;
        private MainMenu _mainMenu;
        private Stopwatch stopWatch = new Stopwatch();

        //Score
        private int Score = 0;
        private int TotalScore = 0;
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

            //Prepare song
            MIDIController.OpenMidi(path);
            SongController.LoadSong(new MetricTimeSpan(500));

            //Get TotalScore
            TotalScore = CalculateMaximalScore();

            //Play
            SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;
            SongController.PlaySong();
        }

        private int CalculateMaximalScore()
        {
            Queue<PianoKey> allKeys = new(SongController.CurrentSong.PianoKeys); //Copy over
            int totalScore = 0;
            while (allKeys.Count > 0)
            {
                PianoKey key = allKeys.Dequeue();
                totalScore += 100;
            }
            return totalScore;
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
                        //TODO Make a concept and then implement it
                        ScoreLabel.Content = "Score = " + Score + "/" + TotalScore;
                    }));
                }
                catch (TaskCanceledException) //Just in case
                {
                    Environment.Exit(0);
                }

                //Go to main menu after playing
                //TODO Wait for merge with dev before doing this. Before closing it should STOP ALL PIANO KEYS SOUNDS
                //if (!SongController.CurrentSong.IsPlaying)
                //{
                //    Dispatcher.Invoke(new Action(() =>
                //    {
                //        //TODO Sometimes hangs
                //        Thread.Sleep(1000);
                //        NavigationService?.Navigate(_mainMenu);
                //    }));
                //}
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
                //todo fIX
                //Score += ((ReleasedAt - playing[e.Key.Note]) / 10);
                //playing.Remove(e.Key.Note);
                //playedNotes.Add(e.Key);
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
                        pianoGrid.DisplayPianoKey(key); //TODO CHANGE COLOUR DEPENDING ON HOW WELL
                    }));
                }
                else
                {
                    PianoController.StopPianoSound(key);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        pianoGrid.DisplayPianoKey(key); //TODO CHANGE COLOUR DEPENDING ON HOW WELL
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
                pianoGrid.DisplayPianoKey(key); //TODO CHANGE COLOUR DEPENDING ON HOW WELL
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
                pianoGrid.DisplayPianoKey(key); //TODO CHANGE COLOUR DEPENDING ON HOW WELL

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
                if (playing.ContainsKey(key.Note)) playing.Remove(key.Note);
            }
        }
    }
}