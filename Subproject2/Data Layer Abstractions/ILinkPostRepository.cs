using Models;

namespace Data_Layer_Abstractions
{
    public interface ILinkPostRepository
    {
        LinkPost GetByQuestionAndLinkedPostIds(int questionId, int linkedPostId);
    }
}
