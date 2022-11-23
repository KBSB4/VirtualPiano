using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace UnitTests
{
    public class Controller_PianoController_Tests
    {
        [SetUp]
        public void SetUp()
        {

        }

        /// <summary>
        /// Test #1: PianoSoundPlayer path is valid (not null)
        /// Test #2: Piano was created (not null)
        /// </summary>
        [Test]
        public void PianoController_CreatePiano()
        {
            PianoController.CreatePiano();
            Assert.IsNotNull(PianoController.Piano);
            Assert.IsNotNull(PianoController._player);
        }

        /// <summary>
        /// Test #1: 
        /// </summary>
        [Test]
        public void PianoController_GetPressedPianoKey()
        {

        }

        [Test]
        public void PianoController_GetReleasedKey()
        {

        }
    }
}
