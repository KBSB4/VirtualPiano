using Melanchall.DryWetMidi.Core;
using Model;

namespace Controller
{
    public static class SongController
    {
        public static Song? CurrentSong { get; set; }

        public static void LoadSong()
        {
            MidiFile? file = PlayList.RetrieveMidiFile();
            if (file is not null)
            {
                CurrentSong = MidiConverter.Convert(file);
            }
        }

        public static void PlaySong()
        {
            if (CurrentSong is not null)
            {
                CurrentSong.Play();
            }
        }
    }
}
