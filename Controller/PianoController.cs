using Model;
using VirtualPiano.PianoSoundPlayer;

namespace Controller
{
    public class PianoController
    {
        public static Piano Piano { get; set; }

        /// <summary>
        /// Creates the piano for the program
        /// </summary>
        /// <returns>Piano object</returns>
        public static void CreatePiano()
        {
            Piano = new Piano();
        }
    }
}