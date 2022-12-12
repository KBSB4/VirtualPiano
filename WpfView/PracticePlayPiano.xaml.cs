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
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
            SongLogic.startCountDown += StartCountDown;

            PlaySelectedSong(songID);

            //Start thread for updating practice notes
            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateView))
            {
                IsBackground = true
            };
            updateVisualNoteThread.Start();
            stopWatch.Start();
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
                CountDownImage.Visibility = System.Windows.Visibility.Visible;
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
                CountDownImage.Visibility = System.Windows.Visibility.Hidden;
            }));
        }

        private void PlaySelectedSong(int songID)
        {
            //TODO In the future, this should get the song file from the database based on the songID and then play it. For now we set our own path for testing
            //TODO For demo do this based on easy and hero- rush e
            string path = "../../../../WpfView/sm64.mid";

            //Prepare song
            MidiController.OpenMidi(path);
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

            for (int i = 0; i < 8; i++)
            {
                allKeys.Dequeue(); //Delete first 8 notes which are the starting notes
            }
            while (allKeys.Count > 0)
            {
                PianoKey key = allKeys.Dequeue();
                totalScore += (int)key.Duration.TotalMilliseconds;
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
                        ScoreLabel.Content = "Score = " + Score + "/" + TotalScore;
                        ScoreBar.Value = Math.Round((double)Score / (double)TotalScore * (double)100);
                    }));
                }
                catch (TaskCanceledException) //Just in case
                {
                    Environment.Exit(0);
                }

                //Go to main menu after playing
                //TODO Properly close this page so it doesnt hang on other pages
                //if (!SongController.CurrentSong.IsPlaying)
                //{
                //    Dispatcher.Invoke(new Action(() =>
                //    {
                //        if (PianoLogic.currentPlayingAudio is not null)
                //        {
                //            foreach (KeyValuePair<PianoKey, FadingAudio> entry in PianoLogic.currentPlayingAudio)
                //            {
                //                entry.Value.StopPlaying(0);
                //            }
                //        }
                //        Thread.Sleep(5000);
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

                PianoController.PlayPianoSound(key);

                //SCORE FUNCTION
                PressedAt = (int)((MetricTimeSpan)SongLogic.PlaybackDevice.GetCurrentTime(TimeSpanType.Metric)).TotalMilliseconds;

                PianoKey upcomingKey = practiceNotes.upcoming.Where(x => x.Note == key.Note && x.Octave == key.Octave).FirstOrDefault();
                if (!playing.ContainsKey(key.Note))
                {
                    pianoGrid.DisplayPianoKey(key);
                    if (upcomingKey != null && !playedNotes.Contains(upcomingKey))
                    {
                        //Upcomingkey is the key that should be played next for the current PianoKey
                        //todo TWEAK VALUES
                        if (PressedAt > upcomingKey.TimeStamp.TotalMilliseconds)
                        {
                            //played, add score
                            if (!playedNotes.Contains(upcomingKey))
                            {
                                //Score += 50;
                                //playedNotes.Add(upcomingKey);
                                var rating = Rating.Perfect;
                                pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.Green));
                                practiceNotes.DisplayNoteFeedBack(key, rating);
                            }
                        } else
                        {
                            var rating = Rating.Miss;
                            pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.Red));
                            practiceNotes.DisplayNoteFeedBack(key, rating);
                        }
                        playing.Add(key.Note, PressedAt);
                        //else if (PressedAt > upcomingKey.TimeStamp.TotalMilliseconds && !playedNotes.Contains(upcomingKey))
                        //{
                        //    //Too late, no points
                        //    if (!playing.ContainsKey(upcomingKey.Note))
                        //    {
                        //        //Var rating = Rating.Ok;
                        //        //practiceNotes.DisplayNoteFeedBack(key, rating);
                        //        //pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.Yellow));
                        //        playing.Add(key.Note, PressedAt);
                        //        Debug.WriteLine("Added NO points with " + key.Note);
                        //    }
                        //}
                        //else if(!playedNotes.Contains(upcomingKey))
                        //{
                        //    //var rating = Rating.Miss;
                        //    //pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.Red));
                        //    //practiceNotes.DisplayNoteFeedBack(key, rating);
                        //}

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
        bool firstNote = true;
        public void KeyReleased(object source, KeyEventArgs e)
        {
            int ReleasedAt;
            int intValue = (int)e.Key;
            double MaxScore = 0;
            PianoKey? key = PianoController.GetReleasedKey(intValue);
            if (key is not null)
            {
                PianoKey upcomingKey = practiceNotes.upcoming.Where(x => x.Note == key.Note && x.Octave == key.Octave).FirstOrDefault();
                PianoController.StopPianoSound(key);

                //SCORE FUNCTION
                ReleasedAt = (int)((MetricTimeSpan)SongLogic.PlaybackDevice.GetCurrentTime(TimeSpanType.Metric)).TotalMilliseconds;
                int scoreToAdd = 0;
                if (upcomingKey is not null)
                {
                    double PerfectTimeToPress = upcomingKey.TimeStamp.TotalMilliseconds;
                    double PerfectTimeToRelease = PerfectTimeToPress + upcomingKey.Duration.TotalMilliseconds;
                    MaxScore = (double)upcomingKey.Duration.TotalMilliseconds;
                    ////todo TWEAK VALUES
                    //if (ReleasedAt < (upcomingKey.TimeStamp.TotalMilliseconds + upcomingKey.Duration.TotalMilliseconds))
                    //{
                    //    //played, add score based on how long pressed
                    //    if (playing.ContainsKey(key.Note))
                    //    {
                    //        Score += (ReleasedAt - playing[key.Note]);
                    //        //var rating = Rating.Great;
                    //        //practiceNotes.DisplayNoteFeedBack(key, rating);
                    //        //pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.Orange));
                    //    }
                    //}

                    //TODO FIX FIRST NOTE, gives less than it should
                    if (playing.ContainsKey(key.Note))
                    {
                        if (firstNote)
                        {
                            playing[key.Note] += 300;
                            ReleasedAt += 200;
                            firstNote = false;
                        }
                        int pressed = playing[key.Note];
                        if (pressed >= PerfectTimeToPress)
                        {
                            if (pressed < PerfectTimeToPress) pressed = (int)PerfectTimeToPress;
                            scoreToAdd = (int)(MaxScore - ((double)(PerfectTimeToPress - pressed)) - (double)(PerfectTimeToRelease - ReleasedAt));
                            Score += scoreToAdd;
                        }
                    }

                }

                if (playing.ContainsKey(key.Note)) playing.Remove(key.Note);
                //Give visual
                switch (scoreToAdd / MaxScore * 100)
                {
                    case < 10:
                        practiceNotes.DisplayNoteFeedBack(key, Rating.Miss);
                        //pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.Red));
                        break;
                    case < 25:
                        practiceNotes.DisplayNoteFeedBack(key, Rating.Ok);
                        //pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.Orange));
                        break;
                    case < 50:
                        practiceNotes.DisplayNoteFeedBack(key, Rating.Good);
                        //pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.Yellow));
                        break;
                    case < 75:
                        practiceNotes.DisplayNoteFeedBack(key, Rating.Great);
                        //pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.LightGreen));
                        break;
                    case < 100:
                        practiceNotes.DisplayNoteFeedBack(key, Rating.Perfect);
                        //pianoGrid.DisplayPianoKey(key, new System.Windows.Media.SolidColorBrush(Colors.Green));
                        break;
                    default:
                        pianoGrid.DisplayPianoKey(key);
                        break;
                }
                pianoGrid.DisplayPianoKey(key);
            }
        }
    }
}