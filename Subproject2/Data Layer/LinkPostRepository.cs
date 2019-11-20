using System.Linq;
using Data_Layer.Database_Context;
using Data_Layer_Abstractions;
using Models;

namespace Data_Layer
{
    public class LinkPostRepository : ILinkPostRepository
    {
        private readonly SOVAContext _databaseContext;

        public LinkPostRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public LinkPost GetByQuestionAndLinkedPostIds(int questionId, int linkedPostId)
        {
            return _databaseContext.LinkPosts.FirstOrDefault(l => l.QuestionId == questionId && l.LinkPostId == linkedPostId);
        }
    }
}