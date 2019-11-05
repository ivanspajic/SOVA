using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class QuestionsTag
    {
        public int QuestionId { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public Question Question { get; set; }
    }
}
