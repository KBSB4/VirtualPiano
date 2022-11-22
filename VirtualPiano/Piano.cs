using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualPiano
{
    public class Piano
    {
        private int[] MicrosoftKeybinds = { 60, 66, 48, 61, 63, 68, 64, 52, 58, 59, 44, 62, 47, 49, 50, 51, 53, 54, 55, 69, 67, 46, 65, 45, 57, 56 };

        static bool up;

        public List<CustomKey> CustomKeys { get; set; }


        public Piano()
        {
            CustomKeys = new();
            AssembleKeyBindings();

        }

        public void UpdateOctaveAndNote(ref Notes currentNote, ref Octaves currentOctave)
        {
            if (currentNote.Equals(Notes.Unknown))
            {
                currentNote = Notes.A;
                currentOctave++;
            }
        }

        public void CreateKey(Octaves octave, ref Notes note, int getal)
        {
            if (!up)
            {
                CustomKeys.Add(new CustomKey(octave, note, getal) { KeyBindChar = (char)((char)getal + 53) });
            }
            else
            {
                CustomKeys.Add(new CustomKey(octave, note, getal) { KeyBindChar = (char)((char)getal + 21) });

            }
            note++;
        }

        public void AssembleKeyBindings()
        {
            Octaves currentoctave = 0;  // first octave two
            Notes currentnote = Notes.C; // first key of the virtual keyboard

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
