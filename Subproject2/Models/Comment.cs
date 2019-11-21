namespace Models
{
    public class Comment
    {
        public int Id { get; set; } // This is the id of the comment.
        public int SubmissionId { get; set; } // This is the Id of the parent, i.e either question or answer. A comment can be made on either questions or answers.
        public Submission Submission { get; set; }
        public Submission ParentSubmission { get; set; }
    }
}
