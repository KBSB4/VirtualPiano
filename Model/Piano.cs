using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{
	public class Piano
	{
		//Sets which octave is active. Lower or upper
		public bool lowerOctaveActive = true;
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
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.C, MicrosoftKeybinds.Z));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.CSharp, MicrosoftKeybinds.S));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.D, MicrosoftKeybinds.X));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.DSharp, MicrosoftKeybinds.D));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.E, MicrosoftKeybinds.C));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.F, MicrosoftKeybinds.V));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.FSharp, MicrosoftKeybinds.G));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.G, MicrosoftKeybinds.B));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.GSharp, MicrosoftKeybinds.H));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.A, MicrosoftKeybinds.N));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.ASharp, MicrosoftKeybinds.J));
			PianoKeys.Add(CreateKey(Octave.Two, NoteName.B, MicrosoftKeybinds.M));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.C, MicrosoftKeybinds.Q));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.CSharp, MicrosoftKeybinds.D2));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.D, MicrosoftKeybinds.W));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.DSharp, MicrosoftKeybinds.D3));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.E, MicrosoftKeybinds.E));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.F, MicrosoftKeybinds.R));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.FSharp, MicrosoftKeybinds.D5));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.G, MicrosoftKeybinds.T));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.GSharp, MicrosoftKeybinds.D6));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.A, MicrosoftKeybinds.Y));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.ASharp, MicrosoftKeybinds.D7));
			PianoKeys.Add(CreateKey(Octave.Three, NoteName.B, MicrosoftKeybinds.U));
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