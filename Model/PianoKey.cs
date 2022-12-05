using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{

    public class PianoKey
    {
        public Octave Octave { get; set; }
        public bool PressedDown { get; set; }
        public NoteName Note { get; set; }
        public MicrosoftKeybinds MicrosoftBind { get; set; }

        public MetricTimeSpan TimeStamp { get; set; }
        public MetricTimeSpan Duration { get; set; }

        public PianoKey(Octave octave, NoteName note, MicrosoftKeybinds bind)
        {
            Octave = octave;
            Note = note;
            MicrosoftBind = bind;
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
