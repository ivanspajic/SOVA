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
        private readonly SOVAContext _databaseContext;

        public AnswerRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<Answer> GetAnswersForQuestionById(int questionId)
        {
            return _databaseContext.Answers.Include(a => a.Submission).Where(a => a.ParentId == questionId);
        }
    }
}
