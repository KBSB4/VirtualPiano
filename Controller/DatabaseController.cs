using BusinessLogic;
using Model;
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
		private static TaskManager taskManager = new TaskManager();

		private static IDatabaseManager databaseManager = new SQLDatabaseManager();

		public static async Task DeleteSong(string songName)
		{
			Task deleteSongTask = databaseManager.DeleteSong(songName);

			await taskManager.QueueAndWait(deleteSongTask);

			await deleteSongTask;

			taskManager.CompleteTask();
		}

		public static async Task<Song?> GetSong(string songName)
		{
			Task<Song?> getSongTask = databaseManager.GetSong(songName);

			await taskManager.QueueAndWait(getSongTask);

			Song? result = await getSongTask;

			taskManager.CompleteTask();

			return result;
		}

		public static async Task<Song?> GetSong(int songId)
		{
			Task<Song?> getSongTask = databaseManager.GetSong(songId);

			await taskManager.QueueAndWait(getSongTask);

			Song? result = await getSongTask;

			taskManager.CompleteTask();

			return result;
		}

		public static async Task<Song[]> GetAllSongs()
		{
			Task<Song[]> getAllSongsTask = databaseManager.GetAllSongs();

			await taskManager.QueueAndWait(getAllSongsTask);

			Song[] result = await getAllSongsTask;

			taskManager.CompleteTask();

			return result;
		}

		public static async Task UploadSong(Song song)
		{
			Task uploadSongTask = databaseManager.UploadSong(song);

			await taskManager.QueueAndWait(uploadSongTask);

			await uploadSongTask;

			taskManager.CompleteTask();
		}
	}


}
