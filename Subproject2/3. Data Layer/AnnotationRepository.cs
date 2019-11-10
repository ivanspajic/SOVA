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
    public class AnnotationRepository : IAnnotationRepository
    {
        private readonly SOVAContext _databaseContext;

        public AnnotationRepository(SOVAContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Annotation Create(string annotation, int submissionId)
        {
            Delete(submissionId); //only allow 1 annotation at a time

            var ant = new Annotation
            {
                SubmissionId = submissionId,
                AnnotationString = annotation
            };

            _databaseContext.Annotations.Add(ant);

            _databaseContext.SaveChanges();

            return ant;
        }

        public bool Delete(int submissionId)
        {
            if (_databaseContext.Annotations.Find(submissionId) != null)
            {
                _databaseContext.Annotations.Remove(_databaseContext.Annotations.Find(submissionId));
                _databaseContext.SaveChanges();

                return true;
            }
            return false;
        }

        public Annotation GetBySubmissionId(int submissionId)
        {
            var ant = _databaseContext.Annotations.Find(submissionId);

            return ant;
        }

        public bool Update(string annotation, int submissionId)
        {
            var ant = _databaseContext.Annotations.Find(submissionId);
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