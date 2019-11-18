using System;
using Models;

namespace SOVA.Models
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
