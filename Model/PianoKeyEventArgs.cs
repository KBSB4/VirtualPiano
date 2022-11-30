namespace Model
{
    public class PianoKeyEventArgs : EventArgs
    {
        private PianoKey pianoKey;

        public PianoKey Keys { get; set; }
        public TimeSpan Offset { get; set; }
        public PianoKeyEventArgs(PianoKey keys, TimeSpan offset)
        {
            Keys = keys;
            Offset = offset;
        }

        public PianoKeyEventArgs(PianoKey pianoKey)
        {
            this.pianoKey = pianoKey;
        }
    }
}
