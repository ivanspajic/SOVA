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
    public class AnnotationRepository : IAnnotationRepository
    {
        private readonly SOVAContext _databaseContext;

        public AnnotationRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Annotation Create(string annotation, int submissionId, int userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(annotation) || userId <= 0) return null;
                Delete(submissionId, userId); //only allow 1 annotation at a time
                var ant = new Annotation
                {
                    SubmissionId = submissionId,
                    AnnotationString = annotation,
                    UserId = userId
                };

                _databaseContext.Annotations.Add(ant);
                _databaseContext.SaveChanges();
                return ant;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool Delete(int submissionId, int userId)
        {
            Annotation annotationToDelete = _databaseContext.Annotations.Find(submissionId, userId);
            if (annotationToDelete != null)
            {
                _databaseContext.Annotations.Remove(annotationToDelete);
                _databaseContext.SaveChanges();

                return true;
            }
            return false;
        }

        public Annotation GetBySubmissionAndUserIds(int submissionId, int userId)
        {
            var ant = _databaseContext.Annotations.Find(submissionId, userId);

            return ant;
        }

        public bool Update(string annotation, int submissionId, int userId)
        {
            var ant = _databaseContext.Annotations.Find(submissionId, userId);
            if (ant != null)
            {
                ant.AnnotationString = annotation;
                _databaseContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}