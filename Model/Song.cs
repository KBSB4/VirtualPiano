using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace Model
{
    public class Song
    {
        public MidiFile File { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public MetricTimeSpan Duration { get; set; }
        public Queue<PianoKey> PianoKeys { get; set; }
        public MetricTimeSpan TimeInSong { get; set; }
        public MetricTimeSpan Offset { get; set; }
        public Thread SongTimerThread { get; set; }
        public Boolean IsPlaying { get; set; }
        public TempoMap TempoMap { get; set; }

        public event EventHandler<PianoKeyEventArgs> NotePlayed;

        public Song(MidiFile file, string name, Difficulty difficulty, MetricTimeSpan duration, Queue<PianoKey> pianoKeys, TempoMap tempoMap)
        {
            File = file;
            Name = name;
            Difficulty = difficulty;
            Duration = duration;
            PianoKeys = pianoKeys;
            TimeInSong = new MetricTimeSpan(0);
            Offset = new TimeSpan(0);
            TempoMap = tempoMap;
        }

        public void InvokeNotePlayed(Song song, PianoKeyEventArgs pianoKeyEventArgs)
        {
            song.NotePlayed?.Invoke(song, pianoKeyEventArgs);
        }
    }
}

