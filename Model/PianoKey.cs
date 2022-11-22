using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{

    public class PianoKey
    {

        public Octaves Octave { get; set; }
        public Boolean PressedDown { get; set; }
        public NoteName Note { get; set; }
        public double Frequency { get; set; }
        public char KeyBindChar { get; set; }
        public int MicrosoftBind { get; set; }

        public PianoKey(Octaves octave, NoteName note, int bind)
        {
            Octave = octave;
            Note = note;
            MicrosoftBind = bind;
        }
    }
}
