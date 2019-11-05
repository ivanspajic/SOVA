using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class Comment : Submission
    {
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
    }
}
