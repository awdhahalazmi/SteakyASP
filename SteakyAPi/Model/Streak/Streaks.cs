namespace StreakyAPi.Model.Streak
{
    public class Streaks
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BusinessId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Business> Businsses { get; set; } = new List<Business>();
        public ICollection<UserStreak> UserStreaks { get; set; } = new List<UserStreak>();
        public ICollection<Reward> Rewards { get; set; } = new List<Reward>();  // Add this line







    }
}
