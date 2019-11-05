using System;
using System.Collections.Generic;
using System.Text;

namespace _0._Models
{
    public class UserHistory
    {
        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public History History { get; set; }
        public User User { get; set; }
    }
}
