using Models;

namespace SOVA.Models
{
    public class CommentDto
    {
        public string Link { get; set; }
        public int Id { get; set; }
        public Submission CommentSubmission { get; set; }
    }
}
