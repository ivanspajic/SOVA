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

        public bool Bookmark(int submissionId, int userId)
        {
            if (IsMarked(submissionId, userId))
                return false;
            Marking mark = new Marking
            {
                SubmissionId = submissionId,
                UserId = userId
            };
            _databaseContext.Markings.Add(mark);
            return true;
        }

        public bool RemoveBookmark(int submissionId, int userId)
        {
            if (!IsMarked(submissionId, userId))
                return false;
            Marking mark = _databaseContext.Markings.Find(submissionId, userId);
            _databaseContext.Markings.Remove(mark);
            return true;
        }

        public bool IsMarked(int submissionId, int userId)
        {
            if (_databaseContext.Markings.Find(submissionId, userId) == null)
                return false;
            return true;
        }

        public int NoOfMarkings(int userId)
        {
            return _databaseContext.Markings
                .Where(m => m.UserId == userId)
                .Count();
        }

    }
}