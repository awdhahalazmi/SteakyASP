using System;
using System.Collections.Generic;

namespace StreakyFrontWeb.Models
{
    public class Streaks
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BusinessId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Reward> Rewards { get; set; } = new List<Reward>();
    }
}



//    public class Streak
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public DateTime StreakDate { get; set; } 
//    }
//}
