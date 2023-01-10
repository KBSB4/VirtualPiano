using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{
    public class PianoKey
    {
        public Octave Octave { get; set; }
        public bool PressedDown { get; set; }
        public NoteName Note { get; set; }
        public KeyBind? KeyBind { get; set; }
        public MetricTimeSpan? TimeStamp { get; set; }
        public MetricTimeSpan? Duration { get; set; }

        public PianoKey(Octave octave, NoteName note)
        {
            Octave = octave;
            Note = note;
        }

        public PianoKey(Octave octave, NoteName note, MetricTimeSpan timeStamp, MetricTimeSpan duration) : this(octave, note)
        {
            TimeStamp = timeStamp;
            Duration = duration;
            PressedDown = true;
        }

        public PianoKey(Octave octave, NoteName note, KeyBind bind) : this(octave, note)
        {
            KeyBind = bind;
        }

        public override string ToString() //Debug
        {
            return $"{Octave} | {Note} | {TimeStamp} | {Duration}";
        }
    }
}