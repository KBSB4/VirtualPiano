using BusinessLogic;
using Melanchall.DryWetMidi.Core;
using Model;

namespace Controller
{
	public static class MidiController
	{


		/// <summary>
		/// Read MIDI File using <paramref name="midiPath"/> and loads the song in the <see cref="SongController"/>.
		/// </summary>
		/// <param name="midiPath"></param>
		public static void OpenMidi(string midiPath)
		{
			bool fileFound = false;
			try
			{
				MidiLogic.currentMidi = MidiFile.Read(midiPath);
				fileFound = true;
			}
			catch { }
			if (fileFound) 
				SongController.LoadSong();
		}

		/// <summary>
		/// Convert MIDI to Song and its notes to PianoKeys
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static Song Convert(MidiFile file)
		{
			return MidiLogic.ConvertMidiFile(file);
		}

		public static MidiFile RemovePianoNotes(MidiFile file)
		{
			return MidiLogic.RemovePianoNotes(file);
		}

		public static MidiFile AddStartTune(MidiFile midiFile)
		{
			return MidiLogic.AddStartTune(midiFile);
		}

		public static MidiFile GetMidiFile()
		{
			return MidiLogic.currentMidi;
		}
	}
}