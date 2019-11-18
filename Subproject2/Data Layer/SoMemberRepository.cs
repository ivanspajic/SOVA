using System.Linq;
using Data_Layer.Database_Context;
using Data_Layer_Abstractions;
using Models;

namespace Data_Layer
{
    public class SoMemberRepository : ISoMemberRepository
    {
        private readonly SOVAContext _databaseContext;

        public SoMemberRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public SoMember GetSoMemberById(int soMemberId)
        {
            return _databaseContext.SoMembers.FirstOrDefault(s => s.Id == soMemberId);
        }
    }
}
