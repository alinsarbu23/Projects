using GameSquad.Model;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameSquad.Service
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gamesquad.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            InitializeDatabaseAsync().ConfigureAwait(false);
        }

        public async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Position>();
            await _database.CreateTableAsync<Player>();
            await _database.CreateTableAsync<Team>();
            await _database.CreateTableAsync<TeamPlayer>();
            await _database.CreateTableAsync<Match>();
            await _database.CreateTableAsync<MatchTeam>();
            await _database.CreateTableAsync<Record>();
        }

        public SQLiteAsyncConnection GetDatabase() => _database;
    }
}
