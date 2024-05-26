namespace StreakyAPi.Model.Responses
{
    public class RewardResponse
    {
        public int Id { get; set; }
        public int StreakId { get; set; }
        public string StreakTitle { get; set; }
        public string Description { get; set; }
        public int PointsClaimed { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string BusinessImage { get; set; }  

    }
}
