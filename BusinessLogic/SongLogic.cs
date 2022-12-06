using Melanchall.DryWetMidi.Multimedia;
using Model;
using System.Diagnostics;

namespace BusinessLogic
{
    public static class SongLogic
    {
        public static Playback PlaybackDevice;
        public static OutputDevice OutputDevice;

        public static void Play(Song song)
        {
            song.NotePlayed += Song_NotePlayed;
            new Thread(new ParameterizedThreadStart(PlayFile)).Start(song);
        }

        private static void PlayFile(object? obj)
        {
            Song song = obj as Song;
            song.SongTimerThread.Start();
            Thread.Sleep(2700);

            OutputDevice = OutputDevice.GetByIndex(0);
            PlaybackDevice = song.File.GetPlayback(OutputDevice);
            PlaybackDevice.Start();
            SpinWait.SpinUntil(() => !PlaybackDevice.IsRunning);

            OutputDevice.Dispose();
            PlaybackDevice.Dispose();
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
            Thread.Sleep(1000);
            song.IsPlaying = true;
            while (song.PianoKeys.Count > 0)
            {
                PianoKey pianoKey = song.PianoKeys.Dequeue();
                song.InvokeNotePlayed(song, new PianoKeyEventArgs(pianoKey));
                if (song.PianoKeys.TryPeek(out PianoKey? nextKey))
                {
                    Thread.Sleep(nextKey.TimeStamp - song.TimeInSong);
                    song.TimeInSong = nextKey.TimeStamp;
                }
                Debug.WriteLine(pianoKey.ToString());
            }
            song.IsPlaying = false;
        }
    }
}
