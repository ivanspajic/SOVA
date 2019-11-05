using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _0._Models;
using _2._Data_Layer_Abstractions;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;

namespace _3._Data_Layer
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly SOVAContext databaseContext;

        public SubmissionRepository(SOVAContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public IEnumerable<Submission> getLatestTenQuestions()
        {
            return databaseContext.Submissions.OrderByDescending(s => s.CreationDate).Take(10);
        }
    }
}
