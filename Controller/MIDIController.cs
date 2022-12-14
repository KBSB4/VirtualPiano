using BusinessLogic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
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

			//MetricTimeSpan duration = newFile.GetDuration<MetricTimeSpan>();
			//return new Song(newFile, "temp", Difficulty.Easy, duration, pianoKeyList, TempoMap);
		}


        /// <summary>
        /// Illiterates through the trackChuncks of the MidiFile and removes the pianokeys. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static MidiFile RemovePiano(MidiFile file)
		{
			return MidiLogic.RemovePianoNotes(file);
		}

        /// <summary>
        ///  Appends the generic startTune to the midiFile
        /// </summary>
        /// <param name="midiFile"></param>
        /// <returns></returns>
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