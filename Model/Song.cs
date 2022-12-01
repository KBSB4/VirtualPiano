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
				Stopwatch sw = Stopwatch.StartNew();
				PianoKey nextKey = PianoKeys.Dequeue();
				sw.Start();

				while (nextKey != null)
				{
					if (sw.ElapsedMilliseconds >= nextKey.TimeStamp + Offset)
					{
						PianoKeyEventArgs keyEventArgs = new PianoKeyEventArgs(nextKey);
						keyEventArgs.Offset = Offset;
						NotePlayed.Invoke(this, keyEventArgs);
						nextKey = PianoKeys.Dequeue();
						PianoKeysPlayed.Enqueue(nextKey);
					}
				}

				sw.Stop();
			});
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

	public enum Difficulty
	{
		Easy,
		Medium,
		Hard,
		Extreme,
		ExtremeHeroSuperDeluxe
	}
}
