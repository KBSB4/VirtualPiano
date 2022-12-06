﻿using Melanchall.DryWetMidi.MusicTheory;
using Model;
using Octave = Model.Octave;

namespace BusinessLogic
{
    public class PianoLogic
    {
        /// <summary>
        /// Creates the PianoKeys for the piano
        /// </summary>
        /// <param name="octave"></param>
        /// <param name="note"></param>
        /// <param name="getal"></param>
        public static PianoKey CreateKey(Octave octave, NoteName note, KeyBind keyBind)
        {
            PianoKey key = new(octave, note, keyBind);
            return key;
        }

        /// <summary>
        /// Hardcoded PianoKeys with keybinding as default
        /// </summary>
        public static void AssembleKeyBindings(Piano piano)
        {
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

			//Keyboard binds
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.C, KeyBind.K0));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.CSharp, KeyBind.K1));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.D, KeyBind.K2));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.DSharp, KeyBind.K3));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.E, KeyBind.K4));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.F, KeyBind.K5));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.FSharp, KeyBind.K6));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.G, KeyBind.K7));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.GSharp, KeyBind.K8));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.A, KeyBind.K9));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.ASharp, KeyBind.K10));
			piano.PianoKeys.Add(CreateKey(Octave.Two, NoteName.B, KeyBind.K11));

			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.C, KeyBind.K12));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.CSharp, KeyBind.K13));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.D, KeyBind.K14));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.DSharp, KeyBind.K15));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.E, KeyBind.K16));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.F, KeyBind.K17));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.FSharp, KeyBind.K18));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.G, KeyBind.K19));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.GSharp, KeyBind.K20));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.A, KeyBind.K21));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.ASharp, KeyBind.K22));
			piano.PianoKeys.Add(CreateKey(Octave.Three, NoteName.B, KeyBind.K23));

			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.C, KeyBind.K24));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.CSharp, KeyBind.K25));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.D, KeyBind.K26));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.DSharp, KeyBind.K27));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.E, KeyBind.K28));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.F, KeyBind.K29));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.FSharp, KeyBind.K30));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.G, KeyBind.K31));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.GSharp, KeyBind.K32));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.A, KeyBind.K33));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.ASharp, KeyBind.K34));
			piano.PianoKeys.Add(CreateKey(Octave.Four, NoteName.B, KeyBind.K35));

			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.C, KeyBind.K36));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.CSharp, KeyBind.K37));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.D, KeyBind.K38));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.DSharp, KeyBind.K39));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.E, KeyBind.K40));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.F, KeyBind.K41));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.FSharp, KeyBind.K42));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.G, KeyBind.K43));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.GSharp, KeyBind.K44));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.A, KeyBind.K45));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.ASharp, KeyBind.K46));
			piano.PianoKeys.Add(CreateKey(Octave.Five, NoteName.B, KeyBind.K47));
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