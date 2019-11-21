using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _0._Models
{
    public class Answer
    {
        public int ParentId { get; set; }
        public Question Question { get; set; }
        public bool Accepted { get; set; }
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
    }
}
