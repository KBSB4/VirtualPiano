using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    internal class Model_Piano_Keybindings_Tests
    {
        private Piano piano;

        [SetUp]
        public void SetUp()
        {
            piano = new Piano();
        }

        [TestCase(0, MicrosoftKeybinds.Z)]
        [TestCase(1, MicrosoftKeybinds.S)]
        [TestCase(2, MicrosoftKeybinds.X)]
        [TestCase(3, MicrosoftKeybinds.D)]
        [TestCase(4, MicrosoftKeybinds.C)]
        [TestCase(5, MicrosoftKeybinds.V)]
        [TestCase(6, MicrosoftKeybinds.G)]
        [TestCase(7, MicrosoftKeybinds.B)]
        [TestCase(8, MicrosoftKeybinds.H)]
        [TestCase(9, MicrosoftKeybinds.N)]
        [TestCase(10, MicrosoftKeybinds.J)]
        [TestCase(11, MicrosoftKeybinds.M)]
        [TestCase(12, MicrosoftKeybinds.Q)]
        [TestCase(13, MicrosoftKeybinds.D2)]
        [TestCase(14, MicrosoftKeybinds.W)]
        [TestCase(15, MicrosoftKeybinds.D3)]
        [TestCase(16, MicrosoftKeybinds.E)]
        [TestCase(17, MicrosoftKeybinds.R)]
        [TestCase(18, MicrosoftKeybinds.D5)]
        [TestCase(19, MicrosoftKeybinds.T)]
        [TestCase(20, MicrosoftKeybinds.D6)]
        [TestCase(21, MicrosoftKeybinds.Y)]
        [TestCase(22, MicrosoftKeybinds.D7)]
        [TestCase(23, MicrosoftKeybinds.U)]
       

        public void Piano_GetsCorrectKeys(int index, MicrosoftKeybinds m)
        {

            MicrosoftKeybinds test= piano.PianoKeys[index].MicrosoftBind;
            Assert.AreEqual(m, test);

        }

        [TestCase(1, MicrosoftKeybinds.Y)]
        [TestCase(2, MicrosoftKeybinds.D7)]
        [TestCase(3, MicrosoftKeybinds.U)]
        public void IsInvalidKeyBind(int index, MicrosoftKeybinds m)
        {
            MicrosoftKeybinds test = piano.PianoKeys[index].MicrosoftBind;
            Assert.AreNotEqual(m, test);
        }

    }
}
