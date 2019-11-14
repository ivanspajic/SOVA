using _0._Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2._Data_Layer_Abstractions
{
    public interface IQuestionRepository
    {
        int NoOfResults(string queryString);
        int NoOfAnswers(int questionId);
        IEnumerable<Answer> GetAnswersForQuestionById(int questionId, PagingAttributes pagingAttributes);
        IEnumerable<Question> GetTenRandomQuestions();
        Question GetById(int submissionId);
        IEnumerable<SearchResult> SearchQuestions(string queryString, PagingAttributes pagingAttributes);
        IEnumerable<Question> GetLinkedPosts(int questionId);
        IEnumerable<Tag> GetTags(int questionId);
    }
}
