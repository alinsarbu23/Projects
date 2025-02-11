using GameSquad.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSquad.Service
{
    public class AuthService
    {
        private readonly UserService _userService;
        private User? _currentUser;

        public AuthService(UserService userService)
        {
            _userService = userService;
        }
        public async Task<bool> Login(string username, string password)
        {
            var user = await _userService.AuthenticateUser(username, password);
            if (user != null)
            {
                _currentUser = user;
                return true;
            }
            return false;
        }
        public void Logout()
        {
            _currentUser = null;
        }
        public User? GetCurrentUser()
        {
            return _currentUser;
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            return await _userService.RegisterUser(user);
        }

    }
}
