namespace Model
{
    public class PianoKeyEventArgs : EventArgs
    {
        public PianoKey Key { get; set; }

        public PianoKeyEventArgs(PianoKey pianoKey)
        {
            Key = pianoKey;
        }
    }
}