using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace _3._Data_Layer
{
    class SubmissionRepository
    {
        private readonly DbContext databaseContext;

        public SubmissionRepository(DbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
    }
}
