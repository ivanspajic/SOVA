using System;

namespace Models
{
    public class Submission
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime? CreationDate { get; set; }
        public int Score { get; set; }
        public int? SoMemberId { get; set; }
        public SoMember? SoMember { get; set; }
    }
}
