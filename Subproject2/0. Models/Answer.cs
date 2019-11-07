using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class Answer
    {
        public int ParentId { get; set; }
        public bool Accepted { get; set; }
        public Submission Submission { get; set; }
        public int SubmissionId { get; set; }
    }
}
