using _0._Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1._SOVA.Models
{
    public class AnnotationDto
    {
        public string Link { get; set; }
        public string AnnotationString { get; set; }
        public Submission Submission { get; set; }
        public User User { get; set; }
    }
}
