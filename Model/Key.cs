namespace Model
{

    public class Key
    {
        
        public Octaves Octave { get; set; }
        public Boolean PressedDown { get; set; }
        public Notes Note { get; set; }
        public double Frequency { get; set; }
        public char KeyBind { get; set; }

        public Key(Octaves octave, Notes note, char keyBind)
        {
            Octave = octave;
            Note = note;
            KeyBind = keyBind;
        }
    }
}
