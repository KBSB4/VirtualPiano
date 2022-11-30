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
            TempoMap = file.GetTempoMap();
            Queue<PianoKey> pianoKeyList = new();
            foreach (Note? midiKey in file.GetNotes())
            {
                PianoKey? pianoKey = ConvertPianoKey(midiKey);
                if (pianoKey is not null)
                {
                    pianoKeyList.Enqueue(pianoKey);
                }
            }

            MetricTimeSpan duration = file.GetDuration<MetricTimeSpan>();
            return new Song(file, "TempName", Difficulty.Easy, duration, pianoKeyList);
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
