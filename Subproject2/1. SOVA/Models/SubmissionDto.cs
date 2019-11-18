using _0._Models;
using System;

namespace _1._SOVA.Models
{
    public class SubmissionDto
    {
        public string Link { get; set; }
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime? CreationDate { get; set; }
        public int Score { get; set; }
        public int? SoMemberId { get; set; }
        public SoMember? SoMember { get; set; }
    }
}
