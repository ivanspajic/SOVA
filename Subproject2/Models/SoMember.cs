using System;

namespace Models
{
    public class SoMember
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int? Age { get; set; }
        public string? Location { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
