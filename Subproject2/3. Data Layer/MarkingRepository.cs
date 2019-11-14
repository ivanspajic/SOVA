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
    public class MarkingRepository : IMarkingRepository
    {
        private readonly SOVAContext _databaseContext;

        public MarkingRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public bool AddBookmark(int submissionId, int userId)
        {
            if (!IsMarked(submissionId, userId))
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
            if (IsMarked(submissionId, userId))
                return false;
            var mark = _databaseContext.Markings.Find(submissionId, userId);
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
            var bookmarkedSubmission = _databaseContext.Markings.Where(m => m.SubmissionId == submissionId && m.UserId == userId);
            //if (bookmarkedSubmission == null)
            //    return false;
            return true;
        }

        public int NoOfMarkings(int userId)
        {
            return _databaseContext.Markings
                .Where(m => m.UserId == userId)
                .Count();
        }

        public IEnumerable<Submission> GetMarkedPosts(int userId, PagingAttributes pagingAttributes)
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