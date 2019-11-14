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

        public IEnumerable<Comment> GetMarkedComments(int userId, PagingAttributes pagingAttributes)
        {
            return _databaseContext.Comments
                .Include(comment => comment.CommentSubmission)
                    .ThenInclude(submission => submission.Markings)
                .Where(comment => comment.CommentSubmission.Markings.All(marking => marking.UserId == userId))
                .Skip(pagingAttributes.Page * pagingAttributes.PageSize)
                .Take(pagingAttributes.PageSize);
        }
    }
}