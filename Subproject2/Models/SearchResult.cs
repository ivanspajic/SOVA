namespace Models
{
    public class SearchResult
    {
        public int Id { get; set; }
        public double Rank { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public bool IsQuestion { get; set; }
    }
}
