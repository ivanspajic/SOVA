using _0._Models;

namespace _1._SOVA.Models
{
    public class AnswerDto
    {
        public string Link { get; set; }
        public int ParentId { get; set; }
        public bool Accepted { get; set; }
        public Submission Submission { get; set; }
    }
}
