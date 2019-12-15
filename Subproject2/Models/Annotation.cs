namespace Models
{
    public class Annotation
    {
        public int SubmissionId { get; set; }
        public int UserId { get; set; }
        public string AnnotationString { get; set; }
        public Question Question { get; set; }
        public User User { get; set; }
    }
}
