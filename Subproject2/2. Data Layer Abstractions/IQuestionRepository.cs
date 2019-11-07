using _0._Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2._Data_Layer_Abstractions
{
    public interface IQuestionRepository
    {
        public IEnumerable<Question> GetRandomTenQuestions();
    }
}
