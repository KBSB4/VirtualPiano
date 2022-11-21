using System.Diagnostics;

namespace Model
{
    public class Piano
    {
        public List<Key> Keys { get; set; }

        /// <summary>
        /// Constructor for piano that creates 50 keys for itself.
        /// </summary>
        public Piano()
        {

            Keys = new();
            AddKeys();

        }

        /// <summary>
        /// Binds the keys. White keys get assigned to a lower case character and black keys get assigned to an upper case characters. Default value is 'a'
        /// </summary>
        /// <param name="currentnote"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        public char Keybindings(Notes currentnote, Key previous)
        {
            if (previous != null)
            {
                char currentChar = (char)(previous.KeyBind);

                //UNDONE Uppercase letters get wrongly assigned
                if (previous.Note.ToString().Contains("sharp") && !currentnote.ToString().Contains("sharp"))
                {
                    return currentChar += (char)33;
                }else if (currentnote.ToString().Contains("sharp"))
                {
                   return currentChar -= (char)32;
                }
                else
                {
                    return currentChar += (char)1;
                }

            }
            else
            {
                return (char)97;
            }

        }

        /// <summary>
        /// Adds all the keys of the piano with the correct octave, note and keybinding. 
        /// </summary>
        public void AddKeys()
        {
            Octaves currentoctave = 0;  // first octave two
            Notes currentnote = Notes.C; // first key of the virtual keyboard
            Key previous = null;

            for (int i = 1; i <= 48; i++)
            {
                Keys.Add(new Key(currentoctave, currentnote, Keybindings(currentnote, previous)));
                previous = Keys[i - 1];
                currentnote++;
                if (currentnote == Notes.Unknown)
                {
                    currentoctave++;
                    currentnote = Notes.A;
                }

            }
        }

        /// <summary>
        /// Handles the KeyDown event from MainWindow
        /// </summary>
        /// <param name="Key"></param>
        public void KeyDownHandler(char Key)
        {
            foreach (Key key in Keys)
            {
                if (key.KeyBind == Key)
                {
                    Debug.WriteLine($"Key with {key.KeyBind} got pressed by key {Key}");
                    key.PressedDown = true;
                }
            }
        }

        /// <summary>
        /// Handles the KeyUp event from MainWindow
        /// </summary>
        /// <param name="Key"></param>
        public void KeyUpHandler(char Key)
        {
            foreach (Key key in Keys)
            {
                if (key.KeyBind == Key)
                {
                    Debug.WriteLine($"Key with {key.KeyBind} got let go by key {Key}");
                    key.PressedDown = false;
                }
            }
        }
    }
}