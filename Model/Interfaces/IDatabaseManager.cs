using Model.DatabaseModels;

namespace Model.Interfaces
{
    public interface IDatabaseManager
    {
		#region Songs
        public Task<Song?> GetSong(string songname);

		public Task<Song[]> GetAllSongs();

		public Task UploadSong(Song song);

		public Task DeleteSong(string songname);
		#endregion

		#region Users
		public Task<User> GetUser(string username);
		#endregion


	}
}
