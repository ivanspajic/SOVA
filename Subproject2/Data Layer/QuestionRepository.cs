using System;
using System.Collections.Generic;
using System.Linq;
using Data_Layer.Database_Context;
using Data_Layer_Abstractions;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data_Layer
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly SOVAContext _databaseContext;

        public QuestionRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<Question> GetQuestions(PagingAttributes pagingAttributes)
        {
            var randomOffSet = new Random().Next(1, 1000);
            return _databaseContext.Questions.Skip(randomOffSet).Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public Question GetById(int submissionId)
        {
            return _databaseContext.Questions.Find(submissionId);
        }

        public List<QuestionsTag> GetQuestionsByTags(string tagName, PagingAttributes pagingAttributes)
        {
            var tag = _databaseContext.Tags.FirstOrDefault(t => t.TagString == tagName);
            if (tag == null)
                return null;
            return _databaseContext.QuestionsTags.Include(qt => qt.Question).Where(qt => qt.TagId == tag.Id).Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public IEnumerable<SearchResult> SearchQuestions(string queryString, int? userId, PagingAttributes pagingAttributes)
        {
            if (queryString == null)
                return null;
            return _databaseContext.SearchResults.FromSqlRaw("SELECT * from best_match_weighted({0}, {1})", userId, queryString)
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public int NoOfResults(string queryString, int? userId)
        {
            if (queryString == null || userId < 1)
                return 0;
            return _databaseContext.SearchResults.FromSqlRaw("SELECT * from best_match_weighted({0}, {1})", userId, queryString)
                .Count();
        }

        public int NoOfRandomQuestions()
        {
            var randomOffSet = new Random().Next(1, 1000);
            return _databaseContext.Questions.Skip(randomOffSet).Count();
        }
    }
}
