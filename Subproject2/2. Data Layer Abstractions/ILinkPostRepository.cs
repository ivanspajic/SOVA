using _0._Models;

namespace _2._Data_Layer_Abstractions
{
    public interface ILinkPostRepository
    {
        LinkPost GetByQuestionAndLinkedPostIds(int questionId, int linkedPostId);
    }
}
