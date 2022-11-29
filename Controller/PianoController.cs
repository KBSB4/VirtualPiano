using BusinessLogic;
using Melanchall.DryWetMidi.Core;
using Model;
using VirtualPiano.PianoSoundPlayer;

namespace Controller
{
    public static class PianoController
    {
        public static Piano Piano { get; set; }
        public static PianoSoundPlayer SoundPlayer { get; set; }

        //Used to play multiple keys at once, also tracks the playing keys
        public static Dictionary<PianoKey, FadingAudio> currentPlayingAudio = new();

        /// <summary>
        /// Creates the piano and soundplayer for the program
        /// </summary>
        /// <returns>Piano object</returns>
        public static void CreatePiano()
        {
            Piano = new Piano();
            SoundPlayer = new("../../../../Controller/PianoSoundPlayer/Sounds/Piano/", "", ".wav");
            PianoLogic.AssembleKeyBindings(Piano);
        }

        #region Piano Creation

        //TODO Hier alle functies van Piano in model?

        #endregion

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
                    return key;
                }
            }
            return null;
        }

        public static PianoKey? ParseMidiNote(MidiEvent midiEvent)
        {
            //TODO if notenumber is not 2 characters
            int number = int.Parse(midiEvent.ToString().Substring(13, 2));
            //Debug.WriteLine(midiEvent);
            bool pressed = int.Parse(midiEvent.ToString().Substring(17, 1)) != 0;

            if (midiEvent.EventType == MidiEventType.NoteOff)
            {
                pressed = false;
            }

            int octave = (number / 12) - 1;
            int noteIndex = (number % 12);

            PianoKey? key = Piano.PianoKeys.Find(x => (int)x.Note == noteIndex && (int)x.Octave == octave);
            if (key is not null)
            {
                key.PressedDown = pressed;
            }
            return key;
        }

        public static void PlayPianoSound(PianoKey key)
        {
            if (!currentPlayingAudio.ContainsKey(key))
            {
                FadingAudio fadingAudio = SoundPlayer.GetFadingAudio(key.Note, (int)key.Octave);

                if (fadingAudio != null)
                {
                    fadingAudio.StartPlaying();
                    currentPlayingAudio.Add(key, fadingAudio);
                }
            }
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
                    return key;
                }
            }
            return null;
        }

        public static void StopPianoSound(PianoKey key)
        {
            if (currentPlayingAudio.ContainsKey(key))
            {
                currentPlayingAudio[key].StopPlaying(50);
                currentPlayingAudio.Remove(key);
            }
        }
    }
}