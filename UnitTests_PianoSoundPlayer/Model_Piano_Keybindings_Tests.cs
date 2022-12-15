using Model;

namespace UnitTests
{
    [TestFixture]
    internal class Model_Piano_Keybindings_Tests
    {
        private Piano piano;

        [SetUp]
        public void SetUp()
        {
            Controller.PianoController.CreatePiano();
            piano = Controller.PianoController.Piano;

        }

        [TestCase(0, KeyBind.Z)]
        [TestCase(1, KeyBind.S)]
        [TestCase(2, KeyBind.X)]
        [TestCase(3, KeyBind.D)]
        [TestCase(4, KeyBind.C)]
        [TestCase(5, KeyBind.V)]
        [TestCase(6, KeyBind.G)]
        [TestCase(7, KeyBind.B)]
        [TestCase(8, KeyBind.H)]
        [TestCase(9, KeyBind.N)]
        [TestCase(10, KeyBind.J)]
        [TestCase(11, KeyBind.M)]
        [TestCase(12, KeyBind.Q)]
        [TestCase(13, KeyBind.D2)]
        [TestCase(14, KeyBind.W)]
        [TestCase(15, KeyBind.D3)]
        [TestCase(16, KeyBind.E)]
        [TestCase(17, KeyBind.R)]
        [TestCase(18, KeyBind.D5)]
        [TestCase(19, KeyBind.T)]
        [TestCase(20, KeyBind.D6)]
        [TestCase(21, KeyBind.Y)]
        [TestCase(22, KeyBind.D7)]
        [TestCase(23, KeyBind.U)]


        public void Piano_GetsCorrectKeys(int index, KeyBind m)
        {

            KeyBind? test = piano.PianoKeys[index].KeyBind;
            Assert.That(test, Is.EqualTo(m));

        }

        [TestCase(1, KeyBind.Y)]
        [TestCase(2, KeyBind.D7)]
        [TestCase(3, KeyBind.U)]
        public void IsInvalidKeyBind(int index, KeyBind m)
        {
            KeyBind? test = piano.PianoKeys[index].KeyBind;
            Assert.That(test, Is.Not.EqualTo(m));
        }

    }
}
