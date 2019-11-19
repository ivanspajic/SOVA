using System.Collections.Generic;
using Models;

namespace Data_Layer_Abstractions
{
    public interface IAnswerRepository
    {
        Answer GetAnswerById(int answerId);

        IEnumerable<Answer> GetAnswersForQuestionById(int questionId);

        int NoOfAnswers(int questionId);
    }
}
