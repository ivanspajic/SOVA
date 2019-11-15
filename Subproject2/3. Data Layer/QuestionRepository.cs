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
            return _databaseContext.Questions
                .Include(question => question.Submission)
                .Include(question => question.Comments)
                    .ThenInclude(comment => comment.CommentSubmission)
                .Include(question => question.LinkedPosts)
                    .ThenInclude(linkedPosts => linkedPosts.LinkedPost)
                        .ThenInclude(linkedPost => linkedPost.Submission)
                .Include(question => question.QuestionsTags)
                    .ThenInclude(questionsTags => questionsTags.Tag)
                .Where(question => question.SubmissionId == submissionId)
                .FirstOrDefault();
        }

        public IEnumerable<SearchResult> SearchQuestions(string queryString, int? userId, PagingAttributes pagingAttributes)
        {
            return _databaseContext.SearchResults.FromSqlRaw("SELECT * from best_match_weighted({0}, {1})", userId, queryString)
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public int NoOfResults(string queryString, int? userId)
        {
            return _databaseContext.SearchResults.FromSqlRaw("SELECT * from best_match_weighted({0}, {1})", userId, queryString)
                .Count();
        }

        public IEnumerable<Question> GetMarkedQuestions(int userId, PagingAttributes pagingAttributes)
        {
            return _databaseContext.Questions
                .Include(question => question.Submission)
                    .ThenInclude(submission => submission.Markings)
                .Where(question => question.Submission.Markings.All(marking => marking.UserId == userId))
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize);
        }
    }
}
