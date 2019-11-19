using System.Collections.Generic;
using Models;

namespace Data_Layer_Abstractions
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> GetAllCommentsBySubmissionId(int parentId);
        Comment GetCommentById(int commentId);
        int NoOfComments(int submissionId);
    }
}
