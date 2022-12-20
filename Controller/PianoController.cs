using BusinessLogic;
using Melanchall.DryWetMidi.Core;
using Model;

namespace Controller
{
	public static class PianoController
	{
		public static Piano? Piano
		{
			get
			{
				return PianoLogic.Piano;
			}
			set
			{
				PianoLogic.Piano = value;
			}
		}

		/// <summary>
		/// Creates the piano and soundplayer for the program
		/// </summary>
		/// <returns></returns>
		public static void CreatePiano()
		{
			PianoLogic.CreatePiano();
		}

		/// <summary>
		/// Figures out which key is pressed and set it to true + play audio
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Piano key pressed</returns>
		public static PianoKey? GetPressedPianoKey(int value)
		{
			return PianoLogic.GetPressedPianoKey(value);
		}

		/// <summary>
		/// Takes an MIDIevent as input and detects whether the key is pressed and what key this is
		/// </summary>
		/// <param name="midiEvent"></param>
		/// <returns>The resulting pianokey</returns>
		public static PianoKey? ParseMidiNote(MidiEvent midiEvent)
		{
			return PianoLogic.ParseMidiNote(midiEvent);
		}

		/// <summary>
		/// Plays a sound using <paramref name="key"/> to define which <see cref="PianoKey"/> to play
		/// </summary>
		/// <param name="key"></param>
		public static void PlayPianoSound(PianoKey key)
		{
			PianoLogic.PlayPianoSound(key);
		}

		/// <summary>
		/// Figures out which key is pressed and set it to false + stop audio
		/// </summary>
		/// <param name="intValue"></param>
		/// <returns>Pianokey released</returns>
		public static PianoKey? GetReleasedKey(int intValue)
		{
			return PianoLogic.GetReleasedKey(intValue);
		}

		/// <summary>
		/// Stops the pianosound of <paramref name="key"/> 
		/// </summary>
		/// <param name="key"></param>
		public static void StopPianoSound(PianoKey key)
		{
			PianoLogic.StopPianoSound(key);
		}

		public static void SetVolume(float volume)
		{
			PianoLogic.Volume = volume;
		}
	}
}