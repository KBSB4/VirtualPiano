using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System.Diagnostics;

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
            new Thread(new ParameterizedThreadStart(PlayFile)).Start(File);
        }

        private void PlayFile(object? obj)
        {
            SongTimerThread.Start();
            Thread.Sleep(2700);
            //Thread.Sleep(200);
            File.Play(OutputDevice.GetByIndex(0));
        }

        private void Song_NotePlayed(object? sender, PianoKeyEventArgs e)
        {
            //Debug.WriteLine("Note Played");
            if (e.Key.PressedDown)
            {
                new Thread(new ParameterizedThreadStart(PlayNote)).Start(e.Key);

            }
            //Console.WriteLine(e.Key.ToString());
        }

        private void PlayNote(object? obj)
        {
            PianoKey pianoKey = (PianoKey)obj;

            Thread.Sleep(pianoKey.Duration);
            pianoKey.PressedDown = false;
            NotePlayed?.Invoke(this, new PianoKeyEventArgs(pianoKey));
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
            Thread.Sleep(1000);
            while (PianoKeys.Count > 0)
            {
                PianoKey pianoKey = PianoKeys.Dequeue();
                NotePlayed?.Invoke(this, new PianoKeyEventArgs(pianoKey));
                if (PianoKeys.TryPeek(out PianoKey? nextKey))
                {
                    Thread.Sleep(nextKey.TimeStamp - TimeInSong);
                    TimeInSong = nextKey.TimeStamp;
                }
                Debug.WriteLine(pianoKey.ToString());
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
