using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class Question
    {
        public int SubmissionId { get; set; }
        public string Title { get; set; }
        public DateTime ClosedDate { get; set; }
        public Submission Submission { get; set; }
    }
}
