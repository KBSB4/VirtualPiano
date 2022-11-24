using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

namespace UnitTests
{
    public class Controller_PianoController_Tests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void CreatePiano_()
        {
            PianoController.CreatePiano();
            Assert.IsNotNull(PianoController.Piano);
        }

        [Test]
        public void CreatePiano_SoundPlayer_IsNotNull()
        {

        }
    }
}
