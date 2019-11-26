using System.Collections.Generic;

namespace Models
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
