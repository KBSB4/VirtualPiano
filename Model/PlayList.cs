using Melanchall.DryWetMidi.Core;

namespace Model
{
    public class PlayList
    {
        Queue<MidiFile> midiFiles;

        /// <summary>
        /// Selfexplanatory, not used right now
        /// </summary>
        /// <returns></returns>
        public static MidiFile? RetrieveMidiFile()
        {
            return MidiFile.Read("..\\..\\..\\..\\Controller\\PianoSoundPlayer\\twinkle-twinkle-little-star.mid");
        }
    }
}
