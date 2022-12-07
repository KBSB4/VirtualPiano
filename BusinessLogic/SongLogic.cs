using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Model;

namespace BusinessLogic
{
    public static class SongLogic
    {
        private const int SONG_OFFSET = 2000;
        public static Playback PlaybackDevice;
        public static OutputDevice OutputDevice;

        private static Song SongForTime;

        public static void Play(Song song)
        {
            song.NotePlayed += Song_NotePlayed;
            new Thread(new ParameterizedThreadStart(PlayFile)).Start(song);
        }

        private static void PlayFile(object? obj)
        {
            if (obj is not Song song) return;
            //TODO Properly stop and start thread when song finishes fully
            song.SongTimerThread.Start();
            SongForTime = song;
            OutputDevice = OutputDevice.GetByIndex(0);
            PlaybackDevice = song.File.GetPlayback(OutputDevice);
            PlaybackCurrentTimeWatcher.Instance.AddPlayback(PlaybackDevice, TimeSpanType.Metric);
            PlaybackCurrentTimeWatcher.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
            PlaybackCurrentTimeWatcher.Instance.Start();

            Thread.Sleep(SONG_OFFSET);
            PlaybackDevice.Start();
            SpinWait.SpinUntil(() => !PlaybackDevice.IsRunning);

            OutputDevice.Dispose();
            PlaybackDevice.Dispose();
            song.IsPlaying = false;
        }

        private static void OnCurrentTimeChanged(object sender, PlaybackCurrentTimeChangedEventArgs e)
        {
            foreach (var playbackTime in e.Times)
            {
                SongForTime.TimeInSong = (MetricTimeSpan)playbackTime.Time;
            }
        }

        private static void Song_NotePlayed(object? sender, PianoKeyEventArgs e)
        {
            object[] objs = { sender, e.Key };
            if (e.Key.PressedDown) new Thread(new ParameterizedThreadStart(PlayNote)).Start(objs);
        }

        private static void PlayNote(object? obj)
        {
            object[] objs = (object[])obj;

            PianoKey pianoKey = (PianoKey)objs[1];
            Song song = (Song)objs[0];

            Thread.Sleep(pianoKey.Duration);
            pianoKey.PressedDown = false;
            song.InvokeNotePlayed(song, new PianoKeyEventArgs(pianoKey));
        }

        public static void PlaySong(Song song)
        {
            DateTime now = DateTime.Now;
            while (song.PianoKeys.Count > 0)
            {
                PianoKey pianoKey = song.PianoKeys.Dequeue();
                song.InvokeNotePlayed(song, new PianoKeyEventArgs(pianoKey));
                if (song.PianoKeys.TryPeek(out PianoKey? nextKey))
                {
                    MetricTimeSpan timeSpan;
                    if (PlaybackDevice is null || !PlaybackDevice.IsRunning)
                    {
                        timeSpan = DateTime.Now - now;
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
                //Debug.WriteLine(pianoKey.ToString());
            }
        }
    }
}
