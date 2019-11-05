using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    class Marking
    {
        public int SubmissionId { get; set; }
        public int UserId { get; set; }
        public Submission Submission { get; set; }
        public User User { get; set; }
    }
}
