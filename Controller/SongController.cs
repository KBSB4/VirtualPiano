using BusinessLogic;
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
            MidiFile? file = MIDIController.OriginalMIDI;
            if (file is not null)
            {
                CurrentSong = MIDIController.Convert(file);
                CurrentSong.SongTimerThread = new Thread(() => SongLogic.PlaySong(CurrentSong));
                //CurrentSong.File = MIDIController.RemovePiano(CurrentSong.File.Clone());
            }
        }

        /// <summary>
        /// Load song and set offset
        /// </summary>
        /// <param name="Offset"></param>
        public static void LoadSong(MetricTimeSpan Offset)
        {
            LoadSong();
            //TODO NOT USED RIGHT NOW
            //CurrentSong.Offset = Offset;
        }

        /// <summary>
        /// Plays <see cref="CurrentSong"/>
        /// </summary>
        public static void PlaySong()
        {
            if (CurrentSong is not null && !CurrentSong.IsPlaying)
            {
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

                //TODO Sometimes Stop() crashes with an AccessViolationException
                //SongLogic.PlaybackDevice.Stop();
                SongLogic.OutputDevice.Dispose();
                SongLogic.PlaybackDevice.Dispose();
            }
        }
    }
}
