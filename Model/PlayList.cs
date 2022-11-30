using Melanchall.DryWetMidi.Core;

namespace Model
{
    public class PlayList
    {
        List<MidiFile> midiFiles;

        public static MidiFile? RetrieveMidiFile()
        {
            return MidiFile.Read("..\\..\\..\\PianoSoundPlayer\\testMidiFile.mid");
            //return null;
            //Midibestanden ophalen
        }
    }
}
