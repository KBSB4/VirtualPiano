using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace Model
{
    public class Song
    {
        public int Id { get; set; }
        public MidiFile File { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public MetricTimeSpan Duration { get; set; }
        public Queue<PianoKey> PianoKeys { get; set; }
        public Thread? SongTimerThread { get; set; }
        public bool IsPlaying { get; set; }
        public string Description { get; set; }
        public byte[] FullFile { get; set; }

        public event EventHandler<PianoKeyEventArgs>? NotePlayed;

        public Song(MidiFile file, string name, Difficulty difficulty, MetricTimeSpan duration, Queue<PianoKey> pianoKeys)
        {
            File = file;
            Name = name;
            Difficulty = difficulty;
            Duration = duration;
            PianoKeys = pianoKeys;
        }

		public Song()
		{
		}

		public static void InvokeNotePlayed(Song song, PianoKeyEventArgs pianoKeyEventArgs)
        {
            song.NotePlayed?.Invoke(song, pianoKeyEventArgs);
        }
    }
}