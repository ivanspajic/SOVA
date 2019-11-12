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

        public Question GetById(int submissionId)
        {
            return _databaseContext.Questions.Find(submissionId);
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

        public IEnumerable<SearchResult> SearchQuestions(string queryString, int userId, PagingAttributes pagingAttributes)
        {
            return _databaseContext.SearchResults.FromSqlRaw("SELECT * from best_match_weighted({0})", queryString)
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public int NoOfResults(string queryString)
        {
            return _databaseContext.SearchResults.FromSqlRaw("SELECT * from best_match_weighted({0})", queryString)
                .Count();
        }

        public int NoOfAnswers(int questionId)
        {
            return _databaseContext.Answers
                .Include(a => a.Submission)
                .Count(a => a.ParentId == questionId);
        }
    }
}
