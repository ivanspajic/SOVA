using System;
using System.Collections.Generic;
using System.Linq;
using Data_Layer.Database_Context;
using Data_Layer_Abstractions;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data_Layer
{
    public class CommentRepository : ICommentRepository
    {
        private readonly SOVAContext _databaseContext;

        public CommentRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<Comment> GetAllCommentsBySubmissionId(int submissionId)
        {
            return _databaseContext.Comments
                .Include(c => c.Submission)
                .Include(a => a.Submission.SoMember)
                .Where(c => c.SubmissionId == submissionId)
                .ToList();
        }

        public Comment GetCommentById(int commentId)
        {
            return _databaseContext.Comments.Include(c => c.Submission).FirstOrDefault(c => c.Submission.Id == commentId);
        }

        public int NoOfComments(int submissionId)
        {
            return _databaseContext.Comments
                .Count(c => c.SubmissionId == submissionId);
        }
    }
}