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
    public class UserHistoryRepository : IUserHistoryRepository
    {
        private readonly SOVAContext _databaseContext;

        public UserHistoryRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<UserHistory> GetUserHistoryByUserId(int userId, PagingAttributes pagingAttributes)
        {
            if (userId <= 0) // User Id is auto generated and is always positive.
            {
                return null;
            }
            return _databaseContext.UserHistory.Include(u => u.History).Where(u => u.UserId == userId).Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public int NoOfUserHistory(int userId)
        {
            return _databaseContext.UserHistory.Count(u => u.UserId == userId);
        }
    }
}