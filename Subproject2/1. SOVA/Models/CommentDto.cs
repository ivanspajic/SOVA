using _0._Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1._SOVA.Models
{
    public class CommentDto
    {
        public string Link { get; set; }
        public int Id { get; set; }
        public Submission CommentSubmission { get; set; }
    }
}
