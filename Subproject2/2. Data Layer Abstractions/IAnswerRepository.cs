using System;
using System.Collections.Generic;
using System.Text;
using _0._Models;

namespace _2._Data_Layer_Abstractions
{
    public interface IAnswerRepository
    {
        IEnumerable<Answer> GetAnswerForQuestion(int questionId);

    }
}
