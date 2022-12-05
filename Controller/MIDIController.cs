using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using Model;
using Note = Melanchall.DryWetMidi.Interaction.Note;
using Octave = Model.Octave;

namespace Controller
{
	public static class MIDIController
	{
		private static TempoMap TempoMap;
		public static MidiFile OriginalMIDI { get; set; } //Full MIDI
		private static MidiFile MIDI { get; set; } //MIDI without the track in MIDITrackIsolated
		private static MidiFile MIDITrackIsolated { get; set; } //Selfexplanatory, depends on which track we use which should be set in database

		/// <summary>
		/// Read MIDI File and get karoake MIDIs out of it as well.
		/// </summary>
		/// <param name="MIDIpath"></param>
		public static void OpenMidi(string MIDIpath)
		{
			OriginalMIDI = MidiFile.Read(MIDIpath);
			SongController.LoadSong();
		}

		/// <summary>
		/// Convert MIDI to Song and its notes to PianoKeys
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static Song Convert(MidiFile file)
		{
			var trackList = file.GetTrackChunks().ToList();
			TempoMap = file.GetTempoMap();
			Queue<PianoKey> pianoKeyList = new();
			var notes = file.GetNotes();

			foreach (Note? midiKey in notes)
			{
				PianoKey? pianoKey = ConvertPianoKey(midiKey);
				if (pianoKey is not null)
				{
					//Gets the piano channel
					if (GetPianoChannel(trackList) == midiKey.Channel)
					{
						pianoKeyList.Enqueue(pianoKey);
					}
				}
			}

			MetricTimeSpan duration = file.GetDuration<MetricTimeSpan>();
			return new Song(file, "temp", Difficulty.Easy, duration, pianoKeyList, TempoMap);
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
			bool channelFound = false;
			List<FourBitNumber> programNumbersFound = new List<FourBitNumber>();
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
			foreach(FourBitNumber channel in trackList.GetChannels())
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
			MetricTimeSpan timeStamp = midiNote.TimeAs<MetricTimeSpan>(TempoMap);
			MetricTimeSpan duration = midiNote.LengthAs<MetricTimeSpan>(TempoMap);

			var noteName = midiNote.NoteName;
			var octave = midiNote.Octave;
			return new PianoKey((Octave)octave, noteName, timeStamp, duration);
		}
	}
}