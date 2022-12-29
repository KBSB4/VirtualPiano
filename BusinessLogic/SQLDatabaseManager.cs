using Microsoft.Data.SqlClient;
using Model;
using Model.DatabaseModels;
using Model.Interfaces;
using System.Data;

namespace BusinessLogic
{
    public class SQLDatabaseManager : IDatabaseManager
    {
        private static readonly string connectionString =
            "Server=127.0.0.1;" +
            "User ID=SA;" +
            "Password=Backing-Crumpet4;" +
            "Encrypt=yes;" +
            "Trusted_Connection=no;" +
            "TrustServerCertificate=True;" +
            "Initial Catalog=PianoHero;";

        /// <summary>
        /// Start connection
        /// </summary>
        public SQLDatabaseManager()
        {
            new Thread(new ThreadStart(Connect)).Start();
        }

        /// <summary>
        /// Connects to the SSH on port 1433.
        /// </summary>
        private void Connect()
        {
            ProgramSSH.ExecuteSshConnection();
        }

        #region Users

        /// <summary>
        /// Gets <see cref="User"/> by the given <paramref name="username"/> from the SQL database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>If <paramref name="username"/> exists in the database returns: New <see cref="User"/>. Otherwise returns null.</returns>
        public async Task<User?> GetUserByName(string username)
        {
            using (SqlConnection connection = new(connectionString))
            {
                string query = "SELECT * FROM UserAccount WHERE username = @username";

                await connection.OpenAsync();

                SqlCommand command = new(query, connection);

                SqlParameter userNameParam = new("@username", SqlDbType.VarChar) { Value = username };

                command.Parameters.Add(userNameParam);

                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                User[] users = await ReadUsers(dataReader);

                await CloseAndDispose(connection, command, dataReader);

                if (users.Length > 0)
                    return users[0];

                return null;
            }
        }

