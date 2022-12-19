using Microsoft.Data.SqlClient;
using Model;
using Model.DatabaseModels;
using Model.Interfaces;
using System.Data;
using System.Data.Common;

namespace BusinessLogic
{
	public class SQLDatabaseManager : IDatabaseManager
	{
		private const string connectionString = "Data Source=127.0.0.1;" +
			"Initial Catalog=PianoHero;" +
			"Persist Security Info=True;" +
			"User ID=SA;" +
			"Password=Backing-Crumpet4;" +
			"TrustServerCertificate=True;";

		public SQLDatabaseManager()
		{
			//ProgramSSH.ExecuteSshConnection();
		}

		#region Users
		public async Task<User> GetUser(string username)
		{
			throw new NotImplementedException();
		}

		public async Task<User?> GetUser(int id)
		{
			using (SqlConnection connection = new(connectionString))
			{
				string query = "SELECT * FROM UserAccount WHERE idUser = @userId";

				await connection.OpenAsync();

				SqlCommand command = new(query, connection);

				SqlParameter userIdParam = new("@userId", SqlDbType.Int) { Value = id };

				command.Parameters.Add(userIdParam);

				SqlDataReader dataReader = await command.ExecuteReaderAsync();

				User[] users = await ReadUsers(dataReader);

				await CloseAndDispose(connection, command, dataReader);

				if (users.Length > 0)
					return users[0];

				return null;
			}

		}

		private async Task<User[]> ReadUsers(SqlDataReader dataReader)
		{
			List<User> result = new();

			while (await dataReader.ReadAsync())
			{
				result.Add(new User()
				{
					Name = await dataReader.GetFieldValueAsync<string>("username"),
					Id = await dataReader.GetFieldValueAsync<int>("idUser"),
					Password = await dataReader.GetFieldValueAsync<string>("passphrase"),
					Email = await dataReader.GetFieldValueAsync<string>("email"),
					isAdmin = await dataReader.GetFieldValueAsync<byte>("isAdmin") == 0
				});
			}

			return result.ToArray();
		}
		#endregion

		#region Songs
		/// <summary>
		/// Gets the first found song in the Song table using <paramref name="songname"/> to find it.
		/// </summary>
		/// <param name="songname"></param>
		/// <returns>New <see cref="Song"/> with <b>SongId</b>, <b>Name</b>, <b>FullFile</b>, <b>Difficulty</b> and <b>Description</b></returns>
		public async Task<Song?> GetSong(string songname)
		{
			using (SqlConnection connection = new(connectionString))
			{

				string query = "SELECT * FROM Song WHERE name = @name";

				await connection.OpenAsync();

				SqlCommand command = new(query, connection);

				SqlParameter nameParam = new("@name", SqlDbType.VarChar) { Value = songname, Size = songname.Length };

				command.Parameters.Add(nameParam);

				SqlDataReader dataReader = await command.ExecuteReaderAsync();

				Song[] result = await ReadSongs(dataReader);

				await CloseAndDispose(connection, command, dataReader);

				if (result.Length > 0)
					return result[0];

				return null;
			}
		}

		/// <summary>
		/// Gets the first found song in the Song table using <paramref name="songId"/> to find it.
		/// </summary>
		/// <param name="songname"></param>
		/// <returns>New <see cref="Song"/> with <b>SongId</b>, <b>Name</b>, <b>FullFile</b>, <b>Difficulty</b> and <b>Description</b></returns>
		public async Task<Song?> GetSong(int songId)
		{
			using (SqlConnection connection = new(connectionString))
			{
				string query = "SELECT * FROM Song WHERE idSong = @songId";

				await connection.OpenAsync();

				SqlCommand command = new(query, connection);

				SqlParameter songIdParam = new("@songId", SqlDbType.Int) { Value = songId };

				command.Parameters.Add(songIdParam);

				SqlDataReader dataReader = await command.ExecuteReaderAsync();

				Song[] result = await ReadSongs(dataReader);

				await CloseAndDispose(connection, command, dataReader);

				if (result.Length > 0)
					return result[0];

				return null;
			}
		}

		/// <summary>
		/// Deletes a <see cref="Song"/> from database table: <b>Song</b> using <paramref name="songname"/>
		/// </summary>
		/// <param name="songname"></param>
		public async Task DeleteSong(string songname)
		{
			using (SqlConnection connection = new(connectionString))
			{
				string query = "DELETE FROM Song WHERE name = @name";

				await connection.OpenAsync();

				SqlParameter songnameParam = new("@name", SqlDbType.VarChar) { Value = songname, Size = songname.Length };

				SqlCommand command = new(query, connection);

				command.Parameters.Add(songnameParam);

				await command.ExecuteNonQueryAsync();

				await CloseAndDispose(connection, command);
			}
		}

