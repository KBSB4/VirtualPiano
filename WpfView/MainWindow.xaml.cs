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
        public Piano Piano { get; set; }

        private PianoSoundPlayer PianoSoundPlayer { get; set; }
        private Dictionary<Key, FadingAudio> currentPlayingAudio = new();

        public MainWindow()
        {
            InitializeComponent();
            _ = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);

            //Create piano
            Piano = PianoController.CreatePiano();
            PianoSoundPlayer = new("../../../../Controller/Audio/Sounds/Piano/", "", ".wav");

            //Add keydown event for the keys
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;
        }

        public void KeyPressed(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;
            if (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift))
            {
                foreach (var key in Piano.PianoKeys)
                {
                    if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString().ToUpper()))
                    {
                        key.PressedDown = true;
                        if (!currentPlayingAudio.ContainsKey(e.Key))
                        {
                            FadingAudio? fadingAudio = new FadingAudio();
                            fadingAudio = PianoSoundPlayer.GetFadingAudio(key.Note, (int)key.Octave);

                            if (fadingAudio != null)
                            {
                                fadingAudio.StartPlaying();
                                currentPlayingAudio.Add(e.Key, fadingAudio);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var key in Piano.PianoKeys)
                {
                    if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString().ToLower()))
                    {
                        key.PressedDown = true;
                        if (!currentPlayingAudio.ContainsKey(e.Key))
                        {
                            FadingAudio? fadingAudio = new FadingAudio();
                            fadingAudio = PianoSoundPlayer.GetFadingAudio(key.Note, (int)key.Octave);

                            if (fadingAudio != null)
                            {
                                fadingAudio.StartPlaying();
                                currentPlayingAudio.Add(e.Key, fadingAudio);
                            }
                        }
                    }
                }
            }
        }


        public void KeyReleased(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;


            foreach (var key in Piano.PianoKeys)
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

        //public void KeyReleased(object source, KeyEventArgs e)
        //{
        //    int intValue;
        //    string keyValue;
        //    GetKeyWithShift(e, out intValue, out keyValue);
        //    UpdateKey(e, intValue, false);

        //    if (currentPlayingAudio.ContainsKey(e.Key))
        //    {
        //        currentPlayingAudio[e.Key].StopPlaying(50);
        //        currentPlayingAudio.Remove(e.Key);
        //    }
        //}

        private void UpdateKey(KeyEventArgs e, int intValue, Boolean PressDown)
        {
            foreach (var key in Piano.PianoKeys)
            {
                if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(e.Key.ToString()))
                {
                    key.PressedDown = PressDown;

                    //Speel noot af
                    if (!currentPlayingAudio.ContainsKey(e.Key))
                    {
                        FadingAudio? fadingAudio = new FadingAudio();
                        fadingAudio = PianoSoundPlayer.GetFadingAudio(key.Note, (int)key.Octave);

                        if (fadingAudio != null)
                        {
                            fadingAudio.StartPlaying();
                            currentPlayingAudio.Add(e.Key, fadingAudio);
                        }
                    }
                }
            }
        }

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
