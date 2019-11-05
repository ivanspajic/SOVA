using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class Answer : Submission
    {
        public int ParentId { get; set; }
        public bool Accepted { get; set; }
    }
}
