using _0._Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2._Data_Layer_Abstractions
{
    public interface IQuestionRepository
    {
        int NoOfResults(string queryString, int? userId);
        IEnumerable<Question> GetTenRandomQuestions();
        Question GetById(int submissionId);
        IEnumerable<SearchResult> SearchQuestions(string queryString, int? userId, PagingAttributes pagingAttributes);
        List<QuestionsTag> GetQuestionsByTags(string tagName, PagingAttributes pagingAttributes);
    }
}
