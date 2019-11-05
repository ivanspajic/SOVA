using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class Answer
    {
        public int SubmissionId { get; set; }
        public int ParentId { get; set; }
        public bool Accepted { get; set; }
    }
}
