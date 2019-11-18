using _0._Models;
using System.Collections.Generic;

namespace _2._Data_Layer_Abstractions
{
    public interface IQuestionRepository
    {
        int NoOfRandomQuestions();
        int NoOfResults(string queryString, int? userId);
        IEnumerable<Question> GetQuestions(PagingAttributes pagingAttributes);
        Question GetById(int submissionId);
        IEnumerable<SearchResult> SearchQuestions(string queryString, int? userId, PagingAttributes pagingAttributes);
        List<QuestionsTag> GetQuestionsByTags(string tagName, PagingAttributes pagingAttributes);
    }
}
