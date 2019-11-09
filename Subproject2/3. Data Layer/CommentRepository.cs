﻿using System;
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

        public IEnumerable<Comment> GetAllCommentsByParentId(int parentId)
        {
            return _databaseContext.Comments.Include(c => c.PostSubmission).Where(c => c.SubmissionId == parentId);
        }

        public Comment GetCommentById(int commentId)
        {
            return _databaseContext.Comments.Include(c => c.SubmissionId).FirstOrDefault(c => c.SubmissionId == commentId);
        }
    }
}