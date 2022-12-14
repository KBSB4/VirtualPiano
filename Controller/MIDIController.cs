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

			MetricTimeSpan duration = newFile.GetDuration<MetricTimeSpan>();
			return new Song(newFile, "temp", Difficulty.Easy, duration, pianoKeyList, TempoMap);
		}


        /// <summary>
        /// Illiterates through the trackChuncks of the MidiFile and removes the pianokeys. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static MidiFile RemovePiano(MidiFile file)
		{
			var trackList = file.GetTrackChunks().ToList();
			file.RemoveNotes(x => x.Channel == GetPianoChannel(trackList));
			return file;
		}

		public static MidiFile AddStartTune(MidiFile midiFile)
		{
			return MidiLogic.AddStartTune(midiFile);
		}


		/// <summary>
		///  Appends the generic startTune to the midiFile
		/// </summary>
		/// <param name="midiFile"></param>
		/// <returns></returns>
		public static MidiFile AddStartTune(MidiFile midiFile)
		{
			var fileNameOut = "testName.mid";
			var midiFileOut = new MidiFile()
			{
				TimeDivision = midiFile.TimeDivision // copied from master file
			};

			MidiFile StartTune = MidiFile.Read("..\\..\\..\\..\\Controller\\PianoSoundPlayer\\Sounds\\startTune.mid");
			// Add all parts after shifting them
			long addedSoFarMicroseconds = 0;

			List<MidiFile> lsToWrite = new()
			{
				StartTune,
				midiFile
			};

			foreach (var midiPart in lsToWrite) // lsToWrite is a list of MidiFile objects
			{
				var currentDuration = midiPart.GetDuration<MetricTimeSpan>();
				midiPart.ShiftEvents(new MetricTimeSpan(addedSoFarMicroseconds));
				midiFileOut.Chunks.AddRange(midiPart.Chunks);
				addedSoFarMicroseconds += currentDuration.TotalMicroseconds;
			}
			midiFileOut.Write(fileNameOut, true);

			return midiFileOut;
		}
	}
}