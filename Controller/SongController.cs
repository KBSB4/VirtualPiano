using BusinessLogic;
using BusinessLogic.SoundPlayer;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Model;

namespace Controller
{
    public static class SongController
    {
        public static Song? CurrentSong { get; set; }

        public static void LoadSong()
        {
            MidiFile? file = MidiController.GetMidiFile();
            if (file is not null)
            {
                CurrentSong = MidiController.Convert(file);
                CurrentSong.SongTimerThread = new Thread(() => SongLogic.PlaySong(CurrentSong));
            }
        }

        /// <summary>
        /// Load song and set offset
        /// </summary>
        /// <param name="Offset"></param>
        public static void LoadSong(MetricTimeSpan Offset)
        {
            LoadSong();
        }

        /// <summary>
        /// Plays <see cref="CurrentSong"/>
        /// </summary>
        public static void PlaySong()
        {
            if (CurrentSong is not null && !CurrentSong.IsPlaying)
            {
                CurrentSong.IsPlaying = true;
                SongLogic.Play(CurrentSong);
            }
        }

        /// <summary>
        /// Stops the song by making a new queue which sets its count to 0
        /// </summary>
        public static void StopSong()
        {
            if (CurrentSong is not null && CurrentSong.IsPlaying)
            {
                //Stops the keys from appearing
                CurrentSong.PianoKeys = new();

                //TODO Does not work
                SongLogic.PlaybackDevice.Dispose();
                SongLogic.OutputDevice.Dispose();
            }
        }
    }
}
