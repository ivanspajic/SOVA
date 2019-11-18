using Data_Layer.Database_Context;
using Data_Layer_Abstractions;

namespace Data_Layer
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