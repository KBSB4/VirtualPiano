using BusinessLogic.SoundPlayer;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.MusicTheory;
using Model;
using Octave = Model.Octave;

namespace BusinessLogic
{
    public static class PianoLogic
    {
        public static Piano? Piano { get; set; }
        public static PianoSoundPlayer? SoundPlayer { get; set; }
        public static float Volume { get; set; }

        //Used to play multiple keys at once, also tracks the playing keys
        public static Dictionary<PianoKey, FadingAudio> CurrentPlayingAudio { get => currentPlayingAudio; set => currentPlayingAudio = value; }
        private static Dictionary<PianoKey, FadingAudio> currentPlayingAudio = new();

        private const int AmountOfKeys = 24;

        /// <summary>
        /// Creates the piano and soundplayer for the application
        /// </summary>
        /// <returns></returns>
        public static void CreatePiano()
        {
            Piano = new Piano();
            SoundPlayer = new("", ".wav");
            AssembleKeyBindings(Piano);
        }

        /// <summary>
        /// Creates the PianoKeys for the piano
        /// </summary>
        /// <param name="octave"></param>
        /// <param name="note"></param>
        /// <param name="keyBind"></param>
        /// <returns></returns>
        public static PianoKey CreateKey(Octave octave, NoteName note, KeyBind keyBind)
        {
            return new PianoKey(octave, note, keyBind);
        }

        /// <summary>
        /// Creates the PianoKeys for the piano
        /// </summary>
        /// <param name="octave"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static PianoKey CreateKey(Octave octave, NoteName note)
        {
            return new PianoKey(octave, note);
        }

        /// <summary>
        /// Swap octave to higher or lower to keep amount of keybindings low
        /// </summary>
        public static void SwapOctave(Piano piano)
        {
            for (int i = 0; i < AmountOfKeys; i++)
            {
                PianoKey key = piano.PianoKeys[i];
                if (piano.lowerOctaveActive) key.Octave += 2;
                else key.Octave -= 2;
            }

            piano.lowerOctaveActive = !piano.lowerOctaveActive;
        }

        /// <summary>
        /// Stops the sound of <paramref name="key"/> in <see cref="CurrentPlayingAudio"/>
        /// </summary>
        /// <param name="key"></param>
        public static void StopPianoSound(PianoKey key)
        {
            if (CurrentPlayingAudio.ContainsKey(key))
            {
                CurrentPlayingAudio[key].StopPlaying(50);
                CurrentPlayingAudio.Remove(key);
            }
        }

        /// <summary>
        /// Returns the key that has been pressed, <paramref name="value"/> is the keybind that has been pressed
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PianoKey? GetPressedPianoKey(int value)
        {
            if (Piano is not null)
            {
                foreach (PianoKey? key in Piano.PianoKeys.Where(key => key.KeyBind is not null && (int)key.KeyBind == value))
                {
                    key.PressedDown = true;
                    return key;
                }
            }
            return null;
        }

        /// <summary>
        /// Used for the MIDI-keyboard. Translates input into <see cref="PianoKey"/>
        /// </summary>
        /// <param name="midiEvent"></param>
        /// <returns></returns>
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

            if (Piano is null) return null;

            PianoKey? key = Piano.PianoKeys.Find(x => ((int)x.Octave == octave) && ((int)x.Note == noteIndex));
            if (key is not null)
            {
                key.PressedDown = pressed;
            }

            return key;
        }

        /// <summary>
        /// Plays pianosound using <paramref name="key"/>
        /// </summary>
        /// <param name="key"></param>
        public static void PlayPianoSound(PianoKey key)
        {
            if (!CurrentPlayingAudio.ContainsKey(key))
            {
                FadingAudio? fadingAudio = SoundPlayer?.GetFadingAudio(key.Note, (int)key.Octave, Volume);

                if (fadingAudio is not null)
                {
                    fadingAudio.StartPlaying();
                    CurrentPlayingAudio.Add(key, fadingAudio);
                }
            }
        }

        /// <summary>
        /// Uses <paramref name="intValue"/> to set the <see cref="PianoKey.PressedDown"/> to false stops audio
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns><see cref="PianoKey"/> released</returns>
        public static PianoKey? GetReleasedKey(int intValue)
        {
            if (Piano is not null)
            {
                foreach (PianoKey key in Piano.PianoKeys)
                {
                    if (key.KeyBind is not null && (int)key.KeyBind == intValue)
                    {
                        key.PressedDown = false;
                        return key;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Hardcoded PianoKeys with keybinding as default
        /// </summary>
        /// <param name="piano"></param>
        private static void AssembleKeyBindings(Piano piano)
        {
            //PC-Keyboard bindings
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.C, KeyBind.Z));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.CSharp, KeyBind.S));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.D, KeyBind.X));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.DSharp, KeyBind.D));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.E, KeyBind.C));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.F, KeyBind.V));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.FSharp, KeyBind.G));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.G, KeyBind.B));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.GSharp, KeyBind.H));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.A, KeyBind.N));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.ASharp, KeyBind.J));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.B, KeyBind.M));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.C, KeyBind.Q));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.CSharp, KeyBind.D2));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.D, KeyBind.W));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.DSharp, KeyBind.D3));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.E, KeyBind.E));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.F, KeyBind.R));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.FSharp, KeyBind.D5));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.G, KeyBind.T));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.GSharp, KeyBind.D6));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.A, KeyBind.Y));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.ASharp, KeyBind.D7));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.B, KeyBind.U));

            //Midi-Keyboard bindings
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.C));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.CSharp));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.D));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.DSharp));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.E));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.F));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.FSharp));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.G));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.GSharp));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.A));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.ASharp));
            piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.B));

            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.C));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.CSharp));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.D));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.DSharp));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.E));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.F));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.FSharp));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.G));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.GSharp));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.A));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.ASharp));
            piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.B));
        }
    }
}