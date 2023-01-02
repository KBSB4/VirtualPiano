using Controller;
using Model.DatabaseModels;

namespace UnitTests
{
    internal class Controller_DatabaseController_Tests
    {
        
        /// <summary>
        /// Get all highscores of a song that 100% has scores
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetHighscoresTestOfSong7()
        {
            Highscore[]? highscores = await DatabaseController.GetHighscores(7);
            Highscore? score = highscores.FirstOrDefault();
            Assert.That(score, Is.Not.Null);
        }

        //[Test] //Unable to test this as we do not have a way of deleting existing scores
        //public void UploadScoreMinus555ToSong55()
        //{
        //    Highscore[] highscores = await DatabaseController.GetHighscores(55);
        //}

        /// <summary>
        /// Update score of user 31 in song 7 to 0
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task AlterScoreMinus555OfUser31InSong7()
        {
            Highscore score = new()
            {
                User = DatabaseController.GetUserByID(31).Result,
                Song = await DatabaseController.GetSong(7),
                Score = -555
            };
            await DatabaseController.UpdateHighscore(score);
            score.Score = 0;
            await DatabaseController.UpdateHighscore(score);

            Highscore[]? highscores = await DatabaseController.GetHighscores(7);
            Highscore? databasescore = highscores.Where(item => item.User.Id == score.User.Id).FirstOrDefault();
            Assert.That(databasescore.Score, Is.EqualTo(score.Score));
        }
    }
}