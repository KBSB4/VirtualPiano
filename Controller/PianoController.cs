using Model;
using VirtualPiano.PianoSoundPlayer;

namespace Controller
{
    public class PianoController
    {
        public static Piano Piano { get; set; }
        public static PianoSoundPlayer _player { get; set; }

        //Used to play multiple keys at once, also tracks the playing keys
        public static Dictionary<string, FadingAudio> currentPlayingAudio = new();

        /// <summary>
        /// Creates the piano and soundplayer for the program
        /// </summary>
        /// <returns>Piano object</returns>
        public static void CreatePiano()
        {
            Piano = new Piano();
            _player = new("../../../../Controller/Audio/Sounds/Piano/", "", ".wav");
        }

        /// <summary>
        /// Set PressedDown for the pianokey to true and play the key
        /// </summary>
        /// <param name="KeybordKey"></param>
        /// <param name="intValue"></param>
        /// <param name="PressedKey"></param>
        public static void GetPressedPianoKey(string KeybordKey, int intValue, string PressedKey)
        {
            foreach (var key in PianoController.Piano.PianoKeys)
            {
                if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(PressedKey))
                {
                    key.PressedDown = true;
                    //Play 
                    if (!PianoController.currentPlayingAudio.ContainsKey(KeybordKey))
                    {
                        FadingAudio fadingAudio = PianoController._player.GetFadingAudio(key.Note, (int)key.Octave);

                        if (fadingAudio != null)
                        {
                            fadingAudio.StartPlaying();
                            PianoController.currentPlayingAudio.Add(KeybordKey, fadingAudio);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set PressedDown for the pianokey to false and stop playing the key
        /// </summary>
        /// <param name="intValue"></param>
        /// <param name="pressedKey"></param>
        public static void ReleaseKeyStopAudio(int intValue, string pressedKey)
        {
            foreach (var key in PianoController.Piano.PianoKeys)
            {
                if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(pressedKey.ToLower()))
                {
                    key.PressedDown = false;
                    //Stop playing
                    if (PianoController.currentPlayingAudio.ContainsKey(pressedKey))
                    {
                        PianoController.currentPlayingAudio[pressedKey].StopPlaying(50);
                        PianoController.currentPlayingAudio.Remove(pressedKey);
                    }
                }
            }
        }
    }
}