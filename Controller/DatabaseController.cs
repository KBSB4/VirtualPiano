using BusinessLogic;
using Model;
using Model.Interfaces;
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
			await databaseManager.DeleteSong(songName);
		}

		public static async Task<Song> GetSong(string songName)
		{
			return await databaseManager.GetSong(songName);
		}

		public static async Task<Song[]> GetAllSongs()
		{
			return await databaseManager.GetAllSongs();
		}
	}
}
