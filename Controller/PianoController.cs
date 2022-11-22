using Model;
using VirtualPiano.PianoSoundPlayer;

namespace Controller
{
    public class PianoController
    {
        public static Piano Piano { get; set; }
        private static PianoSoundPlayer _player { get; set; }
        private static Dictionary<PianoKey, FadingAudio> currentPlayingAudio = new();

        /// <summary>
        /// Creates the piano for the program
        /// </summary>
        /// <returns>Piano object</returns>
        public static Piano CreatePiano()
        {
            Piano piano = new Piano();
            _player = new("../../../../Controller/Audio/Sounds/Piano/", "", ".wav");

            return piano;
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
            if (!currentPlayingAudio.ContainsKey(pianokey) && pianokey is not null)
            {
                FadingAudio? fadingAudio = new FadingAudio();
                if (fadingAudio != null)
                {
                    fadingAudio.StartPlaying();
                    currentPlayingAudio.Add(pianokey, fadingAudio);
                }
            }
        }
    }
}