using System;
using System.Collections.Generic;
using System.Text;
using _0._Models;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace _3._Data_Layer
{
    public class AnswerRepository
    {
        private readonly SOVAContext databaseContext;

        public AnswerRepository(SOVAContext databaseContext)
        {
            this.databaseContext = databaseContext;

        }

        public IEnumerable<Answer> GetLatestTenAnswers()
        {
            return databaseContext.Answers.OrderByDescending(s => s.Submission.CreationDate).Take(10);
        }

    }
}
