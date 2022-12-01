using Controller;
using Model;

namespace UnitTests
{
    public class Controller_PianoController_Tests
    {
        private static Piano piano;
        [SetUp]
        public void SetUp()
        {
            PianoController.CreatePiano();
            piano = PianoController.Piano;
        }

        [Test]
        public void PianoController_Piano_NotNull()
        {
            Assert.That(piano, Is.Not.Null);
        }

        [Test]
        public void PianoController_Piano_CorrectAmountOfKeys()
        {
            Assert.That(piano.PianoKeys, Has.Count.EqualTo(72));
        }

        [TestCase(999991235, -1)]
        [TestCase(0, -1)]
        [TestCase(-124123, -1)]
        [TestCase(-1, -1)]
        [TestCase(69, 0)]
        [TestCase(62, 1)]
        [TestCase(67, 2)]
        [TestCase(47, 3)]
        [TestCase(46, 4)]
        [TestCase(65, 5)]
        [TestCase(50, 6)]
        [TestCase(45, 7)]
        [TestCase(51, 8)]
        [TestCase(57, 9)]
        [TestCase(53, 10)]
        [TestCase(56, 11)]
        [TestCase(60, 12)]
        [TestCase(36, 13)]
        [TestCase(66, 14)]
        [TestCase(37, 15)]
        [TestCase(48, 16)]
        [TestCase(61, 17)]
        [TestCase(39, 18)]
        [TestCase(63, 19)]
        [TestCase(40, 20)]
        [TestCase(68, 21)]
        [TestCase(41, 22)]
        [TestCase(64, 23)]
        public void PianoController_GetPressedPianoKey(int intValue, int expected)
        {
            PianoKey? pianoKey = PianoController.GetPressedPianoKey(intValue);
            if (expected == -1)
            {
                Assert.That(pianoKey, Is.Null);
            }
            else
            {
                Assert.That(pianoKey, Is.EqualTo(piano.PianoKeys[expected]));
            }
        }

        [TestCase(999991235, -1)]
        [TestCase(0, -1)]
        [TestCase(-124123, -1)]
        [TestCase(-1, -1)]
        [TestCase(69, 0)]
        [TestCase(62, 1)]
        [TestCase(67, 2)]
        [TestCase(47, 3)]
        [TestCase(46, 4)]
        [TestCase(65, 5)]
        [TestCase(50, 6)]
        [TestCase(45, 7)]
        [TestCase(51, 8)]
        [TestCase(57, 9)]
        [TestCase(53, 10)]
        [TestCase(56, 11)]
        [TestCase(60, 12)]
        [TestCase(36, 13)]
        [TestCase(66, 14)]
        [TestCase(37, 15)]
        [TestCase(48, 16)]
        [TestCase(61, 17)]
        [TestCase(39, 18)]
        [TestCase(63, 19)]
        [TestCase(40, 20)]
        [TestCase(68, 21)]
        [TestCase(41, 22)]
        [TestCase(64, 23)]
        public void PianoController_GetReleasedPianoKey(int intValue, int expected)
        {
            PianoKey? pianoKey = PianoController.GetReleasedKey(intValue);
            if (expected == -1)
            {
                Assert.That(pianoKey, Is.Null);
            }
            else
            {
                Assert.That(pianoKey, Is.EqualTo(piano.PianoKeys[expected]));
            }
        }
    }
}
