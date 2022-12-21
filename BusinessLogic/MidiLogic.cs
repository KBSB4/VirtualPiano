using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Model;
using System.Reflection;

namespace BusinessLogic
{
	public static class MidiLogic
	{
		private static TempoMap? tempoMap;
		public static MidiFile? CurrentMidi { get; set; } //Full MIDI

		/// <summary>
		/// Convert MIDI to Song and its notes to PianoKeys
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static Song? ConvertMidiFile(MidiFile file)
		{
			if (file.Chunks.Count == 0) return null;

			MidiFile newFile = AddStartTune(file);
			List<TrackChunk> trackList = newFile.GetTrackChunks().ToList();

			tempoMap = newFile.GetTempoMap();
			Queue<PianoKey> pianoKeyList = new();
			List<Note> notes = (List<Note>)newFile.GetNotes();

			foreach (Note? midiKey in notes)
			{
				PianoKey? pianoKey = ConvertPianoKey(midiKey);
				if (pianoKey is not null)
				{
					if (GetPianoChannel(trackList) == midiKey.Channel)
					{
						pianoKeyList.Enqueue(pianoKey);
					}
				}
			}

			MetricTimeSpan duration = newFile.GetDuration<MetricTimeSpan>();
			return new Song(newFile, "temp", Difficulty.Easy, duration, pianoKeyList);
		}

		/// <summary>
		/// Returns whether the channel is used as a piano.
		/// </summary>
		/// <param name="trackList"></param>
		/// <param name="channel"></param>
		/// <returns></returns>
		private static FourBitNumber GetPianoChannel(List<TrackChunk> trackList)
		{
			FourBitNumber resultChannel = (FourBitNumber)0;
			List<FourBitNumber> programNumbersFound = new();

			foreach (TrackChunk chunk in trackList)
			{
				var programNumbers = chunk
					.Events
					.OfType<ProgramChangeEvent>()
					.Select(e => new { e.ProgramNumber, e.Channel })
					.ToArray();

				//Check if a program change has and is a piano program change
				foreach (var number in programNumbers)
				{
					//See Wikipedia General MIDI - Everything under 8 is Piano
					programNumbersFound.Add(number.Channel);
					if (number.ProgramNumber < 8)
					{
						return number.Channel;
					}
				}
			}

			foreach (FourBitNumber channel in trackList.GetChannels())
			{
				if (!programNumbersFound.Contains(channel))
					return channel;
			}

			return (FourBitNumber)trackList.GetChannels().Count();
		}

		/// <summary>
		/// Convert note to PianoKey for visualisation and plays
		/// </summary>
		/// <param name="midiNote"></param>
		/// <returns></returns>
		private static PianoKey? ConvertPianoKey(Note? midiNote)
		{
			if (midiNote is null)
			{
				return null;
			}
			MetricTimeSpan timeStamp = midiNote.TimeAs<MetricTimeSpan>(tempoMap);
			MetricTimeSpan duration = midiNote.LengthAs<MetricTimeSpan>(tempoMap);

			var noteName = midiNote.NoteName;
			var octave = midiNote.Octave;
			return new PianoKey((Octave)octave, noteName, timeStamp, duration);
		}

		/// <summary>
		/// Adds a pre-made tune to the beginning of the <paramref name="midiFile"/> to sync the song with the practice notes
		/// </summary>
		/// <param name="midiFile"></param>
		/// <returns></returns>
		private static MidiFile AddStartTune(MidiFile midiFile)
		{
			MidiFile midiFileOut = new()
			{
				TimeDivision = midiFile.TimeDivision
			};

			var assembly = Assembly.GetExecutingAssembly();
			var file = assembly.GetManifestResourceStream(ProjectSettings.GetPath(PianoHeroPath.StartTune));
            MidiFile StartTune = MidiFile.Read(file); 


			// Add all parts after shifting them
			long addedSoFarMicroseconds = 0;

			List<MidiFile> lsToWrite = new()
			{
				StartTune,
				midiFile
			};

			foreach (MidiFile midiPart in lsToWrite)
			{
				MetricTimeSpan currentDuration = midiPart.GetDuration<MetricTimeSpan>();
				midiPart.ShiftEvents(new MetricTimeSpan(addedSoFarMicroseconds));
				midiFileOut.Chunks.AddRange(midiPart.Chunks);
				addedSoFarMicroseconds += currentDuration.TotalMicroseconds;
			}

			midiFileOut.Write("current-playing-song.mid", true);

			return midiFileOut;
		}

		/// <summary>
		/// Removes the piano-notes from the <paramref name="file"/>
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static MidiFile RemovePianoNotes(MidiFile file)
		{
			List<TrackChunk> trackList = file.GetTrackChunks().ToList();
			tempoMap = file.GetTempoMap();
			FourBitNumber pianoChannel = GetPianoChannel(trackList);
			file.RemoveNotes(x => x.Channel == pianoChannel);

			return file;
		}
	}
}