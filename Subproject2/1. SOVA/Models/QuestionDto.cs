using _0._Models;
using System;

namespace _1._SOVA.Models
{
    public class QuestionDto
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public DateTime? ClosedDate { get; set; }
        public Submission Submission { get; set; }
        public int SubmissionId { get; set; }
    }
}
