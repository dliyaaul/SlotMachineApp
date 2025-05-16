using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachineApp.Services
{
    public class AuthService
    {
        private readonly Dictionary<int, (string Username, string Password, string Role, int Balance)> _users = new();
        private int _currentUserId = 1;

        public AuthService()
        {
            if (_users.Count == 0)
            {
                _users.Add(_currentUserId++, ("admin", "123", "admin", 0));
            }
        }

        public bool Login(string username, string password, out int userId, out string role, out int balance)
        {
            userId = -1;
            role = string.Empty;
            balance = 0;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username/password tidak boleh kosong");

            foreach (var user in _users)
            {
                if (user.Value.Username == username && user.Value.Password == password)
                {
                    userId = user.Key;
                    role = user.Value.Role;
                    balance = user.Value.Balance;
                    return true;
                }
            }

            return false;
        }

        public bool Register(string username, string password, out int userId, out string role)
        {
            userId = -1;
            role = "user";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username/password tidak boleh kosong");

            if (_users.Values.Any(u => u.Username == username))
            {
                Console.WriteLine("Nama Sudah Diambil");
                return false;
                
            }

            if (_users.Count == 0)
            {
                role = "admin";
            }

            userId = _currentUserId++;
            _users.Add(userId, (username, password, role, 1000));
            return true;
        }

        public List<(string Username, string Password, string Role, int Balance)> GetUsers()
        {
            return _users.Values.Select(u => (u.Username,u.Password, u.Role, u.Balance)).ToList();
        }

        public bool AddBalance(string username, int amount)
        {
            foreach (var user in _users)
            {
                if (user.Value.Username == username)
                {
                    var updatedUser = user.Value;
                    updatedUser.Balance += amount;

                    _users[user.Key] = updatedUser;
                    return true;
                }
            }

            return false;
        }

        public bool DecreaseBalance(string username, int amount)
        {
            foreach (var user in _users)
            {
                if (user.Value.Username == username)
                {
                    var updatedUser = user.Value;
                    updatedUser.Balance -= amount;

                    _users[user.Key] = updatedUser;
                    return true;
                }
            }

            return false;
        }
    }
}
