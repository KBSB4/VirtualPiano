using BusinessLogic;
using Controller;
using Model;

namespace UnitTests
{
    [TestFixture]
    internal class BusinessLogic_PianoLogic_Tests
    {
        Piano piano;
        [SetUp]
        public void Setup()
        {
            PianoController.CreatePiano();
            piano = PianoController.Piano;
        }

        [Test]
        public void PianoLogic_SwapOctave_SwapsOnce()
        {
            Octave pianoKey = piano.PianoKeys[0].Octave;
            PianoLogic.SwapOctave(piano);
            Octave pianoKey2 = piano.PianoKeys[0].Octave;
            PianoLogic.SwapOctave(piano);
            Octave pianoKey3 = piano.PianoKeys[0].Octave;
            Assert.That(pianoKey, Is.Not.EqualTo(pianoKey2));
        }

        [Test]
        public void PianoLogic_SwapOctave_SwapsBackandForth()
        {
            Octave pianoKey = piano.PianoKeys[0].Octave;
            PianoLogic.SwapOctave(piano);
            PianoLogic.SwapOctave(piano);
            Octave pianoKey3 = piano.PianoKeys[0].Octave;
            Assert.That(pianoKey, Is.EqualTo(pianoKey3));
        }
    }
}