using System.Collections.Generic;
using Models;

namespace Data_Layer_Abstractions
{
    public interface IUserHistoryRepository
    {
        IEnumerable<UserHistory> GetUserHistoryByUserId(int userId);
        int NoOfUserHistory(int userId);
    }
}
