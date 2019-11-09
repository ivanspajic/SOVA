using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using _0._Models;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using _2._Data_Layer_Abstractions;
using Npgsql;

namespace _3._Data_Layer
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
