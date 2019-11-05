using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace _3._Data_Layer
{
    class CommentRepository
    {
        private readonly DbContext databaseContext;

        public CommentRepository(DbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
    }
}
