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
        private readonly SOVAContext _databaseContext;

        public HistoryRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public History GetHistoryForUser(int id)
        {
            return _databaseContext.History.FirstOrDefault(h => h.Id == id);
        }
    }
}
