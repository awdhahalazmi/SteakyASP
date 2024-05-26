namespace StreakyAPi.Model.Streak
{
    public class UserStreak
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StreakId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public UserAccount User { get; set; }
        public Streaks Streak { get; set; }
    }
}
