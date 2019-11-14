using _0._Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2._Data_Layer_Abstractions
{
    public interface IMarkingRepository
    {
        bool AddBookmark(int submissionId, int userId);
        int NoOfMarkings(int userId);
        IEnumerable<Submission> GetMarkedPosts(int userId, PagingAttributes pagingAttributes);
        bool IsMarked(int submissionId, int userId);
        bool RemoveBookmark(int submissionId, int userId);
    }
}
