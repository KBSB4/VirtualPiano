using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for PracticePlayPiano.xaml
    /// </summary>
    public partial class PracticePlayPiano : Page
    {
        private readonly MainMenu? _mainMenu;
        private readonly SongSelectPage _songSelectPage;
        private readonly PianoGridGenerator pianoGrid;
        readonly PracticeNotesGenerator practiceNotes;

        bool hasStarted = false;

        int score = 0;
        List<PianoKey>? notesToBePressed;
        readonly List<PianoKey> currentlyPlaying = new();
        private const int MAXNOTESCORE = 1000;
        private int maxTotalScore;

        public PracticePlayPiano(MainMenu mainMenu, SongSelectPage songSelectPage)
        {
            _mainMenu = mainMenu;
            _songSelectPage = songSelectPage;
            InitializeComponent();
            _mainMenu?.CheckInputDevice(SettingsPage.IndexInputDevice);
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(PracticeColumnWhiteKeys, PracticeColumnBlackKeys, 28);
            KeyDown += KeyPressed;
            KeyUp += KeyReleased;

        }

        public void PlaySelectedSong(int songID)
        {
            //TODO In the future, this should get the song file from the database based on the songID and then play it. For now we set our own path for testing

            //string path = "../../../../WpfView/DebugMidi/sm64.mid";
            //string path = "../../../../WpfView/DebugMidi/RUshE.mid";
            //string path = "../../../../WpfView/DebugMidi/silent_night_easy.mid";
            string path = "../../../../WpfView/DebugMidi/test2.mid";

            //Prepare song
            MidiController.OpenMidi(path);
            SongController.LoadSong();

            //Play
            if (SongController.CurrentSong is null) return;
            SongLogic.StartCountDown += StartCountDown;
            SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;
            SongController.PlaySong();

            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateVisualNotes))
            {
                IsBackground = true
            };
            updateVisualNoteThread.Start();

            hasStarted = true;
        }

        /// <summary>
        /// Start visualisation of the countdown images on screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartCountDown(object? sender, EventArgs e)
        {
            Thread countDownThread = new(new ParameterizedThreadStart(CountDown));
            countDownThread.Start();

            if (SongController.CurrentSong is null) return;
            notesToBePressed = SongController.CurrentSong.PianoKeys.ToList();
            notesToBePressed.RemoveRange(0, 8);
            maxTotalScore = notesToBePressed.Count * MAXNOTESCORE * 2;// * 2 because of pressing AND releasing
            score = 0;
            UpdateScoreVisual();
        }

        /// <summary>
        /// Handles the displaying for the images
        /// </summary>
        /// <param name="obj"></param>
        private void CountDown(object? obj)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                CountDownImage.Visibility = Visibility.Visible;
                CountDownImage.Source = new BitmapImage(new Uri("/Images/CountdownReady.png", UriKind.Relative));
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
                CountDownImage.Visibility = Visibility.Hidden;
                MenuBackButton.IsEnabled = true; //Prevents crash if you try to go back way too early
            }));  
        }

        /// <summary>
        /// Thread that updates the visual position of already placed notes
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateVisualNotes(object? obj)
        {
            var next = DateTime.Now;
            while (true)
            {
                Thread.Sleep(Math.Abs((DateTime.Now - next).Milliseconds));
                next = DateTime.Now.AddMilliseconds(25);// 25 milliseconds equals 40 frames per second
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

                UploadScoreDialog();
            }
        }

        private void UploadScoreDialog()
        {
            if (SongController.CurrentSong is not null && !SongController.CurrentSong.IsPlaying && hasStarted)
            {
                hasStarted = false;
                bool? dialogResult = false;
                UploadScoreDialog? uploadScoreDialog = null;
                Dispatcher.Invoke(new Action(() =>
                {
                    uploadScoreDialog = new(score, maxTotalScore); ;
                    dialogResult = uploadScoreDialog.ShowDialog();
                }));

                if ((bool)dialogResult)
                {
                    if (false) //TODO if logged in
                    {
                        //TODO Upload score

                        //Go to menu
                        Dispatcher.Invoke(new Action(() =>
                        {
                            if (uploadScoreDialog is not null) uploadScoreDialog.Close();
                        }));
                    }
                    else
                    {
                        //TODO Go to login, wait for a response then return here
                        SettingsPage? accountPage = null;
                        Dispatcher.Invoke(new Action(() =>
                        {
                            //NOTE SETTINGS PAGE IS TEMPORARY
                            accountPage = new SettingsPage(this);
                            NavigationService?.Navigate(accountPage);
                        }));

                        while (accountPage is not null)
                        {
                            //await till we return and then open dialogue again
                            if (accountPage.Closed)
                            {
                                //open dialogue box again
                                accountPage = null;
                                hasStarted = true; //So we can open the dialog again
                                UploadScoreDialog();
                                return;
                            }
                        }
                    }
                }

                //Return to Songselectpage and update leaderboard
                SongLogic.StartCountDown -= StartCountDown;
                SongController.CurrentSong.NotePlayed -= CurrentSong_NotePlayed;

                Dispatcher.Invoke(new Action(() =>
                {
                    _songSelectPage.CreateShowLeaderboard();
                    NavigationService?.Navigate(_songSelectPage);
                }));
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

        /// <summary>
        /// Event fired on MIDI-input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnMidiEventReceived(object? sender, MidiEventReceivedEventArgs e)
        {
            if (hasStarted)
            {
                PianoKey? key = PianoController.ParseMidiNote(e.Event);

                UpdateKey(key);
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
            if (hasStarted)
            {
                int intValue = (int)e.Key;

                PianoKey? key = PianoController.GetPressedPianoKey(intValue);
                UpdateKey(key);

                if (e.Key == Key.CapsLock && PianoController.Piano is not null)
                    PianoLogic.SwapOctave(PianoController.Piano);
            }
        }

        /// <summary>
        /// Occurs when a key is pressed or released, for keyboard and midi
        /// </summary>
        /// <param name="key"></param>
        private void UpdateKey(PianoKey? key)
        {
            if (key is not null)
            {
                if (key.PressedDown)
                {
                    PianoController.PlayPianoSound(key);
                    ApplyPressedScore(key);
                }
                else
                {
                    PianoController.StopPianoSound(key);
                    ApplyReleasedScore(key);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        pianoGrid.DisplayPianoKey(key);
                    }));
                }
            }
        }

        /// <summary>
        /// Calculate score when played note is released and apply to <see cref="score"/>
        /// </summary>
        /// <param name="key"></param>
        //TODO Merge with ApplyPressedScore
        private void ApplyReleasedScore(PianoKey key)
        {
            if (SongLogic.PlaybackDevice is not null && (SongLogic.PlaybackDevice.IsRunning || currentlyPlaying.Count > 0))
            {
                if (currentlyPlaying.Contains(key))
                {
                    int noteScore;
                    Rating rating;

                    MetricTimeSpan releasedAt = (MetricTimeSpan)SongLogic.PlaybackDevice.GetCurrentTime(TimeSpanType.Metric);

                    if (notesToBePressed is null) return;
                    PianoKey? closestNote = notesToBePressed.Where(x => x.Octave == key.Octave && x.Note == key.Note).OrderBy(item => Math.Abs(releasedAt.TotalSeconds - (item.TimeStamp + item.Duration).TotalSeconds)).FirstOrDefault();
                    if (closestNote is not null)
                    {
                        Debug.WriteLine($"Original note: {key} released at: {releasedAt} [][][] Closest note: {closestNote}");
                        int timeDifference = TimeSpan.Compare(releasedAt, (closestNote.TimeStamp + closestNote.Duration));
                        int difference = timeDifference switch
                        {
                            -1 => (int)((closestNote.TimeStamp + closestNote.Duration) - releasedAt).TotalMilliseconds,
                            1 => (int)(releasedAt - (closestNote.TimeStamp + closestNote.Duration)).TotalMilliseconds,
                            _ => 0,
                        };

                        noteScore = Math.Max(MAXNOTESCORE - difference, 0);
                        rating = GetRating(noteScore);
                    }
                    else
                    {
                        noteScore = 0;
                        rating = GetRating(0);
                    }
                    currentlyPlaying.Remove(key);
                    score += noteScore;
                }
            }
            UpdateScoreVisual();
        }

        /// <summary>
        /// Updates the scorelabel and scorebar
        /// </summary>
        private void UpdateScoreVisual()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                ScoreBar.Value = Math.Round((double)score / maxTotalScore * 100);
                ScoreLabel.Content = "Score = " + score + "/" + maxTotalScore;
            }));
        }

        /// <summary>
        /// Calculate score when played note is pressed and apply to <see cref="score"/>
        /// </summary>
        /// <param name="key"></param>
        //TODO Merge with ApplyReleasedScore
        private void ApplyPressedScore(PianoKey key)
        {
            if (SongLogic.PlaybackDevice is not null && SongLogic.PlaybackDevice.IsRunning)
            {
                if (!currentlyPlaying.Contains(key))
                {
                    int noteScore;
                    Rating rating;

                    MetricTimeSpan pressedAt = (MetricTimeSpan)SongLogic.PlaybackDevice.GetCurrentTime(TimeSpanType.Metric);

                    if (notesToBePressed is null) return;
                    PianoKey? closestNote = notesToBePressed.Where(x => x.Octave == key.Octave && x.Note == key.Note).OrderBy(item =>
                    { if (item.TimeStamp is null) return false; Math.Abs(pressedAt.TotalSeconds - item.TimeStamp.TotalSeconds); return true; }).FirstOrDefault();

                    if (closestNote is not null)
                    {
                        currentlyPlaying.Add(key);
                        int timeDifference = TimeSpan.Compare(pressedAt, closestNote.TimeStamp);

                        int difference = timeDifference switch
                        {
                            -1 => (int)(closestNote.TimeStamp - pressedAt).TotalMilliseconds,
                            1 => (int)(pressedAt - closestNote.TimeStamp).TotalMilliseconds,
                            _ => 0,
                        };

                        noteScore = Math.Max(MAXNOTESCORE - difference, 0);
                        rating = GetRating(noteScore);
                    }
                    else
                    {
                        noteScore = 0;
                        rating = GetRating(0);
                    }

                    Dispatcher.Invoke(new Action(() =>
                    {
                        practiceNotes.DisplayNoteFeedBack(key, rating);
                        pianoGrid.DisplayPianoKey(key, rating);
                    }));
                    score += noteScore;

                    Debug.WriteLine($"Score += {noteScore}");
                }
            }
            UpdateScoreVisual();
        }


        /// <summary>
        /// Ties every score to a rating
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        private static Rating GetRating(int score)
        {
            return score switch
            {
                >= 900 => Rating.Perfect,
                >= 750 => Rating.Great,
                >= 500 => Rating.Good,
                >= 1 => Rating.Ok,
                _ => Rating.Miss,
            };
        }

        /// <summary>
        /// If pressed down keyboard key gets released, stop the audio playing for the pianokey and unpress it.
        /// Updates the view
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void KeyReleased(object source, KeyEventArgs e)
        {
            if (hasStarted)
            {
                int intValue = (int)e.Key;
                PianoKey? key = PianoController.GetReleasedKey(intValue);
                UpdateKey(key);
            }
        }

        /// <summary>
        /// lets the player go back to the main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            hasStarted = false; //TODO rename name to be more clear
            SongController.StopSong();
            NavigationService?.Navigate(_songSelectPage);
        }
    }
}