        /// <summary>
        /// Gets <see cref="User"/> by the given <paramref name="id"/> from the SQL database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If <paramref name="id"/> exists in the database returns: New <see cref="User"/>. Otherwise returns null.</returns>
        public async Task<User?> GetUserById(int id)
        {
            using SqlConnection connection = new(connectionString);
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

        /// <summary>
        /// Gets the user with the corresponding <paramref name="username"/> and <paramref name="password"/> from the SQL database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>If the <paramref name="username"/> and <paramref name="password"/> match returns: New <see cref="User"/>. Otherwise returns null.</returns>
        public async Task<User?> GetLoggingInUser(string username, string password)
        {
            using SqlConnection connection = new(connectionString);
            string query = "SELECT * FROM UserAccount WHERE username = @username AND passphrase = @passphrase";

            await connection.OpenAsync();

            SqlCommand command = new(query, connection);

            SqlParameter usernameParam = new("@username", SqlDbType.VarChar) { Value = username };

            SqlParameter passwordParam = new("@passphrase", SqlDbType.VarChar) { Value = password };

            command.Parameters.Add(usernameParam);

            command.Parameters.Add(passwordParam);

            SqlDataReader dataReader = await command.ExecuteReaderAsync();

            User[] users = await ReadUsers(dataReader);

            await CloseAndDispose(connection, command, dataReader);

            if (users.Length > 0)
                if (username == users[0].Name && password == users[0].Password) return users[0];
            return null;
        }

        /// <summary>
        /// Gets all the users from the sql database.
        /// </summary>
        /// <returns>New <see cref="User"/>[] with <b>UserId</b>, <b>Name</b>, <b>Password</b>, <b>Email</b> and <b>IsAdmin</b></returns>
        public async Task<User[]?> GetAllUsers()
        {
            using SqlConnection connection = new(connectionString);
            string query = "SELECT * FROM UserAccount";

            await connection.OpenAsync();

            SqlCommand command = new(query, connection);

            SqlDataReader dataReader = await command.ExecuteReaderAsync();

            User[] result = await ReadUsers(dataReader);

            await CloseAndDispose(connection, command, dataReader);

            return result;
        }

        /// <summary>
        /// Uses <paramref name="dataReader"/> to get all fields from table UserAccount and creates a new <see cref="User"/> object with these fields.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns>New <see cref="User"/> with <b>UserId</b>, <b>Name</b>, <b>Password</b>, <b>Email</b> and <b>IsAdmin</b></returns>
        private static async Task<User[]> ReadUsers(SqlDataReader dataReader)
        {
            List<User> result = new();

            while (await dataReader.ReadAsync())
            {
                result.Add(new User()
                {
                    Name = await dataReader.GetFieldValueAsync<string>("username"),
                    Id = await dataReader.GetFieldValueAsync<int>("idUser"),
                    Password = await dataReader.GetFieldValueAsync<string>("passphrase"),
                    Email = await dataReader.IsDBNullAsync("email") ? null : await dataReader.GetFieldValueAsync<string>("email"),
                    IsAdmin = await dataReader.GetFieldValueAsync<byte>("isAdmin") != 0
                });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Adds <paramref name="user"/> to the SQL Database in the <b>UserAccount</b> table.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UploadNewUser(User user)
        {
            using SqlConnection connection = new(connectionString);

            string query = "INSERT INTO UserAccount (username, passphrase, isAdmin, email) VALUES (@username, @passphrase, @isAdmin, @email)";

            await connection.OpenAsync();

            SqlCommand command = new(query, connection);

            SqlParameter usernameParam = new("@username", SqlDbType.VarChar) { Value = user.Name };

            SqlParameter passwordParam = new("@passphrase", SqlDbType.VarChar) { Value = user.Password };

            SqlParameter isAdminParam = new("@isAdmin", SqlDbType.TinyInt) { Value = user.IsAdmin };

            SqlParameter emailParam = new("@email", SqlDbType.VarChar) { Value = user.Email };

            command.Parameters.AddRange(new SqlParameter[] { usernameParam, passwordParam, isAdminParam, emailParam });

            await command.ExecuteNonQueryAsync();

            await CloseAndDispose(connection, command);
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
            using SqlConnection connection = new(connectionString);
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

        /// <summary>
        /// Gets the first found song in the Song table using <paramref name="songId"/> to find it.
        /// </summary>
        /// <param name="songname"></param>
        /// <returns>New <see cref="Song"/> with <b>SongId</b>, <b>Name</b>, <b>FullFile</b>, <b>Difficulty</b> and <b>Description</b></returns>
        public async Task<Song?> GetSong(int songId)
        {
            using SqlConnection connection = new(connectionString);
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

        /// <summary>
        /// Deletes a <see cref="Song"/> from database table: <b>Song</b> using <paramref name="songname"/>
        /// </summary>
        /// <param name="songname"></param>
        public async Task DeleteSong(string songname)
        {
            using SqlConnection connection = new(connectionString);
            string query = "DELETE FROM Song WHERE name = @name";

            await connection.OpenAsync();

            SqlParameter songnameParam = new("@name", SqlDbType.VarChar) { Value = songname, Size = songname.Length };

            SqlCommand command = new(query, connection);

            command.Parameters.Add(songnameParam);

            await command.ExecuteNonQueryAsync();

            await CloseAndDispose(connection, command);
        }

        /// <summary>
        /// Adds <paramref name="song"/> to the SQL Database in the <b>Song</b> table.
        /// </summary>
        /// <param name="song"></param>
        public async Task UploadSong(Song song)
        {
            using SqlConnection connection = new(connectionString);

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

        /// <summary>
        /// Gets all the songs from the sql database
        /// </summary>
        /// <returns>New <see cref="Song"/>[] with <b>SongId</b>, <b>Name</b>, <b>FullFile</b>, <b>Difficulty</b> and <b>Description</b></returns>
        public async Task<Song[]?> GetAllSongs()
        {
            using SqlConnection connection = new(connectionString);
            string query = "SELECT * FROM Song";

            await connection.OpenAsync();

            SqlCommand command = new(query, connection);

            SqlDataReader dataReader = await command.ExecuteReaderAsync();

            Song[] result = await ReadSongs(dataReader);

            await CloseAndDispose(connection, command, dataReader);

            return result;
        }

        /// <summary>
        /// Uses <paramref name="dataReader"/> to get all fields from table Song and creates a new <see cref="Song"/> object with these fields
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns>New <see cref="Song"/> with <b>SongId</b>, <b>Name</b>, <b>FullFile</b>, <b>Difficulty</b> and <b>Description</b></returns>
        private static async Task<Song[]> ReadSongs(SqlDataReader dataReader)
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
                    Description = await dataReader.GetFieldValueAsync<string?>("description"),
                });
            }

            return result.ToArray();
        }
        #endregion

        #region Highscores
        /// <summary>
        /// Gets all highscores of a song by <paramref name="songId"/>
        /// </summary>
        /// <param name="songId"></param>
        /// <returns><see cref="Highscore"/> array</returns>
        public async Task<Highscore[]?> GetHighscores(int songId)
        {
            using SqlConnection connection = new(connectionString);
            List<Highscore> highscores = new();

            string query = "SELECT * FROM SongScore WHERE idSong = @songId ORDER BY score DESC";

            await connection.OpenAsync();

            SqlCommand command = new(query, connection);

            SqlParameter songIdParam = new("@songId", SqlDbType.Int) { Value = songId };

            command.Parameters.Add(songIdParam);

            SqlDataReader dataReader = await command.ExecuteReaderAsync();

            while (await dataReader.ReadAsync())
            {
                User? user = await GetUserById(await dataReader.GetFieldValueAsync<int>("idUser"));
                Song? song = await GetSong(await dataReader.GetFieldValueAsync<int>("idSong"));
                if (user is null || song is null)
                {
                    continue;
                }
                Highscore highscore = new()
                {
                    User = user,
                    Song = song,
                    Score = await dataReader.GetFieldValueAsync<int>("score")
                };

                highscores.Add(highscore);
            }

            await CloseAndDispose(connection, command, dataReader);

            return highscores.ToArray();
        }

        /// <summary>
        /// Uploads score based on what is inside <paramref name="highscore"/>
        /// </summary>
        /// <param name="highscore"></param>
        /// <returns><see cref="Task"/></returns>
        public async Task UploadHighscore(Highscore highscore)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO SongScore (idSong, idUser, score) VALUES (@songId, @userId, @score)";

                await connection.OpenAsync();

                SqlParameter songIdParam = new("@songId", SqlDbType.Int)
                {
                    Value = highscore.Song.Id
                };

                SqlParameter userIdParam = new SqlParameter("@userId", SqlDbType.Int) { Value = highscore.User.Id };

                SqlParameter scoreParam = new SqlParameter("@score", SqlDbType.Int) { Value = highscore.Score };

                SqlCommand command = new(query, connection);

                command.Parameters.AddRange(new SqlParameter[] { songIdParam, userIdParam, scoreParam });

                await command.ExecuteNonQueryAsync();

                await CloseAndDispose(connection, command);
            }
        }

