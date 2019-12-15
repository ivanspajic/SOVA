using Models;
using System.Collections.Generic;

namespace Data_Layer_Abstractions
{
    public interface IAnnotationRepository
    {
        Annotation Create(string annotation, int submissionId, int userId);
        bool Delete(int submissionId, int userId);
        Annotation GetBySubmissionAndUserIds(int submissionId, int userId);
        bool Update(string annotation, int submissionId, int userId);
        List<Annotation> GetUserAnnotations(int userId, PagingAttributes pagingAttributes);
        int NoOfAnnotations(int userId);
    }
}
