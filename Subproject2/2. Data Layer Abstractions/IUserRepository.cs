using System;
using System.Collections.Generic;
using System.Text;
using _0._Models;

namespace _2._Data_Layer_Abstractions
{
    public interface IUserRepository
    {
        User CreateUser(string username, string password, string salt);
        User GetUserById(int userId);
        User GetUserByUsername(string username);
        IEnumerable<User> GetUsers();
    }
}
