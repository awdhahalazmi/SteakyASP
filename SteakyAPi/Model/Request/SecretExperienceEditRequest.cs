namespace StreakyAPi.Model.Request
{
    public class SecretExperienceEditRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? StreakClaimed { get; set; }
        public int? BusinessId { get; set; }
    }
}
