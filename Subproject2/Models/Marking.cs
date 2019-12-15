namespace Models
{
    public class Marking
    {
        public int SubmissionId { get; set; }
        public int UserId { get; set; }
        public Question Question { get; set; }
        public User User { get; set; }
    }
}
