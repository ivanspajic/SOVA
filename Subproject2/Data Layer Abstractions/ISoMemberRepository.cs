using Models;

namespace Data_Layer_Abstractions
{
    public interface ISoMemberRepository
    {
        SoMember GetSoMemberById(int soMemberId);
    }
}
