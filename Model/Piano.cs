using Melanchall.DryWetMidi.MusicTheory;

namespace Model
{
    public class Piano
    {
        private int[] MicrosoftKeybinds = { 60, 66, 48, 61, 63, 68, 64, 52, 58, 59, 44, 62, 47, 49, 50, 51, 53, 54, 55, 69, 67, 46, 65, 45, 57, 56 };

        static bool up;
        public List<PianoKey> PianoKeys { get; set; }

        /// <summary>
        /// Constructor for piano that creates 50 keys for itself.
        /// </summary>
        public Piano()
        {
            PianoKeys = new();
            AssembleKeyBindings();
        }

        //TODO Naar Pianocontroller?
        /// <summary>
        /// Sets octave and note for each key
        /// </summary>
        /// <param name="currentNote"></param>
        /// <param name="currentOctave"></param>
        public void UpdateOctaveAndNote(ref NoteName currentNote, ref Octaves currentOctave)
        {
            if (currentNote.Equals(NoteName.Unknown))
            {
                currentNote = NoteName.A;
                currentOctave++;
            }
        }

        /// <summary>
        /// Creates the PianoKeys for the piano
        /// </summary>
        /// <param name="octave"></param>
        /// <param name="note"></param>
        /// <param name="getal"></param>
        public void CreateKey(Octaves octave, ref NoteName note, int getal)
        {
            if (!up)
            {
                PianoKeys.Add(new PianoKey(octave, note, getal) { KeyBindChar = (char)((char)getal + 53) });
            }
            else
            {
                PianoKeys.Add(new PianoKey(octave, note, getal) { KeyBindChar = (char)((char)getal + 21) });

            }
            note++;
        }

        /// <summary>
        /// Sets keybindings for each key
        /// </summary>
        public void AssembleKeyBindings()
        {
            Octaves currentoctave = 0;  // first octave two
            NoteName currentnote = NoteName.C; // first key of the virtual keyboard

            for (int j = 0; j < 24; j++)
            {

                CreateKey(currentoctave, ref currentnote, MicrosoftKeybinds[j]);
                UpdateOctaveAndNote(ref currentnote, ref currentoctave);
                up = true;
                CreateKey(currentoctave, ref currentnote, MicrosoftKeybinds[j]);
                UpdateOctaveAndNote(ref currentnote, ref currentoctave);
                up = false;

            }
        }
    }
}