using _0._Models;
using _3._Data_Layer.Database_Context;
using System.Linq;
using _2._Data_Layer_Abstractions;

namespace _3._Data_Layer
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
            return _databaseContext.LinkPosts.Where((l) => l.QuestionId == questionId && l.LinkPostId == linkedPostId).FirstOrDefault();
        }
    }
}