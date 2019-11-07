﻿using _0._Models;
using _2._Data_Layer_Abstractions;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public History GetSingleForUser(int id, string username)
        {
            return databaseContext.History.Include("").Where(h => h.Id == id).FirstOrDefault();
        }
    }
}
