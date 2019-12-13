using System.Collections.Generic;
using Models;

namespace Data_Layer_Abstractions
{
    public interface IQuestionRepository
    {
        int NoOfRandomQuestions();
        int NoOfResults(string queryString, int? userId);
        int NoOfResultsForTag(string tagString, int? userId);
        IEnumerable<Question> GetQuestions(PagingAttributes pagingAttributes);
        Question GetById(int submissionId);
        IEnumerable<SearchResult> SearchQuestions(string queryString, int? userId, PagingAttributes pagingAttributes);
        IEnumerable<QuestionsTag> GetQuestionsByTags(string tagName, PagingAttributes pagingAttributes);
        IEnumerable<QuestionsTag> GetQuestionsTags(int questionId);
        IEnumerable<CloudElement> GetWord2Words(string queryString, int? userId);
    }
}
