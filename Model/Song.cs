using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

namespace Model
{
    public class Song
    {
        public Song(MidiFile file, string name, Difficulty difficulty, TimeSpan duration, Queue<PianoKey> pianoKeys)
        {
            File = file;
            Name = name;
            Difficulty = difficulty;
            Duration = duration;
            PianoKeys = pianoKeys;
            TimeInSong = TimeSpan.Zero;
            SongTimerThread = new Thread(new ThreadStart(PlaySong));
        }

        public MidiFile File { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public TimeSpan Duration { get; set; }
        public Queue<PianoKey> PianoKeys { get; set; }
        public TimeSpan TimeInSong { get; set; }
        public Thread SongTimerThread { get; set; }

        public event EventHandler<PianoKeyEventArgs> NotePlayed;

        public void Play()
        {
            NotePlayed += Song_NotePlayed;
            SongTimerThread.Start();
            File.Play(OutputDevice.GetByIndex(0));
        }

        private void Song_NotePlayed(object? sender, PianoKeyEventArgs e)
        {
            Console.WriteLine("Note Played");
            Console.WriteLine(e.Keys);
        }

        public void Stop()
        {

        }

        public PianoKey[] GetNextPianoKeys()
        {
            return PianoKeys.Take(1).ToArray();
        }

        private void PlaySong()
        {
            while (PianoKeys.Count > 0)
            {
                PianoKey pianoKey = PianoKeys.Dequeue();
                //TimeInSong = File.
                Thread.Sleep(pianoKey.TimeStamp - TimeInSong);
                TimeInSong = pianoKey.TimeStamp;
                Console.WriteLine(pianoKey.ToString());
                NotePlayed?.Invoke(this, new PianoKeyEventArgs(pianoKey));
            }
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Extreme,
        ExtremeHeroSuperDeluxe
    }
}
