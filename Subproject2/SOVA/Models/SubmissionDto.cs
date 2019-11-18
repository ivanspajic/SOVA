using System;
using Models;

namespace SOVA.Models
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
