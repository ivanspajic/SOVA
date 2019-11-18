using _3._Data_Layer.Database_Context;
using _2._Data_Layer_Abstractions;

namespace _3._Data_Layer
{
    public class QuestionTagRepository : IQuestionTagRepository
    {
        private readonly SOVAContext _databaseContext;

        public QuestionTagRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
    }
}