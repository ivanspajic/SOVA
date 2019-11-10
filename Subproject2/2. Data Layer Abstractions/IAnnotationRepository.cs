using _0._Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2._Data_Layer_Abstractions
{
    public interface IAnnotationRepository
    {
        Annotation Create(string annotation, int submissionId);
        bool Delete(int submissionId);
        Annotation GetBySubmissionId(int submissionId);
    }
}
