using Melanchall.DryWetMidi.Core;

namespace Model
{
    public class PlayList
    {
        List<MidiFile> midiFiles;

        public static MidiFile? RetrieveMidiFile()
        {
            return MidiFile.Read("..\\..\\..\\..\\Controller\\PianoSoundPlayer\\twinkletesttwo.mid");
            //return null;
            //Midibestanden ophalen
        }
    }
}