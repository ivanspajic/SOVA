namespace Models
{
    public class Marking
    {
        public int SubmissionId { get; set; }
        public int UserId { get; set; }
        public Submission Submission { get; set; }
        public User User { get; set; }
    }
}
