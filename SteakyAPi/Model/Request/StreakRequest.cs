namespace StreakyAPi.Model.Request
{
    public class StreakRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> BusinessIds { get; set; }
    }
}
