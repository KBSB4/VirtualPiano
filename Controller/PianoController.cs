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
        /// <returns></returns>
        public static void CreatePiano()
        {
            Piano = new Piano();
            SoundPlayer = new("../../../../Controller/PianoSoundPlayer/Sounds/Piano/", "", ".wav");
            PianoLogic.AssembleKeyBindings(Piano);
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
                if ((int)key.KeyBind == intValue)
                {
                    key.PressedDown = true;
                    return key;
                }
            }
            return null;
        }

        /// <summary>
        /// Takes an MIDIevent as input and detects whether the key is pressed and what key this is
        /// </summary>
        /// <param name="midiEvent"></param>
        /// <returns>The resulting pianokey</returns>
        public static PianoKey? ParseMidiNote(MidiEvent midiEvent)
        {
            int number;
            bool pressed;
            if (midiEvent is NoteOnEvent noteOnEvent)
            {
                number = noteOnEvent.NoteNumber;
                pressed = noteOnEvent.Velocity != 0;
            }
            else if (midiEvent is NoteOffEvent noteOffEvent)
            {
                number = noteOffEvent.NoteNumber;
                pressed = false;
            }
            else
            {
                return null;
            }

            int octave = (number / 12) - 1;
            int noteIndex = (number % 12);

            //PianoKey? key = Piano.PianoKeys.Find(x => x.MicrosoftBind == (MicrosoftKeybinds)(99 + octave * noteIndex));
            PianoKey? key = Piano.PianoKeys.Find(x => ((int)x.Octave == octave) && ((int)x.Note == noteIndex));
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
                FadingAudio? fadingAudio = SoundPlayer.GetFadingAudio(key.Note, (int)key.Octave);

                if (fadingAudio is not null)
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
                if ((int)key.KeyBind == intValue)
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