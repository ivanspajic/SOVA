using System;
using Models;

namespace SOVA.Models
{
    public class UserHistoryDto
    {
        public string Link { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public History UserHistory { get; set; }
    }
}
