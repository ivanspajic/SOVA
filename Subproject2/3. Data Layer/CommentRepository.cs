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
    public class CommentRepository : ICommentRepository
    {
        private readonly SOVAContext _databaseContext;

        public CommentRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<Comment> GetTenRandomComments()
        {
            var randomOffSet = new Random().Next(1, 1000);
            return _databaseContext.Comments.Skip(randomOffSet).Take(10);
        }

        public Comment GetById(int submissionId)
        {
            return _databaseContext.Comments.Find(submissionId);
        }
    }
}