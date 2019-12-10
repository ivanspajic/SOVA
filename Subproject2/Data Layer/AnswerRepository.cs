using System.Collections.Generic;
using System.Linq;
using Data_Layer.Database_Context;
using Data_Layer_Abstractions;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data_Layer
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
            return _databaseContext.Answers.Include(a => a.Submission)
                .Include(a => a.Submission.SoMember)
                .Include(a => a.Submission)
                .ThenInclude(submission => submission.Comments)
                .ThenInclude(comment => comment.Submission)
                .ThenInclude(a => a.SoMember)                
                .FirstOrDefault(a => a.SubmissionId == answerId);
        }

        public IEnumerable<Answer> GetAnswersForQuestionById(int questionId)
        {
            var answers = _databaseContext.Answers
                .Include(a => a.Submission)
                .Include(a => a.Submission.SoMember)
                .Where(a => a.ParentId == questionId)
                .ToList();
            if (answers.Count == 0)
                return null;
            return answers;
        }

        public int NoOfAnswers(int questionId)
        {
            return _databaseContext.Answers
                .Include(a => a.Submission)
                .Count(a => a.ParentId == questionId);
        }
    }
}
