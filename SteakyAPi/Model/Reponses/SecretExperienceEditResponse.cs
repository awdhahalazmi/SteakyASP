namespace StreakyAPi.Model.Responses
{
    public class SecretExperienceEditResponse
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StreakClaimed { get; set; }
        public string BusinessName { get; set; }
        public string BusinessImage { get; set; }
    }
}
