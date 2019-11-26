using System;
using System.Collections.Generic;

namespace Models
{
    public class Question
    {
        public string Title { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
        public IEnumerable<QuestionsTag> QuestionsTags { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public IEnumerable<LinkPost> LinkedPosts { get; set; }
    }
}
