using System;
using System.Collections.Generic;
using System.Text;
using _0._Models;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using _2._Data_Layer_Abstractions;

namespace _3._Data_Layer
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly SOVAContext databaseContext;

        public AnswerRepository(SOVAContext databaseContext)
        {
            this.databaseContext = databaseContext;

        }
    }
}
