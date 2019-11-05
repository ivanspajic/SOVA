using _2._Data_Layer_Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3._Data_Layer
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly DbContext databaseContext;

        public HistoryRepository(DbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
    }
}
