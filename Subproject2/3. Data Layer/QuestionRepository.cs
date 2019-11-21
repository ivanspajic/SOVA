﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using _0._Models;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using _2._Data_Layer_Abstractions;
using Npgsql;
using System.Diagnostics;

namespace _3._Data_Layer
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
            try
            {
                return _databaseContext.Questions
                    .Include(question => question.Submission)
                        .ThenInclude(submission => submission.Comments)
                            .ThenInclude(comment => comment.CommentSubmission)
                    .Include(question => question.QuestionsTags)
                        .ThenInclude(questionTag => questionTag.Tag)
                    .Include(question => question.Answers)
                        .ThenInclude(answer => answer.Submission)
                            .ThenInclude(submission => submission.Comments)
                                .ThenInclude(comment => comment.CommentSubmission)
                    .Include(question => question.LinkedPosts)
                        .ThenInclude(linkPost => linkPost.LinkedPost)
                            .ThenInclude(linkedPost => linkedPost.Submission)
                    .FirstOrDefault(question => question.SubmissionId == submissionId);
            }
            catch (PostgresException x)
            {
                Debug.WriteLine(x.Message);
            }
            return null;
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
            if (queryString == null)
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
