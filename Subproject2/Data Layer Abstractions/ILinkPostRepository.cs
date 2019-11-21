using System.Collections.Generic;
using Models;

namespace Data_Layer_Abstractions
{
    public interface ILinkPostRepository
    {
        IEnumerable<LinkPost> GetLinkedPostByQuestionId(int questionId);
    }
}
