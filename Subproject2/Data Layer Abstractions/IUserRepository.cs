using System.Collections.Generic;
using Models;

namespace Data_Layer_Abstractions
{
    public interface IUserRepository
    {
        User CreateUser(string username, string password, string salt);
        User GetUserById(int userId);
        User GetUserByUsername(string username);
        User UpdateUser(int userId, string? username, string? password, string salt);
        IEnumerable<User> GetUsers();
        bool DeleteUser(string username);
    }
}
