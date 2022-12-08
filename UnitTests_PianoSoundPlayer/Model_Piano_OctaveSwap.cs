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
            piano = new();
        }


        [TestCase(true, 0, Octave.Four)]
        [TestCase(false, 0, Octave.Two)]
        [TestCase(true, 12, Octave.Five)]
        [TestCase(false, 12, Octave.Three)]
        public void OctaveGetsIncreased(bool b, int index, Octave result)
        {
            if (b)
            {
                piano.SwapOctave();
            }

            Assert.AreEqual(result, piano.PianoKeys[index].Octave);

        }


        [TestCase(true, 0, Octave.Two)]
        [TestCase(true, 12, Octave.Three)]
        public void OctaveGetsDecreased(bool b, int index, Octave result)
        {
            if (b)
            {
                piano.SwapOctave();

            }


            if (b)
            {

                piano.SwapOctave();
            }

            Assert.AreEqual(result, piano.PianoKeys[index].Octave);

        }

    }
}
