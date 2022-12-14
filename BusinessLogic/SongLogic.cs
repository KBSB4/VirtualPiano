using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Model;

namespace BusinessLogic
{
    public static class SongLogic
    {
        private const int SONG_OFFSET = 2000;
        private const int STARTTUNENOTES = 7;

        public static Playback PlaybackDevice;
        public static OutputDevice OutputDevice;

        public static event EventHandler startCountDown;

        /// <summary>
        /// Plays <paramref name="song"/> in a new thread
        /// </summary>
        /// <param name="song"></param>
        public static void Play(Song? song)
        {
            if (song is not null && !song.IsPlaying)
            {
                song.NotePlayed += Song_NotePlayed;
                new Thread(new ParameterizedThreadStart(PlayFile)).Start(song);
                song.IsPlaying = true;
            }
        }

        /// <summary>
        /// Plays <see cref="Song.File"/> from <paramref name="obj"/>
        /// </summary>
        /// <param name="obj"></param>
        private static void PlayFile(object? obj)
        {
            startCountDown?.Invoke(null, null);

            if (obj is not Song song) return;
            //TODO Properly remake thread when song wants to be played again after it finishes
            song.SongTimerThread.Start();
            OutputDevice = OutputDevice.GetByIndex(0);
            PlaybackDevice = song.File.GetPlayback(OutputDevice);

            Thread.Sleep(SONG_OFFSET);
            PlaybackDevice.Start();
            SpinWait.SpinUntil(() => !PlaybackDevice.IsRunning);

            OutputDevice.Dispose();
            PlaybackDevice.Dispose();
            song.IsPlaying = false;
        }

        /// <summary>
        /// If the key is pressedDown, starts the PlayNote thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Song_NotePlayed(object? sender, PianoKeyEventArgs e)
        {
            object[] objs = { sender, e.Key };
            if (e.Key.PressedDown) new Thread(new ParameterizedThreadStart(PlayNote)).Start(objs);
        }

        /// <summary>
        /// Waits for the note to have ended, releases the key and fires the NotePlayed event
        /// </summary>
        /// <param name="obj"></param>
        private static void PlayNote(object? obj)
        {
            object[] objs = (object[])obj;

            PianoKey pianoKey = (PianoKey)objs[1];
            Song song = (Song)objs[0];

            Thread.Sleep(pianoKey.Duration);
            pianoKey.PressedDown = false;
            song.InvokeNotePlayed(song, new PianoKeyEventArgs(pianoKey));
        }

        /// <summary>
        /// Loops through every note in the song, and fires an event when this note should appear visually
        /// </summary>
        /// <param name="song"></param>
        public static void PlaySong(Song song)
        {
            DateTime now = DateTime.Now;
            int ignoreNote = STARTTUNENOTES;
            while (song.PianoKeys.Count > 0)
            {
                PianoKey pianoKey = song.PianoKeys.Dequeue();

                if (ignoreNote < 0)
                {
                    song.InvokeNotePlayed(song, new PianoKeyEventArgs(pianoKey));
                }
                else
                {
                    ignoreNote--;
                }

                if (song.PianoKeys.TryPeek(out PianoKey? nextKey))
                {
                    MetricTimeSpan timeSpan;
                    if (PlaybackDevice is null || !PlaybackDevice.IsRunning)
                    {
                        timeSpan = pianoKey.TimeStamp;
                    }
                    else
                    {
                        timeSpan = (MetricTimeSpan)PlaybackDevice.GetCurrentTime(TimeSpanType.Metric)
                        + (MetricTimeSpan)TimeSpan.FromMilliseconds(SONG_OFFSET);
                    }

                    if (nextKey.TimeStamp > timeSpan)
                    {
                        Thread.Sleep(nextKey.TimeStamp - timeSpan);
                    }
                }
            }
        }

        /// <summary>
        /// Stops current playing song
        /// </summary>
        /// <param name="song"></param>
        public static void StopSong(Song? song)
        {
            if (song is not null && song.IsPlaying)
            {
                //Stops the keys from appearing
                song.PianoKeys = new();

                PlaybackDevice.PlaybackEnd = new MetricTimeSpan(0);
                PlaybackDevice.Dispose();
                OutputDevice.Dispose();
            }
        }
    }
}
