using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Model;
using Model.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        //Window properties
        private readonly MainMenu _mainMenu;
        private readonly SongSelectPage _songSelectPage;
        private readonly PianoGridGenerator pianoGrid;
        readonly PracticeNotesGenerator practiceNotes;

        //Prevents playing before song starts and allows dialogbox to appear
        bool Playing = false;

        //Score properties
        int score = 0;
        List<PianoKey>? notesToBePressed;
        readonly List<PianoKey> currentlyPlaying = new();
        private const int MAXNOTESCORE = 1000;
        private int maxTotalScore = 0;

        Thread? updateVisualNoteThread;

        private Song? selectedSong;
        private bool stopVisualNoteThread;

        public PracticePlayPiano(MainMenu mainMenu, SongSelectPage songSelectPage)
        {
            _mainMenu = mainMenu;
            _songSelectPage = songSelectPage;
            InitializeComponent();
            _mainMenu.CheckInputDevice(SettingsPage.IndexInputDevice);
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(PracticeColumnWhiteKeys, PracticeColumnBlackKeys, 28);
            KeyDown += KeyPressed;
            KeyUp += KeyReleased;

            IsVisibleChanged += UI_IsVisibleChanged;
        }

        /// <summary>
        /// If page gets navigates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// Translate labels
        /// </summary>
        private void UpdateUI()
        {
            MenuBackButton.Header = LanguageController.GetTranslation(TranslationKey.Menubar_BackToMain);
        }

        /// <summary>
        /// Get song from database by ID and start playing
        /// </summary>
        /// <param name="songID"></param>
        public async void PlaySelectedSong(int songID)
        {
            selectedSong = await DatabaseController.GetSong(songID);

            if (selectedSong is not null && selectedSong?.FullFile is not null)
            {
                SongController.CurrentSong = selectedSong;

                string path = "currentlyPlaying.mid";

                await File.WriteAllBytesAsync(path, selectedSong.FullFile);
                MidiController.OpenMidi(path);

                //Play
                if (SongController.CurrentSong is null) return;
                SongLogic.StartCountDown += StartCountDown;
                SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;

                if (updateVisualNoteThread is null || !updateVisualNoteThread.IsAlive)
                {
                    updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateVisualNotes))
                    {
                        IsBackground = true
                    };
                    stopVisualNoteThread = false;
                    updateVisualNoteThread.Start();
                }

                SongController.PlaySong();
                Playing = true;
            }
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
            if (notesToBePressed.Count > 0)
            {
                notesToBePressed.RemoveRange(0, 2);
            }

            if (notesToBePressed.Count > 0)
            {
                maxTotalScore = notesToBePressed.Count * MAXNOTESCORE * 2;// * 2 because of pressing AND releasing
            }
            else maxTotalScore = 0;
            score = 0;
            UpdateScoreVisual();
        }

        /// <summary>
        /// Handles the displaying for the images
        /// </summary>
        /// <param name="obj"></param>
        private void CountDown(object? obj)
        {
            if (SongController.CurrentSong is null)
            {
                NavigationService?.Navigate(_songSelectPage);
                return;
            }

            Dispatcher.Invoke(new Action(() =>
            {
                CountDownImage.Visibility = Visibility.Visible;
                CountDownImage.Source = new BitmapImage(new Uri("/Images/CountdownReady.png", UriKind.Relative));
            }));

            Tempo x = Tempo.FromBeatsPerMinute(SongController.CurrentSong.File.GetTempoMap().GetTempoAtTime((MetricTimeSpan)TimeSpan.FromSeconds(20)).BeatsPerMinute);
            double b = (60d / x.BeatsPerMinute) * 2.5d;
            double y = b;

            Thread.Sleep(TimeSpan.FromSeconds(y));
            Dispatcher.Invoke(new Action(() =>
            {
                CountDownImage.Source = new BitmapImage(new Uri("/Images/CountdownSet.png", UriKind.Relative));
            }));
            Thread.Sleep(TimeSpan.FromSeconds(y));

            Dispatcher.Invoke(new Action(() =>
            {
                CountDownImage.Source = new BitmapImage(new Uri("/Images/CountdownGo.png", UriKind.Relative));
            }));
            Thread.Sleep(TimeSpan.FromSeconds(y));

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
                if (stopVisualNoteThread) // what im waiting for...
                    break;
            }
        }

        /// <summary>
        /// Opens dialog box after song has finished played
        /// </summary>
        private async void UploadScoreDialog()
        {
            if (SongController.CurrentSong is not null && !SongController.CurrentSong.IsPlaying && Playing)
            {
                //Open UploadScoreDialog
                Playing = false;
                bool? dialogResult = false;
                UploadScoreDialog? uploadScoreDialog = null;
                Dispatcher.Invoke(new Action(() =>
                {
                    uploadScoreDialog = new(score, maxTotalScore);
                    dialogResult = uploadScoreDialog.ShowDialog();
                }));

                //If true, we upload
                if ((bool)dialogResult)
                {
                    if (_mainMenu.LoggedInUser is not null)
                    {
                        Highscore highscore = new()
                        {
                            User = _mainMenu.LoggedInUser,
                            Song = selectedSong,
                            Score = score
                        };

                        //Check if score is already in the database so we just update it
                        Highscore[]? highscores = await DatabaseController.GetHighscores(highscore.Song.Id);
                        Highscore? FoundScore = highscores?.Where(score => score?.User?.Id == highscore.User.Id).FirstOrDefault();

                        if (FoundScore is null)
                        {
                            await DatabaseController.UploadHighscore(highscore);
                        }
                        else
                        {
                            if (FoundScore.Score < highscore.Score)
                            {
                                await DatabaseController.UpdateHighscore(highscore);
                            }
                            else
                            {
                                MessageBox.Show(LanguageController.GetTranslation(TranslationKey.MessageBox_HighscoreHigherThanText),
                                LanguageController.GetTranslation(TranslationKey.MessageBox_HighscoreHigherThanCaption), MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }

                        //Go to menu
                        Dispatcher.Invoke(new Action(() =>
                        {
                            uploadScoreDialog?.Close();
                        }));
                    }
                    else
                    {
                        AccountPage? accountPage = null;
                        Dispatcher.Invoke(new Action(() =>
                        {
                            accountPage = new AccountPage(_mainMenu, this);
                            NavigationService?.Navigate(accountPage);
                        }));

                        while (accountPage is not null)
                        {
                            //await till we return and then open dialogue again
                            if (accountPage.Closed)
                            {
                                //open dialogue box again
                                accountPage = null;
                                Playing = true; //So we can open the dialog again
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
            if (Playing)
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
            if (Playing)
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

                    MetricTimeSpan releasedAt = (MetricTimeSpan)SongLogic.PlaybackDevice.GetCurrentTime(TimeSpanType.Metric);

                    if (notesToBePressed is null) return;
                    PianoKey? closestNote = notesToBePressed.Where(x => x.Octave == key.Octave && x.Note == key.Note).OrderBy(item => Math.Abs(releasedAt.TotalSeconds - (item.TimeStamp + item.Duration).TotalSeconds)).FirstOrDefault();
                    if (closestNote is not null && closestNote.PressedDown)
                    {
                        Debug.WriteLine($"Original note: {key} released at: {releasedAt} [][][] Closest note: {closestNote}");
                        int timeDifference = TimeSpan.Compare(releasedAt, (closestNote.TimeStamp + closestNote.Duration));
                        int difference = timeDifference switch
                        {
                            -1 => (int)((closestNote.TimeStamp + closestNote.Duration) - releasedAt).TotalMilliseconds,
                            1 => (int)(releasedAt - (closestNote.TimeStamp + closestNote.Duration)).TotalMilliseconds,
                            _ => 0,
                        };

                        noteScore = Math.Max(MAXNOTESCORE - difference, -100);
                    }
                    else
                    {
                        noteScore = -100;
                    }
                    currentlyPlaying.Remove(key);
                    score += noteScore;

                    score = Math.Max(score, 0);
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
                if (maxTotalScore > 0)
                {
                    ScoreBar.Value = Math.Round((double)score / maxTotalScore * 100);
                }
                else
                {
                    ScoreBar.Value = 100;
                }
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
                    {
                        if (item.TimeStamp is null) return 99999;
                        return Math.Abs(pressedAt.TotalSeconds - item.TimeStamp.TotalSeconds);
                    }).FirstOrDefault();

                    if (closestNote is not null && !closestNote.PressedDown)
                    {
                        currentlyPlaying.Add(key);
                        int timeDifference = TimeSpan.Compare(pressedAt, closestNote.TimeStamp);

                        int difference = timeDifference switch
                        {
                            -1 => (int)(closestNote.TimeStamp - pressedAt).TotalMilliseconds,
                            1 => (int)(pressedAt - closestNote.TimeStamp).TotalMilliseconds,
                            _ => 0,
                        };

                        noteScore = Math.Max(MAXNOTESCORE - difference, -100);
                        rating = GetRating(noteScore);
                        closestNote.PressedDown = true;
                    }
                    else
                    {
                        noteScore = -100;
                        rating = GetRating(0);
                    }

                    Dispatcher.Invoke(new Action(() =>
                    {
                        practiceNotes.DisplayNoteFeedBack(key, rating);
                        pianoGrid.DisplayPianoKey(key, rating);
                    }));
                    score += noteScore;
                    score = Math.Max(score, 0);

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
            if (Playing)
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
            stopVisualNoteThread = true;
            Playing = false;
            SongController.StopSong();
            NavigationService?.Navigate(_songSelectPage);
        }
    }
}