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
    public class QuestionRepository : IQuestionRepository
    {
        private readonly SOVAContext _databaseContext;

        public QuestionRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<Question> GetTenRandomQuestions()
        {
            var randomOffSet = new Random().Next(1, 1000);
            return _databaseContext.Questions.Skip(randomOffSet).Take(10);
        }

        // TODO: Fix this function call. Currently it fails with error: `column "chocholate" does not exist`

        //public IEnumerable<Question> SearchQuestions(string queryString)
        //{
        //    return _databaseContext.Questions.FromSqlRaw("SELECT * from best_match({0})", queryString);
        //}

    }
}
