namespace StreakyAPi.Model.Responses
{
    public class StreakResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }  // Ensure this property is correctly named
        public List<BusinessResponse> Businesses { get; set; }
    }
}
