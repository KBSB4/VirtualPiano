using Melanchall.DryWetMidi.MusicTheory;
using Model;
using Octave = Model.Octave;

namespace BusinessLogic
{
    public class PianoLogic
    {
        /// <summary>
        /// Sets octave and note for each key
        /// </summary>
        /// <param name="currentNote"></param>
        /// <param name="currentOctave"></param>
        //public void UpdateOctaveAndNote(ref NoteName currentNote, ref Octave currentOctave)
        //      {
        //          if (currentNote.Equals(NoteName.B))
        //          {
        //              currentNote = NoteName.C;
        //              currentOctave++;
        //          }
        //      }

        /// <summary>
        /// Creates the PianoKeys for the piano
        /// </summary>
        /// <param name="octave"></param>
        /// <param name="note"></param>
        /// <param name="getal"></param>
        public static PianoKey CreateKey(Octave octave, NoteName note, MicrosoftKeybinds keyBind)
        {
            PianoKey key = new PianoKey(octave, note, keyBind);
            return key;
        }

        /// <summary>
        /// Hardcoded PianoKeys with keybinding
        /// </summary>
        public static void AssembleKeyBindings(Piano piano)
        {
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.C, MicrosoftKeybinds.Z));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.CSharp, MicrosoftKeybinds.S));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.D, MicrosoftKeybinds.X));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.DSharp, MicrosoftKeybinds.D));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.E, MicrosoftKeybinds.C));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.F, MicrosoftKeybinds.V));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.FSharp, MicrosoftKeybinds.G));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.G, MicrosoftKeybinds.B));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.GSharp, MicrosoftKeybinds.H));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.A, MicrosoftKeybinds.N));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.ASharp, MicrosoftKeybinds.J));
            piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.B, MicrosoftKeybinds.M));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.C, MicrosoftKeybinds.Q));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.CSharp, MicrosoftKeybinds.D2));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.D, MicrosoftKeybinds.W));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.DSharp, MicrosoftKeybinds.D3));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.E, MicrosoftKeybinds.E));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.F, MicrosoftKeybinds.R));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.FSharp, MicrosoftKeybinds.D5));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.G, MicrosoftKeybinds.T));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.GSharp, MicrosoftKeybinds.D6));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.A, MicrosoftKeybinds.Y));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.ASharp, MicrosoftKeybinds.D7));
            piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.B, MicrosoftKeybinds.U));
        }

        /// <summary>
        /// Swap octave to higher or lower to keep amount of keybindings low
        /// </summary>
        public static void SwapOctave(Piano piano)
        {
            foreach (PianoKey key in piano.PianoKeys)
            {
                if (piano.lowerOctaveActive) key.Octave += 2;
                else key.Octave -= 2;
            }
            piano.lowerOctaveActive = !piano.lowerOctaveActive;
        }
    }
}