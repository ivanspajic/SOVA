using Data_Layer.Database_Context;
using Data_Layer_Abstractions;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _databaseContext.Questions.Include(q => q.Submission).ThenInclude(q => q.SoMember).Include(q => q.QuestionsTags).ThenInclude(q => q.Tag).Skip(randomOffSet).Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public Question GetById(int submissionId)
        {
            return _databaseContext.Questions
                .Include(q => q.Submission.SoMember)
                .Include(question => question.Submission)
                    .ThenInclude(submission => submission.Comments)
                        .ThenInclude(comment => comment.Submission)
                .Include(question => question.Submission)
                    .ThenInclude(submission => submission.Comments)
                        .ThenInclude(comment => comment.Submission.SoMember)
                .Include(question => question.QuestionsTags)
                    .ThenInclude(questionTag => questionTag.Tag)
                .Include(question => question.Answers)
                    .ThenInclude(answer => answer.Submission)
                        .ThenInclude(submission => submission.Comments)
                            .ThenInclude(comment => comment.Submission)
                .Include(question => question.Answers)
                    .ThenInclude(answer => answer.Submission)
                        .ThenInclude(submission => submission.Comments)
                            .ThenInclude(comment => comment.Submission.SoMember)
                .Include(question => question.Answers)
                    .ThenInclude(a => a.Submission.SoMember)
                .Include(question => question.LinkedPosts)
                    .ThenInclude(linkPost => linkPost.LinkedPost)
                        .ThenInclude(linkedPost => linkedPost.Submission)
                .FirstOrDefault(question => question.SubmissionId == submissionId);
        }

        public IEnumerable<QuestionsTag> GetQuestionsByTags(string tagName, PagingAttributes pagingAttributes)
        {
            var tag = _databaseContext.Tags.FirstOrDefault(t => t.TagString == tagName);
            if (tag == null)
                return null;
            return _databaseContext.QuestionsTags.Include(qt => qt.Question)
                .Include(qt => qt.Question.Submission)
                    .ThenInclude(sub => sub.SoMember)
                .Where(qt => qt.TagId == tag.Id)
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }
        public IEnumerable<QuestionsTag> GetQuestionsTags(int questionId)
        {
            var questionTags = _databaseContext.QuestionsTags.Include(qt => qt.Tag).Where(qt => qt.QuestionId == questionId);
            if (!questionTags.Any())
                return null;
            return questionTags.ToList();
        }

        public IEnumerable<SearchResult> SearchQuestions(string queryString, int? userId, PagingAttributes pagingAttributes)
        {
            if (queryString == null)
                return null;
            var newStr = "\'";
            newStr += queryString.Replace(" ", "\', \'");
            newStr += "\'";

            var query = $"SELECT * from best_match_weighted({userId}, {newStr})";


            return _databaseContext.SearchResults.FromSqlRaw(query)
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public IEnumerable<CloudElement> GetWord2Words(string queryString, int? userId)
        {
            if (queryString == null)
                return null;
            var newStr = "\'";
            newStr += queryString.Replace(" ", "\', \'");
            newStr += "\'";

            var query = $"SELECT * from word_2_words({userId}, {newStr})";

            return _databaseContext.CloudElements.FromSqlRaw(query);
        }

        public int NoOfResults(string queryString, int? userId)
        {
            if (queryString == null)
                return 0;
            var newStr = "\'";
            newStr += queryString.Replace(" ", "\', \'");
            newStr += "\'";

            var query = $"SELECT * from best_match_weighted({userId}, {newStr})";

            return _databaseContext.SearchResults.FromSqlRaw(query).Count();
        }

        public int NoOfResultsForTag(string tagString, int? userId)
        {
            var tag = _databaseContext.Tags.FirstOrDefault(t => t.TagString == tagString);
            if (tag == null || userId < 1)
                return 0;
            return _databaseContext.QuestionsTags.Include(qt => qt.Question).Where(qt => qt.TagId == tag.Id).Count();
        }

        public int NoOfRandomQuestions()
        {
            var randomOffSet = new Random().Next(1, 1000);
            return _databaseContext.Questions.Skip(randomOffSet).Count();
        }
    }
}
