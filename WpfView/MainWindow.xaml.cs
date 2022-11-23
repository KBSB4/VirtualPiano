using Controller;
using Model;

using System;
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
        private static PianoSoundPlayer _player { get; set; }
        public static Dictionary<Key, FadingAudio> currentPlayingAudio = new();

        public MainWindow()
        {
            InitializeComponent();
            _ = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
            PianoController.CreatePiano();
            _player = new("../../../../Controller/Audio/Sounds/Piano/", "", ".wav");

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
            int intValue = (int)e.Key;
            string pressedKey;
            Key key = e.Key;
            if (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift))
            {
                pressedKey = e.Key.ToString().ToUpper();
                GetPressedPianoKey(key, intValue, pressedKey);
            }
            else
            {
                pressedKey = e.Key.ToString().ToLower();
                GetPressedPianoKey(key, intValue, pressedKey);
            }
        }

        public static void GetPressedPianoKey(Key e, int intValue, string PressedKey)
        {
            foreach (var key in PianoController.Piano.PianoKeys)
            {
                if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(PressedKey))
                {
                    key.PressedDown = true;
                    if (!currentPlayingAudio.ContainsKey(e))
                    {
                        FadingAudio? fadingAudio = new FadingAudio();
                        fadingAudio = _player.GetFadingAudio(key.Note, (int)key.Octave);

                        if (fadingAudio != null)
                        {
                            fadingAudio.StartPlaying();
                            currentPlayingAudio.Add(e, fadingAudio);
                        }
                    }
                }
            }
        }

        public void KeyReleased(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;

            foreach (var key in PianoController.Piano.PianoKeys)
            {
                if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString().ToLower()))
                {
                    key.PressedDown = false;
                    if (currentPlayingAudio.ContainsKey(e.Key))
                    {
                        currentPlayingAudio[e.Key].StopPlaying(50);
                        currentPlayingAudio.Remove(e.Key);
                    }
                }
            }
        }
    }
}
