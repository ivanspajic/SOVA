﻿using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class LinkPost
    {
        public int LinkPostId { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public Question LinkedPost { get; set; }
    }
}