using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using System.Diagnostics;

namespace Model
{
	public class Song
	{
		public MidiFile File { get; set; }
		public string Name { get; set; }
		public Difficulty Difficulty { get; set; }
		public MidiTimeSpan Duration { get; set; }
		public Queue<PianoKey> PianoKeys { get; set; }
		public Queue<PianoKey> PianoKeysPlayed { get; set; }
		public MidiTimeSpan TimeInSong { get; set; }
		public MidiTimeSpan Offset { get; set; }
		public Stopwatch SongTimer { get; set; }

		public Thread SongTimerThread { get; set; }

		public event EventHandler<PianoKeyEventArgs> NotePlayed;

		public Song(MidiFile file, string name, Difficulty difficulty, MidiTimeSpan duration, Queue<PianoKey> pianoKeys)
		{
			File = file;
			Name = name;
			Difficulty = difficulty;
			Duration = duration;
			PianoKeys = pianoKeys;
			PianoKeysPlayed = new();
			TimeInSong = new MidiTimeSpan(0);
			Offset = new MidiTimeSpan(0);
		}

		/// <summary>
		/// Starts a new <see cref="Thread"/> that keeps going until the song is done. This method <b>Invokes</b> <see cref="NotePlayed"/> 
		/// then adds the <see cref="PianoKey"/>s to <see cref="PianoKeysPlayed"/>
		/// </summary>
		public void Play()
		{
			SongTimerThread = new Thread(() =>
			{
				SongTimer = Stopwatch.StartNew();
				PianoKey nextKey = PianoKeys.Dequeue();

				while (PianoKeys.Count > 0)
				{
					if (SongTimer.ElapsedMilliseconds >= nextKey.TimeStamp + Offset)
					{
						PianoKeyEventArgs keyEventArgs = new PianoKeyEventArgs(nextKey);
						keyEventArgs.Offset = Offset;
						NotePlayed.Invoke(this, keyEventArgs);
						nextKey = PianoKeys.Dequeue();
						PianoKeysPlayed.Enqueue(nextKey);
					}
				}
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

				SongTimer.Stop();
			});
			//TODO The program does not close properly when exiting it due to this thread not exiting when closing
			SongTimerThread.Start();
		}

		public void Stop()
		{

		}

		public void Reset()
		{

		}

		public PianoKey[] GetNextPianoKeys()
		{
			return PianoKeys.Take(1).ToArray();
		}
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
