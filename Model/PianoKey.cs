using Melanchall.DryWetMidi.MusicTheory;
using static Model.Piano;

namespace Model
{

    public class PianoKey
    {

        public Octave Octave { get; set; }
        public Boolean PressedDown { get; set; }
        public NoteName Note { get; set; }
        public double Frequency { get; set; }
        public char KeyBindChar { get; set; }
        public MicrosoftKeybind MicrosoftBind { get; set; }

        public PianoKey(Octave octave, NoteName note, MicrosoftKeybind bind)
        {
            Octave = octave;
            Note = note;
            MicrosoftBind = bind;
        }
    }
}
