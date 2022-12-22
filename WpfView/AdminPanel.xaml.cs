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
        private byte[] lastOpenedFile;

		private List<Song> songList = new();
		private readonly MainMenu _mainMenu;
		public AdminPanel(MainMenu mainMenu)
		{
			_mainMenu = mainMenu;
			GenerateSongList();
			InitializeComponent();
			IsVisibleChanged += UI_IsVisibleChanged;
		}

		private void UI_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			UpdateUI();
		}

		private void UpdateUI()
		{

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
			if (songList.Count > 0 && !IsUniqueSongName(titleTextBox.Text))
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

            MessageBox.Show(errorMessage, "Invalid value", MessageBoxButton.OK, MessageBoxImage.Error);
            isValid = false;
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
			Song song = new()
			{
				Description = descriptionTextBox.Text,
				Difficulty = d,
				FullFile = lastOpenedFile,
				File = MidiLogic.CurrentMidi,
				Name = titleTextBox.Text
			};
			await DatabaseController.UploadSong(song);
			MakeSongVisable(song);
		}


		/// <summary>
		/// Gets all songs out of the database and displayes them on the screen.
		/// </summary>
		public async void GenerateSongList()
		{
			Song[] songs = await DatabaseController.GetAllSongs();
			songList = new List<Song>();
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
			ListBoxItem one = new() { Content = song.Name };
			ListBoxItem del = new FemkesListBoxItem() { SongTitle = song.Name, Content = "X" };
			SongListAdminPanel.Items.Add(one);
			RemoveSongsList.Items.Add(del);
		}


		public static async void DeleteSong(string name)
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
			foreach (ListBoxItem item in SongListAdminPanel.Items)
			{
				if (item.Content.Equals(song)) return false;
			}

			return true;
		}

        private void RemoveSongsList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            FemkesListBoxItem deleteSong = null;

            deleteSong = (FemkesListBoxItem)((ListBox)sender).SelectedItem;

			if (deleteSong != null)
			{
				var result = MessageBox.Show($"Are you sure u want to delete {deleteSong.SongTitle}?", "Confirm Delete", MessageBoxButton.OKCancel);

				if (result == MessageBoxResult.OK)
				{
					DeleteSong(deleteSong.SongTitle);
					Song? found = songList.Find(x => x.Name.Equals(deleteSong.SongTitle));
					if (found != null)
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

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			NavigationService?.Navigate(_mainMenu);
		}
	}

	class FemkesListBoxItem : ListBoxItem
	{
		public string SongTitle { get; set; }
	}




}
