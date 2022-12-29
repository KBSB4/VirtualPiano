namespace Model
{
    public class Piano
    {
        //Sets which octave is active. Lower or upper
        public bool LowerOctaveActive = true;
        public List<PianoKey> PianoKeys { get; set; } = new();
    }
}