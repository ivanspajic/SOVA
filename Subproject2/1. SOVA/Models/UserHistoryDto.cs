using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0._Models;

namespace _1._SOVA.Models
{
    public class UserHistoryDto
    {
        public string Link { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SearchTerm { get; set; }
        public DateTime SearchDate { get; set; }
        public History UserHistory { get; set; }
    }
}
