using Controller;
using Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using VirtualPiano.PianoSoundPlayer;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Piano Piano { get; set; }

        private PianoSoundPlayer PianoSoundPlayer { get; set; }
        private Dictionary<Key, FadingAudio> currentPlayingAudio = new();

        //TODO Naar PianoController
        public MainWindow()
        {
            InitializeComponent();
            _ = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);

            //Create piano
            Piano = PianoController.CreatePiano();
            PianoSoundPlayer = new("../../../../WpfView/Sounds/Piano/", "", ".wav");

            //Add keydown event for the keys
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;
        }

        /// <summary>
        /// Eventhandler for when the key gets pressed. Updates key and plays the audio
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void KeyPressed(object source, KeyEventArgs e)
        {
            int intValue;
            string keyValue;
            PianoKey pianoKey;
            GetKeyWithShift(e, out intValue, out keyValue);
            pianoKey = PianoController.UpdateKey(e.Key.ToString(), intValue, true);
            PianoController.PlayKey(pianoKey);
        }

        /// <summary>
        /// Eventhandler for when the key no longer gets pressed. Updates key and stops playing the audio
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void KeyReleased(object source, KeyEventArgs e)
        {
            int intValue;
            string keyValue;
            GetKeyWithShift(e, out intValue, out keyValue);
            PianoController.UpdateKey(e.Key.ToString(), intValue, false);

            if (currentPlayingAudio.ContainsKey(e.Key))
            {
                currentPlayingAudio[e.Key].StopPlaying(50);
                currentPlayingAudio.Remove(e.Key);
            }
        }

        /// <summary>
        /// Get the key that got pressed and check if it has been pressed with SHIFT. Updates the string accordingly
        /// </summary>
        /// <param name="e"></param>
        /// <param name="intValue"></param>
        /// <param name="keyValue"></param>
        private static void GetKeyWithShift(KeyEventArgs e, out int intValue, out string keyValue)
        {
            intValue = (int)e.Key;
            keyValue = e.Key.ToString();
            if (Keyboard.IsKeyDown(System.Windows.Input.Key.RightShift) || Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift))
            {
                keyValue = keyValue.ToUpper();
            }
            else
            {
                keyValue = keyValue.ToLower();
            }
        }
    }
}
