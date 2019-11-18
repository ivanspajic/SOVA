﻿using System.Collections.Generic;
using _0._Models;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using _2._Data_Layer_Abstractions;

namespace _3._Data_Layer
{
    public class MarkingRepository : IMarkingRepository
    {
        private readonly SOVAContext _databaseContext;

        public MarkingRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public bool AddBookmark(int submissionId, int userId)
        {
            if (_databaseContext.Users.Find(userId) == null || _databaseContext.Submissions.Find(submissionId) == null)
                return false;

            if (IsMarked(submissionId, userId))
                return false;
            var mark = new Marking
            {
                SubmissionId = submissionId,
                UserId = userId
            };

            _databaseContext.Markings.Add(mark);
            _databaseContext.SaveChanges();
            return true;
        }

        public bool RemoveBookmark(int submissionId, int userId)
        {
            if (!IsMarked(submissionId, userId))
                return false;
            var mark = _databaseContext.Markings.FromSqlRaw("Select * from markings where submission_id={0} and user_id={1}",
                submissionId, userId).FirstOrDefault();
            if (mark == null)
            {
                return false;
            }
            _databaseContext.Markings.Remove(mark);
            _databaseContext.SaveChanges();
            return true;
        }

        public bool IsMarked(int submissionId, int userId)
        {
            var bookmarkedSubmission =
                _databaseContext.Markings.FromSqlRaw("Select * from markings where submission_id={0} and user_id={1} limit(1)",
                    submissionId, userId).FirstOrDefault();
            if (bookmarkedSubmission == null)
                return false;
            return true;
        }

        public int NoOfMarkings(int userId)
        {
            return _databaseContext.Markings
                .Count(m => m.UserId == userId);
        }

        public IEnumerable<Submission> GetMarkedSubmissions(int userId, PagingAttributes pagingAttributes)
        {
            return _databaseContext.Markings
                .Where(m => m.UserId == userId)
                .Select(m => m.Submission)
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }
    }
}