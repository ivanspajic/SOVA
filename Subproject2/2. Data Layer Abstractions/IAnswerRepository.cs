using System;
using System.Collections.Generic;
using System.Text;
using _0._Models;

namespace _2._Data_Layer_Abstractions
{
    public interface IAnswerRepository
    {
        Answer GetAnswerById(int answerId);

        IEnumerable<Answer> GetAnswersForQuestionById(int questionId, PagingAttributes pagingAttributes);

        int NoOfAnswers(int questionId);

        IEnumerable<Answer> GetMarkedAnswers(int userId, PagingAttributes pagingAttributes);
    }
}
