using BusinessLogic;
using Melanchall.DryWetMidi.Core;
using Model;
using System.Runtime.CompilerServices;

namespace Controller
{
    public static class SongController
    {
        public static Song? CurrentSong { get; set; }

        /// <summary>
        /// Loads the current song for playing by a thread and conversion from the Midicontroller
        /// </summary>
        public static void LoadSong(bool DoKaroake = false)
        {
            MidiFile? file = MidiController.GetMidiFile();
            if (file is not null)
            {
                CurrentSong = MidiController.Convert(file);
                if (CurrentSong is null) return;
                CurrentSong.SongTimerThread = new Thread(() => SongLogic.PlaySong(CurrentSong));
                if (DoKaroake) CurrentSong.File = MidiController.RemovePiano(CurrentSong.File.Clone());
            }
        }

        /// <summary>
        /// Plays <see cref="CurrentSong"/>
        /// </summary>
        public static void PlaySong()
        {
            SongLogic.Play(CurrentSong);
        }

        /// <summary>
        /// Stops the song by making a new queue which sets its count to 0
        /// </summary>
        public static void StopSong()
        {
            SongLogic.StopSong(CurrentSong);
        }
    }
}