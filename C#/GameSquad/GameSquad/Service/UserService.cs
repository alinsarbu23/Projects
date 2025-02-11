using GameSquad.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSquad.Service
{
    public class UserService
    {
        private readonly SQLiteAsyncConnection _database;
        public UserService(DatabaseService database)
        {
            _database = database.GetDatabase();
        }

        public async Task<bool> RegisterUser(User user)
        {
            var existingUser = await _database.Table<User>().Where(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return false;
            }

            await _database.InsertAsync(user);
            return true;
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            return await _database.Table<User>().Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
        }
    }
}
