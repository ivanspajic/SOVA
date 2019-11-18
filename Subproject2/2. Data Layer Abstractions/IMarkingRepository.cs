using _0._Models;
using System.Collections.Generic;

namespace _2._Data_Layer_Abstractions
{
    public interface IMarkingRepository
    {
        bool AddBookmark(int submissionId, int userId);
        int NoOfMarkings(int userId);
        bool IsMarked(int submissionId, int userId);
        bool RemoveBookmark(int submissionId, int userId);
        IEnumerable<Submission> GetMarkedSubmissions(int userId, PagingAttributes pagingAttributes);
    }
}
