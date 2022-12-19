using BusinessLogic;
using Model;

namespace UnitTests
{
    [TestFixture]
    internal class Model_Piano_OctaveSwap
    {
        private Piano piano;

        [SetUp]
        //TODO update tests
        public void SetUp()
        {
            PianoController.CreatePiano();
            piano = PianoController.Piano;
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
