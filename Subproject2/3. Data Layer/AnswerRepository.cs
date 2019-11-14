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

        public Answer GetAnswerById(int answerId)
        {
            return _databaseContext.Answers.Include(a => a.Submission).FirstOrDefault(a => a.SubmissionId == answerId);
        }

        public IEnumerable<Answer> GetAnswersForQuestionById(int questionId, PagingAttributes pagingAttributes)
        {
            return _databaseContext.Answers
                .Include(a => a.Submission)
                .Where(a => a.ParentId == questionId)
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public int NoOfAnswers(int questionId)
        {
            return _databaseContext.Answers
                .Include(a => a.Submission)
                .Count(a => a.ParentId == questionId);
        }
    }
}
