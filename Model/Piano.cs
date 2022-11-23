using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{
	public class Piano
	{
		//Sets which octave is active. Lower or upper
		private bool lowerOctaveActive = true;
		public List<PianoKey> PianoKeys { get; set; }

		/// <summary>
		/// Constructor for piano that creates 50 keys for itself.
		/// </summary>
		public Piano()
		{
			PianoKeys = new();
			AssembleKeyBindings();
		}

		//TODO Naar controller?
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
		public PianoKey CreateKey(Octave octave, NoteName note, MicrosoftKeybinds keyBind)
		{
			PianoKey key = new PianoKey(octave, note, keyBind);
			return key;
		}

		/// <summary>
		/// Hardcoded PianoKeys with keybinding
		/// </summary>
		public void AssembleKeyBindings()
		{
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
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.B, MicrosoftKeybind.M));
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

		/// <summary>
		/// Swap octave to higher or lower to keep amount of keybindings low
		/// </summary>
		public void SwapOctave()
		{
			foreach (PianoKey key in PianoKeys)
			{
				if (lowerOctaveActive) key.Octave += 2;
				else key.Octave -= 2;
			}
			lowerOctaveActive = !lowerOctaveActive;
		}
	}
}