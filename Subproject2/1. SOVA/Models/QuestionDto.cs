using _0._Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1._SOVA.Models
{
    public class QuestionDto
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public DateTime? ClosedDate { get; set; }
        public Submission Submission { get; set; }
        public int SubmissionId { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Question> LinkedPosts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
