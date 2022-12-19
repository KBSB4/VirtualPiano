using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Core;
using Microsoft.Win32;
using Model;
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

namespace WpfView
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
  
        private List<Song> Songs= new List<Song>();
        public AdminPanel()
        {
           // GenerateSongList();
            InitializeComponent();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
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
            }
        }


        /// <summary>
        /// Returns true if all input fields are valid.
        /// </summary>
        /// <returns></returns>
        public bool Validator()
        {
            bool isValid = true;
            string errorMessage;
            if (titleTextBox.Text.Length == 0)
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

        public async void Upload()
        {
            int difficulty = int.Parse(difficultyTextBox.Text);
            Difficulty d = (Difficulty)difficulty;
            Song song = new Song() { Description = descriptionTextBox.Text, Difficulty = d, File = MidiLogic.CurrentMidi, Name = titleTextBox.Text };
            Songs.Add(song); // TODO REMOVE  USED FOR TESTING
            MakeSongVisable(song);

           // await DatabaseController.UploadSong(song);

        }

        /// <summary>
        /// Retrieves all the songs that are stored in the database
        /// </summary>
        public async void GenerateSongList()
        {
           // Song[] songs = await DatabaseController.GetAllSongs();
           // Debug.WriteLine(songs.Count());
            foreach (Song song in Songs)
            {
                MakeSongVisable(song);
            }

        }

        public void MakeSongVisable(Song song)
        {
            ListBoxItem one = new ListBoxItem() { Content = song.Name};
            ListBoxItem del = new ListBoxItem() { Content = "X", Name = song.Name, };
            SongListAdminPanel.Items.Add(one);
            RemoveSongsList.Items.Add(del);
        }

      

        private void RemoveSongsList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ListViewItem DeleteSong = (ListViewItem)sender;
            //DatabaseController.DeleteSong(DeleteSong.Name);
        }

        private void RemoveSongsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem DeleteSong = null; 
            

            foreach(ListBoxItem s in RemoveSongsList.Items)
            {
                if(s.IsSelected)
                {
                    DeleteSong= (ListBoxItem)s;
                }
            }
           
            
            if(DeleteSong != null)
            {
               // SongListAdminPanel.Items.Remove(ItemDel(DeleteSong.Name));
               Songs.Remove(ItemDel(DeleteSong.Name));
               RemoveSongsList.Items.Clear();
                SongListAdminPanel.Items.Clear();
               GenerateSongList();
            }
            
           
            MessageBox.Show("worsk");
        }

        public Song ItemDel(string name)
        {
            foreach (var item in Songs)
            {
                if (item.Name.Equals(name))
                {
                    return item;
                }
            }
            return null;
        }
        public void RenewList()
        {
            SongListAdminPanel.Items.Clear();
            GenerateSongList();
        }
    }

   
}
