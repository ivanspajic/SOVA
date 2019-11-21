using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using _0._Models;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using _2._Data_Layer_Abstractions;
using Npgsql;

namespace _3._Data_Layer
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
            return _databaseContext.Users.FirstOrDefault(u => u.Id == userId);
        }
        public User GetUserByUsername(string username)
        {
            return _databaseContext.Users.FirstOrDefault(u => u.Username == username);
        }
        public User CreateUser(string username, string password, string salt)
        {
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
            var user = GetUserById(userId);
            if (updatedUsername != null)
            {
                user.Username = updatedUsername;
            }
            if (updatedPassword != null || updatedSalt != null)
            {
                user.Password = updatedPassword;
                user.Salt = updatedSalt;
            }
            _databaseContext.Users.Update(user);
            _databaseContext.SaveChanges();
            return user;
        }
    }
}
