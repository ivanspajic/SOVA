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

        public IEnumerable<Comment> GetAllCommentsBySubmissionId(int submissionId, PagingAttributes pagingAttributes)
        {
            return _databaseContext.Comments
                .Include(c => c.CommentSubmission)
                .Where(c => c.SubmissionId == submissionId)
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize)
                .ToList();
        }

        public Comment GetCommentById(int commentId)
        {
            return _databaseContext.Comments.Include(c => c.CommentSubmission).FirstOrDefault(c => c.SubmissionId == commentId);
        }

        public int NoOfComments(int submissionId)
        {
            return _databaseContext.Comments
                .Where(c => c.SubmissionId == submissionId)
                .Count();
        }
    }
}