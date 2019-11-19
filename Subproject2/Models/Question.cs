using System;
using System.Collections.Generic;

namespace Models
{
    public class Question
    {
        public string Title { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
    }
}
