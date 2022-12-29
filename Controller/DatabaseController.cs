using BusinessLogic;
using Model;
using Model.DatabaseModels;
using Model.Interfaces;

namespace Controller
{
    public static class DatabaseController
    {
        private static readonly IDatabaseManager databaseManager = new SQLDatabaseManager();
        //TODO Summaries
        public static async Task<User?> GetUserByID(int userID)
        {
            Task<User?> getUserByIDTask = databaseManager.GetUserById(userID);

            User? result = await getUserByIDTask;

            return result;
        }

        public static async Task<User?> GetUserByName(string username)
        {
            Task<User?> getUserByName = databaseManager.GetUserByName(username);

            User? result = await getUserByName;

            return result;
        }

        public static async Task UploadNewUser(User user)
        {
            Task uploadNewUserTask = databaseManager.UploadNewUser(user);

            await uploadNewUserTask;
        }

        public static async Task<User?> GetLoggingInUser(string username, string password)
        {
            Task<User?> getLoggingInUserTask = databaseManager.GetLoggingInUser(username, password);

            User? result = await getLoggingInUserTask;

            return result;
        }

        public static async Task<User[]?> GetAllUsers()
        {
            Task<User[]?> getAllUsersTask = databaseManager.GetAllUsers();

            User[]? result = await getAllUsersTask;

            return result;
        }

        /// <summary>
        /// Deletes a song from a database using <see cref="databaseManager"/>
        /// </summary>
        /// <param name="songName"></param>
        public static async Task DeleteSong(string songName)
        {
            Task deleteSongTask = databaseManager.DeleteSong(songName);

            await deleteSongTask;
        }

        /// <summary>
        /// Finds a song with <paramref name="songId"/> from a database using <see cref="databaseManager"/>
        /// </summary>
        /// <param name="songId"></param>
        /// <returns>New <see cref="Song"/> object, is null if not found in database</returns>
        public static async Task<Song?> GetSong(int songId)
        {
            Task<Song?> getSongTask = databaseManager.GetSong(songId);

            Song? result = await getSongTask;

            return result;
        }

        /// <summary>
        /// Gets all songs from a database using <see cref="databaseManager"/>
        /// </summary>
        /// <returns>A new <see cref="Song"/>[], could be an empty array</returns>
        public static async Task<Song[]?> GetAllSongs()
        {
            Task<Song[]?> getAllSongsTask = databaseManager.GetAllSongs();

            Song[]? result = await getAllSongsTask;

            return result;
        }

        /// <summary>
        /// Adds a <paramref name="song"/> to a database using <see cref="databaseManager"/> 
        /// </summary>
        /// <param name="song"></param>
        public static async Task UploadSong(Song song)
        {
            Task uploadSongTask = databaseManager.UploadSong(song);

            await uploadSongTask;
        }

        /// <summary>
        /// Gets highscores from a song found by <paramref name="songId"/> from a database using <see cref="databaseManager"/> 
        /// </summary>
        /// <param name="songId"></param>
        /// <returns>New <see cref="Highscore"/>[], is empty if nothing found</returns>
        public static async Task<Highscore[]?> GetHighscores(int songId)
        {
            Task<Highscore[]> getHighscoresTask = databaseManager.GetHighscores(songId);

            Highscore[]? result = await getHighscoresTask;

            return result;
        }

        /// <summary>
        /// Upload highscore with the <see cref="Highscore"/> class
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async Task UploadHighscore(Highscore score)
        {
            Task uploadHighscoreTask = databaseManager.UploadHighscore(score);

            await uploadHighscoreTask;
        }

        /// <summary>
        /// Update highscore with the <see cref="Highscore"/> class
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async Task UpdateHighscore(Highscore score)
        {
            Task updateHighscoreTask = databaseManager.UpdateHighscore(score);

            await updateHighscoreTask;
        }
    }
}