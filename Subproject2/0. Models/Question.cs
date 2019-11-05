using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class Question : Submission
    {
        public string Title { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}
