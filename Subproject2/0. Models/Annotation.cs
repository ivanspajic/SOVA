using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace _0._Models
{
    public class Annotation
    {
        public int SubmissionId { get; set; }
        public int UserId { get; set; }
        public string AnnotationString { get; set; }
        public Submission Submission { get; set; }
        public User User { get; set; }
    }
}
