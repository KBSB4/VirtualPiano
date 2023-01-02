using Controller;
using Model.DatabaseModels;

namespace UnitTests
{
    internal class Controller_DatabaseController_Tests
    {

        [Test]
        public async Task GetHighscoresTestOfSong55()
        {
            Highscore[] highscores = await DatabaseController.GetHighscores(55);
            Highscore score = highscores.FirstOrDefault();
            Assert.IsNotNull(score);
        }

        //[Test] //Unable to test this as we do not have a way of deleting existing scores
        //public void UploadScoreMinus555ToSong55()
        //{
        //    Highscore[] highscores = await DatabaseController.GetHighscores(55);
        //}

        [Test]
        public async Task AlterScoreMinus556OfUser12InSong55()
        {
            Highscore score = new()
            {
                User = DatabaseController.GetUserByID(12).Result,
                Song = await DatabaseController.GetSong(57),
                Score = -555
            };
            await DatabaseController.UpdateHighscore(score);
            score.Score = -556;
            await DatabaseController.UpdateHighscore(score);

            Highscore[] highscores = await DatabaseController.GetHighscores(57);
            Highscore databasescore = highscores.Where(item => item.User.Id == score.User.Id).FirstOrDefault();
            Assert.AreEqual(score.Score, databasescore.Score);
        }
    }
}