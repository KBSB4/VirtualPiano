using BusinessLogic;
using Controller;
using Model;

namespace UnitTests
{
    [TestFixture]
    internal class Model_Piano_OctaveSwap
    {
        [SetUp]
        public void SetUp()
        {
            PianoController.CreatePiano();
        }


        [TestCase(true, 0, Octave.Four)]
        [TestCase(false, 0, Octave.Two)]
        [TestCase(true, 12, Octave.Five)]
        [TestCase(false, 12, Octave.Three)]
        public void OctaveGetsIncreased(bool b, int index, Octave result)
        {
            if (b)
            {
                PianoLogic.SwapOctave(PianoController.Piano);
            }

            Assert.That(PianoController.Piano.PianoKeys[index].Octave, Is.EqualTo(result));

        }


        [TestCase(true, 0, Octave.Two)]
        [TestCase(true, 12, Octave.Three)]
        public void OctaveGetsDecreased(bool b, int index, Octave result)
        {
            if (b)
            {
                PianoLogic.SwapOctave(PianoController.Piano);

            }
            if (b)
            {

                PianoLogic.SwapOctave(PianoController.Piano);
            }

            Assert.That(PianoController.Piano.PianoKeys[index].Octave, Is.EqualTo(result));

        }

    }
}
