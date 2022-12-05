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
            MidiFile file = PlayList.RetrieveMidiFile();
            if (file is not null)
            {
                CurrentSong = MidiConverter.Convert(file);
            }
        }

        public static void LoadSong(MetricTimeSpan Offset)
        {
            LoadSong();
            CurrentSong.Offset = Offset;
        }

        /// <summary>
        /// Plays <see cref="CurrentSong"/>
        /// </summary>
        public static void PlaySong()
        {
            if (CurrentSong is not null)
            {
                CurrentSong.Play();
            }
        }
    }
}
