using BusinessLogic;
using Model;
using Model.DatabaseModels;
using Model.Interfaces;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public static class DatabaseController
	{
		private static IDatabaseManager databaseManager = new SQLDatabaseManager();

		public static async Task DeleteSong(string songName)
		{
			Task deleteSongTask = databaseManager.DeleteSong(songName);

			await deleteSongTask;
		}

		public static async Task<Song?> GetSong(string songName)
		{
			Task<Song?> getSongTask = databaseManager.GetSong(songName);

			Song? result = await getSongTask;

			return result;
		}

		public static async Task<Song?> GetSong(int songId)
		{
			Task<Song?> getSongTask = databaseManager.GetSong(songId);

			Song? result = await getSongTask;

			return result;
		}

		public static async Task<Song[]> GetAllSongs()
		{
			Task<Song[]> getAllSongsTask = databaseManager.GetAllSongs();

			Song[] result = await getAllSongsTask;

			return result;
		}

		public static async Task UploadSong(Song song)
		{
			Task uploadSongTask = databaseManager.UploadSong(song);

			await uploadSongTask;
		}

		public static async Task<Highscore[]> GetHighscores(int songId)
		{
			Task<Highscore[]> getHighscoresTask = databaseManager.GetHighscores(songId);

			Highscore[] result = await getHighscoresTask;

			return result;
		}
	}


}
