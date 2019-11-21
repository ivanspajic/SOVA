namespace Models
{
    public class LinkPost
    {
        public int LinkPostId { get; set; }
        public int QuestionId { get; set; }
        public Submission Submission { get; set; }

    }
}
