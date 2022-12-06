using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{

    public class PianoKey
    {
        public Octave Octave { get; set; }
        public bool PressedDown { get; set; }
        public NoteName Note { get; set; }
        public KeyBind KeyBind { get; set; }

        public PianoKey(Octave octave, NoteName note, KeyBind bind)
        public MetricTimeSpan TimeStamp { get; set; }
        public MetricTimeSpan Duration { get; set; }
        {
            Octave = octave;
            Note = note;
            KeyBind = bind;
        }

        public PianoKey(Octave octave, NoteName note, MetricTimeSpan timeStamp, MetricTimeSpan duration)
        {
            Octave = octave;
            Note = note;
            TimeStamp = timeStamp;
            Duration = duration;
            PressedDown = true;
        }

        public override string ToString()
        {
            return $"{Octave} | {Note} | {TimeStamp} | {Duration}";
        }
    }
}
