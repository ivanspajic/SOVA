﻿using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public List<UserHistory>? UserHistory { get; set; }
        public List<Annotation>? UserAnnotations { get; set; }
        public List<Marking>? UserMarkings { get; set; }
    }
}
