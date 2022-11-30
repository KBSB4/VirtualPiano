using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{

    public class PianoKey
    {
        public Octave Octave { get; set; }
        public bool PressedDown { get; set; }
        public NoteName Note { get; set; }
        public MicrosoftKeybinds MicrosoftBind { get; set; }

        public TimeSpan TimeStamp { get; set; }
        public TimeSpan Duration { get; set; }

        public PianoKey(Octave octave, NoteName note, MicrosoftKeybinds bind)
        {
            Octave = octave;
            Note = note;
            MicrosoftBind = bind;
        }

        public PianoKey(Octave octave, NoteName note, TimeSpan timeStamp, TimeSpan duration)
        {
            Octave = octave;
            Note = note;
            TimeStamp = timeStamp;
            Duration = duration;
        }

        public override string ToString()
        {
            return $"{Octave} | {Note} | {TimeStamp} | {Duration}";
        }
    }
}
