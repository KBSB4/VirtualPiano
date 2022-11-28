using BusinessLogic;
using Controller;
using Microsoft.Win32;
using Model;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PianoGridGenerator pianoGrid;

        public MainWindow()
        {
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);

            //Add keydown event for the keys
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;
        }

        /// <summary>
        /// Eventhandler for when the key gets pressed. Updates key and plays the audio
        /// Updates the view
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void KeyPressed(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;

            PianoKey? key = PianoController.GetPressedPianoKey(intValue);
            if (key is not null)
            {
                pianoGrid.DisplayPianoKey(key, true);
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
        public void KeyReleased(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;
            PianoKey? key = PianoController.GetReleasedKey(intValue);
            if (key is not null)
            {
                pianoGrid.DisplayPianoKey(key, false);
            }

        }

        #region MIDI
        /// <summary>
        /// Opens the dialog to select a MIDI file and open it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMIDIFileDialog(object sender, RoutedEventArgs e)
        {
            if (t is null)
            {
                var openFileDialog = new OpenFileDialog();

                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "MIDI Files (*.mid)|*.mid";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if ((bool)openFileDialog.ShowDialog())
                {
                    //Get the path of specified file
                    MIDIController.OpenMidi(openFileDialog.FileName);
                }
            } else
            {
                MessageBox.Show("There is a MIDI still playing! Stop the playback of the current playing MIDI to continue",
                    "MIDI is still playing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        //To play MIDIs without hogging the main thread
        Thread t = null;

        /// <summary>
        /// Play MIDI File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayMIDIFile(object sender, RoutedEventArgs e)
        {
            Boolean isisolated = IsolatedPiano.IsChecked;
            if (MIDIController.OriginalMIDI is not null && t is null)
            {
                //Play MIDI when program starts
                t = new Thread(() =>
                {
                    MIDIController.PlayMidi(isisolated);
                    t = null;
                });
                //Makes the thread close when application close
                t.IsBackground = true;
                t.Start();
            } else
            {
                if(MIDIController.OriginalMIDI is null)
                {
                    MessageBox.Show("Select a MIDI File first before playing",
                    "No MIDI selected", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
                if(t is not null)
                {
                    MessageBox.Show("There is a MIDI still playing! Stop the playback of the current playing MIDI to continue",
                    "MIDI is still playing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Stop playing the MIDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopMIDIFile(object sender, RoutedEventArgs e)
        {
            if (t is not null)
            {
                t.Interrupt();
                t = null;
            } else
            {
                MessageBox.Show("There is no MIDI playing right now.",
                "No MIDI playing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateMainImage(object sender, EventArgs e)
        {
                this.MainImage.Dispatcher.BeginInvoke(

                    DispatcherPriority.Render,
                    new Action(() =>
                    {
                        this.MainImage.Source = null;
                        this.MainImage.Source = PracticeNoteGenerator.CreateBitmapSourceFromGdiBitmap(PracticeNoteGenerator.DrawNotes());
                    }));
        }
        #endregion
    }
}
