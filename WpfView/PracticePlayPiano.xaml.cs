﻿using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Microsoft.Win32;
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
        private MainMenu _mainMenu;
        private PianoGridGenerator pianoGrid;
        readonly PracticeNotesGenerator practiceNotes;

        bool hasStarted = false;

        int score = 0;
        List<PianoKey> notesToBePressed;
        List<PianoKey> currentlyPlaying = new();
        private int MAXNOTESCORE = 1000;
        private int maxTotalScore;

        public PracticePlayPiano(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            InitializeComponent();
            _mainMenu?.CheckInputDevice(SettingsPage.IndexInputDevice);
            //_inputDevice = _mainMenu.FreePlay.InputDevice;
            //_mainMenu.FreePlay.CheckInputDevice(-1);
            //CheckInputDevice(SettingsPage.IndexInputDevice);
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(PracticeColumnWhiteKeys, PracticeColumnBlackKeys, 28);
            KeyDown += KeyPressed;
            KeyUp += KeyReleased;
            SongLogic.startCountDown += StartCountDown;

            //Start thread for updating practice notes
        }


        public void PlaySelectedSong(int songID)
        {
            //TODO In the future, this should get the song file from the database based on the songID and then play it. For now we set our own path for testing
            //TODO For demo do this based on easy and hero- rush e


            //string path = "../../../../WpfView/sm64.mid";
            //string path = "../../../../WpfView/RUshE.mid";
            //string path = "../../../../WpfView/silent_night_easy.mid";
            string path = "../../../../WpfView/test2.mid";


            //Prepare song
            MidiController.OpenMidi(path);
            SongController.LoadSong();

            //Play
            SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;
            SongController.PlaySong();

            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateVisualNotes))
            {
                IsBackground = true
            };
            updateVisualNoteThread.Start();

            hasStarted = true;
        }

        private void StartCountDown(object? sender, EventArgs e)
        {
            Thread countDownThread = new(new ParameterizedThreadStart(CountDown));
            countDownThread.Start();

            notesToBePressed = SongController.CurrentSong.PianoKeys.ToList();
            notesToBePressed.RemoveRange(0, 8);

            //notesToBePressed = SongController.CurrentSong.PianoKeys.Take(new Range(7, SongController.CurrentSong.PianoKeys.Count)).ToList();
            maxTotalScore = notesToBePressed.Count * MAXNOTESCORE * 2;// * 2 because of pressing AND releasing

        }

        private void CountDown(object? obj)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                CountDownImage.Visibility = Visibility.Visible;
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
                CountDownImage.Visibility = Visibility.Hidden;
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

                if (e.Key == Key.CapsLock)
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

                Debug.WriteLine($"Totalscore: {score} / {maxTotalScore}");
            }
        }

        private void ApplyReleasedScore(PianoKey key)
        {
            if (SongLogic.PlaybackDevice.IsRunning || currentlyPlaying.Count > 0)
            {
                if (currentlyPlaying.Contains(key))
                {
                    int noteScore;
                    Rating rating;

                    MetricTimeSpan releasedAt = (MetricTimeSpan)SongLogic.PlaybackDevice.GetCurrentTime(TimeSpanType.Metric);

                    PianoKey? closestNote = notesToBePressed.Where(x => x.Octave == key.Octave && x.Note == key.Note).OrderBy(item => Math.Abs(releasedAt.TotalSeconds - (item.TimeStamp + item.Duration).TotalSeconds)).FirstOrDefault();
                    if (closestNote is not null)
                    {
                        Debug.WriteLine($"Original note: {key} released at: {releasedAt} [][][] Closest note: {closestNote}");
                        int timeDifference = TimeSpan.Compare(releasedAt, (closestNote.TimeStamp + closestNote.Duration));
                        int difference = timeDifference switch
                        {
                            -1 => (int)((closestNote.TimeStamp + closestNote.Duration) - releasedAt).TotalMilliseconds,
                            0 => 0,
                            1 => (int)(releasedAt - (closestNote.TimeStamp + closestNote.Duration)).TotalMilliseconds,
                        };

                        noteScore = Math.Max(MAXNOTESCORE - difference, 0);
                        rating = GetRating(noteScore);
                    }
                    else
                    {
                        noteScore = 0;
                        rating = GetRating(0);
                    }
                    //practiceNotes.DisplayNoteFeedBack(key, rating);

                    currentlyPlaying.Remove(key);
                    score += noteScore;

                    Debug.WriteLine($"Score += {noteScore}");
                }
            }
            UpdateScoreVisual();
        }

        private void UpdateScoreVisual()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                ScoreBar.Value = Math.Round((double)score / maxTotalScore * 100);
                ScoreLabel.Content = "Score = " + score + "/" + maxTotalScore;
            }));
        }

        private void ApplyPressedScore(PianoKey key)
        {
            if (SongLogic.PlaybackDevice.IsRunning)
            {
                if (!currentlyPlaying.Contains(key))
                {
                    int noteScore;
                    Rating rating;

                    MetricTimeSpan pressedAt = (MetricTimeSpan)SongLogic.PlaybackDevice.GetCurrentTime(TimeSpanType.Metric);

                    PianoKey? closestNote = notesToBePressed.Where(x => x.Octave == key.Octave && x.Note == key.Note).OrderBy(item => Math.Abs(pressedAt.TotalSeconds - item.TimeStamp.TotalSeconds)).FirstOrDefault();
                    if (closestNote is not null)
                    {
                        currentlyPlaying.Add(key);
                        int timeDifference = TimeSpan.Compare(pressedAt, closestNote.TimeStamp);

                        int difference = timeDifference switch
                        {
                            -1 => (int)(closestNote.TimeStamp - pressedAt).TotalMilliseconds,
                            0 => 0,
                            1 => (int)(pressedAt - closestNote.TimeStamp).TotalMilliseconds,
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

        #region Menubar event clicks

        /// <summary>
        /// lets the player go back to the main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(_mainMenu);
        }

        /// <summary>
        /// lets the player go to the settings page of Piano Hero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            _mainMenu.SettingsPage.GenerateInputDevices();
            NavigationService?.Navigate(_mainMenu.SettingsPage);
        }

        #endregion
    }
}
