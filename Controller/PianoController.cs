using Model;
using VirtualPiano.PianoSoundPlayer;

namespace Controller
{
    public class PianoController
    {
        public static Piano Piano { get; set; }
        private static PianoSoundPlayer _player { get; set; }
        public static Dictionary<PianoKey, FadingAudio> currentPlayingAudio = new();

        /// <summary>
        /// Creates the piano for the program
        /// </summary>
        /// <returns>Piano object</returns>
        public static void CreatePiano()
        {
            Piano = new Piano();
            _player = new("../../../../Controller/Audio/Sounds/Piano/", "", ".wav");
        }

        /// <summary>
        /// Updates key and returns the key if it gets updated
        /// </summary>
        /// <param name="pianoKey"></param>
        /// <param name="intValue"></param>
        /// <param name="PressDown"></param>
        /// <returns>The PianoKey with the right keybind</returns>
        public static PianoKey UpdateKey(string pianoKey, int intValue, Boolean PressDown)
        {
            foreach (var key in Piano.PianoKeys)
            {
                if (key.MicrosoftBind == intValue && key.KeyBindChar.ToString().Equals(pianoKey))
                {
                    key.PressedDown = PressDown;
                    return key;
                }
            }
            return null;
        }

        /// <summary>
        /// Plays the note associated with the PianoKey
        /// </summary>
        /// <param name="pianokey"></param>
        public static void PlayKey(PianoKey pianokey)
        {
            if (pianokey is not null)
            {
                if (!currentPlayingAudio.ContainsKey(pianokey))
                {
                    FadingAudio? fadingAudio = new FadingAudio();
                    fadingAudio = _player.GetFadingAudio(pianokey.Note, (int)pianokey.Octave);

                    if (fadingAudio != null)
                    {
                        fadingAudio.StartPlaying();
                        currentPlayingAudio.Add(pianokey, fadingAudio);
                    }
                }
            }
        }
    }
}