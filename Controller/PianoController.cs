using Model;

namespace Controller
{
    public class PianoController
    {
        public Piano Piano { get; set; }

        /// <summary>
        /// Creates the piano for the program
        /// </summary>
        /// <returns>Piano object</returns>
        public static Piano CreatePiano()
        {
            Piano piano = new Piano();

            return piano;
        }
    }
}