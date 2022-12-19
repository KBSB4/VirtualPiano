﻿using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Core;
using Microsoft.Win32;
using Model;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        private List<Song> songList = new();
        public AdminPanel()
        {
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
            }
        }


        /// <summary>
        /// Returns true if all fields are valid. 
        /// </summary>
        /// <returns></returns>
        public bool Validator()
        {
            bool isValid = true;
            string errorMessage = string.Empty;

            if (IsUniqueSongName(titleTextBox.Text))
            {
                errorMessage = "Title has already been used!";
            }
            else if (titleTextBox.Text.Length == 0)
            {
                errorMessage = "Title is required!";
            }
            else if (titleTextBox.Text.Length > 30)
            {
                errorMessage = "Title must be between 1 and 30 characters.";
            }
            else if (descriptionTextBox.Text.Length > 65)
            {
                errorMessage = "Description must be between 0 and 65 characters.";
            }
            else if (!int.TryParse(difficultyTextBox.Text, out int difficulty) || !(difficulty > -1 && difficulty < 4))
            {
                errorMessage = "Difficulty must be number between 0 {easy}, 1 {medium}, 2 {hard} or 3 {hero}.";
            }
            else if (MidiLogic.CurrentMidi == null)
            {
                errorMessage = "MidiFile required!";
            }
            else
            {
                return isValid;
            }

            MessageBox.Show(errorMessage, "Invalid value",MessageBoxButton.OK,MessageBoxImage.Error);
            isValid= false;
            return isValid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
            int difficulty = int.Parse(difficultyTextBox.Text);
            Difficulty d = (Difficulty)difficulty;
            Song song = new Song() { Description = descriptionTextBox.Text, Difficulty = d, File = MidiLogic.CurrentMidi, Name = titleTextBox.Text };
            await DatabaseController.UploadSong(song);
            MakeSongVisable(song);
        }


        /// <summary>
        /// Gets all songs out of the database and displayes them on the screen.
        /// </summary>
        public async void GenerateSongList()
        {
            Song[] songs = await DatabaseController.GetAllSongs();
            songList = songs.ToList();
            foreach (Song song in songs)
            {
                MakeSongVisable(song);
            }
        }

        /// <summary>
        /// Fills a listbox with songs 
        /// </summary>
        /// <param name="song"></param>
        public void MakeSongVisable(Song song)
        {
            ListBoxItem one = new ListBoxItem() { Content = song.Name };
            ListBoxItem del = new ListBoxItem() { Content = "X", Name = song.Name, };
            SongListAdminPanel.Items.Add(one);
            RemoveSongsList.Items.Add(del);
        }


        public async void DeleteSong( string name)
        {
           await DatabaseController.DeleteSong(name);
        }

        /// <summary>
        /// Returns true if song name is unique.
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public bool IsUniqueSongName(string song)
        {

            foreach (Song s in songList)
            {
                if (s.Name.Equals(song))
                {
                    return false;
                }
            }
            return true;
        }

        private void RemoveSongsList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem deleteSong = null;
            foreach (ListBoxItem s in RemoveSongsList.Items)
            {
                if (s.IsSelected)
                {
                    deleteSong = (ListBoxItem)s;
                }
            }
            if (deleteSong != null)
            {
               var result = MessageBox.Show($"Are you sure u want to delete {deleteSong.Name}?", "Confirm Delete", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    DeleteSong(deleteSong.Name);
                    Song? found = songList.Find(x => x.Name.Equals(deleteSong.Name));
                    if (found != null) songList.Remove(found);
                    RenewUploadedSongList();
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


    }

     


}
