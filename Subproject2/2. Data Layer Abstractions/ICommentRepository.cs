using System;
using System.Collections.Generic;
using System.Text;
using _0._Models;

namespace _2._Data_Layer_Abstractions
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> GetAllCommentsBySubmissionId(int parentId, PagingAttributes pagingAttributes);
        Comment GetCommentById(int commentId);
        int NoOfComments(int submissionId);

        IEnumerable<Comment> GetMarkedComments(int userId, PagingAttributes pagingAttributes);
    }
}
