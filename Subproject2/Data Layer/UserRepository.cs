using System.Collections.Generic;
using System.Linq;
using Data_Layer.Database_Context;
using Data_Layer_Abstractions;
using Models;

namespace Data_Layer
{
    public class UserRepository : IUserRepository
    {
        private readonly SOVAContext _databaseContext;

        public UserRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        // The following method "GetUsers" is only for debugging. Only sys-admins should be able to get all users.
        public IEnumerable<User> GetUsers()
        {
            return _databaseContext.Users;
        }
        public User GetUserById(int userId)
        {
            var user = _databaseContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            return user;
        }
        public User GetUserByUsername(string username)
        {
            if (username == null)
            {
                return null;
            }
            var user = _databaseContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public User CreateUser(string username, string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(salt))
            {
                return null;
            }

            if (_databaseContext.Users.FirstOrDefault(v => v.Username.ToLower() == username.ToLower()) != null) // This is only for test to create a user with username "testUser". If it exists, it doesn't create again. For the application, there's a check in the controller.
            {
                return null;
            }
            // If the Users table is not empty, increment the existing ID by 1; else set the ID to 1.
            var userId = _databaseContext.Users.Any() ? _databaseContext.Users.Max(x => x.Id) + 1 : 1;
            var user = new User()
            {
                Id = userId,
                Username = username,
                Password = password,
                Salt = salt

            };
            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();
            return user;
        }

        public User UpdateUser(int userId, string? updatedUsername, string? updatedPassword, string? updatedSalt)
        {
            //if (updatedUsername == null || (updatedPassword == null && updatedSalt == null))
            //{
            //    return null;
            //}

            var user = GetUserById(userId);
            if (!string.IsNullOrWhiteSpace(updatedUsername) && user.Username != updatedUsername)
            {
                user.Username = updatedUsername;
            }
            if (!string.IsNullOrWhiteSpace(updatedPassword) && !string.IsNullOrWhiteSpace(updatedSalt))
            {
                user.Password = updatedPassword;
                user.Salt = updatedSalt;
            }
            _databaseContext.Users.Update(user);
            _databaseContext.SaveChanges();
            return user;
        }
        public bool DeleteUser(string username)
        {
            var user = GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }
            _databaseContext.Users.Remove(user);
            _databaseContext.SaveChanges();
            return true;
        }
    }
}
