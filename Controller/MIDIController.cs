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
            foreach (Note? midiKey in file.GetNotes())
            {
                PianoKey? pianoKey = ConvertPianoKey(midiKey);
                if (pianoKey is not null)
                {
                    //TODO Later for database
                    //if (IsPianoChannel(trackList, midiKey.Channel))
                    //{
                    pianoKeyList.Enqueue(pianoKey);
                    //}
                }
            }

            MetricTimeSpan duration = file.GetDuration<MetricTimeSpan>();
            return new Song(file, "temp", Difficulty.Easy, duration, pianoKeyList);
        }

        /// <summary>
        /// Find piano channel - NOT USED RIGHT NOW
        /// </summary>
        /// <param name="trackList"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        private static bool IsPianoChannel(List<TrackChunk> trackList, FourBitNumber channel)
        {
            foreach (TrackChunk chunk in trackList)
            {
                var programNumbers = chunk
                    .Events
                    .OfType<ProgramChangeEvent>()
                    .Select(e => new { e.ProgramNumber, e.Channel })
                    .ToArray();
                foreach (var test in programNumbers)
                {
                    if (test.Channel == channel)
                        //See Wikipedia General MIDI - Everything under 8 is Piano
                        if (test.ProgramNumber < 8)
                            return true;
                }
            }
            return false;
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