        /// <summary>
        /// Updates highscore based on what is inside <paramref name="highscore"/>
        /// </summary>
        /// <param name="highscore"></param>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateHighscore(Highscore highscore)
        {
            using SqlConnection connection = new(connectionString);
            string query = "UPDATE SongScore SET score = @score WHERE idSong = @songId AND idUser = @userId";

            await connection.OpenAsync();

            SqlParameter songIdParam = new("@songId", SqlDbType.Int) { Value = highscore.Song.Id };

            SqlParameter userIdParam = new("@userId", SqlDbType.Int) { Value = highscore.User.Id };

            SqlParameter scoreParam = new("@score", SqlDbType.Int) { Value = highscore.Score };

            SqlCommand command = new(query, connection);

            command.Parameters.AddRange(new SqlParameter[] { songIdParam, userIdParam, scoreParam });

            await command.ExecuteNonQueryAsync();

            await CloseAndDispose(connection, command);
        }

        #endregion

        /// <summary>
        /// Disposes SQL query with includes datareader
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <param name="dataReader"></param>
        /// <returns><see cref="Task"/></returns>
        private async Task CloseAndDispose(SqlConnection connection, SqlCommand command, SqlDataReader dataReader)
        {
            await CloseAndDispose(connection, command);
            await dataReader.DisposeAsync();
        }

        /// <summary>
        /// Disposes SQL query
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns><see cref="Task"/></returns>
        private async Task CloseAndDispose(SqlConnection connection, SqlCommand command)
        {
            await connection.CloseAsync();
            await command.DisposeAsync();
        }
    }
}