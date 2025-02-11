using GameSquad.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        public async Task InitializeDatabaseAsync() //operasii asincrone pentru a nu se bloca UI-ul interfetei
        {
            await _database.CreateTableAsync<User>(); //se executa pe rand si se verifica daca tabelul este sau nu creat
            await _database.CreateTableAsync<Position>();
            await _database.CreateTableAsync<Player>();
            await _database.CreateTableAsync<Team>();
            await _database.CreateTableAsync<TeamPlayer>();
            await _database.CreateTableAsync<Match>();
            await _database.CreateTableAsync<MatchTeam>();
            await _database.CreateTableAsync<Record>();
        }

        public SQLiteAsyncConnection GetDatabase() => _database; //obtine conexiunea
    }
}
