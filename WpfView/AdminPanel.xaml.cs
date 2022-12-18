using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Core;
using Microsoft.Win32;
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
  
        public AdminPanel()
        {
           
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

        public void Upload()
        {
            int difficulty = int.Parse(difficultyTextBox.Text);
            using (SqlConnection connection = new SqlConnection())
            {
                String query = "INSERT INTO Song (name, midifile, difficulty, description) VALUES (@name, @file, @difficulty, @description)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", titleTextBox);
                    command.Parameters.AddWithValue("@file", MidiLogic.CurrentMidi);
                    command.Parameters.AddWithValue("@difficulty", difficulty);
                    command.Parameters.AddWithValue("@description", descriptionTextBox);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                }
            }
        }
    }

   
}
