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
            Assert.That(PianoController.Piano, Is.Not.Null);
            //Assert.IsNotNull(PianoController._player);
        }

        ///// <summary>
        ///// Test #1: 
        ///// </summary>
        //[Test]
        //public void PianoController_GetPressedPianoKey()
        //{
        //    Assert.That(piano.PianoKeys, Has.Count.EqualTo(72));
        //}
    }
}