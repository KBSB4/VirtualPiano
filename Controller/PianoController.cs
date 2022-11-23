using Model;
using VirtualPiano.PianoSoundPlayer;

namespace Controller
{
    public class PianoController
    {
        public static Piano Piano { get; set; }
        public static PianoSoundPlayer _player { get; set; }

        //Used to play multiple keys at once, also tracks the playing keys
        public static Dictionary<PianoKey, FadingAudio> currentPlayingAudio = new();

        /// <summary>
        /// Creates the piano and soundplayer for the program
        /// </summary>
        /// <returns>Piano object</returns>
        public static void CreatePiano()
        {
            Piano = new Piano();
            _player = new("../../../../Controller/PianoSoundPlayer/Sounds/Piano/", "", ".wav");
        }

        /// <summary>
        /// Figures out which key is pressed and set it to true + play audio
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns>Piano key pressed</returns>
        public static PianoKey? GetPressedPianoKey(int intValue)
        {
            foreach (var key in Piano.PianoKeys)
            {
                if ((int)key.MicrosoftBind == intValue)
                {
                    key.PressedDown = true;
                    //Play 
                    if (!currentPlayingAudio.ContainsKey(key))
                    {
                        FadingAudio fadingAudio = _player.GetFadingAudio(key.Note, (int)key.Octave);

                        if (fadingAudio != null)
                        {
                            fadingAudio.StartPlaying();
							currentPlayingAudio.Add(key, fadingAudio);
                        }
					}
					return key;
				}
            }
            return null;
        }

        /// <summary>
        /// Figures out which key is pressed and set it to false + stop audio
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns>Pianokey released</returns>
        public static PianoKey? GetReleasedKey(int intValue)
        {
            foreach (var key in Piano.PianoKeys)
            {
                if ((int)key.MicrosoftBind == intValue)
                {
                    key.PressedDown = false;
                    //Stop playing
                    if (currentPlayingAudio.ContainsKey(key))
                    {
						currentPlayingAudio[key].StopPlaying(50);
						currentPlayingAudio.Remove(key);
                    }
                    return key;
                }
            }
            return null;
        }
    }
}