using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class Comment
    {
        public int Id { get; set; } // This is the id of the comment.
        public Submission CommentSubmission { get; set; }
        public int SubmissionId { get; set; } // This is the Id of the parent, i.e either question or answer. A comment can be made on either questions or answers.
        public Submission ParentSubmission { get; set; }
    }
}
