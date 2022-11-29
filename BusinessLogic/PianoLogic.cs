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

			//Keyboard binds
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.C, MicrosoftKeybinds.K0));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.CSharp, MicrosoftKeybinds.K1));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.D, MicrosoftKeybinds.K2));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.DSharp, MicrosoftKeybinds.K3));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.E, MicrosoftKeybinds.K4));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.F, MicrosoftKeybinds.K5));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.FSharp, MicrosoftKeybinds.K6));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.G, MicrosoftKeybinds.K7));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.GSharp, MicrosoftKeybinds.K8));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.A, MicrosoftKeybinds.K9));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.ASharp, MicrosoftKeybinds.K10));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.B, MicrosoftKeybinds.K11));

			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.C, MicrosoftKeybinds.K12));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.CSharp, MicrosoftKeybinds.K13));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.D, MicrosoftKeybinds.K14));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.DSharp, MicrosoftKeybinds.K15));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.E, MicrosoftKeybinds.K16));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.F, MicrosoftKeybinds.K17));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.FSharp, MicrosoftKeybinds.K18));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.G, MicrosoftKeybinds.K19));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.GSharp, MicrosoftKeybinds.K20));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.A, MicrosoftKeybinds.K21));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.ASharp, MicrosoftKeybinds.K22));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.B, MicrosoftKeybinds.K23));

			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.C, MicrosoftKeybinds.K24));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.CSharp, MicrosoftKeybinds.K25));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.D, MicrosoftKeybinds.K26));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.DSharp, MicrosoftKeybinds.K27));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.E, MicrosoftKeybinds.K28));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.F, MicrosoftKeybinds.K29));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.FSharp, MicrosoftKeybinds.K30));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.G, MicrosoftKeybinds.K31));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.GSharp, MicrosoftKeybinds.K32));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.A, MicrosoftKeybinds.K33));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.ASharp, MicrosoftKeybinds.K34));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.B, MicrosoftKeybinds.K35));

			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.C, MicrosoftKeybinds.K36));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.CSharp, MicrosoftKeybinds.K37));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.D, MicrosoftKeybinds.K38));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.DSharp, MicrosoftKeybinds.K39));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.E, MicrosoftKeybinds.K40));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.F, MicrosoftKeybinds.K41));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.FSharp, MicrosoftKeybinds.K42));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.G, MicrosoftKeybinds.K43));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.GSharp, MicrosoftKeybinds.K44));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.A, MicrosoftKeybinds.K45));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.ASharp, MicrosoftKeybinds.K46));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.B, MicrosoftKeybinds.K47));
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