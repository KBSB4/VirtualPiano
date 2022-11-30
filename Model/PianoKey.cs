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
        {
            Octave = octave;
            Note = note;
            KeyBind = bind;
        }
    }
}
