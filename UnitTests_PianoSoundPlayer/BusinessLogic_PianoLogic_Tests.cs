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
            _ = piano.PianoKeys[0].Octave;
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

        [TestCase(true, 0, Octave.Four)]
        [TestCase(false, 0, Octave.Two)]
        [TestCase(true, 12, Octave.Five)]
        [TestCase(false, 12, Octave.Three)]
        public void OctaveGetsIncreased(bool b, int index, Octave result)
        {
            if (b)
            {
                PianoLogic.SwapOctave(piano);
            }

            Assert.That(piano.PianoKeys[index].Octave, Is.EqualTo(result));
        }

        [TestCase(true, 0, Octave.Two)]
        [TestCase(true, 12, Octave.Three)]
        public void OctaveGetsDecreased(bool b, int index, Octave result)
        {
            if (b)
            {
                PianoLogic.SwapOctave(piano);
            }

            if (b)
            {
                PianoLogic.SwapOctave(piano);
            }

            Assert.That(piano.PianoKeys[index].Octave, Is.EqualTo(result));
        }
    }
}