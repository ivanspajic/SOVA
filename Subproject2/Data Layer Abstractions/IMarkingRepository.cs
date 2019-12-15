using System.Collections.Generic;
using Models;

namespace Data_Layer_Abstractions
{
    public interface IMarkingRepository
    {
        bool AddBookmark(int submissionId, int userId);
        int NoOfMarkings(int userId);
        bool IsMarked(int submissionId, int userId);
        bool RemoveBookmark(int submissionId, int userId);
        IEnumerable<Question> GetMarkedSubmissions(int userId, PagingAttributes pagingAttributes);
    }
}
