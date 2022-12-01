using Melanchall.DryWetMidi.Interaction;

namespace Model
{
    public class PianoKeyEventArgs : EventArgs
    {
        public PianoKey Key { get; set; }
        public MidiTimeSpan Offset { get; set; }


		public PianoKeyEventArgs(PianoKey pianoKey) : this(pianoKey, (MidiTimeSpan)0)
		{
			
		}

		public PianoKeyEventArgs(PianoKey key, MidiTimeSpan offset)
        {
            Key = key;
            Offset = offset;
        }
    }
}
