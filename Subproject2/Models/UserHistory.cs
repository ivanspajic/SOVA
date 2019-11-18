namespace Models
{
    public class UserHistory
    {
        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public History History { get; set; }
    }
}
