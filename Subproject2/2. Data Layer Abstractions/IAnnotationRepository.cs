using _0._Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2._Data_Layer_Abstractions
{
    public interface IAnnotationRepository
    {
        Annotation Create(string annotation, int submissionId, int userId);
        bool Delete(int submissionId, int userId);
        Annotation GetBySubmissionAndUserIds(int submissionId, int userId);
        bool Update(string annotation, int submissionId, int userId);
    }
}
