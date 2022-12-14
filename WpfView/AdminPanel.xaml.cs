using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Core;
using Microsoft.Win32;
using Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        private byte[]? lastOpenedFile;

        private List<Song> songList = new();
        private readonly MainMenu _mainMenu;
        public AdminPanel(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            GenerateSongList();
            InitializeComponent();
        }

        /// <summary>
        ///  Sets the midi-file that has been selected for <see cref=" MidiLogic.CurrentMidi"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadMidiFile_Click(object sender, RoutedEventArgs e)
        {
            MidiLogic.CurrentMidi = null;
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
                MidiLogic.CurrentMidi = MidiFile.Read(openFileDialog.FileName);

                lastOpenedFile = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Returns true if all fields are valid. 
        /// </summary>
        /// <returns></returns>
        public bool Validator()
        {
            bool isValid = true;
            string errorMessage;
            if (!ValidationController.AdminPanelValidationMessageTitle(titleTextBox.Text).Equals(string.Empty))
            {
                errorMessage = ValidationController.AdminPanelValidationMessageTitle(titleTextBox.Text);
            }
            else if (songList.Count > 0 && !IsUniqueSongName(titleTextBox.Text))
            {
                errorMessage = "Title has already been used!";
            }
            else if (!ValidationController.AdminPanelValidationMessageDescription(descriptionTextBox.Text).Equals(string.Empty))
            {
                errorMessage = ValidationController.AdminPanelValidationMessageDescription(descriptionTextBox.Text);
            }
            else if (!ValidationController.AdminPanelValidationMessageMidiFile().Equals(string.Empty))
            {
                errorMessage = ValidationController.AdminPanelValidationMessageMidiFile();
            }
            else
            {
                return isValid;
            }


            MessageBox.Show(errorMessage, "Invalid value", MessageBoxButton.OK, MessageBoxImage.Error);
            isValid = false;
            return isValid;
        }

        private void UploadSongClick(object sender, RoutedEventArgs e)
        {
            if (Validator())
            {
                Upload();
            }
        }

        /// <summary>
        /// Uploads a song to the databases and displays the song on the screen.
        /// </summary>
        public async void Upload()
        {
            //int difficulty = int.Parse(difficultyTextBox.Text);
            int difficulty = difficultyComboBox.SelectedIndex;
            Difficulty d = (Difficulty)difficulty;
            Song song = new()
            {
                Description = descriptionTextBox.Text,
                Difficulty = d,
                FullFile = lastOpenedFile,
                File = MidiLogic.CurrentMidi,
                Name = titleTextBox.Text
            };
            await DatabaseController.UploadSong(song);
            songList.Add(song);
            MakeSongVisible(song);
        }

        /// <summary>
        /// Gets all songs out of the database and displayes them on the screen.
        /// </summary>
        public async void GenerateSongList()
        {
            Song[]? songs = await DatabaseController.GetAllSongs();
            if (songs is null) return;
            songList = new List<Song>();
            songList = songs.ToList();
            foreach (Song song in songs)
            {
                MakeSongVisible(song);
            }
        }

        /// <summary>
        /// Fills a listbox with songs 
        /// </summary>
        /// <param name="song"></param>
        public void MakeSongVisible(Song song)
        {
            ListBoxItem one = new() { Content = song.Name };
            ListBoxItem del = new FemkesListBoxItem() { SongTitle = song.Name, Content = "X" };
            SongListAdminPanel.Items.Add(one);
            RemoveSongsList.Items.Add(del);
        }

        public static async void DeleteSong(string? name)
        {
            if (name is null) return;
            await DatabaseController.DeleteSong(name);
        }

        /// <summary>
        /// Returns true if song name is unique.
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public bool IsUniqueSongName(string song)
        {
            foreach (ListBoxItem item in SongListAdminPanel.Items)
            {
                if (item.Content.Equals(song)) return false;
            }

            return true;
        }

        private void RemoveSongsList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            FemkesListBoxItem? deleteSong = null;

            deleteSong = ((ListBox)sender).SelectedItem as FemkesListBoxItem;

            if (deleteSong is not null)
            {
                var result = MessageBox.Show($"Are you sure u want to delete {deleteSong.SongTitle}?", "Confirm Delete", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    Song? found = songList.Find(x => x.Name.Equals(deleteSong.SongTitle));
                    DeleteSong(deleteSong.SongTitle);
                    if (found is not null)
                    {
                        songList.Remove(found);
                        RenewUploadedSongList();
                    }
                }
            }
        }

        /// <summary>
        /// Updates the screen after a song has been deleted.
        /// </summary>
        public void RenewUploadedSongList()
        {
            SongListAdminPanel.Items.Clear();
            RemoveSongsList.Items.Clear();
            GenerateSongList();
        }

        private void BackToMenuButtonClick(object sender, RoutedEventArgs e)
        {
            if (_mainMenu.LoggedInUser is not null)
            {
                _mainMenu.LoggedInUser = null;
            }
            _mainMenu.Account_ChangeIconBasedOnUser();
            NavigationService?.Navigate(_mainMenu);
        }

        /// <summary>
        /// Used to show songs in the list
        /// </summary>
        class FemkesListBoxItem : ListBoxItem
        {
            public string? SongTitle { get; set; }
        }
    }
}
