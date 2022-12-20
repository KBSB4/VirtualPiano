using Model.DatabaseModels;

namespace Model.Interfaces
{
    public interface IDatabaseManager
    {
		#region Songs
        public Task<Song?> GetSong(string songname);

		public Task<Song?> GetSong(int songId);

		public Task<Song[]> GetAllSongs();

		public Task UploadSong(Song song);

		public Task DeleteSong(string songname);
		#endregion

		#region Users
		public Task<User> GetUser(string username);
        public Task<User> GetUser(int userID);

		public Task UploadNewUser(User user);

		public Task<User[]?> GetAllUsernamesAndPassphrases();
        #endregion

		#region Highscore
		public Task<Highscore[]> GetHighscores(int songId);

		public Task UploadHighscore(Highscore highscore);
		#endregion
	}
}
