using System.Collections.Generic;
using System.Linq;
using Data_Layer.Database_Context;
using Data_Layer_Abstractions;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<LinkPost> GetLinkedPostByQuestionId(int questionId)
        {
            return _databaseContext.LinkPosts.Include(l => l.Question).ThenInclude(q => q.Submission).ThenInclude(s => s.SoMember).Where(l => l.QuestionId == questionId);
        }
    }
}