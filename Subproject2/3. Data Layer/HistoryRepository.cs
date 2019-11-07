using _2._Data_Layer_Abstractions;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3._Data_Layer
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly SOVAContext databaseContext;

        public HistoryRepository(SOVAContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
    }
}
