using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{
	public class Piano
	{
		private bool lowerOctaveActive = true;

		public enum MicrosoftKeybind
		{
			D1 = 35,
			D2 = 36,
			D3 = 37,
			D4 = 38,
			D5 = 39,
			D6 = 40,
			D7 = 41,
			D8 = 42,
			D9 = 43,
			D0 = 44,
			Q = 60, 
			W = 66, 
			E = 48, 
			R = 61, 
			T = 63, 
			Y = 68, 
			U = 64, 
			I = 52, 
			O = 58, 
			P = 59, 
			A = 44, 
			S = 62, 
			D = 47, 
			F = 49, 
			G = 50, 
			H = 51, 
			J = 53, 
			K = 54, 
			L = 55, 
			Z = 69, 
			X = 67, 
			C = 46, 
			V = 65, 
			B = 45, 
			N = 57, 
			M = 56
		}

		//private int[] MicrosoftKeybinds = { 60, 66, 48, 61, 63, 68, 64, 52, 58, 59, 44, 62, 47, 49, 50, 51, 53, 54, 55, 69, 67, 46, 65, 45, 57, 56 };
		public List<PianoKey> PianoKeys { get; set; }

		/// <summary>
		/// Constructor for piano that creates 50 keys for itself.
		/// </summary>
		public Piano()
		{
			PianoKeys = new();
			AssembleKeyBindings();
		}

		//TODO Naar Pianocontroller?
		/// <summary>
		/// Sets octave and note for each key
		/// </summary>
		/// <param name="currentNote"></param>
		/// <param name="currentOctave"></param>
		public void UpdateOctaveAndNote(ref NoteName currentNote, ref Octave currentOctave)
		{
			if (currentNote.Equals(NoteName.B))
			{
				currentNote = NoteName.C;
				currentOctave++;
			}
		}

		/// <summary>
		/// Creates the PianoKeys for the piano
		/// </summary>
		/// <param name="octave"></param>
		/// <param name="note"></param>
		/// <param name="getal"></param>
		public PianoKey CreateKey(Octave octave, NoteName note, MicrosoftKeybind keyBind)
		{
			PianoKey key = new PianoKey(octave, note, keyBind);
			return key;
		}

		/// <summary>
		/// Sets keybindings for each key
		/// </summary>
		public void AssembleKeyBindings()
		{
			Octave currentoctave = 0;  // first octave two
			NoteName currentnote = NoteName.C; // first key of the virtual keyboard

			PianoKeys.Add(CreateKey(Octave.Two, NoteName.C, MicrosoftKeybind.Z));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.CSharp, MicrosoftKeybind.S));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.D, MicrosoftKeybind.X));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.DSharp, MicrosoftKeybind.D));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.E, MicrosoftKeybind.C));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.F, MicrosoftKeybind.V));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.FSharp, MicrosoftKeybind.G));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.G, MicrosoftKeybind.B));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.GSharp, MicrosoftKeybind.H));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.A, MicrosoftKeybind.N));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.ASharp, MicrosoftKeybind.J));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.C, MicrosoftKeybind.Q));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.CSharp, MicrosoftKeybind.D2));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.D, MicrosoftKeybind.W));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.DSharp, MicrosoftKeybind.D3));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.E, MicrosoftKeybind.E));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.F, MicrosoftKeybind.R));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.FSharp, MicrosoftKeybind.D5));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.G, MicrosoftKeybind.T));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.GSharp, MicrosoftKeybind.D6));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.A, MicrosoftKeybind.Y));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.ASharp, MicrosoftKeybind.D7));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.B, MicrosoftKeybind.U));
		}

		public void SwapOctave()
		{
			foreach(PianoKey key in PianoKeys)
			{
				if (lowerOctaveActive) key.Octave += 2;
				else key.Octave -= 2;
			}
			lowerOctaveActive = !lowerOctaveActive;
		}
	}
}