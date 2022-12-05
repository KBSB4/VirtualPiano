namespace Model
{
    public class PianoKeyEventArgs : EventArgs
    {
        public PianoKey Key { get; set; }
        public TimeSpan Offset { get; set; }
        public PianoKeyEventArgs(PianoKey key, TimeSpan offset)
        {
            Key = key;
            Offset = offset;
        }

        public PianoKeyEventArgs(PianoKey pianoKey)
        {
            Key = pianoKey;
        }
    }
}
