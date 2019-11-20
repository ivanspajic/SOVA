using System;
using System.Collections.Generic;
using Models;

namespace SOVA.Models
{
    public class QuestionDto
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public DateTime? ClosedDate { get; set; }
        public Submission Submission { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<QuestionsTag> Tags { get; set; }
    }
}
