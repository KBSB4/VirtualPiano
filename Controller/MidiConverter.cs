using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using Model;
using Note = Melanchall.DryWetMidi.Interaction.Note;
using Octave = Model.Octave;

namespace Controller
{
	public static class MidiConverter
	{
		private static TempoMap TempoMap;
		public static Song Convert(MidiFile file)
		{
			var trackList = file.GetTrackChunks().ToList();
			TempoMap = file.GetTempoMap();
			Queue<PianoKey> pianoKeyList = new();
			foreach (Note? midiKey in file.GetNotes())
			{
				PianoKey? pianoKey = ConvertPianoKey(midiKey);
				if (pianoKey is not null)
				{
					//if (IsPianoChannel(trackList, midiKey.Channel))
					//{
						pianoKeyList.Enqueue(pianoKey);
					//}
				}
			}

			MetricTimeSpan duration = file.GetDuration<MetricTimeSpan>();
			return new Song(file, "temp", Difficulty.Easy, duration, pianoKeyList);
		}

		private static bool IsPianoChannel(List<TrackChunk> trackList, FourBitNumber channel)
		{
			foreach (TrackChunk chunk in trackList)
			{
				var programNumbers = chunk
					.Events
					.OfType<ProgramChangeEvent>()
					.Select(e => new { e.ProgramNumber, e.Channel })
					.ToArray();
					foreach (var test in programNumbers) {
						if (test.Channel == channel) 
							if(test.ProgramNumber < 8) 
								return true;
					}
			}
			return false;
		}

		private static PianoKey? ConvertPianoKey(Note? midiNote)
		{
			if (midiNote is null)
			{
				return null;
			}
			MetricTimeSpan timeStamp = midiNote.TimeAs<MetricTimeSpan>(TempoMap);
			MetricTimeSpan duration = midiNote.LengthAs<MetricTimeSpan>(TempoMap);

			var noteName = midiNote.NoteName;
			var octave = midiNote.Octave;
			return new PianoKey((Octave)octave, noteName, timeStamp, duration);
		}
	}
}
