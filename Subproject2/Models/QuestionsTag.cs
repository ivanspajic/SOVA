﻿namespace Models
{
    public class QuestionsTag
    {
        public int QuestionId { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