		/// <summary>
		/// Adds <paramref name="song"/> to the SQL Database in the <b>Song</b> table
		/// </summary>
		/// <param name="song"></param>
		public async Task UploadSong(Song song)
		{

			using (SqlConnection connection = new(connectionString))
			{

				string query = "INSERT INTO Song (name, midifile, difficulty, description) VALUES (@name, @file, @difficulty, @description)";

				await connection.OpenAsync();

				SqlParameter midiParameter = new("@file", SqlDbType.VarBinary) { Value = song.FullFile };

				SqlParameter nameParam = new("@name", SqlDbType.VarChar) { Value = song.Name };

				SqlParameter difficultyParam = new("@difficulty", SqlDbType.Int) { Value = song.Difficulty };

				SqlParameter descriptionParam = new("@description", SqlDbType.VarChar) { Value = song.Description };

				SqlCommand command = new(query, connection);

				command.Parameters.AddRange(new SqlParameter[] { midiParameter, nameParam, difficultyParam, descriptionParam });

				await command.ExecuteNonQueryAsync();

				await CloseAndDispose(connection, command);
			}
		}

		/// <summary>
		/// Gets all the songs from the sql database
		/// </summary>
		/// <returns>New <see cref="Song"/>[] with <b>SongId</b>, <b>Name</b>, <b>FullFile</b>, <b>Difficulty</b> and <b>Description</b></returns>
		public async Task<Song[]> GetAllSongs()
		{

			using (SqlConnection connection = new(connectionString))
			{
				string query = "SELECT * FROM Song";

				await connection.OpenAsync();

				SqlCommand command = new(query, connection);

				SqlDataReader dataReader = await command.ExecuteReaderAsync();

				Song[] result = await ReadSongs(dataReader);

				await CloseAndDispose(connection, command, dataReader);

				return result;
			}
		}

		/// <summary>
		/// Uses <paramref name="dataReader"/> to get all field from table Song and creates a new <see cref="Song"/> object with these fields
		/// </summary>
		/// <param name="dataReader"></param>
		/// <returns>New <see cref="Song"/> with <b>SongId</b>, <b>Name</b>, <b>FullFile</b>, <b>Difficulty</b> and <b>Description</b></returns>
		private async Task<Song[]> ReadSongs(SqlDataReader dataReader)
		{
			List<Song> result = new();

			while (await dataReader.ReadAsync())
			{
				result.Add(new Song()
				{
					Name = await dataReader.GetFieldValueAsync<string>("name"),
					Id = await dataReader.GetFieldValueAsync<int>("idSong"),
					FullFile = await dataReader.GetFieldValueAsync<byte[]>("midifile"),
					Difficulty = await dataReader.GetFieldValueAsync<Difficulty>("difficulty"),
					Description = await dataReader.GetFieldValueAsync<string>("description"),
				});
			}

			return result.ToArray();
		}
		#endregion

		#region Highscores
		public async Task<Highscore[]> GetHighscores(int songId)
		{
			using (SqlConnection connection = new(connectionString))
			{
				List<Highscore> highscores = new();

				string query = "SELECT * FROM SongScore WHERE idSong = @songId ORDER BY score DESC";

				await connection.OpenAsync();

				SqlCommand command = new(query, connection);

				SqlParameter songIdParam = new("@songId", SqlDbType.Int) { Value = songId };

				command.Parameters.Add(songIdParam);

				SqlDataReader dataReader = await command.ExecuteReaderAsync();

				while (await dataReader.ReadAsync())
				{
					Highscore highscore = new()
					{
						User = await GetUser(await dataReader.GetFieldValueAsync<int>("idUser")),
						Song = await GetSong(await dataReader.GetFieldValueAsync<int>("idSong")),
						Score = await dataReader.GetFieldValueAsync<int>("score")
					};

					highscores.Add(highscore);
				}

				await CloseAndDispose(connection, command, dataReader);

				return highscores.ToArray();
			}
		}

		public async Task UploadHighscore(Highscore highscore)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string query = "INSERT INTO SongScore (idSong, idUser, score) VALUES (@songId, @userId, @score)";

				SqlCommand command = new(query, connection);

				SqlParameter songIdParam = new SqlParameter("@songId", SqlDbType.Int) { Value = highscore.Song.Id };

				SqlParameter userIdParam = new SqlParameter("@songId", SqlDbType.Int) { Value = highscore.User.Id };

				SqlParameter scoreParam = new SqlParameter("@score", SqlDbType.Int) { Value = highscore.Score };

				command.Parameters.AddRange(new SqlParameter[] { songIdParam, userIdParam, scoreParam});

				await CloseAndDispose(connection, command);
			}
		}
		#endregion

		private async Task CloseAndDispose(SqlConnection connection, SqlCommand command, SqlDataReader dataReader)
		{
			await CloseAndDispose(connection, command);
			await dataReader.DisposeAsync();
		}

		private async Task CloseAndDispose(SqlConnection connection, SqlCommand command)
		{
			await connection.CloseAsync();
			await command.DisposeAsync();
		}
	}
}
