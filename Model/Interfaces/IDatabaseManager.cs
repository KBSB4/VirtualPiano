using Model.DatabaseModels;

namespace Model.Interfaces
{
	public interface IDatabaseManager
	{
		#region Songs
		public Task<Song?> GetSong(string songname);

        public Task<Song?> GetSong(int songId);

		public Task<Song[]?> GetAllSongs();

        public Task UploadSong(Song song);

        public Task DeleteSong(string songname);
        #endregion

		#region Users
		public Task<User> GetUserByName(string username);
        public Task<User?> GetUserById(int userID);

		public Task UploadNewUser(User user);

		public Task<User[]?> GetAllUsers();

		public Task<User?> GetLoggingInUser(string username, string password);
        #endregion

		#region Highscore
		public Task<Highscore[]?> GetHighscores(int songId);

		public Task UploadHighscore(Highscore highscore);
		public Task UpdateHighscore(Highscore score);
		#endregion
	}
}