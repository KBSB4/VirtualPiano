namespace VirtualPiano
{
    public class PianoKey
    {

        public Octaves Octave { get; set; }
        public bool PressedDown { get; set; }
        public Notes Note { get; set; }
        public char KeyBindChar { get; set; }

        public int MicrosoftBind { get; set; }

        public PianoKey(Octaves octave, Notes note, int bind)
        {
            Octave = octave;
            Note = note;
            MicrosoftBind = bind;
        }
    }

    public enum Octaves
    {
        Two,
        Three,
        Four,
        Five
    }

    public enum Notes
    {
        A,
        Asharp,
        B,
        C,
        Csharp,
        D,
        Dsharp,
        E,
        F,
        Fsharp,
        G,
        Gsharp,

        Unknown
    }
}

