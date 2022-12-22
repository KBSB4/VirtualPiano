using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using Microsoft.Win32;
using Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for FreePlayPiano.xaml
    /// </summary>
    public partial class FreePlayPiano : Page
    {
        private readonly PianoGridGenerator pianoGrid;
        private readonly PracticeNotesGenerator practiceNotes;
        private readonly MainMenu _mainMenu;

        private bool BeenPlayed = false;

        public FreePlayPiano(MainMenu _mainMenu)
        {
            this._mainMenu = _mainMenu;
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
            practiceNotes = new PracticeNotesGenerator(PracticeColumnWhiteKeys, PracticeColumnBlackKeys, 28);
            KeyDown += KeyPressed;
            KeyUp += KeyReleased;

            //Start thread for updating practice notes
            Thread updateVisualNoteThread = new(new ParameterizedThreadStart(UpdateVisualNotes))
            {
                IsBackground = true
            };
            updateVisualNoteThread.Start();
			IsVisibleChanged += UI_IsVisibleChanged;
		}

		private void UI_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			UpdateUI();
		}

		private void UpdateUI()
		{
            BackMenu.Header = LanguageController.GetTranslation(TranslationKey.Menubar_BackToMain);
            SettingsMenuItem.Header = LanguageController.GetTranslation(TranslationKey.Menubar_Settings);
            OpenItem.Header = LanguageController.GetTranslation(TranslationKey.Menubar_MIDI_Open);
            PlayItem.Header = LanguageController.GetTranslation(TranslationKey.Menubar_MIDI_Play);
            StopItem.Header = LanguageController.GetTranslation(TranslationKey.Menubar_MIDI_Stop);
            KaraokeBox.Header = LanguageController.GetTranslation(TranslationKey.Menubar_MIDI_Karaoke);
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
                }));
            }
            catch (TaskCanceledException) //Just in case
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Event fired on MIDI-input
        /// DO NOT REMOVE - Used for MIDI-Keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnMidiEventReceived(object? sender, MidiEventReceivedEventArgs e)
        {
            PianoKey? key = PianoController.ParseMidiNote(e.Event);
            UpdateKey(key);
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
            UpdateKey(key);

            if (PianoController.Piano is null) return;
            if (e.Key == Key.CapsLock) PianoLogic.SwapOctave(PianoController.Piano);
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
                }
                else
                {
                    PianoController.StopPianoSound(key);
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    pianoGrid.DisplayPianoKey(key);
                }));
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
            UpdateKey(key);
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
            StopMIDIFile(null, new RoutedEventArgs());
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
            StopMIDIFile(null, new RoutedEventArgs());
        }

        #endregion

        #region MIDI
        /// <summary>
        /// Checks if everything is okay and not playing before attempting to load new file in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMIDIFileDialog(object sender, RoutedEventArgs e)
        {
            if (SongController.CurrentSong is null)
            {
                BeenPlayed = false;
                StartDialog(KaraokeBox.IsChecked);
            }
            else if (SongController.CurrentSong.IsPlaying)
            {
                MessageBox.Show("There is a MIDI still playing! Stop the playback of the current playing MIDI to continue",
                "MIDI is still playing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                BeenPlayed = false;
                StartDialog(KaraokeBox.IsChecked);
            }
        }

        /// <summary>
        /// Open dialog and prepares MIDI
        /// </summary>
        private static void StartDialog(bool Karoake)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "MIDI Files (*.mid)|*.mid",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            bool? fileOpened = openFileDialog.ShowDialog();
            if (fileOpened == true)
            {
                //Get the path of specified file
                MidiController.OpenMidi(openFileDialog.FileName);
                SongController.DoKaroake = Karoake;
                SongController.LoadSong();
            }
        }

        /// <summary>
        /// Plays the selected MIDI file and checks if a correct MIDI file is selected. when true it plays otherwise it shows a popup with an error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayMIDIFile(object sender, RoutedEventArgs e)
        {
            MidiFile? currentMidiFile = MidiController.GetMidiFile();

            if (currentMidiFile is not null && SongController.CurrentSong is not null && !SongController.CurrentSong.IsPlaying)
            {
                if (!BeenPlayed)
                {
                    SongController.PlaySong();
                    BeenPlayed = true;
                }
                else
                {
                    SongController.DoKaroake = KaraokeBox.IsChecked;
					SongController.LoadSong();
                    SongController.PlaySong();
                }
                SongController.CurrentSong.NotePlayed += CurrentSong_NotePlayed;
            }
            else
            {
                if (currentMidiFile is null)
                {
                    MessageBox.Show("Select a MIDI File first before playing",
                    "No MIDI selected", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (SongController.CurrentSong is not null && SongController.CurrentSong.IsPlaying)
                {
                    MessageBox.Show("There is a MIDI still playing! Stop the playback of the current playing MIDI to continue",
                    "MIDI is still playing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Stop playing the MIDI file which is selected. then produced with an exception it displays a popup with 'No MIDI playing'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopMIDIFile(object? sender, RoutedEventArgs e)
        {
            if (SongController.CurrentSong is not null && SongController.CurrentSong.IsPlaying)
            {
                SongController.CurrentSong.NotePlayed -= CurrentSong_NotePlayed;
                SongController.StopSong();
            }
            else if (sender is not null)
            {
                MessageBox.Show("There is no MIDI playing right now.",
                "No MIDI playing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}