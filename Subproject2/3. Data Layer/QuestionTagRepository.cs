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
    public class QuestionTagRepository : IQuestionTagRepository
    {
        private readonly SOVAContext _databaseContext;

        public QuestionTagRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<QuestionsTag> GetTenRandomQuestionsTag()
        {
            var randomOffSet = new Random().Next(1, 1000);
            return _databaseContext.QuestionsTags.Skip(randomOffSet).Take(10);
        }

        public QuestionsTag GetById(int submissionId)
        {
            return _databaseContext.QuestionsTags.Find(submissionId);
        }
    }
}