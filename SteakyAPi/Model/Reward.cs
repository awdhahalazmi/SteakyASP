using StreakyAPi.Model.Streak;

namespace StreakyAPi.Model
{
    public class Reward
    {
        public int Id { get; set; }
        public int StreakId { get; set; }
        public Streaks Streak { get; set; }
        public string Description { get; set; }
        public int PointsClaimed { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
