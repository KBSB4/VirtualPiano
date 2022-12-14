using BusinessLogic;
using Melanchall.DryWetMidi.Core;
using Model;

namespace Controller
{
    public static class MidiController
    {
        /// <summary>
        /// Read MIDI File using <paramref name="midiPath"/> and loads the song in the <see cref="SongController"/>.
        /// </summary>
        /// <param name="midiPath"></param>
        public static void OpenMidi(string midiPath)
        {
            bool fileFound = false;
            try
            {
                MidiLogic.currentMidi = MidiFile.Read(midiPath);
                fileFound = true;
            }
            catch { } //Ignore exception

            if (fileFound)
                SongController.LoadSong();
        }

        /// <summary>
        /// Convert MIDI to Song and its notes to PianoKeys
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Song Convert(MidiFile file)
        {
            return MidiLogic.ConvertMidiFile(file);
        }

        /// <summary>
        /// Illiterates through the trackChuncks of the MidiFile and removes the pianokeys. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static MidiFile RemovePiano(MidiFile file)
        {
            return MidiLogic.RemovePianoNotes(file);
        }

        /// <summary>
        /// </summary>
        /// <returns><see cref="MidiLogic.currentMidi"/></returns>
        public static MidiFile GetMidiFile()
        {
            return MidiLogic.currentMidi;
        }
    }
}