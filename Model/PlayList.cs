using Melanchall.DryWetMidi.Core;

namespace Model
{
    public class PlayList
    {
        List<MidiFile> midiFiles;

        public static MidiFile? RetrieveMidiFile()
        {
            return MidiFile.Read("..\\..\\..\\..\\Controller\\PianoSoundPlayer\\twinkle-twinkle-little-star.mid");
            //return null;
            //Midibestanden ophalen
        }
    }
}
