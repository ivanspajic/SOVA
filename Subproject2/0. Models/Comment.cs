using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class Comment
    {
        public int Id { get; set; }
        public Submission CommentSubmission { get; set; }
        public Submission PostSubmission { get; set; }
        public int SubmissionId { get; set; }
    }
}
