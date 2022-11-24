using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{

    public class PianoKey
    {
        public Octave Octave { get; set; }
        public Boolean PressedDown { get; set; }
        public NoteName Note { get; set; }
        public MicrosoftKeybinds MicrosoftBind { get; set; }

        public PianoKey(Octave octave, NoteName note, MicrosoftKeybinds bind)
        {
            Octave = octave;
            Note = note;
            MicrosoftBind = bind;
        }
    }
}
